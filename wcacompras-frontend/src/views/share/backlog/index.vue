<template>
  <div>
    <bread-crumbs title="Backlog" :show-button="false" />
    <!--FILTROS -->
    <v-row v-show="!isLoading.form">
      <v-col>
        <v-select
          label="Filiais"
          v-model="filter.filialId"
          :items="listFiliais"
          density="compact"
          item-title="text"
          item-value="value"
          variant="outlined"
          color="primary"
          :hide-details="true"
          v-show="isMatriz"
        ></v-select>
      </v-col>
      <v-col>
        <v-autocomplete
          label="Clientes"
          v-model="filter.clienteId"
          :items="listClientes"
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
          :items="listUsuarios"
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
      <!-- <v-col cols="3">
        <v-select
          label="Status"
          v-model="filter.status"
          :items="
            solicitacaoStore.statusSolicitacao
          "
          density="compact"
          item-title="statusIntermediario"
          item-value="id"
          variant="outlined"
          color="primary"
          :hide-details="true"
        ></v-select>
      </v-col> -->
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
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isLoading.busy || isLoading.form"
      class="mb-5"
    ></v-progress-linear>
    <!--BODY -->
    <v-row v-show="!isLoading.form">
      <v-col
        ><card-list
          :pagination-data="listPendente"
          color="orange-lighten-4"
          card-title="Pendentes"
          @page-change="getPendentes($event, true)"
        ></card-list
      ></v-col>
      <v-col
        ><card-list
          :pagination-data="listAndamento"
          color="blue-accent-1"
          card-title="Em Andamento"
          @page-change="getEmAndamento($event, true)"
        ></card-list
      ></v-col>
      <v-col
        ><card-list
          :pagination-data="listConcluido"
          color="green-lighten-3"
          card-title="Concluídos"
          @page-change="getConcluido($event, true)"
        ></card-list
      ></v-col>
    </v-row>
  </div>
</template>

<script setup>
//LIBRARYS
import { onBeforeMount, ref } from "vue";
import handleErrors from "@/helpers/HandleErrors";
import moment from "moment";

//COMPONENTS
import cardList from "@/components/share/cardlist.vue";
import breadCrumbs from "@/components/breadcrumbs.vue";

//SERVICES
import filialService from "@/services/filial.service";

//STORES
import { useShareSolicitacaoStore } from "@/store/share/solicitacao.store";
import { useAuthStore } from "@/store/auth.store";
import { useShareClienteStore } from "@/store/share/cliente.store";
import { useShareUsuarioStore } from "@/store/share/usuario.store";

//VARIABLES
const pageSize = 5//process.env.VUE_APP_PAGE_SIZE;
const solicitacaoStore = useShareSolicitacaoStore();
const authStore = useAuthStore();
const isMatriz = ref(true)
const listPendente = ref(
  {
    currentPage: 1,
    totalPage: 1,
    totalCount: 5,
    pageSize: 10,
    hasPrevious: false,
    hasNext: false,
    items: [
      { id: 1, nome: "Comunicado", list: 1 },
      { id: 2, nome: "Desligamento", list: 1 },
      { id: 3, nome: "Férias", list: 1 },
      { id: 4, nome: "Mudança de base", list: 1 },
      { id: 5, nome: "Vaga", list: 1 },
    ]
  }
);

const listAndamento = ref([
{ id: 1, nome: "Comunicado", list: 2 },
  { id: 2, nome: "Desligamento", list: 2 },
  { id: 3, nome: "Férias", list: 2 },
  { id: 4, nome: "Mudança de base", list: 2 },
  { id: 5, nome: "Vaga", list: 2 },
]);

const listConcluido = ref([
  { id: 1, nome: "Comunicado", list: 3 },
  { id: 2, nome: "Desligamento", list: 3 },
  { id: 3, nome: "Férias", list: 3 },
  { id: 4, nome: "Mudança de base", list: 3 },
  { id: 5, nome: "Vaga", list: 3 },
]);

