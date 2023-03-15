<template>
    <div>
        <bread-crumbs title="Nova Requisição" :show-button="false" :custom-button-show="true" custom-button-text="Salvar"
            @customClick="salvar()" />
        <v-form ref="formCadastro">
            <v-row>
                <v-col cols="5">
                    <v-select label="Clientes" v-model="requisicao.clienteId" :items="clientes" density="compact"
                        item-title="nome" item-value="id" variant="outlined" color="primary" :hide-details="false"
                        :rules="[(v) => !!v || 'Campo obrigatório']"></v-select>
                </v-col>
                <v-col cols="5">
                    <v-select label="Fornecedor" v-model="requisicao.fornecedorId" :items="fornecedores" density="compact"
                        item-title="nome" item-value="id" variant="outlined" color="primary" :hide-details="false"
                        :rules="[(v) => !!v || 'Campo obrigatório']" :disabled="requisicao.clienteId == null"></v-select>
                </v-col>
                <v-col cols="2">
                    <v-select label="Destino" v-model="requisicao.destino" :items="destinos" density="compact"
                        item-title="text" item-value="value" variant="outlined" color="primary"
                        :hide-details="false"></v-select>
                </v-col>
            </v-row>
            <v-row>
                <v-col cols="10">
                    <v-text-field label="Local Entrega" v-model="requisicao.localEntrega"
                        placeholder="Local de entrega do Pedido" density="compact" variant="outlined" color="primary"
                        :rules="[(v) => !!v || 'Campo obrigatório']">
                    </v-text-field>
                </v-col>

                <v-col cols="2">
                    <v-text-field-money label-text="ICMS (%)" v-model="requisicao.icms" color="primary"
                        :number-decimal="2"></v-text-field-money>
                </v-col>
            </v-row>
            <v-row v-show="requisicao.clienteId > 0">
                <v-col cols="10" class="text-left">
                    <div>
                        <span class="text-grey">Entregas: <small>Legenda: (M - Manhã, T - Tarde, N - Noite)</small></span>
                        <br />
                        <span v-for="(dia, index) in diasDaSemana" :key="dia"
                            :class="index > 0 ? 'text-primary ml-2' : 'text-primary'"
                            v-show="requisicao.periodoEntrega[dia.value].selected && requisicao.periodoEntrega[dia.value].periodo.length > 0">
                            {{ dia.text }}:&nbsp;
                            <v-avatar color="info" size="x-small"
                                v-for="periodo in requisicao.periodoEntrega[dia.value].periodo">{{ periodo.slice(0, 1)
                                }}</v-avatar>
                        </span>
                    </div>
                </v-col>
                <v-col>
                    <v-btn color="primary" @click="openPeriodoForm = true">Alterar Período</v-btn>
                </v-col>
            </v-row>
        </v-form>
        <v-divider class="mt-5 mb-2"></v-divider>
        <v-row>
            <v-col cols="5">
                <v-text-field label="Pesquisar Produto" v-model="filter" placeholder="Nome ou Código" density="compact"
                    variant="outlined" color="info">
                </v-text-field>
            </v-col>
            <v-col cols="1"></v-col>
            <v-col v-show="cliente.naoUltrapassarLimitePorRequisicao">
                <v-row>
                    <v-col class="text-right text-grey">Valor limite por pedido: </v-col>
                    <v-col>
                        <v-progress-linear
                            :color="(parseFloat(valorTotalPedido) / cliente.valorLimiteRequisicao * 100) > 100 ? 'red' : (parseFloat(valorTotalPedido) / cliente.valorLimiteRequisicao * 100) > 60 ? 'warning' : 'success'"
                            :model-value="valorTotalPedido" :max="cliente.valorLimiteRequisicao" :height="7"
                            title="Valor Máximo Pedido" class="mt-2">
                        </v-progress-linear>
                        <span style="font-size:12px;" class="text-grey">
                            {{ formatToCurrencyBRL(valorTotalPedido) }} / {{
                                formatToCurrencyBRL(cliente.valorLimiteRequisicao.toFixed(2)) }}
                        </span>
                    </v-col>
                </v-row>

            </v-col>
        </v-row>
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <small class="text-error" v-show="!hasProduto">Adicione pelo menos 1 produto</small>
        <v-table class="elevation-2">
            <thead>
                <tr>
                    <th class="text-center text-grey">CÓDIGO</th>
                    <th class="text-center text-grey">PRODUTO (U.M)</th>
                    <th class="text-center text-grey">VALOR</th>
                    <th class="text-center text-grey">TX.GESTÃO</th>
                    <th class="text-center text-grey">IPI (%)</th>
                    <th class="text-center text-grey">VL. PRODUTO</th>
                    <th class="text-center text-grey">QUANT.</th>
                    <th class="text-center text-grey">TOTAL</th>
                    <th class="text-center text-grey">REMOVER</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in requisicao.requisicaoItens" :key="item.id">
                    <td class="text-left">{{ item.codigo }}</td>
                    <td class="text-left">{{ item.nome + ` (${item.unidadeMedida})` }}</td>
                    <td class="text-right">{{ formatToCurrencyBRL(item.valor.toFixed(2)) }}</td>
                    <td class="text-right">{{ formatToCurrencyBRL(item.taxaGestao.toFixed(2)) }}</td>
                    <td class="text-right">{{ item.percentualIPI.toFixed(2) }}</td>
                    <td class="text-right">{{ formatToCurrencyBRL(retornarValorTotalProduto(item)) }}</td>
                    <td class="text-left">
                        <v-text-field density="compact" variant="outlined" type="number" color="primary"
                            :hide-details="true" class="sm ml-12" v-model="item.quantidade" min=0
                            @change="adicionarRemoverProduto(item)">
                        </v-text-field>

                    </td>
                    <td class="text-right">
                        {{ formatToCurrencyBRL((retornarValorTotalProduto(item) * (isNaN(item.quantidade) ? 0 :
                            item.quantidade)).toFixed(2)) }}
                    </td>
                    <td class="text-center">
                        <v-btn icon="mdi-package-variant-minus" variant="plain" color="red" title="Remover Produto"
                            @click="removeProdutoRequisicao(item)"></v-btn>
                    </td>
                </tr>

                <tr v-for="item in produtos" :key="item.id" class="text-grey">
                    <td class="text-left">{{ item.codigo }}</td>
                    <td class="text-left">{{ item.nome + ` (${item.unidadeMedida})` }}</td>
                    <td class="text-right">{{ formatToCurrencyBRL(item.valor.toFixed(2)) }}</td>
                    <td class="text-right">{{ formatToCurrencyBRL(item.taxaGestao.toFixed(2)) }}</td>
                    <td class="text-right">{{ item.percentualIPI.toFixed(2) }}</td>
                    <td class="text-right">{{ formatToCurrencyBRL(retornarValorTotalProduto(item)) }}</td>
                    <td class="text-left">
                        <v-text-field density="compact" variant="outlined" type="number" color="primary"
                            :hide-details="true" class="sm ml-12" v-model="item.quantidade" min="0"
                            @change="adicionarRemoverProduto(item)">
                        </v-text-field>
                    </td>
                    <td class="text-right">
                        {{ formatToCurrencyBRL((retornarValorTotalProduto(item) * (isNaN(item.quantidade) ? 0 :
                            item.quantidade)).toFixed(2)) }}
                    </td>
                    <td class="text-center">
                        <!-- <v-btn icon="mdi-package-variant-plus" variant="plain" color="success"
                            :disabled="(isNaN(item.quantidade) ? 0 : item.quantidade) == 0"
                            @click="adicionarProdutoRequisicao(item)" title="Incluir Produto"></v-btn> -->
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr style="font-weight:600;">
                    <td colspan="2" class="text-right">SUBTOTAL:</td>
                    <td class="text-right">{{ formatToCurrencyBRL((requisicao.valorTotal - requisicao.valorIcms).toFixed(2))
                    }}</td>
                    <td class="text-right">ICMS:</td>
                    <td class="text-right">{{ formatToCurrencyBRL(requisicao.valorIcms) }}</td>
                    <td colspan="2" class="text-right">TOTAL PEDIDO:</td>
                    <td class="text-right">{{ formatToCurrencyBRL(valorTotalPedido) }}</td>
                    <td></td>
                </tr>
            </tfoot>
        </v-table>

        <v-row class="mt-10">
            <v-col cols="2" class="text-right" v-for="config in orcamento" :key="config.tipoFornecimentoId">
                <span style="font-size:12px;" class="text-grey text-left">{{
                    getTipoFornecimentoNome(config.tipoFornecimentoId) }}</span>
                <v-progress-linear :color="config.percentual > 100 ? 'red' : config.percentual > 60 ? 'warning' : 'success'"
                    :model-value="config.valorTotal" :max="config.valorPedido * (1 + config.tolerancia / 100)" :height="7"
                    :title="getTipoFornecimentoNome(config.tipoFornecimentoId)"></v-progress-linear>
                <span style="font-size:12px;" class="text-grey">{{ config.valorTotal.toFixed(2) }} /
                    {{ (config.valorPedido * (1 + config.tolerancia / 100)).toFixed(2) }}</span>
            </v-col>
        </v-row>
        <v-dialog v-model="openPeriodoForm" max-width="700" :absolute="false" persistent>
            <v-form ref="produtoForm" @submit.prevent="">
                <v-card>
                    <v-card-title class="text-primary text-h5 text-left mb-2 mt-2">
                        Confirmar período de Entrega
                    </v-card-title>
                    <v-card-text>
                        <v-row>
                            <v-col>
                                <periodo v-model:dia-selecionado="requisicao.periodoEntrega" />
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col class="text-right">
                                <v-btn color="primary" @click="openPeriodoForm = false">Confirmar</v-btn>
                            </v-col>
                        </v-row>
                    </v-card-text>
                </v-card>
            </v-form>
        </v-dialog>
    </div>
