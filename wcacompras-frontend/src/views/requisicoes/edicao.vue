<template>
    <div>
        <bread-crumbs :title="'Pedido #' + requisicao.id" :show-button="false" :custom-button-show="false"
            custom-button-text="Salvar" @customClick="salvar()" />
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <v-container v-show="!isBusy">
            <v-row>
                <v-col cols="6" class="text-left">
                    <p>
                        <span class="text-primary wca-texto">Cliente: </span>
                        <span class="text-grey wca-texto">{{ requisicao.cliente.nome }}</span>
                    </p>
                    <p>
                        <span class="text-primary wca-texto">CNPJ: </span>
                        <span class="text-grey wca-texto">{{ requisicao.cliente.cnpj }}</span>
                    </p>
                    <p>
                        <span class="text-primary wca-texto">Endereço: </span>
                        <span class="text-grey wca-texto">{{ enderecoComplento }}</span>
                    </p>
                    <p>
                        <span class="text-primary wca-texto">Supervisor: </span>
                        <span class="text-grey wca-texto">{{ requisicao.usuario.text }}</span>
                    </p>
                </v-col>
                <v-col cols="6">
                    <v-row>
                        <v-col cols="4" class="text-right" v-for="config in orcamento" :key="config.tipoFornecimentoId">
                            <v-progress-linear
                                :color="config.percentual > 100 ? 'red' : config.percentual > 60 ? 'warning' : 'success'"
                                :model-value="config.valorTotal"
                                :max="config.valorPedido * (1 + config.tolerancia / 100)" :height="7"
                                :title="getTipoFornecimentoNome(config.tipoFornecimentoId)"></v-progress-linear>
                            <span style="font-size:12px;" class="text-grey">{{ config.valorTotal.toFixed(2) }} /
                                {{ (config.valorPedido * (1 + config.tolerancia / 100)).toFixed(2) }}</span>
                        </v-col>

                    </v-row>
                    <v-row>
                        <v-col align-self="end" class="text-right">
                            <v-btn :color="getStatus(requisicao.status).color" variant="tonal" density="compact"
                                class="text-center">
                                {{ getStatus(requisicao.status).text }}
                            </v-btn>
                        </v-col>
                    </v-row>

                </v-col>
            </v-row>
            <!--TABELA DE EXIBIÇÃO DOS PRODUTOS -->
            <v-row class="mt-2">
                <v-col cols="10" offset="1">
                    <v-table>
                        <thead>
                            <tr>
                                <th class="text-center text-grey">CÓDIGO</th>
                                <th class="text-center text-grey">PRODUTO</th>
                                <th class="text-center text-grey">VALOR</th>
                                <th class="text-center text-grey">QUANTIDADE</th>
                                <th class="text-center text-grey">U. M.</th>
                                <th class="text-center text-grey">TOTAL</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="item in requisicao.requisicaoItens" :key="item.id">
                                <td class="text-left">{{ item.codigo }}</td>
                                <td class="text-left">{{ item.nome }}</td>
                                <td class="text-right">{{ item.valor.toFixed(2) }}</td>
                                <td class="text-center">{{ item.quantidade }}</td>
                                <td class="text-center">{{ item.unidadeMedida }}</td>
                                <td class="text-right">{{ (item.valorTotal).toFixed(2) }}
                                </td>
                            </tr>
                        </tbody>
                    </v-table>
                </v-col>
            </v-row>
            <!-- BOTÕES DE APROVAÇÃO / RECUSA -->
            <!-- <v-row>
                <v-col cols="10" offset="1" class="text-right">
                    <v-btn :color="requisicao.id == null ? 'grey' : '#EDCCCC'" class="mr-4"
                        style="color:#950000; font-weight: bold;" @click="aprovarReprovar(false)"
                        :disabled="requisicao.id == null">
                        Recusar
                    </v-btn>
                    <v-btn :disabled="requisicao.id == null" :color="requisicao.id == null ? 'grey' : 'success'"
                        style="font-weight: bold;" @click="aprovarReprovar(true)">
                        Aprovar
                    </v-btn>
                </v-col>
            </v-row> -->
            <v-row class="mt-2">
                <v-col cols="12" class="text-left">
                    <h4 class="text-grey">Histórico</h4>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="6" class="text-left">
                    <v-timeline align="start" side="end">
                        <v-timeline-item dot-color="grey" size="small" v-for="item in requisicao.requisicaoHistorico">
                            <div class="d-flex">
                                <div>
                                    <div class="text-caption"
                                        v-html="formatarDataHora(item.dataHora) + ' - ' + item.evento">

                                    </div>
                                </div>
                            </div>
                        </v-timeline-item>
                    </v-timeline>
                </v-col>
            </v-row>
        </v-container>
    </div>
