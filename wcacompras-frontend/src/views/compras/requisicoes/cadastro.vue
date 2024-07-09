<template>
  <div>
  <v-app-bar elevation="1" :height="orcamento!=null ? 84: 0" >
    <v-row style="margin: 1px 0 1px 0;">
      <v-col v-show="cliente.naoUltrapassarLimitePorRequisicao">
        <span style="font-size: 11px" class="text-grey text-left">
          Valor Máximo Pedido
        </span>
        <v-progress-linear
          :color="
            (parseFloat(valorTotalPedido) / cliente.valorLimiteRequisicao) *  100 > 100
              ? 'red'
              : (parseFloat(valorTotalPedido) /
                  cliente.valorLimiteRequisicao) *
                  100 >
                60
              ? 'warning'
              : 'success'
          "
          :model-value="valorTotalPedido"
          :max="cliente.valorLimiteRequisicao"
          :height="7"
          title="Valor Máximo Pedido"
        >
        </v-progress-linear>
        <span style="font-size: 11px" class="text-grey">
          {{ formatToCurrencyBRL(valorTotalPedido) }} /
          {{
            formatToCurrencyBRL(
              cliente.valorLimiteRequisicao.toFixed(2)
            )
          }}
        </span>
      </v-col>
      <v-col v-for="config in orcamento" :key="config.tipoFornecimentoId">
        <span style="font-size: 11px" class="text-grey text-left">{{
          config.nome
        }}</span>
        <v-progress-linear
          :color="
            config.percentual > 100
              ? 'red'
              : config.percentual > 60
              ? 'warning'
              : 'success'
          "
          :model-value="config.valorTotal"
          :max="config.valorPedido * (1 + config.tolerancia / 100)+1"
          :height="7"
          :title="config.nome"
        ></v-progress-linear>
        <span style="font-size: 11px" class="text-grey"
          >{{ formatToCurrencyBRL(config.valorTotal) }} /
          {{
            formatToCurrencyBRL(config.valorPedido * (1 + config.tolerancia / 100))
          }}</span
        >
      </v-col>
      <v-col>
        <span style="font-size: 11px" class="text-grey text-left">Valor Pedido Sem Taxas / Valor Pedido Minímo / Valor Frete</span><br/>
        <v-progress-linear
          :color="TaxaGestaoMenosFreteColor"
          :model-value="valorTotalPedidoSemTaxa"
          :max="fornecedor.valorCompraMinimoSemFrete"
          :height="7"
          title="Valor Pedido Sem Taxas / Valor Pedido Minímo / Valor Frete"
        ></v-progress-linear>
        <span style="font-size: 11px" class="text-grey">
          {{ formatToCurrencyBRL(valorTotalPedidoSemTaxa) }} /
          {{ formatToCurrencyBRL(fornecedor.valorCompraMinimoSemFrete) }} /
          {{ formatToCurrencyBRL(valorTotalPedidoSemTaxa < fornecedor.valorCompraMinimoSemFrete ? fornecedor.valorFrete: 0) }}
        </span>
      </v-col>
    </v-row>
  </v-app-bar>
  <v-app-bar :height="orcamento!=null ? 45: 0">
    <v-row>  
      <v-col>
        <v-table >
          <tbody>
              <tr>
                <td>Total Pedido: {{ formatToCurrencyBRL(valorTotalPedido) }}</td>
                <td>Total S/Taxas: {{ formatToCurrencyBRL(valorTotalPedidoSemTaxa) }}</td>
                <td>Tx. Gestão :{{ formatToCurrencyBRL(requisicao.taxaGestao) }}</td>
                <td>Frete: {{ formatToCurrencyBRL(requisicao.valorFrete) }}</td>
                <td>Tx. Gestão - Frete: <span :class="'text-' + TaxaGestaoMenosFreteColor">{{ formatToCurrencyBRL(TaxaGestaoMenosFrete) }}</span></td>
                <td>Tx. Gestão Miníma: {{ formatToCurrencyBRL(TaxaGestaoMinima) }}</td>
              </tr>
          </tbody>
    </v-table>
      </v-col>
    </v-row>
  </v-app-bar>
  <bread-crumbs
      title="Nova Requisição"
      :show-button="false"
      :custom-button-show="true"
      custom-button-text="Salvar"
      :custom-button-disabled="isBusy"
      @customClick="salvar()"
    />
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isBusy"
      class="mt-1 mb-2"
    >
    </v-progress-linear>
    
    <v-row >
      <v-col>
        <v-form ref="formCadastro">
          <v-row>
            <v-col>
              <v-select label="Filiais" v-model="filterFilial" :items="filiais" density="compact"
                item-title="text" item-value="value" variant="outlined" color="primary"
                :hide-details="true"
                clearable>
              </v-select>
            </v-col>
            <v-col>
              <v-select
                label="Clientes"
                v-model="requisicao.clienteId"
                :items="clientes"
                density="compact"
                item-title="nome"
                item-value="id"
                variant="outlined"
                color="primary"
                :hide-details="false"
                :rules="[(v) => !!v || 'Campo obrigatório']"
                :disabled="filterFilial == null"
              ></v-select>
            </v-col>
            <v-col>
              <v-select
                label="Fornecedor"
                v-model="requisicao.fornecedorId"
                :items="fornecedores"
                density="compact"
                item-title="text"
                item-value="value"
                variant="outlined"
                color="primary"
                :hide-details="false"
                :rules="[(v) => !!v || 'Campo obrigatório']"
                :disabled="requisicao.clienteId == null"
              ></v-select>
            </v-col>
            <v-col cols="2">
              <v-select
                label="Destino"
                v-model="requisicao.destino"
                :items="destinos"
                density="compact"
                item-title="text"
                item-value="value"
                variant="outlined"
                color="primary"
                :hide-details="false"
              ></v-select>
            </v-col>
          </v-row>
          <v-row>
            <v-col
              :cols="
                authStore.hasPermissao('requisicao_local_entrega') ? 9 : 12
              "
            >
              <v-text-field
                label="Local Entrega"
                :model-value="localEntrega"
                placeholder="Local de entrega do Pedido"
                density="compact"
                variant="outlined"
                color="primary"
                :rules="[(v) => !!v || 'Campo obrigatório']"
                disabled
              >
              </v-text-field>
            </v-col>

            <v-col v-show="authStore.hasPermissao('requisicao_local_entrega')">
              <v-btn
                color="primary"
                @click="openEnderecoForm = true"
                :disabled="requisicao.clienteId == null"
                >Alterar Local</v-btn
              >
            </v-col>
          </v-row>
          <v-row v-show="requisicao.clienteId > 0">
            <v-col cols="9" class="text-left">
              <div>
                <span class="text-grey"
                  >Entregas:
                  <small
                    >Legenda: (M - Manhã, T - Tarde, N - Noite)</small
                  ></span
                >
                <br />
                <span
                  v-for="(dia, index) in diasDaSemana"
                  :key="dia"
                  :class="index > 0 ? 'text-primary ml-2' : 'text-primary'"
                  v-show="
                    requisicao.periodoEntrega[dia.value].selected &&
                    requisicao.periodoEntrega[dia.value].periodo.length > 0
                  "
                >
                  {{ dia.text }}:&nbsp;
                  <v-avatar
                    color="info"
                    size="x-small"
                    v-for="periodo in requisicao.periodoEntrega[dia.value]
                      .periodo"
                    >{{ periodo.slice(0, 1) }}</v-avatar
                  >
                </span>
              </div>
            </v-col>
            <v-col>
              <v-btn color="primary" @click="openPeriodoForm = true"
                >Alterar Período</v-btn
              >
            </v-col>
          </v-row>
        </v-form>
        <v-divider class="mt-5 mb-2"></v-divider>
        <v-row>
          <v-col cols="5">
            <v-text-field
              label="Pesquisar Produto"
              v-model="filter"
              placeholder="Nome ou Código"
              density="compact"
              variant="outlined"
              color="info"
            >
            </v-text-field>
          </v-col>
          <v-col cols="1"></v-col>
          <v-col v-show="cliente.naoUltrapassarLimitePorRequisicao">
            <v-row>
              <v-col class="text-right text-grey"
                >Valor limite por pedido:
              </v-col>
              <v-col>
                <v-progress-linear
                  :color="
                    (parseFloat(valorTotalPedido) /
                      cliente.valorLimiteRequisicao) *
                      100 >
                    100
                      ? 'red'
                      : (parseFloat(valorTotalPedido) /
                          cliente.valorLimiteRequisicao) *
                          100 >
                        60
                      ? 'warning'
                      : 'success'
                  "
                  :model-value="valorTotalPedido"
                  :max="cliente.valorLimiteRequisicao"
                  :height="7"
                  title="Valor Máximo Pedido"
                  class="mt-2"
                >
                </v-progress-linear>
                <span style="font-size: 12px" class="text-grey">
                  {{ formatToCurrencyBRL(valorTotalPedido) }} /
                  {{
                    formatToCurrencyBRL(
                      cliente.valorLimiteRequisicao.toFixed(2)
                    )
                  }}
                </span>
              </v-col>
            </v-row>
          </v-col>
        </v-row>
        <v-progress-linear
          color="primary"
          indeterminate
          :height="5"
          v-show="isBusy"
        ></v-progress-linear>
        <small class="text-error" v-show="!hasProduto"
          >Adicione pelo menos 1 produto</small
        >
        <v-table class="elevation-2">
          <thead>
            <tr>
              <th class="text-center text-grey">CÓDIGO</th>
              <th class="text-center text-grey">PRODUTO (U.M)</th>
              <th class="text-center text-grey">VALOR</th>
              <th class="text-center text-grey">TX.GESTÃO</th>
              <th class="text-center text-grey">IPI (%)</th>
              <th class="text-center text-grey">ICMS (%)</th>
              <th class="text-center text-grey">VL. PRODUTO</th>
              <th class="text-center text-grey">QUANT.</th>
              <th class="text-center text-grey">TOTAL</th>
              <th class="text-center text-grey">
                <v-icon icon="mdi-package-variant-minus"></v-icon>
              </th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in requisicao.requisicaoItens" :key="item.id">
              <td class="text-left">{{ item.codigo }}</td>
              <td class="text-left">
                {{ item.nome + ` (${item.unidadeMedida})` }}
              </td>
              <td class="text-right">
                {{ formatToCurrencyBRL(item.valor.toFixed(2)) }}
              </td>
              <td class="text-right">
                {{ formatToCurrencyBRL(item.taxaGestao.toFixed(2)) }}
              </td>
              <td class="text-right">{{ item.percentualIPI.toFixed(2) }}%</td>
              <td class="text-right">{{ item.icms.toFixed(2) }}%</td>
              <td class="text-right">
                {{ formatToCurrencyBRL(retornarValorTotalProduto(item)) }}
              </td>
              <td class="text-left">
                <v-text-field
                  density="compact"
                  variant="outlined"
                  type="number"
                  color="primary"
                  :hide-details="true"
                  class="sm ml-12"
                  v-model="item.quantidade"
                  min="0"
                  @change="adicionarRemoverProduto(item)"
                >
                </v-text-field>
              </td>
              <td class="text-right">
                {{
                  formatToCurrencyBRL(
                    (
                      retornarValorTotalProduto(item) *
                      (isNaN(item.quantidade) ? 0 : item.quantidade)
                    ).toFixed(2)
                  )
                }}
              </td>
              <td class="text-center">
                <v-btn
                  icon="mdi-package-variant-minus"
                  variant="plain"
                  color="red"
                  title="Remover Produto"
                  @click="removeProdutoRequisicao(item)"
                ></v-btn>
              </td>
            </tr>

            <tr v-for="item in produtos" :key="item.id" class="text-grey">
              <td class="text-left">{{ item.codigo }}</td>
              <td class="text-left">
                {{ item.nome + ` (${item.unidadeMedida})` }}
              </td>
              <td class="text-right">
                {{ formatToCurrencyBRL(item.valor.toFixed(2)) }}
              </td>
              <td class="text-right">
                {{ formatToCurrencyBRL(item.taxaGestao.toFixed(2)) }}
              </td>
              <td class="text-right">{{ item.percentualIPI.toFixed(2) }}%</td>
              <td class="text-right">{{ item.icms.toFixed(2) }}%</td>
              <td class="text-right">
                {{ formatToCurrencyBRL(retornarValorTotalProduto(item)) }}
              </td>
              <td class="text-left">
                <v-text-field
                  density="compact"
                  variant="outlined"
                  type="number"
                  color="primary"
                  :hide-details="true"
                  class="sm ml-12"
                  v-model="item.quantidade"
                  min="0"
                  @change="adicionarRemoverProduto(item)"
                >
                </v-text-field>
              </td>
              <td class="text-right">
                {{
                  formatToCurrencyBRL(
                    (
                      retornarValorTotalProduto(item) *
                      (isNaN(item.quantidade) ? 0 : item.quantidade)
                    ).toFixed(2)
                  )
                }}
              </td>
              <td class="text-center">
                <!-- <v-btn icon="mdi-package-variant-plus" variant="plain" color="success"
                            :disabled="(isNaN(item.quantidade) ? 0 : item.quantidade) == 0"
                            @click="adicionarProdutoRequisicao(item)" title="Incluir Produto"></v-btn> -->
              </td>
            </tr>
          </tbody>
          <tfoot>
            <tr style="font-weight: 600">
              <!-- <td colspan="3" class="text-right">SUBTOTAL:</td>
                    <td class="text-right">{{ formatToCurrencyBRL((requisicao.valorTotal - requisicao.valorIcms).toFixed(2))
                    }}</td>
                    <td class="text-right">ICMS:</td>
                    <td class="text-right">{{ formatToCurrencyBRL(requisicao.valorIcms) }}</td>
                    -->
              <td colspan="8" class="text-right">TOTAL ITENS:</td>
              <td class="text-right">
                {{ formatToCurrencyBRL(valorTotalPedido - PedidoValorFrete) }}
              </td>
              <td></td>
            </tr>
          </tfoot>
        </v-table>
      </v-col>
    </v-row>

    <v-dialog
      v-model="openPeriodoForm"
      max-width="700"
      :absolute="false"
      persistent
    >
      <v-form>
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
                <v-btn color="primary" @click="openPeriodoForm = false"
                  >Confirmar</v-btn
                >
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-form>
    </v-dialog>
    <v-dialog
      v-model="openEnderecoForm"
      max-width="900"
      :absolute="false"
      persistent
    >
      <v-form>
        <v-card>
          <v-card-title class="text-primary text-h5 text-left mb-2 mt-2">
            Alterar Local de Entrega
          </v-card-title>
          <v-card-text>
            <v-row>
              <v-col>
                <endereco v-model:data="requisicao" />
              </v-col>
            </v-row>
            <v-row>
              <v-col class="text-right">
                <v-btn color="primary" @click="openEnderecoForm = false"
                  >Confirmar</v-btn
                >
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
import handleErrors from "@/helpers/HandleErrors";
import BreadCrumbs from "@/components/breadcrumbs.vue";
import { useAuthStore } from "@/store/auth.store";
import fornecedorService from "@/services/fornecedor.service";
import {
  compararValor,
  retornarValorTotalProduto,
  diasDaSemana,
  formatToCurrencyBRL,
} from "@/helpers/functions";
import router from "@/router";
import tipoFornecimentoService from "@/services/tipofornecimento.service";
import clienteService from "@/services/cliente.service";
import Periodo from "@/components/Periodo.vue";
import endereco from "@/components/endereco.vue";
import filialService from "@/services/filial.service";