</template>
  
<script setup>
import { ref, onMounted, watch, inject, computed } from "vue";
import requisicaoService from "@/services/requisicao.service";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import { useAuthStore } from "@/store/auth.store";
import fornecedorService from "@/services/fornecedor.service";
import { compararValor, retornarValorTotalProduto, diasDaSemana, formatToCurrencyBRL } from "@/helpers/functions"
import router from "@/router";
import tipoFornecimentoService from "@/services/tipofornecimento.service";
import clienteService from "@/services/cliente.service";
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import Periodo from '@/components/Periodo.vue';
import authService from "@/services/auth.service";
//DATA
const authStore = useAuthStore();
const isBusy = ref(false);
const requisicao = ref({
    filialId: authStore.user.filial,
    clienteId: null,
    fornecedorId: null,
    valorTotal: 0,
    taxaGestao: 0,
    destino: 0,
    UsuarioId: null,
    NomeUsuario: null,
    requisicaoItens: [],
    requerAutorizacaoWCA: false,
    requerAutorizacaoCliente: false,
    icms: 0,
    valorIcms: 0,
    periodoEntrega: {
        0: { periodo: [], selected: false },
        1: { periodo: [], selected: false },
        2: { periodo: [], selected: false },
        3: { periodo: [], selected: false },
        4: { periodo: [], selected: false },
        5: { periodo: [], selected: false },
        6: { periodo: [], selected: false }
    },
});
const clientes = ref([]);
const fornecedores = ref([]);
const destinos = ref([
    { value: 0, text: "Outros" },
    { value: 1, text: "Diretoria" },
])
const produtos = ref([]);
const hasProduto = ref(true);
const swal = inject("$swal");
const filter = ref("");
let orcamento = ref(null);
let tipoFornecimento = ref([])
const formCadastro = ref(null);
const openPeriodoForm = ref(false)
const cliente = ref({
    id: null,
    valorLimiteRequisicao: 0,
    naoUltrapassarLimitePorRequisicao: false
})

