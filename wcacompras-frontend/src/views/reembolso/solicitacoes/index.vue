<template>
  <div>
    <bread-crumbs title="Solicitações" :show-button="false" 
     :buttons="formButtons"
     @novo-click="editar('novo')" 
     @report-click="gerarRelatorio"
    />
    <v-row v-show="!isLoading.form">
      <v-col v-show="isMatriz && !isGestor && !isColaborador">
        <v-select
          label="Filiais"
          v-model="filter.filialId"
          :items="filiais"
          density="compact"
          item-title="text"
          item-value="value"
          variant="outlined"
          color="primary"
          :hide-details="true"
        ></v-select>
      </v-col>
      <v-col v-show="!isGestor && !isColaborador">
        <v-select
          label="Clientes"
          v-model="filter.clienteId"
          :items="clientes"
          density="compact"
          item-title="text"
          item-value="value"
          variant="outlined"
          color="primary"
          :hide-details="true"
        ></v-select>
      </v-col>
      <v-col v-show="!isGestor && !isColaborador && isAprovador">
        <v-select
          label="Usuário"
          v-model="filter.usuarioId"
          :items="usuarios"
          density="compact"
          item-title="text"
          item-value="value"
          variant="outlined"
          color="primary"
          :hide-details="true"
        ></v-select>
      </v-col>
    </v-row>
    <v-row v-show="!isLoading.form">
      <v-col cols="3">
        <v-select
          label="Status"
          v-model="filter.status"
          :items="solicitacaoStore.statusSolicitacao"
          density="compact"
          item-title="status"
          item-value="id"
          variant="outlined"
          color="primary"
          :hide-details="true"
          multiple
      ></v-select>
      </v-col>
      <v-col cols="2">
        <v-text-field
          label="Data Início"
          v-model="filter.dataIni"
          type="date"
          variant="outlined"
          color="primary"
          density="compact"
        ></v-text-field>
      </v-col>
      <v-col cols="2">
        <v-text-field
          label="Data Fim"
          v-model="filter.dataFim"
          type="date"
          variant="outlined"
          color="primary"
          density="compact"
        ></v-text-field>
      </v-col>
      <v-col class="text-right">
        <v-btn
          color="primary"
          variant="outlined"
          class="text-capitalize"
          @click="getItems()"
        >
          <b>Aplicar Filtros</b>
        </v-btn>
        &nbsp;
        <v-btn
          color="info"
          variant="outlined"
          class="text-capitalize"
          @click="clearFilters()"
        >
          <b>Limpar Filtros</b>
        </v-btn>
      </v-col>
    </v-row>
    <br />
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isLoading.busy || isLoading.form"
    ></v-progress-linear>
    <v-table class="elevation-2" v-show="!isLoading.form">
      <thead>
        <tr>
          <th class="text-center text-grey">#</th>
          <th class="text-center text-grey">DATA</th>
          <th class="text-left text-grey">CLIENTE</th>
          <th class="text-left text-grey">COLABORADOR</th>
          <th class="text-center text-grey">VALOR ADIANTAMENTO</th>
          <th class="text-center text-grey">VALOR DESPESA</th>
          <th class="text-left text-grey">TIPO SOLICITAÇÃO</th>
          <th class="text-left text-grey">PERÍODO</th>
          <th class="text-left text-grey">STATUS</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in solicitacoes" :key="item.id">
          <th class="text-center">{{ item.id }}</th>
          <td class="text-center">
            {{ moment(item.dataSolicitacao).format("DD/MM/YYYY") }}
          </td>
          <td class="text-left">{{ item.clienteNome }}</td>
          <td class="text-left">{{ item.colaboradorNome }}</td>
          
          <td class="text-right">
            {{ formatToCurrencyBRL(item.valorAdiantamento) }}
          </td>
          <td class="text-right">
            {{ formatToCurrencyBRL(item.valorDespesa) }}
          </td>
          <td class="text-left">
            {{ solicitacaoStore.getTipoSolicitacao(item.tipoSolicitacao).text }}
          </td>
          <td class="text-left">{{ item.dataMenorDespesa ? `${moment(item.dataMenorDespesa).format("DD/MM/YY")} à ${moment(item.dataMaiorDespesa).format("DD/MM/YY")}` : "" }}</td>
          <td class="text-left">
            <v-btn
              :color="solicitacaoStore.getStatus(item.status).color"
              variant="tonal"
              density="compact"
              class="text-center"
              
            >
              {{ solicitacaoStore.getStatus(item.status).status }}</v-btn
            >
          </td>
          <td class="text-right">
            <div class="text-center">
              <v-menu open-on-hover>
                <template v-slot:activator="{ props }">
                  <v-btn
                    icon="mdi-dots-vertical"
                    color="primary"
                    v-bind="props"
                    variant="plain"
                  >
                  </v-btn>
                </template>

                <v-list>
                  <v-list-item v-show="authStore.hasPermissao('solicitacao')">
                    <v-btn
                        prepend-icon="mdi-lead-pencil"
                        variant="plain"
                        color="primary"
                        @click="editar(item.id)"
                        size="small"
                      >Editar</v-btn>
                  </v-list-item>
                  <v-list-item>
                    <v-btn
                        prepend-icon="mdi-history"
                        variant="plain"
                        color="primary"
                        @click="showHistorico(item)"
                        size="small"
                        :disabled="isLoading.busy"
                      >Histórico</v-btn>
                  </v-list-item>
                  <v-list-item v-show="(authStore.hasPermissao('wca_aprovacao') || authStore.hasPermissao('cliente_aprovacao')) && !authStore.hasPermissao('solicitacao')">
                    <v-btn
                        prepend-icon="mdi-text-box-search-outline"
                        variant="plain"
                        color="primary"
                        @click="editar(item.id)"
                        size="small"
                        :disabled="isLoading.busy"
                      >Visualizar</v-btn>
                  </v-list-item>
                </v-list>
              </v-menu>
            </div>
          </td>
        </tr>
      </tbody>
      <tfoot>
        <tr>
          <td colspan="10">
            <v-pagination
              v-model="page"
              :length="totalPages"
              :total-visible="4"
            ></v-pagination>
          </td>
        </tr>
      </tfoot>
    </v-table>
    <!--DIALOG PARA EXIBIR HISTÓRICO -->
    <v-dialog v-model="openHistorico" max-width="900" max-height="500" scrollable>
      <historico
        :eventos="solicitacaoEventos"
        @close-click="openHistorico=false"
      />
    </v-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from "vue";
