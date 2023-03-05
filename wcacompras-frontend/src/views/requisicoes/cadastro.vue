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
                        item-title="text" item-value="value" variant="outlined" color="primary" :hide-details="false"
                        :rules="[(v) => !!v || 'Campo obrigatório']"></v-select>
                </v-col>
                <v-col cols="2">
                    <v-select label="Destino" v-model="requisicao.destino" :items="destinos" density="compact"
                        item-title="text" item-value="value" variant="outlined" color="primary"
                        :hide-details="false"></v-select>
                </v-col>
            </v-row>
            <v-row>
            <v-col cols="10">
                <v-text-field label="Local Entrega" v-model="requisicao.localEntrega" placeholder="Local de entrega do Pedido" density="compact"
                    variant="outlined" color="primary" :rules="[(v) => !!v || 'Campo obrigatório']" counter="300">
                </v-text-field>
            </v-col>

            <v-col cols="2">
                <v-text-field-money label-text="ICMS (%)" v-model="requisicao.icms" color="primary"
                    :number-decimal="2"></v-text-field-money>
            </v-col>
            
        </v-row>
        </v-form>
        <v-row>
            <v-col cols="5">
                <v-text-field label="Pesquisar Produto" v-model="filter" placeholder="Nome ou Código" density="compact"
                    variant="outlined" color="info">
                </v-text-field>
            </v-col>
            <v-col cols="1"></v-col>
            
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
                    <td class="text-right">{{ item.valor.toFixed(2) }}</td>
                    <td class="text-right">{{ item.taxaGestao.toFixed(2) }}</td>
                    <td class="text-right">{{ item.percentualIPI.toFixed(2) }}</td>
                    <td class="text-right">{{ retornarValorTotalProduto(item) }}</td>
                    <td class="text-left">
                        <v-text-field density="compact" variant="outlined" type="number" color="primary"
                            :hide-details="true" class="ml-12" v-model="item.quantidade" min="0"
                            @change="adicionarRemoverProduto(item)">
                        </v-text-field>
                    </td>
                    <td class="text-right">
                        {{ (retornarValorTotalProduto(item) * (isNaN(item.quantidade) ? 0 : item.quantidade)).toFixed(2) }}
                    </td>
                    <td class="text-center">
                        <v-btn icon="mdi-package-variant-minus" variant="plain" color="red" title="Remover Produto"
                            @click="removeProdutoRequisicao(item)"></v-btn>
                    </td>
                </tr>

                <tr v-for="item in produtos" :key="item.id" class="text-grey">
                    <td class="text-left">{{ item.codigo }}</td>
                    <td class="text-left">{{ item.nome + ` (${item.unidadeMedida})` }}</td>
                    <td class="text-right">{{ item.valor.toFixed(2) }}</td>
                    <td class="text-right">{{ item.taxaGestao.toFixed(2) }}</td>
                    <td class="text-right">{{ item.percentualIPI.toFixed(2) }}</td>
                    <td class="text-right">{{ retornarValorTotalProduto(item) }}</td>
                    <td class="text-left">
                        <v-text-field density="compact" variant="outlined" type="number" color="primary"
                            :hide-details="true" class="sm ml-12" v-model="item.quantidade" min="0"
                            @change="adicionarRemoverProduto(item)">
                        </v-text-field>
                    </td>
                    <td class="text-right">
                        {{ (retornarValorTotalProduto(item) * (isNaN(item.quantidade) ? 0 : item.quantidade)).toFixed(2) }}
                    </td>
                    <td class="text-center">
                        <!-- <v-btn icon="mdi-package-variant-plus" variant="plain" color="success"
                            :disabled="(isNaN(item.quantidade) ? 0 : item.quantidade) == 0"
                            @click="adicionarProdutoRequisicao(item)" title="Incluir Produto"></v-btn> -->
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="7" class="text-right">TOTAL PEDIDO:</td>
                    <td class="text-right">{{valorTotalPedido}}</td>
                    <td></td>
                </tr>
            </tfoot>
        </v-table>

        <v-row class="mt-10">
            <v-col cols="2" class="text-right" v-for="config in orcamento" :key="config.tipoFornecimentoId">
                <v-progress-linear :color="config.percentual > 100 ? 'red' : config.percentual > 60 ? 'warning' : 'success'"
                    :model-value="config.valorTotal" :max="config.valorPedido * (1 + config.tolerancia / 100)" :height="7"
                    :title="getTipoFornecimentoNome(config.tipoFornecimentoId)"></v-progress-linear>
                <span style="font-size:12px;" class="text-grey">{{ config.valorTotal.toFixed(2) }} /
                    {{ (config.valorPedido * (1 + config.tolerancia / 100)).toFixed(2) }}</span>
            </v-col>
        </v-row>
    </div>
</template>
  
<script setup>
import { ref, onMounted, watch, inject, computed } from "vue";
import requisicaoService from "@/services/requisicao.service";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import { useAuthStore } from "@/store/auth.store";
import fornecedorService from "@/services/fornecedor.service";
import { compararValor, retornarValorTotalProduto } from "@/helpers/functions"
import router from "@/router";
import tipoFornecimentoService from "@/services/tipofornecimento.service";
import clienteService from "@/services/cliente.service";
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";

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
    valorIcms: 0
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

})

watch(() => requisicao.value.clienteId, (clienteId) => {
    let cliente = clientes.value.filter(c => c.id == clienteId)[0];
    
    orcamento.value = cliente.clienteOrcamentoConfiguracao.filter(c =>  c.ativo == true);
    for (let idx = 0; idx < orcamento.value.length; idx++) {
        orcamento.value[idx].valorTotal = 0
        orcamento.value[idx].percentual = 0
    }

    requisicao.value.localEntrega = `${cliente.endereco}, ${cliente.numero} - ${cliente.cep} - ${cliente.cidade} / ${cliente.uf}`


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

    produtos.forEach(produto => {
        valorTotal += produto.quantidade * parseFloat(retornarValorTotalProduto(produto));
    })
    return valorTotal.toFixed(2);
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
        orcamento.value[index].valorTotal += ((parseFloat(item.valor) + parseFloat(item.taxaGestao)) * item.quantidade)
        orcamento.value[index].percentual = (orcamento.value[index].valorTotal / (orcamento.value[index].valorPedido * (1 + orcamento.value[index].tolerancia / 100))) * 100
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
        let response = await fornecedorService.produtoPaginate(fornecedorId, 99999, 1, filter.value);
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

            //remover o campo id da requisicaoItens
            requisicao.value.requisicaoItens.forEach(produto => {
                delete produto.id
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

            orcamento.value.forEach(o => {
                if (o.percentual > 100) {
                    if (o.aprovadoPor == 1)
                        requisicao.value.requerAutorizacaoCliente = true;
                    else
                        requisicao.value.requerAutorizacaoWCA = true;
                }
            })

            await requisicaoService.create(requisicao.value)
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