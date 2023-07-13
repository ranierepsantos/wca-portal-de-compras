<template>
    <div>
        <bread-crumbs title="Faturamento" @novoClick="editar('novo')" />
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
                    <th class="text-center text-grey">#</th>
                    <th class="text-center text-grey">DATA</th>
                    <th class="text-left text-grey">CLIENTE</th>
                    <th class="text-center text-grey">VALOR</th>
                    <th class="text-left text-grey">STATUS</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in faturamentos" :key="item.id">
                    <td class="text-center">{{item.id}}</td>
                    <td class="text-center">{{ moment(item.dataCriacao).format("DD/MM/YYYY") }}</td>
                    <td class="text-left">
                        {{ clienteStore.getClienteById(item.clienteId).nome }}
                    </td>
                    <td class="text-right">{{ formatToCurrencyBRL(item.valor) }}</td>
                    <td class="text-left">
                        <v-btn :color="faturamentoStore.getStatus(item.status).color" variant="tonal"
                            density="compact" class="text-center"> {{
                                faturamentoStore.getStatus(item.status).text
                            }}</v-btn>    
                    </td>
                    <td class="text-right">
                        <v-btn icon="mdi-lead-pencil" variant="plain" color="primary" @click="editar(item.id)"></v-btn>
                        <v-btn icon="mdi-history" size="smaller" variant="plain" color="primary"
                            title="Histórico" :disabled="isBusy" @click="showHistorico(item)"></v-btn>
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="8">
                        <v-pagination v-model="page" :length="totalPages" :total-visible="4"></v-pagination>
                    </td>
                </tr>
            </tfoot>
        </v-table>
        <!--DIALOG PARA EXIBIR HISTÓRICO -->
        <v-dialog
        v-model="openHistorico"
        max-width="900"
        :absolute="false"
        
      >
        <historico :eventos="faturamentoEventos.sort(compararValor('dataEvento', 'desc'))"/>
      </v-dialog>
    </div>
</template>
  
<script setup>
import { ref, onMounted, watch, inject } from "vue";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import moment from "moment";
import { useFaturamentoStore } from "@/store/reembolso/faturamento.store";
import { useClienteStore } from "@/store/reembolso/cliente.store";
import historico from "@/components/reembolso/historico.vue";
import { compararValor, formatToCurrencyBRL } from "@/helpers/functions";
//DATA
const page = ref(1);
const pageSize = 10;
const isBusy = ref(false);
const totalPages = ref(1);
const faturamentos = ref([]);
const filter = ref("");
const swal = inject("$swal");
const faturamentoStore = useFaturamentoStore()
const clienteStore = useClienteStore()
const faturamentoEventos = ref([])
const openHistorico =ref(false)

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
    router.push({ name: "reembolsoFaturamentoCadastro", query: { id: id } })
}

async function showHistorico(item)
{
    faturamentoEventos.value = item.eventos
    openHistorico.value = true
}

async function getItems()
{
    try
    {
        isBusy.value = true;
        let response = await faturamentoStore.getPaginate(page.value, pageSize)
        faturamentos.value = response.items;

        totalPages.value = response.totalPages;
    } catch (error)
    {
        console.log("faturamento.getItems.error:", error.response);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}
</script>
  
  