//DATA
const authStore = useAuthStore();
const isBusy = ref(false);
const requisicao = ref({
  filialId: authStore.user.filial,
  clienteId: null,
  fornecedorId: null,
  valorTotal: 0,
  taxaGestao: 0,
  valorFrete: 0,
  destino: 0,
  UsuarioId: null,
  NomeUsuario: null,
  requisicaoItens: [],
  requerAutorizacaoWCA: false,
  requerAutorizacaoCliente: false,
  valorIcms: 0,
  periodoEntrega: {
    0: { periodo: [], selected: false },
    1: { periodo: [], selected: false },
    2: { periodo: [], selected: false },
    3: { periodo: [], selected: false },
    4: { periodo: [], selected: false },
    5: { periodo: [], selected: false },
    6: { periodo: [], selected: false },
  },
  endereco: "",
  numero: "",
  cep: "",
  cidade: "",
  uf: "",
});
const clientes = ref([]);
const filterFilial = ref(null);
const fornecedores = ref([]);
const fornecedor = ref({
  text: "",
  value: 0,
  valorCompraMinimoSemFrete: 0,
  valorFrete : 0,
  taxaGestaoMinimaPercentual: 0
})
const destinos = ref([
  { value: 0, text: "Outros" },
  { value: 1, text: "Diretoria" },
]);
const produtos = ref([]);
const filiais = ref([])
const hasProduto = ref(true);
const swal = inject("$swal");
const filter = ref("");
let orcamento = ref(null);
let tipoFornecimento = ref([]);
const formCadastro = ref(null);
const openPeriodoForm = ref(false);
const openEnderecoForm = ref(false);
const cliente = ref({
  id: null,
  valorLimiteRequisicao: 0,
  naoUltrapassarLimitePorRequisicao: false,
});

