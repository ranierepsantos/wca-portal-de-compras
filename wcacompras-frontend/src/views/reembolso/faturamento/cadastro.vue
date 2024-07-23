<template>
  <div>
    <bread-crumbs
      title="Faturamento"
      :show-button="false"
      :buttons="formButtons"
      @add-po-click="openAprovacaoForm = true"
      @salvar-click="salvar()"
      @view-po-click="visualizarPO()"
      @finalizar-click="finalizarDialog = true"
    />
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isLoading.form || isLoading.busy"
    ></v-progress-linear>
    <v-container class="justify-center" v-show="!isLoading.form">
      <v-card>
        <v-card-text>
          <v-form ref="oForm" lazy-validation>
            <v-row>
              <v-col>
                <select-text
                  v-model="faturamento.clienteId"
                  :combo-items="clientes"
                  combo-item-title="nome"
                  combo-item-value="id"
                  :select-mode="faturamento.id == 0"
                  :text-field-value="faturamento.clienteNome"
                  label-text="Cliente"
                  :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                ></select-text>
              </v-col>
              <v-col cols="3" v-show="faturamento.id > 0">
                <v-btn
                  :color="faturamentoStore.getStatus(faturamento.status).color"
                  variant="tonal"
                  class="text-center"
                >
                  {{
                    faturamentoStore.getStatus(faturamento.status).status +
                    (faturamento.status == 2 ? ` ${faturamento.numeroPO}` : "") +
                    (faturamento.status == 3 ? ` - NF: ${faturamento.notaFiscal}` : "")
                  }}</v-btn
                >
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <select-text
                  v-model="faturamento.centroCustoId"
                  :combo-items="listCentroCusto"
                  combo-item-title="nome"
                  combo-item-value="id"
                  :select-mode="
                    faturamento.id == 0 && faturamento.clienteId > 0
                  "
                  :text-field-value="faturamento.centroCustoNome"
                  label-text="Centro de Custo"
                  :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                />
              </v-col>
              <v-col cols="3">
                <v-text-field
                  label="Número DS"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="faturamento.numeroDS"
                  :readonly="faturamento.status > 1"
                  :bg-color="faturamento.status > 1 ?'#f2f2f2':''"
                ></v-text-field>
              </v-col>
              
            </v-row>
          </v-form>
        </v-card-text>
      </v-card>
      <!-- FILTROS -->
      <v-card
        style="margin-top: 10px"
        v-show="
          faturamento.id == 0 &&
          faturamento.clienteId > 0 &&
          faturamento.centroCustoId > 0
        "
      >
        <v-card-title>
          <v-breadcrumbs>
            <div class="text-h6 text-primary">Filtros</div>
          </v-breadcrumbs>
        </v-card-title>
        <v-card-text>
          <v-row>
            <v-col>
              <v-select
                label="Usuário"
                :items="usuariosCliente"
                density="compact"
                item-title="text"
                item-value="value"
                variant="outlined"
                color="primary"
                v-model="filter.usuarioId"
              ></v-select>
            </v-col>
            <v-col cols="2">
              <v-text-field
                label="Período Inicial"
                type="date"
                variant="outlined"
                color="primary"
                density="compact"
                v-model="filter.dataIni"
              ></v-text-field>
            </v-col>
            <v-col cols="2">
              <v-text-field
                label="Período Final"
                type="date"
                variant="outlined"
                color="primary"
                density="compact"
                v-model="filter.dataFim"
              ></v-text-field>
            </v-col>
            <v-col class="text-right" cols="2">
              <v-btn
                icon="mdi-filter-check-outline"
                variant="plain"
                color="primary"
                @click="getSolicitacoes()"
                title="Aplicar Filtros"
              ></v-btn>
              &nbsp;
              <v-btn
                icon="mdi-filter-remove-outline"
                variant="plain"
                color="error"
                @click="
                  clearFilters(faturamento.clienteId, faturamento.centroCustoId)
                "
                title="Remover Filtros"
              ></v-btn>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
      <!-- SOLICITAÇÕES DO FATURAMENTO  -->
      <v-card style="margin-top: 20px">
        <v-card-title>
          <v-breadcrumbs>
            <div class="text-h6 text-primary">
              Items {{ faturamento.id > 0 ? "Faturados" : " a Faturar" }}
            </div>
          </v-breadcrumbs>
        </v-card-title>
        <v-card-text v-show="false"> </v-card-text>
        <!-- SOLICITAÇÕES DO FATURAMENTO  -->
        <v-card-text>
          <small class="text-error" v-show="!hasSolicitacoes"
            >Selecione ao menos 1 solicitação</small
          >
          <v-table class="elevation-2">
            <thead>
              <tr>
                <th class="text-center text-grey">#</th>
                <th class="text-center text-grey">DATA</th>
                <th class="text-center text-grey">COLABORADOR</th>
                <th class="text-center text-grey">CENTRO DE CUSTO</th>
                <th class="text-center text-grey">VALOR</th>
                <th class="text-center text-grey" v-if="faturamento.id == 0">
                  <div>
                    <v-btn
                      variant="plain"
                      :color="selecionarTodosItens ? 'primary' : 'secondary'"
                      :icon="
                        selecionarTodosItens
                          ? 'mdi-radiobox-marked'
                          : 'mdi-radiobox-blank'
                      "
                      @click="selecionarTodos()"
                    ></v-btn>
                    <span @click="selecionarTodos()">SELECIONAR TODOS</span>
                  </div>
                </th>
                <th v-else></th>
              </tr>
            </thead>
            <tbody>
              <template
                v-for="(item, index) in faturamento.faturamentoItem.sort(
                  compararValor('id')
                )"
                :key="index"
                v-if="faturamento.id > 0"
              >
                <tr>
                  <td class="text-center">{{ item.solicitacao.id }}</td>
                  <td class="text-center">
                    {{
                      moment(item.solicitacao.dataSolicitacao).format(
                        "DD/MM/YYYY"
                      )
                    }}
                  </td>
                  <td class="text-left">
                    {{ item.solicitacao.colaboradorNome }}
                  </td>
                  <td class="text-left">
                    {{ getNomeCentroCusto(item.solicitacao.centroCustoId) }}
                  </td>
                  <td class="text-right">
                    {{
                      formatToCurrencyBRL(
                        parseFloat(item.solicitacao.valorDespesa)
                      )
                    }}
                  </td>
                  <td>
                    <v-btn
                      icon="mdi-text-box-search-outline"
                      size="smaller"
                      variant="plain"
                      color="primary"
                      @click="visualizarSolicitacao(item.solicitacao.id)"
                      title="Visualizar"
                    ></v-btn>
                  </td>
                </tr>
              </template>
              <template
                v-for="item in items.sort(compararValor('id'))"
                :key="item.id"
                v-else
              >
                <tr>
                  <td class="text-center">{{ item.id }}</td>
                  <td class="text-center">
                    {{ moment(item.dataSolicitacao).format("DD/MM/YYYY") }}
                  </td>
                  <td class="text-left">{{ item.colaboradorNome }}</td>
                  <td class="text-left">{{ item.centroCustoNome }}</td>
                  <td class="text-right">
                    {{ formatToCurrencyBRL(parseFloat(item.valorDespesa)) }}
                  </td>
                  <td class="text-center">
                    <v-btn
                      variant="plain"
                      :color="item.selected ? 'primary' : 'secondary'"
                      :title="item.selected ? 'Desmarcar' : 'Marcar'"
                      :icon="
                        item.selected
                          ? 'mdi-radiobox-marked'
                          : 'mdi-radiobox-blank'
                      "
                      @click="checkUncheckItem(item)"
                    >
                    </v-btn>
                  </td>
                </tr>
              </template>
            </tbody>
            <tfoot>
              <tr>
                <td class="text-right" colspan="4"><b>TOTAL:</b></td>
                <td class="text-right">
                  {{ formatToCurrencyBRL(valorFaturamento) }}
                </td>
                <td></td>
              </tr>
            </tfoot>
          </v-table>
        </v-card-text>

        <!-- FATURAMENTO - HISTÓRICO -->
        <historico :eventos="faturamento.faturamentoHistorico" />
      </v-card>
    </v-container>
    <!-- FORM PARA INFORMAR P.O -->
    <v-dialog
      v-model="openAprovacaoForm"
      max-width="700"
      :absolute="false"
      persistent
    >
      <v-form>
        <v-card>
          <v-row>
            <v-col>
              <v-card-title class="text-primary text-h5 text-left mb-2 mt-2">
                INFORMAR P.O.
              </v-card-title>
            </v-col>
            <v-col class="text-right">
              <v-btn
                class="mb-2 mt-3 mr-2"
                variant="plain"
                color="grey"
                @click="openAprovacaoForm = false"
                icon="mdi-close-circle-outline"
              ></v-btn>
            </v-col>
          </v-row>
          <v-card-text>
            <v-row>
              <v-col>
                <v-text-field
                  label="Número P.O"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="poFormData.numero"
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-file-input
                  accept=".pdf,.doc,.docx"
                  label="Informe o arquivo de P.O"
                  variant="outlined"
                  density="compact"
                  @change="handleFile"
                ></v-file-input>
              </v-col>
            </v-row>
            <v-row class="text-center text-red" v-show="isInvalidPOData">
              <v-col> Número do P.O e/ou Documento devem ser informados </v-col>
            </v-row>
            <v-row>
              <v-col class="text-right">
                <v-progress-circular
                  color="primary"
                  indeterminate
                  v-show="isRunningEvent"
                ></v-progress-circular>
                <v-btn
                  color="success"
                  @click="enviarPO()"
                  :disabled="isRunningEvent"
                  >Enviar</v-btn
                >
                &nbsp;
                <v-btn
                  color="gray"
                  class="mr-3"
                  style="font-weight: bold"
                  @click="openAprovacaoForm = false"
                  :disabled="isRunningEvent"
                  >Cancelar</v-btn
                >
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-form>
    </v-dialog>
    <!-- FORM PARA FINALIZAR FATURAMENTO -->
    <v-dialog
      v-model="finalizarDialog"
      max-width="700"
      max-height="700"
      :absolute="false"
      persistent
    >
      <v-card>
        <v-card-title class="text-primary text-h5"> FINALIZAR </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="finalizar()" ref="finalizarForm">
            <v-row>
              <v-col>
                <v-text-field
                  label="Número Nota Fiscal"
                  v-model="finalizarData.notaFiscal"
                  type="text"
                  required
                  variant="outlined"
                  color="primary"
                  :rules="[(v) => !!v || 'Número Nota Fiscal é obrigatório']"
                  density="compact"
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-text-field
                  label="Data"
                  v-model="finalizarData.dataFinalizacao"
                  type="date"
                  required
                  variant="outlined"
                  color="primary"
                  :rules="[(v) => !!v || 'Data de finalização é obrigatória']"
                  density="compact"
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col class="text-right">
                <v-btn
                  variant="outlined"
                  color="primary"
                  @click="finalizarDialog = false"
                  >Cancelar</v-btn
                >
                <v-btn color="primary" type="submit" class="ml-3"
                  >Finalizar</v-btn
                >
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
import breadCrumbs from "@/components/breadcrumbs.vue";
import { ref, inject, computed, watch, onMounted } from "vue";
import { useAuthStore } from "@/store/auth.store";
import {
  formatToCurrencyBRL,
  compararValor,
  toBase64,
} from "@/helpers/functions";
import { useClienteStore } from "@/store/reembolso/cliente.store";
import { useSolicitacaoStore } from "@/store/reembolso/solicitacao.store";
import { useUsuarioStore } from "@/store/reembolso/usuario.store";
import {
  Faturamento,
  FaturamentoItem,
  useFaturamentoStore,
} from "@/store/reembolso/faturamento.store";
import moment from "moment";
import { useRoute } from "vue-router";
import router from "@/router";
import handleErrors from "@/helpers/HandleErrors";
import historico from "@/components/reembolso/historico.vue";
import selectText from "@/components/selectText.vue";

