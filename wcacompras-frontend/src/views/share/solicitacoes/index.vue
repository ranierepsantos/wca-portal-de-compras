<template>
  <div>
    <bread-crumbs
      :title="getPageTitle(pageTipo.id)"
      @novoClick="toPage()"
    />
    <v-row v-show="!isLoading.form">
      <v-col>
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
      <v-col>
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
      <v-col>
        <v-select
          label="Responsável"
          v-model="filter.responsavelId"
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
          <th class="text-left text-grey">FUNCIONÁRIO</th>
          <th class="text-left text-grey">RESPONSÁVEL</th>
          <th class="text-left text-grey" v-show="pageTipo.id == 0">TIPO SOLICITAÇÃO</th>
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
          <td class="text-left">{{ item.funcionarioNome }}</td>
          <td class="text-left">{{ item.responsavelNome || "Não atribuído" }}</td>
          <td class="text-left" v-show="pageTipo.id == 0">{{ item.responsaveNome }}</td>
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
                      prepend-icon="mdi-lead-pencil"
                      variant="plain"
                      color="primary"
                      @click="toPage(item.id)"
                      size="small"
                      >Editar</v-btn
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
import { ref, watch } from "vue";
import handleErrors from "@/helpers/HandleErrors";
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import moment from "moment";
import { useShareSolicitacaoStore } from "@/store/share/solicitacao.store";
import historico from "@/components/reembolso/historico.vue";
import { realizarDownload } from "@/helpers/functions";
import filialService from "@/services/filial.service";
import { useAuthStore } from "@/store/auth.store";
import { onBeforeMount } from "vue";
import { useShareClienteStore } from "@/store/share/cliente.store";
import { useShareUsuarioStore } from "@/store/share/usuario.store";
import { getPageTitle } from "@/helpers/share/data";
import { useRoute } from "vue-router";
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
const filter = ref({
  
  filialId: null,
  clienteId: null,
  responsavelId: null,
  status: null,
  dataIni: null,
  dataFim: null,
});
const solicitacaoStore = useShareSolicitacaoStore();
const eventos = ref([]);
const openHistorico = ref(false);
const isMatriz = ref(false);
const isLoading = ref({
  form: true,
  busy: false,
});
const formButtons = ref([]);
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
    debugger
    isLoading.value.form = true;

    if (route.path.includes("desligamento")) {
        pageTipo.value.id = 1;
        pageTipo.value.tipo = "Desligamento";
    } else if (route.path.includes("comunicado")) {
        pageTipo.value.id = 2
        pageTipo.value = "Comunicado";
    } else if (route.path.includes("ferias")) {
        pageTipo.value.id = 3
        pageTipo.value = "Ferias";
    } else if (route.path.includes("mudancabase")) {
        pageTipo.value.id = 4
        pageTipo.value = "MudancaBase";
    }

    await getFiliaisToList();
    let filialUsuario = (
      await useShareUsuarioStore().getFiliais(authStore.user.id)
    )[0];
    isMatriz.value = filialUsuario.text.toLowerCase() == "matriz";
    authStore.user.filial = filialUsuario.value;
    await clearFilters();
    formButtons.value.push({ text: "Novo", icon: "", event: "novoClick" });
  } catch (error) {
  } finally {
    isLoading.value.form = false;
  }
});

watch(page, () => getItems());

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

    await getUsuarioToList(isMatriz.value ? [] : [authStore.user.filialId]);
    await getClientesToList(filter.value.filialId);
    await getItems();
  } catch (error) {
    console.error(error);
  } finally {
    isLoading.busy = false;
  }
}

function toPage(id = null) {
    console.log("toPage")
    if (id && id > 0)
        router.push({ name: `share${pageTipo.value.tipo}Edit`, query: { id: id } });
    else
        router.push({ name: `share${pageTipo.value.tipo}Create`});
}

async function showHistorico(item) {
  eventos.value = item.historico;
  openHistorico.value = true;
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
    filtros.tipoSolicitacao = pageTipo.value.id


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

async function gerarRelatorio() {
  try {
    isLoading.value.busy = true;
    if (
      (filter.value.dataIni && !filter.value.dataFim) ||
      (filter.value.dataFim && !filter.value.dataIni)
    )
      throw new TypeError("Ambas as datas devem ser informadas!");
    else if (moment(filter.value.dataFim) < moment(filter.value.dataIni))
      throw new TypeError("A data fim deve ser maior que a data início!");

    let response = await solicitacaoStore.gerarRelatorio(filter.value);

    if (response.status == 200) {
      let nomeArquivo = `relatorio_solicitacoes_${moment().format(
        "DDMMYYYY_HHmmSS"
      )}.xlsx`;
      await new Promise((r) => setTimeout(r, 2000));
      realizarDownload(
        response,
        nomeArquivo,
        response.headers.getContentType()
      );
    }
  } catch (error) {
    console.log("solicitacoes.gerarRelatorio.error:", error.response);
    handleErrors(error);
  } finally {
    isLoading.value.busy = false;
  }
}
</script>