//VUE METHODS
onMounted(async () => {

    requisicao.value.NomeUsuario = authStore.user.nome;
    requisicao.value.UsuarioId = authStore.user.id;
    await getClienteListByUser()
    await getFornecedorToList(authStore.user.filial)
    await getTipoFornecimentoToList();
});

watch(() => requisicao.value.fornecedorId, async (fornecedorId) => {
    requisicao.value.requisicaoItens = []
    await getProdutosToList(fornecedorId)
    requisicao.value.icms = fornecedores.value.filter(c => c.id == fornecedorId)[0].icms

    let { value: icms } = await swal.fire({
        title: 'Confirmar o percentual do ICMS:',
        input: 'number',
        inputValue: requisicao.value.icms,
        confirmButtonText: 'SIM',
        showCancelButton: false,
        inputValidator: (value) => {
            if (!value || value < 0) {
                return 'Você deve informar um valor maior ou igual a 0'
            }
        }
    })
    requisicao.value.icms = icms;
})

watch(() => requisicao.value.clienteId, (clienteId) => {
    cliente.value = clientes.value.filter(c => c.id == clienteId)[0];

    orcamento.value = cliente.value.clienteOrcamentoConfiguracao.filter(c => c.ativo == true);
    for (let idx = 0; idx < orcamento.value.length; idx++) {
        orcamento.value[idx].valorTotal = 0
        orcamento.value[idx].percentual = 0
    }
    requisicao.value.localEntrega = `${cliente.value.endereco}, ${cliente.value.numero} - ${cliente.value.cep} - ${cliente.value.cidade} / ${cliente.value.uf}`
    requisicao.value.periodoEntrega = JSON.parse(cliente.value.periodoEntrega)
    openPeriodoForm.value = true
})