//VUE METHODS
onMounted(async () => {
  requisicao.value.NomeUsuario = authStore.user.nome;
  requisicao.value.UsuarioId = authStore.user.id;
  
  await getFiliaisByUser();
  await getClienteListByUser();
  await getTipoFornecimentoToList();
});

watch(() => filterFilial.value, async (novoValor) => {
  clientes.value = []
  requisicao.value.clienteId = null
  requisicao.value.fornecedorId = null
  clearFornecedor()
    
  await getClienteListByUser(novoValor ? [novoValor]: [])
  
  
})

watch(
  () => requisicao.value.fornecedorId,
  async (fornecedorId) => {
    requisicao.value.requisicaoItens = [];
    produtos.value = []
    clearFornecedor()
    if (fornecedorId)  
    {
      fornecedor.value = fornecedores.value.find(p => p.value == fornecedorId);
      await getProdutosToList(fornecedorId);
    }
  }
);

watch(
  () => requisicao.value.clienteId,
  async (clienteId) => {

    cliente.value = clienteId ?  clientes.value.find((c) => c.id == clienteId): {
      id: null,
      valorLimiteRequisicao: 0,
      naoUltrapassarLimitePorRequisicao: false,
    };
    fornecedores.value = []
    requisicao.value.endereco = ""
    requisicao.value.numero =""
    requisicao.value.cep = ""
    requisicao.value.cidade = ""
    requisicao.value.uf = ""
    requisicao.value.filialId = null
    orcamento.value = null
    clearFornecedor()
    if (cliente.value.id) {
        await getFornecedorToList([cliente.value.filialId])
        
        requisicao.value.endereco = cliente.value.endereco;
        requisicao.value.numero = cliente.value.numero;
        requisicao.value.cep = cliente.value.cep;
        requisicao.value.cidade = cliente.value.cidade;
        requisicao.value.uf = cliente.value.uf;
        requisicao.value.filialId = cliente.value.filialId
        if (typeof cliente.value.periodoEntrega === Object)
          requisicao.value.periodoEntrega = JSON.parse(cliente.value.periodoEntrega);

        openPeriodoForm.value = true;
    }
  }
);