const openAprovacaoForm = ref(false);
const authStore = useAuthStore();
const solicitacaoStore = useSolicitacaoStore();
const clienteStore = useClienteStore();
const usuarioStore = useUsuarioStore();
const faturamentoStore = useFaturamentoStore();
const faturamento = ref(new Faturamento());
const finalizarForm = ref(null);
const finalizarDialog = ref(false);
const finalizarData = ref({
  notaFiscal: null,
  dataFinalizacao: moment().format("YYYY-MM-DD"),
});
const isRunningEvent = ref(false);
const clientes = ref([]);
const items = ref([]);
const route = useRoute();
const swal = inject("$swal");
const selecionarTodosItens = ref(false);
const formButtons = ref([]);
const page = 1;
const pageSize = 99999;
const filter = ref({
  filialId: null,
  clienteId: null,
  usuarioId: null,
  status: 7,
  dataIni: null,
  dataFim: null,
  centroCustoId: null,
});
const usuarios = ref([]);
const usuariosCliente = ref([]);
const poFormData = ref({
  numero: "",
  documento: null,
});
const listCentroCusto = ref([]);
const isInvalidPOData = ref(false);
const isLoading = ref({
  form: true,
  busy: false,
});
const oForm = ref(null);
const hasSolicitacoes = ref(true);

