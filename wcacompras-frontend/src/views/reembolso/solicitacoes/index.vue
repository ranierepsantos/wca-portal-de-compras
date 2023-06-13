<template>
    <div>
        <bread-crumbs title="Solicitações" @novoClick="editar('novo')" />
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
                    <th class="text-left text-grey">CLIENTE</th>
                    <th class="text-left text-grey">COLABORADOR</th>
                    <th class="text-left text-grey">TIPO SOLICITAÇÃO</th>
                    <th class="text-left text-grey">STATUS</th>
                    <th class="text-center text-grey">ATIVO</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in solicitacoes" :key="item.id">
                    <td class="text-left">
                        <v-icon icon="mdi-home-outline"></v-icon>
                        &nbsp;{{ item.cliente }}
                    </td>
                    <td class="text-left">{{ item.colaborador }}</td>
                    <td class="text-left">{{ item.tipo }}</td>
                    <td class="text-left">{{ item.status }}</td>
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
//import clienteService from "@/services/cliente.service";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";

//DATA
const page = ref(1);
const pageSize = 5;
const isBusy = ref(false);
const totalPages = ref(1);
const solicitacoes = ref([]);
const filter = ref("");
const swal = inject("$swal");

//VUE METHODS
onMounted(async () =>
{
    await getItems();
});

watch(page, () => getItems());
watch(filter, () => getItems());

//METHODS
function editar(id)
{
    router.push({ name: "solicitacaoCadastro", query: { id: id } })
}

async function enableDisable(item)
{
    try
    {
        let text = item.ativo ? "Desativar" : "Ativar"

        let options = {
            title: text,
            text: `Deseja realmente ${text.toLowerCase()} a solicitação: ${item.id}?`,
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
        }

        let response = await swal.fire(options);
        if (response.isConfirmed)
        {
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
        console.log("solicitacoes.enableDisable.error:", error);
        handleErrors(error)
    }
}

async function getItems()
{
    try
    {
        isBusy.value = true;
        //let response = await clienteService.paginate(pageSize, page.value, filter.value);
        let clientesData = [
            {id: 1, cliente: "Cliente Reembolso A", colaborador: "Ivanildo Bacelar", tipo: "Reembolso", status: "Aguardando" , ativo: true},
            {id: 2, cliente: "Cliente Reembolso B", colaborador: "Ivanildo Bacelar", tipo: "Adiantamento", status: "Aprovado" , ativo: true},
            {id: 3, cliente: "Cliente Reembolso B", colaborador: "Ivanildo Bacelar", tipo: "Adiantamento", status: "Reprovado" , ativo: true},
            {id: 4, cliente: "Cliente Reembolso D", colaborador: "Ivanildo Bacelar", tipo: "Reembolso", status: "Aprovado" , ativo: true},
        ];

        solicitacoes.value = clientesData;
        totalPages.value = 1;
    } catch (error)
    {
        console.log("solicitacoes.getItems.error:", error.response);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}
</script>
  
  