</template>

<script setup>
import { ref, onMounted, inject, computed } from "vue";
import requisicaoService from "@/services/requisicao.service";
import handleErrors from "@/helpers/HandleErrors"
import { useRoute } from "vue-router";
import router from "@/router";
import BreadCrumbs from "@/components/breadcrumbs.vue";
import { compararValor } from "@/helpers/functions"
import tipoFornecimentoService from "@/services/tipofornecimento.service";

//DATA
const route = useRoute();
const isBusy = ref(false);
const requisicao = ref({
    id: null,
    cliente: {
        nome: "",
        cnpj: "",
        endereco: "",
        numero: "",
        cidade: "",
        uf: "",
        cep: ""
    },
    valorTotal: 0,
    taxaGestao: 0,
    status: -1,
    destino: 0,
    usuario: {
        text: "",
        value: ""
    },
    requisicaoItens: [],
    requisicaoHistorico: []
});
let orcamento = ref(null);
let status = [
    { value: -1, text: "Todos" },
    { value: 0, text: "Aguardando", color: "warning" },
    { value: 1, text: "Aprovado", color: "success" },
    { value: 2, text: "Rejeitado", color: "error" },
    { value: 3, text: "Finalizado", color: "success" }
]
let tipoFornecimento = ref([])

//VUE METHODS
onMounted(async () =>
{
    let id = route.params.requisicao;
    await getTipoFornecimentoToList();
    await getRequisicaoData(id)
    calcularOrcamentoTotais()

});

const enderecoComplento = computed(() =>
{
    let cliente = requisicao.value.cliente;
    let endereco = cliente.endereco;
    endereco += cliente.numero == "" ? "" : `, ${cliente.numero}`;
    endereco += cliente.cidade == "" ? "" : `, ${cliente.cidade}`;
    endereco += cliente.uf == "" ? "" : `, ${cliente.uf}`;
    endereco += cliente.cep == "" ? "" : `, ${cliente.cep}`
    return endereco
})

//METHODS

function calcularOrcamentoTotais()
{
    clearOrcamentoTotais()
    for (let idx = 0; idx < requisicao.value.requisicaoItens.length; idx++) 
    {
        let item = requisicao.value.requisicaoItens[idx]
        let index = orcamento.value.findIndex(o => o.tipoFornecimentoId == item.tipoFornecimentoId);
        orcamento.value[index].valorTotal += (parseFloat(item.valor) * item.quantidade)
        orcamento.value[index].percentual = (orcamento.value[index].valorTotal / (orcamento.value[index].valorPedido * (1 + orcamento.value[index].tolerancia / 100))) * 100
    }
}
function clearOrcamentoTotais()
{
    for (let idx = 0; idx < orcamento.value.length; idx++)
        orcamento.value[idx].valorTotal = 0;
}

function formatarDataHora(dataHora)
{
    return (new Date(dataHora)).toLocaleString('pt-BR')
}
async function getRequisicaoData(id)
{
    try
    {
        isBusy.value = true;
        let response = await requisicaoService.getById(id);
        requisicao.value = response.data;


        requisicao.value.requisicaoHistorico.sort(compararValor("dataHora", "desc"))
        orcamento.value = requisicao.value.cliente.clienteOrcamentoConfiguracao;
        for (let idx = 0; idx < orcamento.value.length; idx++)
        {
            orcamento.value[idx].valorTotal = 0
            orcamento.value[idx].percentual = 0
        }
    } catch (error)
    {
        console.log("getRequisicaoData.error:", error);
        handleErrors(error)
        router.push({ name: "requisicoes" })
    } finally
    {
        isBusy.value = false
    }
}

function getStatus(codigo)
{
    let dado = status.filter(s => s.value == codigo)[0]
    return dado;
}

function getTipoFornecimentoNome(tipoId)
{
    let tipo = tipoFornecimento.value.filter(t => t.value == tipoId)
    if (tipo.length > 0)
    {
        return tipo[0].text
    }
    return ""
}

async function getTipoFornecimentoToList() 
{
    try
    {
        isBusy.value = true;
        let response = await tipoFornecimentoService.toList();
        tipoFornecimento.value = response.data;
    } catch (error)
    {
        console.log("getTipoFornecimentoToList.error:", error);
        handleErrors(error)
    } finally { isBusy.value = false; }
}

</script>

<style scoped>
.wca-texto {
    font-size: 18px;
}
</style>