const listClientes = ref([])
const listUsuarios = ref([])
const listFiliais = ref([])
const isLoading = ref({
  form: false,
  busy: false,
});
const filter = ref({
  filialId: null,
  clienteId: null,
  responsavelId: null,
  status: null,
  dataIni: null,
  dataFim: null,
});
const meusClientesId = ref([])
const meusCentrosDeCustoId = ref([])
//VUE FUNCTIONS
onBeforeMount (init);
//FUNCTIONS
async function init() {
  try {
    isLoading.value.form = true
    meusClientesId.value = await authStore.retornarMeusClientes(true);
    meusCentrosDeCustoId.value = await authStore.retornarMeusCentrosdeCustos(0, true);
    
    await getFiliaisToList();
    
    isMatriz.value = authStore.sistema.isMatriz;
    authStore.user.filial = authStore.sistema.filial.value;

    await clearFilters();
  
  } catch (error) {
    console.error("init.error", error)
    handleErrors(error);
  }finally
  {
    isLoading.value.form = false
  }
}



async function clearFilters() {
  try {
    isLoading.value.busy = true;
    filter.value = {
      filialId: null,
      clienteId: null,
      responsavelId: null,
      status: null,
      dataIni: null,
      dataFim: null,
    };
    
    if (!isMatriz.value) 
      filter.value.filialId = authStore.user.filial;
    
    await getUsuarioToList(isMatriz.value ? [] : [authStore.user.filialId]);
    await getClientesToList(filter.value.filialId, authStore.user.id);

    await getItems();
  } catch (error) {
    console.error(error);
    handleErrors(error);
  } finally {
    isLoading.value.busy = false;
  }
}

async function getItems() {
  try {
    isLoading.value.busy = true;
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
    filtros.clienteIds = meusClientesId.value
    filtros.centroCustoIds = meusCentrosDeCustoId.value 
    filtros.tipoSolicitacao = 0

    if (filter.value.clienteId != null) 
      filtros.clienteIds = [filter.value.clienteId]

    sessionStorage.setItem("backlog.filtros", JSON.stringify(filtros))

    let pendentes = await getPendentes(1);
    let andamentos = await getEmAndamento(1)
    let concluidos = await getConcluido(1)  

    listPendente.value = pendentes
    listAndamento.value = andamentos  
    listConcluido.value = concluidos

  } catch (error) {
    console.error("getItems.error",error);
    handleErrors(error);
  } finally {
    isLoading.value.busy = false;
  }
  
}


async function getPendentes(page, fillList = false) {
  let filtros = JSON.parse(sessionStorage.getItem('backlog.filtros'))
  filtros.status = [1,4]

    let data = await solicitacaoStore.getPaginate(
      page,
      pageSize,
      filtros
    );
    if (fillList)
      listPendente.value = data;
    else
      return data;
}

async function getEmAndamento(page, fillList = false) {
  let filtros = JSON.parse(sessionStorage.getItem('backlog.filtros'))
  filtros.status = [2]

    let data = await solicitacaoStore.getPaginate(
      page,
      pageSize,
      filtros
    );
    if (fillList)
      listAndamento.value = data;
    else
    return data;
}

async function getConcluido(page, fillList = false) {
  let filtros = JSON.parse(sessionStorage.getItem('backlog.filtros'))
  filtros.status = [3,5]

    let data = await solicitacaoStore.getPaginate(
      page,
      pageSize,
      filtros
    );
    if (fillList)
      listConcluido.value = data;
    else
    return data;
}

async function getClientesToList(filialId = 0, usuarioId = 0) {
  try {
    listClientes.value = await useShareClienteStore().toComboList(filialId, usuarioId);
  } catch (error) {
    console.log("getClientesToList.error:", error.response);
    handleErrors(error);
  }
}

async function getFiliaisToList() {
  try {
    let response = await filialService.toList();
    listFiliais.value = response.data;
  } catch (error) {
    console.log("getFiliais.error:", error.response);
    handleErrors(error);
  }
}

async function getUsuarioToList(filiais = []) {
  try {
    listUsuarios.value = await useShareUsuarioStore().toComboList(filiais);
  } catch (error) {
    console.log("getUsuarioToList.error:", error.response);
    handleErrors(error);
  }
}

</script>
