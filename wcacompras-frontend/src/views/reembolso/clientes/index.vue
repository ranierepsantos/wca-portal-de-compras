<template>
    <div>
        <bread-crumbs title="Clientes" @novoClick="editar('novo')" />
        <v-row>
            <v-col cols="6">
                <v-text-field label="Pesquisar" placeholder="(Nome)" v-model="filter" density="compact"
                    variant="outlined" color="info">
                </v-text-field>
            </v-col>
        </v-row>
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <v-table class="elevation-2">
            <thead>
                <tr>
                    <th class="text-left text-grey">NOME</th>
                    <th class="text-left text-grey">CNPJ</th>
                    <th class="text-center text-grey">ATIVO</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in clientes" :key="item.id">
                    <td class="text-left">
                        <v-icon icon="mdi-home-outline"></v-icon>
                        &nbsp;{{ item.nome }}
                    </td>
                    <td class="text-left">{{ item.cnpj }}</td>
                    <td class="text-center">
                        <v-icon :icon="item.ativo ? 'mdi-check' : 'mdi-close'" variant="plain"
                            :color="item.ativo ? 'success' : 'error'"></v-icon>
                    </td>
                    <td class="text-right">
                        <v-btn icon="mdi-lead-pencil" variant="plain" color="primary" @click="editar(item.id)"></v-btn>
                        <v-btn variant="plain" :color="item.ativo ? 'error' : 'success'"
                            :title="item.ativo ? 'Desativar' : 'Ativar'"
                            :icon="item.ativo ? 'mdi-close-circle-outline' : 'mdi-check-circle-outline'"
                            @click="enableDisable(item)">
                        </v-btn>
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="4">
                        <v-pagination v-model="page" :length="totalPages" :total-visible="4"></v-pagination>
                    </td>
                </tr>
            </tfoot>
        </v-table>
    </div>
</template>
  
<script setup>
import { ref, onMounted, watch, inject } from "vue";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import { useAuthStore } from "@/store/auth.store";
import { useClienteStore } from "@/store/reembolso/cliente.store";
import { useUsuarioStore } from "@/store/reembolso/usuario.store";
//DATA
const page = ref(1);
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const isBusy = ref(false);
const totalPages = ref(1);
const clienteStore = useClienteStore();
const clientes = ref([]);
const filter = ref("");
const swal = inject("$swal");
const authStore = useAuthStore();

//VUE METHODS
onMounted(async () =>
{
    authStore.user.filial = (await useUsuarioStore().getFiliais(authStore.user.id))[0].value
    await getItems();
});

watch(page, () => getItems());
watch(filter, () => getItems());

//METHODS
function editar(id)
{
    router.push({ name: "reembolsoCadastroCliente", query: { id: id } })
}

async function enableDisable(item)
{
    try
    {
        let text = item.ativo ? "Desativar" : "Ativar"

        let options = {
            title: text,
            text: `Deseja realmente ${text.toLowerCase()} o cliente: ${item.nome}?`,
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
        }

        let response = await swal.fire(options);
        if (response.isConfirmed)
        {
            item.ativo = !item.ativo;
            await clienteStore.updateCliente(item) 
            swal.fire({
                toast: true,
                icon: "success",
                position: "top-end",
                title: "Sucesso!",
                text: "Alteração realizada!",
                showConfirmButton: false,
                timer: 2000,
            })
        }
    } catch (error)
    {
        console.log("clientes.enableDisable.error:", error);
        handleErrors(error)
    }
}

async function getItems()
{
    try
    {
        isBusy.value = true;
        let response = await clienteStore.getClientesByPaginate(authStore.user.filial, page.value, pageSize, filter.value);
        clientes.value = response.items;
        totalPages.value = response.totalPages;
    } catch (error)
    {
        console.log("clientes.getItems.error:", error.response);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}
</script>
  
  