import handleErrors from "@/helpers/HandleErrors";
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import moment from "moment";
import { useSolicitacaoStore } from "@/store/reembolso/solicitacao.store";
import historico from "@/components/reembolso/historico.vue";
import { formatToCurrencyBRL, realizarDownload } from "@/helpers/functions";
import filialService from "@/services/filial.service";
import {
  IDPERFILCOLABORADOR,
  IDPERFILGESTOR,
  useUsuarioStore,
} from "@/store/reembolso/usuario.store";
import { useAuthStore } from "@/store/auth.store";
import { computed } from "vue";
import { useClienteStore } from "@/store/reembolso/cliente.store";
//DATA
const authStore = useAuthStore();
const page = ref(1);
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const totalPages = ref(1);
const solicitacoes = ref([]);
const clientes = ref([]);
const filiais = ref([]);
const usuarios = ref([]);
const filter = ref({
  filialId: null,
  clienteId: null,
  usuarioId: null,
  status: null,
  dataIni: null,
  dataFim: null,
});
const solicitacaoStore = useSolicitacaoStore();
const solicitacaoEventos = ref([]);
const openHistorico = ref(false);
const isMatriz = ref(false);
const isLoading = ref({
  form: true,
  busy: false,
});
const formButtons = ref([])
const gestorCentrosDeCusto = ref([])

//COMPUTED'S
const isColaborador = computed(() => {
  return authStore.sistema.perfil.id == IDPERFILCOLABORADOR;
});
const isGestor = computed(() => {
  return authStore.sistema.perfil.id == IDPERFILGESTOR;
});

const isAprovador = computed(() => {
  return authStore.hasPermissao("cliente_aprovacao") || authStore.hasPermissao("wca_aprovacao")
});

//WATCH'S
watch(
  () => filter.value.filialId,
  async () => {
    let _filiais = [];
    if (filter.value.filialId != null) _filiais.push(filter.value.filialId);
    filter.value.clienteId = null;
    filter.value.usuarioId = null;
    //filter.value.usuarioId = hasPermissionAprovador.value ? null : authStore.user.id
    await getClientesToList(_filiais[0]);
    await getUsuarioToList(_filiais);
  }
);