//VUE - FUNCTIONS
onMounted(async () => {
  try {
    isLoading.value.form = true;
    solicitacaoStore;
    clientes.value = await clienteStore.ListByUsuario(authStore.user.id);
    usuarios.value = await usuarioStore.toComboList();

    if (parseInt(route.query.id) > 0) {
      await getById(route.query.id);
      
      let _centros = await clienteStore.ListCentrosDeCusto([faturamento.value.clienteId])
      listCentroCusto.value = _centros.map(c =>  ({id: c.value, nome: c.text}))

      if (
        faturamento.value.status == 1 &&
        authStore.hasPermissao("cliente_faturamento")
      )
        formButtons.value.push({
          text: "Informar P.O",
          icon: "",
          event: "add-po-click",
        });
      if (faturamento.value.status == 2) {
        if (faturamento.value.documentoPO.trim() != "") {
          formButtons.value.push({
            text: "Visualizar P.O",
            icon: "mdi-file-download-outline",
            event: "view-po-click",
          });
        }
        if (authStore.hasPermissao("faturamento")) {
          formButtons.value.push({
            text: "Finalizar",
            icon: "mdi-receipt-text-check",
            event: "finalizar-click",
          });
        }
      }
    } else {
      faturamento.value.usuarioId = authStore.user.id;
    }

    if (faturamento.value.id == 0)
      formButtons.value.push({
        text: "Salvar",
        icon: "",
        event: "salvar-click",
      });
  } catch (error) {
    handleErrors(error);
  } finally {
    isLoading.value.form = false;
  }
});