watch(filter, async () => {
  if (
    requisicao.value.fornecedorId != null &&
    requisicao.value.fornecedorId > 0
  ) {
    await getProdutosToList(requisicao.value.fornecedorId);
  }
});

watch(
  () => requisicao.value.uf,
  async (uf, oldUF) => {
    if (
      oldUF != uf &&
      requisicao.value.fornecedorId != null &&
      uf.length == 2
    ) {
      await getProdutosToList(requisicao.value.fornecedorId);
    }
  }
);

const localEntrega = computed(() => {
  let localEntrega = "";
  if (requisicao.value.endereco.trim() != "")
    localEntrega = `${requisicao.value.endereco}, ${requisicao.value.numero} - ${requisicao.value.cep} - ${requisicao.value.cidade} / ${requisicao.value.uf}`;

  return localEntrega;
});

const PedidoValorFrete = computed(() =>  {
  return valorTotalPedidoSemTaxa.value < fornecedor.value.valorCompraMinimoSemFrete ? fornecedor.value.valorFrete: 0
})

const TaxaGestaoMinima = computed(() =>  {
  let _taxaGestaoMinima = 0;
  _taxaGestaoMinima = requisicao.value.taxaGestao * (fornecedor.value.taxaGestaoMinimaPercentual /100)
  return parseFloat(_taxaGestaoMinima.toFixed(2))
})