//VUE METHODS
onMounted(async () => {
  try {
    isLoading.value.form = true;
    await solicitacaoStore.loadListStatusSolicitacao();
    await getFiliaisToList();
    
    isMatriz.value = authStore.sistema.isMatriz
    authStore.user.filial = authStore.sistema.filial.value;

    gestorCentrosDeCusto.value = []
    if (isGestor.value) 
      gestorCentrosDeCusto.value = (await useUsuarioStore().getCentrosdeCusto(authStore.user.id)) 

    await clearFilters();
    // await getItems();
    if (authStore.hasPermissao("solicitacao_relatorio")) {
      formButtons.value.push({ text: "Gerar relatório", icon: "mdi-microsoft-excel", event: "report-click" });
    }
    if (authStore.hasPermissao("solicitacao")) {
      formButtons.value.push({ text: "Novo", icon: "", event: "novo-click" });
    }
    

  } catch (error) {
    console.error("onMounted.error", error)
  } finally {
    isLoading.value.form = false;
  }
});

watch(page, () => getItems());

//METHODS
async function clearFilters() {
  try {
    isLoading.value.busy =true
    filter.value = {
      filialId: null,
      clienteId: null,
      usuarioId: null,
      status: null,
      dataIni: null,
      dataFim: null,
      centroCustoIds: []
    };
    if (!isMatriz.value) {
      filter.value.filialId = authStore.user.filial;
    }
    await getUsuarioToList(isMatriz.value ? [] : [authStore.user.filialId]);
    // se tiver permissão de visualizar de outros usuario
    if (isColaborador.value ||!isAprovador.value) {
      filter.value.usuarioId = authStore.user.id;
      await getClientesToList(filter.value.filialId, filter.value.usuarioId);
    } else {
      await getClientesToList(filter.value.filialId);
    }
    await getItems();
  } catch (error) {
    console.error(error)

  }finally {
    isLoading.busy =false
  }
}

function editar(id) {
  router.push({ name: "reembolsoSolicitacaoCadastro", query: { id: id } });
}

async function showHistorico(item) {
  solicitacaoEventos.value = item.solicitacaoHistorico;
  openHistorico.value = true;
}

async function getItems() {
  try {
    isLoading.value.busy =true
    if ((filter.value.dataIni && !filter.value.dataFim ) || (filter.value.dataFim && !filter.value.dataIni))
      throw new TypeError("Ambas as datas devem ser informadas!")
    else if (moment(filter.value.dataFim) < moment(filter.value.dataIni)) 
      throw new TypeError("A data fim deve ser maior que a data início!")
    
    if (isGestor.value) {
      filter.value.centroCustoIds = gestorCentrosDeCusto.value.map(x =>  x.id);
    }

    let response = await solicitacaoStore.getPaginate(
      page.value,
      pageSize,
      filter.value
    );
    solicitacoes.value = response.items;
    totalPages.value = response.totalPages;
  } catch (error) {
    console.log("solicitacoes.getItems.error:", error.response);
    handleErrors(error);
  }finally {isLoading.value.busy =false}
}

async function getClientesToList(filialId = 0, usuarioId = 0) {
  try {
    clientes.value = await useClienteStore().toComboList(filialId, usuarioId);
  } catch (error) {
    console.log("solcitacoes.getClientesToList.error:", error.response);
    handleErrors(error);
  }
}

async function getFiliaisToList() {
  try {
    let response = await filialService.toList();
    filiais.value = response.data;
  } catch (error) {
    console.log("solcitacoes.getFiliais.error:", error.response);
    handleErrors(error);
  }
}

async function getUsuarioToList(filiais = []) {
  try {
    usuarios.value = await useUsuarioStore().toComboList(filiais);
  } catch (error) {
    console.log("solcitacoes.getUsuarioToList.error:", error.response);
    handleErrors(error);
  }
}

async function gerarRelatorio() {
  try {
    isLoading.value.busy =true
    if ((filter.value.dataIni && !filter.value.dataFim ) || (filter.value.dataFim && !filter.value.dataIni))
      throw new TypeError("Ambas as datas devem ser informadas!")
    else if (moment(filter.value.dataFim) < moment(filter.value.dataIni)) 
      throw new TypeError("A data fim deve ser maior que a data início!")
      
    let response = await solicitacaoStore.gerarRelatorio(filter.value);
      
    if (response.status == 200) {
          let nomeArquivo = `relatorio_solicitacoes_${moment().format("DDMMYYYY_HHmmSS")}.xlsx`
          await new Promise(r => setTimeout(r, 2000));
          realizarDownload(response, nomeArquivo, response.headers.getContentType());
    }
  } catch (error) {
    console.log("solicitacoes.gerarRelatorio.error:", error.response);
    handleErrors(error);
  }finally {isLoading.value.busy =false}
}

</script>