watch(filter, async () => {
    if (requisicao.value.fornecedorId != null && requisicao.value.fornecedorId > 0) {
        await getProdutosToList(requisicao.value.fornecedorId)
    }
})

const valorTotalPedido = computed(() => {
    if (requisicao.value.requisicaoItens.length == 0) return 0;

    let produtos = requisicao.value.requisicaoItens;
    let valorTotal = 0;
    let valorTaxaGestao = 0;

    produtos.forEach(produto => {
        valorTotal += produto.quantidade * parseFloat(retornarValorTotalProduto(produto));
        valorTaxaGestao += produto.quantidade * produto.taxaGestao
    })
    valorTotal = valorTotal.toFixed(2)
    requisicao.value.valorIcms = (parseFloat(valorTotal) * requisicao.value.icms / 100).toFixed(2)
    requisicao.value.valorTotal = parseFloat(valorTotal) + parseFloat(requisicao.value.valorIcms)
    requisicao.value.taxaGestao = parseFloat(valorTaxaGestao.toFixed(2))
    return requisicao.value.valorTotal.toFixed(2);
})

//METHODS

function adicionarProdutoRequisicao(item) {
    let index = requisicao.value.requisicaoItens.findIndex(p => p.id == item.id)
    if (index == -1) {
        let produto = { ...item }
        requisicao.value.requisicaoItens.push(produto)

        if (!hasProduto.value)
            hasProduto.value = true;

        produtoRemoveFromList(item)
        ordenarRequisicaoItens()
    }
    calcularOrcamentoTotais()
}

function adicionarRemoverProduto(item) {
    if (item.quantidade > 0) {
        adicionarProdutoRequisicao(item)
    } else {
        removeProdutoRequisicao(item)
    }

}
function calcularOrcamentoTotais() {
    clearOrcamentoTotais()
    for (let idx = 0; idx < requisicao.value.requisicaoItens.length; idx++) {
        let item = requisicao.value.requisicaoItens[idx]
        let index = orcamento.value.findIndex(o => o.tipoFornecimentoId == item.tipoFornecimentoId);
        if (index != -1) {
            orcamento.value[index].valorTotal += item.quantidade * parseFloat(retornarValorTotalProduto(item));
            orcamento.value[index].percentual = (orcamento.value[index].valorTotal / (orcamento.value[index].valorPedido * (1 + orcamento.value[index].tolerancia / 100))) * 100
        }
    }
}
function clearOrcamentoTotais() {
    for (let idx = 0; idx < orcamento.value.length; idx++)
        orcamento.value[idx].valorTotal = 0;
}

async function getClienteListByUser() {
    try {
        let response = await clienteService.getListByAuthenticatedUser();
        clientes.value = response.data;
    } catch (error) {
        console.log("getUsuarioToList.error:", error);
        handleErrors(error)
    }
}

async function getFornecedorToList(filial) {
    try {
        let response = await fornecedorService.toList(filial);
        fornecedores.value = response.data;
    } catch (error) {
        console.log("getUsuarioToList.error:", error);
        handleErrors(error)
    }
}

async function getProdutosToList(fornecedorId) {
    try {
        isBusy.value = true;
        let response = await fornecedorService.produtoPaginate(fornecedorId, 99999, 1, filter.value, true);
        produtos.value = response.data.items;

        for (let index = 0; index < requisicao.value.requisicaoItens.length; index++) {
            let item = requisicao.value.requisicaoItens[index];
            produtoRemoveFromList(item)
        }
    } catch (error) {
        console.log("getUsuarioToList.error:", error);
        handleErrors(error)
    } finally {
        isBusy.value = false
    }
}

function getTipoFornecimentoNome(tipoId) {
    let tipo = tipoFornecimento.value.filter(t => t.value == tipoId)
    if (tipo.length > 0) {
        return tipo[0].text
    }
    return ""
}

