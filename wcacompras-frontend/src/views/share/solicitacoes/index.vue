<template>
  <div>
    <bread-crumbs
      :show-button="authStore.hasPermissao(pageTipo.tipo + '-criar')"
      :title="getPageTitle(pageTipo.id)"
      @novoClick="toPage()"
    />
    <v-row v-show="!isLoading.form">
      <v-col>
        <v-autocomplete
          label="Clientes"
          v-model="filter.clienteId"
          :items="clientes"
          density="compact"
          item-title="text"
          item-value="value"
          variant="outlined"
          color="primary"
          :hide-details="true"
        ></v-autocomplete>
      </v-col>
      <v-col>
        <v-autocomplete
          label="Responsável"
          v-model="filter.responsavelId"
          :items="usuarios"
          density="compact"
          item-title="text"
          item-value="value"
          variant="outlined"
          color="primary"
          :hide-details="true"
        ></v-autocomplete>
      </v-col>
      <v-col>
        <v-autocomplete
          label="Funcionário"
          v-model="filter.funcionarioId"
          :items="funcionarios"
          density="compact"
          item-title="text"
          item-value="value"
          variant="outlined"
          color="primary"
          :hide-details="true"
        ></v-autocomplete>
      </v-col>
    </v-row>
    <v-row v-show="!isLoading.form">
      <v-col cols="3">
        <v-select
          label="Status"
          v-model="filter.status"
          :items="solicitacaoStore.statusSolicitacao.sort(compararValor('statusIntermediario'))"
          density="compact"
          item-title="statusIntermediario"
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
      <v-col>
        <v-checkbox v-model="filter.showFinishedStatus" label="Mostrar Concluídos" color="primary"></v-checkbox>
      </v-col>


      <v-col class="text-right">
        <v-btn
          color="primary"
          variant="outlined"
          class="text-capitalize"
          @click="getItems(true)"
        >
          <b>Aplicar Filtros</b>
        </v-btn>
        &nbsp;
        <v-btn
          color="info"
          variant="outlined"
          class="text-capitalize"
          @click="clearFilters(true)"
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
          <th class="text-left text-grey" v-if="pageTipo.id ==2">ASSUNTO</th>
          <th class="text-left text-grey" v-if="pageTipo.id ==5">FUNÇÃO</th>
          <th class="text-left text-grey" v-else>FUNCIONÁRIO</th>
          <th class="text-left text-grey">RESPONSÁVEL</th>
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
          
          <th class="text-left" v-if="pageTipo.id ==2">{{item.comunicado.assuntoNome}}</th>
          <th class="text-left" v-if="pageTipo.id ==5">{{item.vaga.funcaoNome}}</th>
          <td class="text-left" v-else>{{ getFuncNome(item)   }}</td>
          <td class="text-left">{{ item.responsavelNome || "Não atribuído" }}</td>  
          <td class="text-left">
            <v-btn
              :color="item.statusSolicitacao.color"
              variant="tonal"
              density="compact"
              class="text-center"
            >
              {{ item.statusSolicitacao.statusIntermediario }}</v-btn
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
                  <v-list-item>
                    <v-btn
                      prepend-icon="mdi-eye-outline"
                      variant="plain"
                      color="primary"
                      @click="toPage(item.id)"
                      size="small"
                      >Visualizar</v-btn
                    >
                  </v-list-item>
                  <v-list-item>
                    <v-btn
                      prepend-icon="mdi-history"
                      variant="plain"
                      color="primary"
                      @click="showHistorico(item)"
                      size="small"
                      :disabled="isLoading.busy"
                      >Histórico</v-btn
                    >
                  </v-list-item>
                </v-list>
              </v-menu>
            </div>
          </td>
        </tr>
      </tbody>
      <tfoot>
        <tr>
          <td colspan="9">
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
    <v-dialog
      v-model="openHistorico"
      max-width="900"
      max-height="500"
      scrollable
    >
      <historico :eventos="eventos" @close-click="openHistorico = false" />
    </v-dialog>
  </div>
</template>

<script setup>
import { ref, watch, computed } from "vue";
import handleErrors from "@/helpers/HandleErrors";
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import moment from "moment";
import { useShareSolicitacaoStore } from "@/store/share/solicitacao.store";
import historico from "@/components/reembolso/historico.vue";
import filialService from "@/services/filial.service";
import { useAuthStore } from "@/store/auth.store";
import { onBeforeMount } from "vue";
import { useShareClienteStore } from "@/store/share/cliente.store";
import { useShareUsuarioStore } from "@/store/share/usuario.store";
import { getPageTitle } from "@/helpers/share/data";
import { useRoute } from "vue-router";
import { compararValor } from "@/helpers/functions";
import { useShareFuncionarioStore } from "@/store/share/funcionario.store";

//DATA
const route = useRoute();
const authStore = useAuthStore();
const page = ref(1);
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const totalPages = ref(1);
const solicitacoes = ref([]);
const clientes = ref([]);
const filiais = ref([]);
const usuarios = ref([]);
const funcionarios = ref([]);
const filter = ref({
  filialId: null,
  clienteId: null,
  responsavelId: null,
  status: null,
  dataIni: null,
  dataFim: null,
  usuarioId: authStore.user.id,
  showFinishedStatus: false
});
const solicitacaoStore = useShareSolicitacaoStore();
const eventos = ref([]);
const openHistorico = ref(false);
const isMatriz = ref(false);
const isLoading = ref({
  form: true,
  busy: false,
});
const pageTipo = ref({
    id: 0,
    tipo: ""
})
//COMPUTED'S

