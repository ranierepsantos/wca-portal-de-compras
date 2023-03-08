<template>
    <div>
        <bread-crumbs :title="'Pedido #' + (isBusy ? '' : requisicao.id)" :show-button="false"
            :custom-button-show="podeEditar" custom-button-text="Salvar" @customClick="salvar()"
            :custom-button-disabled="!hasChanged" />
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy || isSaving"></v-progress-linear>
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
                    <p v-show="requisicao.status == 3">
                        <span class="text-primary wca-texto">Data Entrega: </span>
                        <span class="text-grey wca-texto">{{
                            moment(requisicao.dataEntrega).format("DD/MM/YYYY")
                        }}</span>
                        &nbsp;&nbsp;
                        <span class="text-primary wca-texto">Nota Fiscal: </span>
                        <span class="text-grey wca-texto">{{ requisicao.notaFiscal }}</span>
                    </p>

                </v-col>
                <v-col cols="6">
                    <v-row v-show="requisicao.status != 3">
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
                <v-col cols="5">
                    <v-text-field v-show="podeEditar" label="Pesquisar Produto" v-model="filter"
                        placeholder="Nome ou Código" density="compact" variant="outlined" color="info">
                    </v-text-field>
                </v-col>
                <v-col class="text-right">
                    <v-btn :disabled="requisicao.id == null" :color="requisicao.id == null ? 'grey' : 'success'"
                        style="font-weight: bold;" @click="dialog = true" v-show="requisicao.status == 1">
                        Finalizar Pedido
                    </v-btn>
                </v-col>
            </v-row>
            <v-progress-linear color="primary" indeterminate :height="5" v-show="isLoadingProduto"></v-progress-linear>
            <small class="text-error" v-show="!hasProduto">Adicione pelo menos 1 produto</small>
            <v-table class="mt-2 elevation-2">
                <thead>
                    <tr>
                        <th class="text-center text-grey">CÓDIGO</th>
                        <th class="text-center text-grey">PRODUTO</th>
                        <th class="text-center text-grey">VALOR</th>
                        <th class="text-center text-grey">QUANTIDADE</th>
                        <th class="text-center text-grey">U. M.</th>
                        <th class="text-center text-grey">VALOR TOTAL</th>
                        <th class="text-center text-grey" v-show="podeEditar">REMOVER</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in requisicao.requisicaoItens" :key="item.id">
                        <td class="text-left">{{ item.codigo }}</td>
                        <td class="text-left">{{ item.nome }}</td>
                        <td class="text-right">{{ item.valor.toFixed(2) }}</td>
                        <td class="text-center">
                            <v-text-field density="compact" variant="outlined" type="number" color="primary"
                                :hide-details="true" class="sm ml-12" v-model="item.quantidade" min=0
                                @change="adicionarRemoverProduto(item)" v-if="podeEditar">
                            </v-text-field>
                            <span v-else>{{ item.quantidade }}</span>
                        </td>
                        <td class="text-center">{{ item.unidadeMedida }}</td>
                        <td class="text-right">
                            {{ ((item.valor + item.taxaGestao) * (isNaN(item.quantidade) ? 0 :
                            item.quantidade)).toFixed(2)}}
                        </td>
                        <td class="text-center" v-show="podeEditar">
                            <v-btn icon="mdi-package-variant-minus" variant="plain" color="red" title="Remover Produto"
                                @click="removeProdutoRequisicao(item)"></v-btn>
                        </td>
                    </tr>

                    <tr v-for="item in produtos" :key="item.id" class="text-grey" v-show="podeEditar">
                        <td class="text-left">{{ item.codigo }}</td>
                        <td class="text-left">{{ item.nome }}</td>
                        <td class="text-right">{{ item.valor.toFixed(2) }}</td>
                        <td class="text-left">
                            <v-text-field density="compact" variant="outlined" type="number" color="primary"
                                :hide-details="true" class="sm ml-12" v-model="item.quantidade" min="0"
                                @change="adicionarRemoverProduto(item)">
                            </v-text-field>

                        </td>
                        <td class="text-center">{{ item.unidadeMedida }}</td>
                        <td class="text-right">
                            {{ ((item.valor + item.taxaGestao) * (isNaN(item.quantidade) ? 0 :
                            item.quantidade)).toFixed(2)}}
                        </td>
                        <td class="text-center">
                            <!-- <v-btn icon="mdi-package-variant-plus" variant="plain" color="success"
                                        :disabled="(isNaN(item.quantidade) ? 0 : item.quantidade) == 0"
                                        @click="adicionarProdutoRequisicao(item)" title="Incluir Produto"></v-btn> -->
                        </td>
                    </tr>
                </tbody>
            </v-table>
            <!--HISTORICO DO PEDIDO -->
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
        <!-- FORM PARA FINALIZAÇÃO DO PEDIDO -->
        <v-dialog v-model="dialog" max-width="700" max-height="700" :absolute="false" persistent>
            <v-card>
                <v-card-title class="text-primary text-h5">
                    FINALIZAR PEDIDO
                </v-card-title>
                <v-card-text>
                    <v-form @submit.prevent="finalizarPedido()" ref="finalizarForm">
                        <v-row>
                            <v-col>
                                <v-text-field label="Número Nota Fiscal" v-model="finalizarData.notaFiscal" type="text"
                                    required variant="outlined" color="primary"
                                    :rules="[(v) => !!v || 'Número Nota Fiscal é obrigatório']"
                                    density="compact"></v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field label="Data Entrega" v-model="finalizarData.dataEntrega" type="date"
                                    required variant="outlined" color="primary"
                                    :rules="[(v) => !!v || 'Data da Entrega é obrigatório']"
                                    density="compact"></v-text-field>
                            </v-col>

                        </v-row>
                        <v-row>
                            <v-col class="text-right">
                                <v-btn variant="outlined" color="primary" @click="dialog = false">Cancelar</v-btn>
                                <v-btn color="primary" type="submit" class="ml-3">Finalizar</v-btn>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-card-text>
                <v-card-actions></v-card-actions>
            </v-card>
        </v-dialog>
    </div>