watch(
  () => faturamento.value.clienteId,
  async (newValue, oldValue) => {
    if (oldValue && newValue != oldValue)
      faturamento.value.faturamentoItem = [];

    listCentroCusto.value = await usuarioStore.getCentrosdeCusto(
      authStore.user.id,
      faturamento.value.clienteId
    );
  }
);

watch(
  () => faturamento.value.centroCustoId,
  async (newValue, oldValue) => {
    if (oldValue && newValue != oldValue)
      faturamento.value.faturamentoItem = [];

    usuariosCliente.value = await usuarioStore.getListUsuarioByCentroDeCusto(
      faturamento.value.centroCustoId
    );
    await clearFilters(
      faturamento.value.clienteId,
      faturamento.value.centroCustoId
    );
  }
);

const valorFaturamento = computed(() => {
  faturamento.value.valor = 0;
  faturamento.value.faturamentoItem.forEach((item) => {
    faturamento.value.valor += parseFloat(item.solicitacao.valorDespesa);
  });
  return faturamento.value.valor;
});

//FUNCTIONS
async function enviarPO() {
  try {
    isRunningEvent.value = true;

    isInvalidPOData.value = false;
    if (
      poFormData.value.numero.trim() == "" &&
      (poFormData.value.documento == null || poFormData.value.documento == "")
    ) {
      isInvalidPOData.value = true;
      return;
    }

    let data = {
      id: faturamento.value.id,
      numeroPO: poFormData.value.numero,
      documentoPO: poFormData.value.documento,
    };

    let status = faturamentoStore.getStatus(2);
    data.notificar = await getUsuarioToNotificar(status);
    data.status = status;

    await faturamentoStore.addPO(data);

    swal.fire({
      toast: true,
      icon: "success",
      index: "top-end",
      title: "Sucesso!",
      text: "P.O enviado com sucesso!",
      showConfirmButton: false,
      timer: 4000,
    });
    router.push({ name: "reembolsoFaturamento" });
  } catch (error) {
    console.log("enviarPO.error:", error);
    handleErrors(error);
  } finally {
    isRunningEvent.value = false;
  }
}

function checkUncheckItem(item) {
  item.selected = !item.selected;
  addRemove(item);
  selecionarTodosItens.value =
    faturamento.value.faturamentoItem.length == items.value.length;
}

function addRemove(solicitacao) {
  let index = faturamento.value.faturamentoItem.findIndex(
    (q) => q.solicitacao.id == solicitacao.id
  );

  if (solicitacao.selected) {
    if (index == -1) {
      let item = new FaturamentoItem();
      item.solicitacaoId = solicitacao.id;
      item.faturamentoId = faturamento.value.id;
      item.solicitacao = { ...solicitacao };
      faturamento.value.adicionarAlterarItem(item);
      hasSolicitacoes.value = true;
    }
  } else {
    if (index != -1)
      faturamento.value.removerItem(faturamento.value.faturamentoItem[index]);
  }
}