//WATCH'S
watch(
  () => filter.value.filialId,
  async () => {
    let _filiais = [];
    if (filter.value.filialId != null) _filiais.push(filter.value.filialId);
    filter.value.clienteId = null;
    filter.value.usuarioId = null;

    await getClientesToList(_filiais[0]);
    await getUsuarioToList(_filiais);
    
  }
);

//VUE METHODS
onBeforeMount(async () => {
  try {
    isLoading.value.form = true;

    if (route.path.includes("desligamento")) {
        pageTipo.value.id = 1;
        pageTipo.value.tipo = "Desligamento";
    } else if (route.path.includes("comunicado")) {
        pageTipo.value.id = 2
        pageTipo.value.tipo = "Comunicado";
    } else if (route.path.includes("ferias")) {
        pageTipo.value.id = 3
        pageTipo.value.tipo = "Ferias";
    } else if (route.path.includes("mudancabase")) {
        pageTipo.value.id = 4
        pageTipo.value.tipo = "MudancaBase";
    }else if (route.path.includes("vagas")) {
        pageTipo.value.id = 5
        pageTipo.value.tipo = "Vaga";
    }

    //meusClientesId.value = await authStore.retornarMeusClientes(true);
    //meusCentrosDeCustoId.value = await authStore.retornarMeusCentrosdeCustos(0, true);
    
    await getFiliaisToList();
    await getFuncionarioToList();
    isMatriz.value = authStore.sistema.isMatriz;
    authStore.user.filial = authStore.sistema.filial.value;
    await clearFilters();
    
  } catch (error) {
  } finally {
    isLoading.value.form = false;
  }
});

watch(page, () => getItems());


//METHODS
async function clearFilters(resetPage = false) {
  try {
    isLoading.value.busy = true;
    filter.value = {
      filialId: null,
      clienteId: null,
      responsavelId: null,
      status: null,
      dataIni: null,
      dataFim: null,
      usuarioId: authStore.user.id,
      funcionarioId: null
    };

    await getUsuarioToList(isMatriz.value ? [] : [authStore.user.filialId]);
    await getClientesToList(filter.value.filialId, authStore.user.id);

    await getItems(resetPage);
  } catch (error) {
    console.error(error);
  } finally {
    isLoading.value.busy = false;
  }
}

function toPage(id = null) {
    if (id && id > 0)
        router.push({ name: `share${pageTipo.value.tipo}Cadastro`, query: { id: id } });
    else
        router.push({ name: `share${pageTipo.value.tipo}Create`});
}

function getFuncNome(item){
  
  if (item.solicitacaoTipoId == 1)
    return item.desligamento.funcionarioNome;
  else if (item.solicitacaoTipoId == 2)
    return item.comunicado.funcionarioNome;
  else if (item.solicitacaoTipoId == 3)
    return item.ferias.funcionarioNome;
  else if (item.solicitacaoTipoId == 4)
    return item.mudancaBase.funcionarioNome;
  else  
     return '';  
}

async function showHistorico(item) {
  eventos.value = item.historico;
  openHistorico.value = true;
}

async function getItems(resetPage = false) {
  try {
    isLoading.value.busy = true;

    if (resetPage) page.value = 1;

    if (
      (filter.value.dataIni && !filter.value.dataFim) ||
      (filter.value.dataFim && !filter.value.dataIni)
    )
      throw new TypeError("Ambas as datas devem ser informadas!");
    else if (moment(filter.value.dataFim) < moment(filter.value.dataIni))
      throw new TypeError("A data fim deve ser maior que a data início!");


    let filtros = {...filter.value }
    delete filtros.clienteId
    filtros.filialId = filtros.filialId ?? 0
    filtros.tipoSolicitacao = pageTipo.value.id

    if (filter.value.clienteId != null) 
      filtros.clienteIds = [filter.value.clienteId]

    if (!filter.value.showFinishedStatus && (!filtros.status || filtros.status.length == 0))
      filtros.status = solicitacaoStore.statusSolicitacao.filter(q => q.id != 3).map(f => f.id);

    let response = await solicitacaoStore.getPaginate(
      page.value,
      pageSize,
      filtros
    );
    solicitacoes.value = response.items;
    totalPages.value = response.totalPages;
  } catch (error) {
    console.log("solicitacoes.getItems.error:", error.response);
    handleErrors(error);
  } finally {
    isLoading.value.busy = false;
  }
}

async function getClientesToList(filialId = 0, usuarioId = 0) {
  try {
    clientes.value = await useShareClienteStore().toComboList(filialId, usuarioId);
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
    usuarios.value = await useShareUsuarioStore().toComboList(filiais);
  } catch (error) {
    console.log("solcitacoes.getUsuarioToList.error:", error.response);
    handleErrors(error);
  }
}

async function getFuncionarioToList() {
  try {
    funcionarios.value = await useShareFuncionarioStore().getToComboByUsuario(authStore.user.id);
  } catch (error) {
    console.log("solcitacoes.getFuncionarioToList.error:", error.response);
    handleErrors(error);
  }
}

</script>