const TaxaGestaoMenosFrete = computed(() =>  {
  let _valortaxaGestaoFinal = requisicao.value.taxaGestao - PedidoValorFrete.value
  return parseFloat(_valortaxaGestaoFinal.toFixed(2))
})

const TaxaGestaoMenosFreteColor = computed(() =>  {
  let _valortaxaGestaoFinal = requisicao.value.taxaGestao - PedidoValorFrete.value
  if (_valortaxaGestaoFinal < 0) 
    return 'red'
  else if (_valortaxaGestaoFinal < TaxaGestaoMinima.value)
    return 'orange'
  else 
    return 'green'
})

const valorTotalPedido = computed(() => {
  if (requisicao.value.requisicaoItens.length == 0) return 0;

  let produtos = requisicao.value.requisicaoItens;
  let valorTotal = 0;
  let valorTaxaGestao = 0;
  let valorIcms = 0;

  produtos.forEach((produto) => {
    let produtoValor = produto.quantidade * parseFloat(retornarValorTotalProduto(produto))
    let produtoTaxaGestao = produto.quantidade * parseFloat(produto.taxaGestao);
    let produtoIcms = produto.quantidade * parseFloat(((produto.valor * produto.icms) / 100).toFixed(2));
    valorTotal += produtoValor - produtoTaxaGestao
    valorTaxaGestao += produtoTaxaGestao
    valorIcms += produtoIcms;
  });
  valorTotal = parseFloat(valorTotal.toFixed(2))
             + parseFloat(PedidoValorFrete.value.toFixed(2))
             + parseFloat(valorTaxaGestao.toFixed(2));
  requisicao.value.valorIcms = parseFloat(valorIcms.toFixed(2));
  requisicao.value.valorTotal = parseFloat(valorTotal);
  requisicao.value.taxaGestao = parseFloat(valorTaxaGestao.toFixed(2));
  requisicao.value.valorFrete = parseFloat(PedidoValorFrete.value.toFixed(2));

  return requisicao.value.valorTotal.toFixed(2);
});

