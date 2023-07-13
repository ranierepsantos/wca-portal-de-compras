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
                    <th class="text-center text-grey">DATA</th>
                    <th class="text-left text-grey">CLIENTE</th>
                    <th class="text-left text-grey">COLABORADOR</th>
                    <th class="text-left text-grey">TIPO SOLICITAÇÃO</th>
                    <th class="text-left text-grey">STATUS</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in solicitacoes" :key="item.id">
                    <td class="text-center">{{ moment(item.dataSolicitacao).format("D/MM/YYYY") }}</td>
                    <td class="text-left">
                        {{ clienteStore.getClienteById(item.clienteId).nome }}
                    </td>
                    <td class="text-left">{{ item.colaborador }}</td>
                    <td class="text-left">{{ solicitacaoStore.getTipoSolicitacao(item.tipoSolicitacao).text }}</td>
                    <td class="text-left">
                        <v-btn :color="solicitacaoStore.getStatus(item.status).color" variant="tonal"
                            density="compact" class="text-center"> {{
                                solicitacaoStore.getStatus(item.status).text
                            }}</v-btn>    
                    </td>
                    <td class="text-right">
                        <v-btn icon="mdi-lead-pencil" variant="plain" color="primary" @click="editar(item.id)"></v-btn>
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="6">
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
import moment from "moment";
import { useSolicitacaoStore } from "@/store/reembolso/solicitacao.store";
import { useClienteStore } from "@/store/reembolso/cliente.store";


//DATA
const page = ref(1);
const pageSize = 10;
const isBusy = ref(false);
const totalPages = ref(1);
const solicitacoes = ref([]);
const filter = ref("");
const swal = inject("$swal");
const solicitacaoStore = useSolicitacaoStore()
const clienteStore = useClienteStore()

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
    router.push({ name: "reembolsoSolicitacaoCadastro", query: { id: id } })
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
        let response = solicitacaoStore.getPaginate(page.value, pageSize)
        solicitacoes.value = response.items;

        response.items.forEach(item => {
            console.log(`tipo: ${item.tipoSolicitacao}, descricao: ${solicitacaoStore.getStatus(item.tipoSolicitacao)}`)
        })


        totalPages.value = response.totalPages;
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
  
  