async function clearFilters(idCliente, idCentroCusto) {
  filter.value = {
    filialId: null,
    clienteId: idCliente,
    usuarioId: null,
    status: 7,
    dataIni: null,
    dataFim: null,
    centroCustoId: idCentroCusto,
  };
  await getSolicitacoes();
}

async function getById(faturamentoId) {
  try {
    //isBusy.value = true;
    let data = await faturamentoStore.getById(faturamentoId);
    faturamento.value = data;
  } catch (error) {
    console.log("getById.error:", error);
    handleErrors(error);
  } finally {
    //isBusy.value = false;
  }
}

async function getSolicitacoes() {
  try {
    isLoading.value.busy = true;
    if (
      (filter.value.dataIni && !filter.value.dataFim) ||
      (filter.value.dataFim && !filter.value.dataIni)
    )
      throw new TypeError("Ambas as datas devem ser informadas!");
    else if (moment(filter.value.dataFim) < moment(filter.value.dataIni))
      throw new TypeError("A data fim deve ser maior que a data início!");

    let filtro = { ...filter.value };
    filtro.centroCustoIds = [filter.value.centroCustoId];

    items.value = (
      await solicitacaoStore.getPaginate(page, pageSize, filtro)
    ).items;
  } catch (error) {
    console.error("getSolicitacoes.error:", error);
    handleErrors(error);
  } finally {
    isLoading.value.busy = false;
  }
}

async function salvar() {
  try {
    isLoading.value.busy = true;
    const { valid } = await oForm.value.validate();
    let data = { ...faturamento.value };
    hasSolicitacoes.value = data.faturamentoItem.length > 0;

    if (valid && hasSolicitacoes.value) {
      let status = faturamentoStore.getStatus(1);
      data.notificar = await getUsuarioToNotificar(status);
      data.status = status;

      await faturamentoStore.add(data);

      swal.fire({
        toast: true,
        icon: "success",
        position: "top-end",
        title: "Sucesso!",
        text: "Dados salvos com sucesso!",
        showConfirmButton: false,
        timer: 2000,
      });
      router.push({ name: "reembolsoFaturamento" });
    }
  } catch (error) {
    console.log("salvar.error:", error);
    handleErrors(error);
  } finally {
    isLoading.value.busy = false;
  }
}

function selecionarTodos() {
  selecionarTodosItens.value = !selecionarTodosItens.value;
  for (let idx = 0; idx < items.value.length; idx++) {
    let item = items.value[idx];
    if (
      item.selected == undefined ||
      item.selected != selecionarTodosItens.value
    ) {
      item.selected = selecionarTodosItens.value;
      addRemove(item);
    }
  }
}

function getNomeCentroCusto(id) {
  let data = listCentroCusto.value.find((q) => q.id == id);
  if (data) return data.nome;
  return "";
}

function getNomeUsuario(id) {
  let colaborador = usuarios.value.find((q) => q.value == id);
  if (colaborador) return colaborador.text;
  return "";
}

function visualizarSolicitacao(id) {
  let rota = router.resolve({
    name: "reembolsoSolicitacaoCadastro",
    query: { id: id },
  });

  window.open(rota.href, "_blank");
}

async function handleFile(event) {
  let file = event.target.files[0];
  var arquivo = await toBase64(file);

  poFormData.value.documento = arquivo;
}

function visualizarPO() {
  window.open(faturamento.value.documentoPO, "_blank");
}

async function finalizar() {
  try {
    isRunningEvent.value = true;
    let data = {
      id: faturamento.value.id,
      dataFinalizacao: finalizarData.value.dataFinalizacao,
      notaFiscal: finalizarData.value.notaFiscal,
      nomeUsuario: authStore.user.nome,
    };
    await faturamentoStore.finalizar(data);

    swal.fire({
      toast: true,
      icon: "success",
      position: "top-end",
      title: "Sucesso!",
      text: "Finalizado com sucesso!",
      showConfirmButton: false,
      timer: 2000,
    });
    router.push({ name: "reembolsoFaturamento" });
  } catch (error) {
    console.log("finalizar.error:", error);
    handleErrors(error);
  } finally {
    isRunningEvent.value = false;
  }
}

async function getUsuarioToNotificar(status) {
  let notificar = [];

  if (status.notifica == 2) {
    notificar = await useUsuarioStore().getUsuarioToNotificacaoByCentroDeCusto(faturamento.value.centroCustoId, "cliente_faturamento")
  } else if (status.notifica == 1) {
    notificar.push(faturamento.value.usuarioId);
  }
  return notificar;
}
</script>