</template>

<script setup>
import { ref, onMounted, inject, computed, watch } from "vue";
import requisicaoService from "@/services/requisicao.service";
import handleErrors from "@/helpers/HandleErrors"
import { useRoute } from "vue-router";
import { useAuthStore } from "@/store/auth.store";
import router from "@/router";
import BreadCrumbs from "@/components/breadcrumbs.vue";
import { status, compararValor, realizarDownload } from "@/helpers/functions"
import tipoFornecimentoService from "@/services/tipofornecimento.service";
import fornecedorService from "@/services/fornecedor.service";
import moment from 'moment'

//DATA
const authStore = useAuthStore();
const route = useRoute();
const swal = inject("$swal");
let isBusy = ref(false);
let isSaving = ref(false);
let requisicao = ref({
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
    requisicaoHistorico: [],
    notaFiscal: null,
    dataEntrega: null

});
let filter = ref("");
let orcamento = ref(null);
let produtos = ref([]);
let tipoFornecimento = ref([])
let isLoadingProduto = ref(false)
let requisicaoOriginal = ref(null)
let dialog = ref(false);
let finalizarForm = ref(null);
let finalizarData = ref({
    id: requisicao.id,
    notaFiscal: null,
    dataEntrega: moment().format("YYYY-MM-DD")
})
let hasProduto = ref(true);
//VUE METHODS
onMounted(async () =>
{
    let id = route.params.requisicao;
    await getTipoFornecimentoToList();
    await getRequisicaoData(id)
    await getProdutosToList()


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

const podeEditar = computed(() => requisicao.value.status != 1 && requisicao.value.status != 3 && requisicao.value.status != 4)
const hasChanged = computed(() =>
{
    let itensOriginals = (JSON.stringify(requisicao.value.requisicaoItens)).replaceAll('"', '').replaceAll('.00', '');
    if (requisicaoOriginal.value != null)
        itensOriginals = (JSON.stringify(requisicaoOriginal.value.requisicaoItens)).replaceAll('"', '').replaceAll('.00', '');

    let itens = (JSON.stringify(requisicao.value.requisicaoItens)).replaceAll('"', '').replaceAll('.00', '');

    return itensOriginals != itens;
})
watch(filter, async () =>
{
    await getProdutosToList()
})

//METHODS
function adicionarProdutoRequisicao(item)
{
    let index = requisicao.value.requisicaoItens.findIndex(p => p.id == item.id)
    if (index == -1)
    {
        let produto = { ...item }
        delete produto.id
        requisicao.value.requisicaoItens.push(produto)

        if (!hasProduto.value) 
            hasProduto.value = true;

        produtoRemoveFromList(item)
        ordenarRequisicaoItens()
    }
    calcularOrcamentoTotais()
}

function adicionarRemoverProduto(item)
{
    if (item.quantidade > 0)
    {
        adicionarProdutoRequisicao(item)
    } else
    {
        removeProdutoRequisicao(item)
    }

}

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

async function finalizarPedido()
{
    try
    {
        let data = {
            id: requisicao.value.id,
            dataEntrega: finalizarData.value.dataEntrega,
            notaFiscal: finalizarData.value.notaFiscal
        }

        await requisicaoService.finalizarPedido(data);

        await swal.fire({
            toast: true,
            icon: "success",
            index: "top-end",
            title: "Sucesso!",
            text: "Pedido finalizado com sucesso!",
            showConfirmButton: false,
            timer: 2000,
        })

    } catch (error)
    {
        console.log("finalizarPedido.error", error)
        handleErrors(error)
    } finally
    {
        router.push({ name: "requisicoes" })
    }
}

function formatarDataHora(dataHora)
{
    return (new Date(dataHora)).toLocaleString('pt-BR')
}

async function getProdutosToList()
{
    try
    {
        isLoadingProduto.value = true;
        let response = await fornecedorService.produtoPaginate(requisicao.value.fornecedorId, 99999, 1, filter.value);
        produtos.value = response.data.items;

        for (let index = 0; index < requisicao.value.requisicaoItens.length; index++)
        {
            let item = requisicao.value.requisicaoItens[index];
            produtoRemoveFromList(item)
        }
    } catch (error)
    {
        console.log("getProdutosToList.error:", error);
        handleErrors(error)
    } finally
    {
        isLoadingProduto.value = false
    }
}

async function getRequisicaoData(id)
{
    try
    {
        isBusy.value = true;
        let response = await requisicaoService.getById(id);
        requisicao.value = response.data;
        requisicao.value.requisicaoHistorico.sort(compararValor("dataHora", "desc"))
        requisicaoOriginal.value = JSON.parse(JSON.stringify(requisicao.value));

        orcamento.value = requisicao.value.cliente.clienteOrcamentoConfiguracao;
        for (let idx = 0; idx < orcamento.value.length; idx++)
        {
            orcamento.value[idx].valorTotal = 0
            orcamento.value[idx].percentual = 0
        }
        calcularOrcamentoTotais()
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

function ordenarRequisicaoItens() 
{
    if (requisicao.value.requisicaoItens.length > 0)
        requisicao.value.requisicaoItens.sort(compararValor("nome"))
}


function produtoRemoveFromList(item)
{
    let index = produtos.value.findIndex(p => p.codigo == item.codigo)
    if (index != -1)
        produtos.value.splice(index, 1)
}

async function removeProdutoRequisicao(item)
{
    let index = requisicao.value.requisicaoItens.findIndex(p => p.codigo == item.codigo)

    if (index != -1)
    {
        requisicao.value.requisicaoItens.splice(index, 1)
        await getProdutosToList()
        ordenarRequisicaoItens()
    }
    calcularOrcamentoTotais()
}


async function salvar()
{
    try
    {
        isSaving.value = true;
        hasProduto.value = requisicao.value.requisicaoItens.length > 0;
        if (hasProduto.value) {
            //remover o campo id da requisicaoItens
            requisicao.value.requisicaoItens.forEach(produto =>
            {
                produto.valorTotal = ((parseFloat(produto.taxaGestao) + parseFloat(produto.valor)) * produto.quantidade)
                // produto.valorTotal = ((parseFloat(produto.valor)) * produto.quantidade)
                // produto.taxaGestao = (parseFloat(produto.taxaGestao) * produto.quantidade)

                requisicao.value.taxaGestao += produto.taxaGestao
                requisicao.value.valorTotal += produto.valorTotal

                produto.valorTotal = produto.valorTotal.toFixed(2)
                produto.taxaGestao = produto.taxaGestao.toFixed(2)
            })
            requisicao.value.valorTotal = requisicao.value.valorTotal.toFixed(2)
            requisicao.value.taxaGestao = requisicao.value.taxaGestao.toFixed(2)
            requisicao.value.NomeUsuario = authStore.user.nome;
            orcamento.value.forEach(o =>
            {
                if (o.percentual > 100)
                {
                    if (o.AprovadorPor == 1)
                        requisicao.value.requerAutorizacaoCliente = true;
                    else
                        requisicao.value.requerAutorizacaoWCA = true;
                }
            })
            let data = { ...requisicao.value }
            if (hasChanged.value == true)
            {
                data.status = -2; //Itens Alterados
            }

            await requisicaoService.update(data);

            swal.fire({
                toast: true,
                icon: "success",
                index: "top-end",
                title: "Sucesso!",
                text: "Dados salvos com sucesso!",
                showConfirmButton: false,
                timer: 2000,
            })
            router.push({ name: "requisicoes" })
        }
    } catch (error)
    {
        console.log("salvar.error:", error);
        handleErrors(error)
    } finally
    {
        isSaving.value = false
    }
}


</script>

<style scoped>
.wca-texto {
    font-size: 18px;
}

table .v-text-field {
    width: 80px;
}
</style>