async function getTipoFornecimentoToList() {
    try {
        isBusy.value = true;
        let response = await tipoFornecimentoService.toList();
        tipoFornecimento.value = response.data;
    } catch (error) {
        console.log("getTipoFornecimentoToList.error:", error);
        handleErrors(error)
    } finally { isBusy.value = false; }
}

function ordenarRequisicaoItens() {
    if (requisicao.value.requisicaoItens.length > 0)
        requisicao.value.requisicaoItens.sort(compararValor("nome"))
}

function produtoRemoveFromList(item) {
    let index = produtos.value.findIndex(p => p.id == item.id)
    if (index != -1)
        produtos.value.splice(index, 1)
}

async function removeProdutoRequisicao(item) {
    let index = requisicao.value.requisicaoItens.findIndex(p => p.id == item.id)
    if (index != -1) {
        requisicao.value.requisicaoItens.splice(index, 1)
        await getProdutosToList(requisicao.value.fornecedorId)
        ordenarRequisicaoItens()
    }
    calcularOrcamentoTotais()
}

async function salvar() {
    try {
        isBusy.value = true;
        let { valid } = await formCadastro.value.validate();
        hasProduto.value = requisicao.value.requisicaoItens.length > 0

        if (valid && hasProduto.value) {
            let data = { ...requisicao.value }
            //remover o campo id da requisicaoItens
            data.requisicaoItens.forEach(produto => {
                delete produto.id
                produto.valorTotal = retornarValorTotalProduto(produto) * produto.quantidade
            })
            data.periodoEntrega = JSON.stringify(data.periodoEntrega);

            // verificar se requer autorização
            orcamento.value.forEach(o => {
                if (o.percentual > 100) {
                    if (o.aprovadoPor == 1)
                        data.requerAutorizacaoCliente = true;
                    else
                        data.requerAutorizacaoWCA = true;
                }
            })
            // verificar se ultrassou o limite estabelecido para o cliente
            if (cliente.value.naoUltrapassarLimitePorRequisicao &&
                parseFloat(cliente.value.valorLimiteRequisicao) < parseFloat(valorTotalPedido.value)) {
                data.requerAutorizacaoWCA = true
            }

            if (data.requerAutorizacaoWCA) {
                if (!authStore.hasPermissao("aprova_requisicao")) {
                    let result = await swal.fire({
                        title: 'Autorização',
                        html: `<input type="email" id="login" class="swal2-input" placeholder="E-mail">
                       <input type="password" id="password" class="swal2-input" placeholder="Senha">`,
                        confirmButtonText: 'Autorizar',
                        focusConfirm: false,
                        preConfirm: async () => {
                            const login = swal.getPopup().querySelector('#login').value
                            const password = swal.getPopup().querySelector('#password').value
                            let emailRegex = new RegExp(/^[A-Za-z0-9_!#$%&'*+\/=?`{|}~^.-]+@[A-Za-z0-9.-]+$/, "gm");

                            if (!login || !password) {
                                swal.showValidationMessage(`Por favor informe o e-mail e senha`)
                            } else if (!emailRegex.test(login)) {
                                swal.showValidationMessage(`Por favor informe um e-mail válido`)
                            } else {
                                let response = await authService.login({ email: login, password: password })
                                if (!response.data.authenticated) {
                                    swal.showValidationMessage(`Usuário e/ou senha inválido!`)
                                } else if (response.data.perfil.permissao.filter(c => c.regra == 'aprova_requisicao').length == 0) {
                                    swal.showValidationMessage(`Usuário não tem permissão para realizar aprovação!`)
                                }
                                return response.data
                            }
                        }
                    })
                    if (!result.isConfirmed) {
                        return
                    }
                    data.requerAutorizacaoWCA = false
                    data.usuarioAutorizador = result.value.usuarioNome
                } else {
                    data.requerAutorizacaoWCA = false
                    data.usuarioAutorizador = authStore.user.nome
                }
            }

            //não sei o que fazer quando tiver autorização cliente, vou manter como antes, envio de e-mail

            await requisicaoService.create(data)
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
    } catch (error) {
        console.log("salvar.error:", error);
        handleErrors(error)
    } finally {
        isBusy.value = false
    }
}

</script>

<style scoped>
table .v-text-field {
    width: 80px;
}
</style>