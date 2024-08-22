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
      <v-col cols="3">
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
    <!--BODY -->
    <v-row>
      <v-col
        ><card-list
          :list-data="listPendente"
          color="orange-lighten-4"
          card-title="Pendentes"
        ></card-list
      ></v-col>
      <v-col
        ><card-list
          :list-data="listAndamento"
          color="blue-accent-1"
          card-title="Em Andamento"
        ></card-list
      ></v-col>
      <v-col
        ><card-list
          :list-data="listConcluido"
          color="green-lighten-3"
          card-title="Concluídos"
        ></card-list
      ></v-col>
    </v-row>
  </div>
</template>

<script setup>
//LIBRARYS
import { ref } from "vue";
import handleErrors from "@/helpers/HandleErrors";
//COMPONENTS
import cardList from "@/components/share/cardlist.vue";
import breadCrumbs from "@/components/breadcrumbs.vue";

//STORES
import { useShareSolicitacaoStore } from "@/store/share/solicitacao.store";


//VARIABLES
const solicitacaoStore = useShareSolicitacaoStore();
const isMatriz = ref(true)
const listPendente = ref([
  { id: 1, nome: "Comunicado", list: 1 },
  { id: 2, nome: "Desligamento", list: 1 },
  { id: 3, nome: "Férias", list: 1 },
  { id: 4, nome: "Mudança de base", list: 1 },
  { id: 5, nome: "Vaga", list: 1 },
]);

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
//METHODS
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

    // await getUsuarioToList(isMatriz.value ? [] : [authStore.user.filialId]);
    // await getClientesToList(filter.value.filialId, authStore.user.id);

    // await getItems();
  } catch (error) {
    console.error(error);
    handleErrors(error);
  } finally {
    isLoading.busy = false;
  }
}
</script>
