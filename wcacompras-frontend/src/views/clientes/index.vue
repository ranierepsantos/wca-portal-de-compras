<template>
    <div>
        <bread-crumbs title="Clientes" @novoClick="editar('novo')" />

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
                        <v-btn icon="mdi-delete" variant="plain" color="error" @click="remove(item)"></v-btn>
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
import clienteService from "@/services/cliente.service";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";

//DATA
const page = ref(1);
const pageSize = 5;
const isBusy = ref(false);
const totalPages = ref(1);
const clientes = ref([]);
const filiais = ref([]);
const swal = inject("$swal");

//VUE METHODS
onMounted(async () =>
{
    await getItems();
});

watch(page, () =>
{
    getItems();
});

//METHODS
function editar(id)
{
    router.push({ name: "clienteCadastro", query: { id: id } })
}
async function remove(item)
{
    try
    {
        let options = {
            title: "Confirma Exclusão?",
            text: "Deseja realmente excluir o cliente: " + item.nome + "?",
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
        }

        let response = await swal.fire(options);
        if (response.isConfirmed)
        {
            await clienteService.remove(item.id);
            debugger
            if (users.value.length == 1)
            {
                page.value--;
            } else
            {
                await this.getItems()
            }

            swal.fire({
                toast: true,
                icon: "success",
                position: "top-end",
                title: "Sucesso!",
                text: "Exclusão realizada!",
                showConfirmButton: false,
                timer: 2000,
            })
        }
    } catch (error)
    {
        console.log("clientes.remove.error:", error);
        handleErrors(error)
    }
}

async function getItems()
{
    try
    {
        isBusy.value = true;
        let response = await clienteService.paginate(pageSize, page.value, "");
        clientes.value = response.data.items;
        totalPages.value = response.data.totalPages;
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
  
  