const valorTotalPedidoSemTaxa = computed(() => {
  if (requisicao.value.requisicaoItens.length == 0) return 0;

  let produtos = requisicao.value.requisicaoItens;
  let valorTotal = 0;
  
  produtos.forEach((produto) => {
    valorTotal +=
      produto.quantidade * parseFloat(produto.valor.toFixed(2));
  });
  valorTotal = valorTotal.toFixed(2);
  
  return valorTotal
});



//METHODS
function clearFornecedor () {
 fornecedor.value = {
    text: "",
    value: 0,
    valorCompraMinimoSemFrete: 0,
    valorFrete : 0,
    taxaGestaoMinimaPercentual: 0
  }
}

function adicionarProdutoRequisicao(item) {
  let index = requisicao.value.requisicaoItens.findIndex(
    (p) => p.id == item.id
  );
  if (index == -1) {
    let produto = { ...item };
    requisicao.value.requisicaoItens.push(produto);

    if (!hasProduto.value) hasProduto.value = true;

    produtoRemoveFromList(item);
    ordenarRequisicaoItens();
  }
  calcularOrcamentoTotais();
}

function adicionarRemoverProduto(item) {
  if (item.quantidade > 0) {
    adicionarProdutoRequisicao(item);
  } else {
    removeProdutoRequisicao(item);
  }
  
}

