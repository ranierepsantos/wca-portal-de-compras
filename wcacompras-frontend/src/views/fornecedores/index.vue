<template>
    <div>
        <bread-crumbs title="Fornecedores" @novoClick="editar('novo')" />
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
                    <th class="text-left text-grey">ICMS</th>
                    <th class="text-center text-grey">ATIVO</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in fornecedores" :key="item.id">
                    <td class="text-left">
                        <v-icon icon="mdi-home-outline"></v-icon>
                        &nbsp;{{ item.nome }}
                    </td>
                    <td class="text-left">{{ item.cnpj }}</td>
                    <td class="text-right">{{ item.icms?.toFixed(2) }} %</td>
                    <td class="text-center">
                        <v-icon :icon="item.ativo ? 'mdi-check' : 'mdi-close'" variant="plain"
                            :color="item.ativo ? 'success' : 'error'"></v-icon>
                    </td>
                    <td class="text-right">
                        <v-btn icon="mdi-lead-pencil" variant="plain" color="primary" @click="editar(item.id)"
                            title="Editar"></v-btn>
                        <v-btn variant="plain" :color="item.ativo ? 'error' : 'success'"
                            :title="item.ativo ? 'Desativar' : 'Ativar'"
                            :icon="item.ativo ? 'mdi-close-circle-outline' : 'mdi-check-circle-outline'"
                            @click="enableDisable(item)">
                        </v-btn>

                        <v-btn icon="mdi-archive-settings-outline" variant="plain" color="primary"
                            title="Itens do Fornecedor"
                            @click="router.push({ name: 'fornecedorProdutos', params: { nome: item.nome }, query: { fornecedor: item.id } })"></v-btn>
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
import fornecedorService from "@/services/fornecedor.service";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";

//DATA
const page = ref(1);
const pageSize = 5;
const isBusy = ref(false);
const totalPages = ref(1);
const fornecedores = ref([]);
const swal = inject("$swal");
const filter = ref("");

//VUE METHODS
onMounted(async () =>
{
    await getItems();
});

watch(page, () => getItems());
watch(filter, () => getItems());

//METHODS
async function enableDisable(item)
{
    try
    {
        let text = item.ativo ? "Desativar" : "Ativar"

        let options = {
            title: text,
            text: `Deseja realmente ${text.toLowerCase()} o fornecedor: ${item.nome}?`,
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
        }

        let response = await swal.fire(options);
        if (response.isConfirmed)
        {
            let data = { ...item }
            data.ativo = !data.ativo
            await fornecedorService.update(data);
            await getItems()

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
        console.log("fornecedores.enableDisable.error:", error);
        handleErrors(error)
    }
}


function editar(id)
{
    router.push({ name: "fornecedorCadastro", query: { id: id } })
}
async function getItems()
{
    try
    {
        isBusy.value = true;
        let response = await fornecedorService.paginate(pageSize, page.value, filter.value);
        fornecedores.value = response.data.items;
        totalPages.value = response.data.totalPages;
    } catch (error)
    {
        console.log("fornecedores.getItems.error:", error.response);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}
</script>