function calcularOrcamentoTotais() {
  clearOrcamentoTotais();
  for (let idx = 0; idx < requisicao.value.requisicaoItens.length; idx++) {
    let item = requisicao.value.requisicaoItens[idx];
    let index = orcamento.value.findIndex(
      (o) => o.tipoFornecimentoId == item.tipoFornecimentoId
    );
    if (index != -1) {
      orcamento.value[index].valorTotal +=
        item.quantidade * parseFloat(retornarValorTotalProduto(item));
      orcamento.value[index].percentual =
        (orcamento.value[index].valorTotal /
          (orcamento.value[index].valorPedido *
            (1 + orcamento.value[index].tolerancia / 100))) *
        100;
    }
  }
}
function clearOrcamentoTotais() {
  for (let idx = 0; idx < orcamento.value.length; idx++)
    orcamento.value[idx].valorTotal = 0;
}

async function getClienteListByUser(filial = []) {
  try {
      let  response = await clienteService.getListByAuthenticatedUser(filial.length > 0? filial : null);
      clientes.value = response.data;
  } catch (error) {
    console.log("getClienteListByUser.error:", error);
    handleErrors(error);
  }
}

async function getFiliaisByUser() {
    try
    {
        isBusy.value = true;
        let response = await filialService.getListByAuthenticatedUser()
        filiais.value = response.data;
        
    } catch (error)
    {
        console.log("requisicao.getFiliaisByUser.error:", error.response);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}

async function getFornecedorToList(filial=[]) {
  try {
      if (filial.length == 0)
          filial = filiais.value.map(p => {return p.value })

        let response = await fornecedorService.toList(filial);
        fornecedores.value = response.data;
  } catch (error) {
    console.log("getUsuarioToList.error:", error);
    handleErrors(error);
  }
}

async function getProdutosToList(fornecedorId) {
  try {
    isBusy.value = true;
    let response = await fornecedorService.produtoListWithIcms(
      fornecedorId,
      requisicao.value.uf,
      filter.value
    );
    produtos.value = response.data;

    carregarConfiguracaoCliente()
    for (
      let index = 0;
      index < requisicao.value.requisicaoItens.length;
      index++
    ) {
      let item = requisicao.value.requisicaoItens[index];
      let produto = produtos.value.filter((c) => c.codigo == item.codigo)[0];
      if (produto != undefined)
        requisicao.value.requisicaoItens[index].icms = produto.icms;
      produtoRemoveFromList(item);
    }
  } catch (error) {
    console.log("getUsuarioToList.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}

function getTipoFornecimentoNome(tipoId) {
  let tipo = tipoFornecimento.value.find((t) => t.value == tipoId);
  if (tipo) {
    return tipo.text;
  } 
  return "";
}

function carregarConfiguracaoCliente() {
  let configuracoes = cliente.value
                    .clienteOrcamentoConfiguracao.filter((c) => c.ativo == true);
    for (let idx = 0; idx < configuracoes.length; idx++) {
      configuracoes[idx].nome = getTipoFornecimentoNome(configuracoes[idx].tipoFornecimentoId)
      configuracoes[idx].valorTotal = 0;
      configuracoes[idx].percentual = 0;
    }

    let categorias = Array.from(new Set(produtos.value.map((item) => item.tipoFornecimentoId)))

    orcamento.value =  configuracoes.filter((item) => categorias.includes(item.tipoFornecimentoId))

}

async function getTipoFornecimentoToList() {
  try {
    isBusy.value = true;
    let response = await tipoFornecimentoService.toList();
    tipoFornecimento.value = response.data;
  } catch (error) {
    console.log("getTipoFornecimentoToList.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}

function ordenarRequisicaoItens() {
  if (requisicao.value.requisicaoItens.length > 0)
    requisicao.value.requisicaoItens.sort(compararValor("nome"));
}

function produtoRemoveFromList(item) {
  let index = produtos.value.findIndex((p) => p.id == item.id);
  if (index != -1) produtos.value.splice(index, 1);
}

async function removeProdutoRequisicao(item) {
  let index = requisicao.value.requisicaoItens.findIndex(
    (p) => p.id == item.id
  );
  if (index != -1) {
    requisicao.value.requisicaoItens.splice(index, 1);
    await getProdutosToList(requisicao.value.fornecedorId);
    ordenarRequisicaoItens();
  }
  calcularOrcamentoTotais();
}

async function salvar() {
  try {
    isBusy.value = true;
    let { valid } = await formCadastro.value.validate();
    hasProduto.value = requisicao.value.requisicaoItens.length > 0;

    //validar se taxa de gestão é maior q 0, isto é impeditivo
    if (TaxaGestaoMenosFrete.value < 0) {
      valid = false;
      swal.fire({
            icon: "error",
            title: "Atenção",
            text: "Pedido com taxa de gestão negativa, favor revisar o pedido!",
          })
    }

    if (valid && hasProduto.value) {
      let data = { ...requisicao.value };
      //remover o campo id da requisicaoItens
      data.requisicaoItens.forEach((produto) => {
        delete produto.id;
        produto.valorTotal =
          retornarValorTotalProduto(produto) * produto.quantidade;
      });
      data.periodoEntrega = JSON.stringify(data.periodoEntrega);

      // verificar se requer autorização
      orcamento.value.forEach((o) => {
        if (o.percentual > 100) {
          if (o.aprovadoPor == 1) data.requerAutorizacaoCliente = true;
          else data.requerAutorizacaoWCA = true;
        }
      });
      // verificar se ultrassou o limite estabelecido para o cliente
      if (
        cliente.value.naoUltrapassarLimitePorRequisicao &&
        parseFloat(cliente.value.valorLimiteRequisicao) < parseFloat(valorTotalPedido.value)
      ) {
        data.requerAutorizacaoWCA = true;
      }

      if (TaxaGestaoMenosFreteColor.value == 'orange')
        data.requerAutorizacaoWCA = true; 
      
      if (data.requerAutorizacaoWCA || data.requerAutorizacaoCliente) {
        if (!authStore.hasPermissao("aprova_requisicao")) {
          let result = await swal.fire({
            icon: "warning",
            title: "Atenção",
            text: "O pedido não atendeu todos os requisitos, o administrador e/ou cliente deve aprovar para dar continuidade a solicitação!",
            confirmButtonText: "Estou ciente",
            focusConfirm: false,
            cancelButtonText: "Cancelar",
            showCancelButton: true,
          });
          if (!result.isConfirmed) {
            return;
          }
        } else {
          data.requerAutorizacaoWCA = false;
          data.usuarioAutorizador = authStore.user.nome;
        }
      }

      await requisicaoService.create(data);
      swal.fire({
        toast: true,
        icon: "success",
        index: "top-end",
        title: "Sucesso!",
        text: "Dados salvos com sucesso!",
        showConfirmButton: false,
        timer: 2000,
      });
      router.push({ name: "requisicoes" });
    }
  } catch (error) {
    console.log("salvar.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}
</script>

<style scoped>
.breadcrumb-fixed {
  position: fixed;
  top: 63px;
  height: auto;
  width: 82%;
  background-color: #fff;
  color: #fff;
  left: auto;
  z-index: 1;
}
.fixed-column-scroll {
  position: fixed;
  /* left: auto; */
  width: 80%;
  background-color: #fff;
  color: #fff;
  padding: 5px;
  /* max-height: 530px; */
  /* overflow-y: scroll;
  overflow-x: hidden; */
  z-index: 2;
}

table .v-text-field {
  width: 80px;
}
</style>
