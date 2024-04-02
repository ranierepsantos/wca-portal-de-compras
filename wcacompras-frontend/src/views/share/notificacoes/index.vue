<template>
  <div>
    <bread-crumbs title="Notificações" :show-button="false" />
    <v-row>
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
        <v-checkbox 
            v-model="filter.naoLido" 
            label="Somente não lido?"
            color="primary">
        </v-checkbox>
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
      v-show="isBusy"
    ></v-progress-linear>
    <v-table class="elevation-2">
      <thead>
        <tr>
          <th class="text-left text-grey">DATA/HORA</th>
          <th class="text-left text-grey">NOTIFICAÇÃO</th>
          <th class="text-center text-grey">LIDO</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in items" :key="item.id">
          <td class="text-left">{{ moment(item.dataHora).format("DD/MM/YYYY HH:mm") }}</td>
          <td class="text-left">{{ item.nota }}</td>
          <td class="text-center">
            <v-icon
              :icon="
                item.lido ? 'mdi-eye-check-outline' : 'mdi-eye-off-outline'
              "
              variant="plain"
              :color="item.lido ? 'success' : 'gray'"
            ></v-icon>
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
                      prepend-icon="mdi-arrow-right-circle-outline"
                      variant="plain"
                      color="primary"
                      @click="toPage(item)"
                      size="small"
                      >Ir para Solicitação</v-btn
                    >
                  </v-list-item>
                  <v-list-item>
                    <v-btn
                      prepend-icon="mdi-note-check-outline"
                      variant="plain"
                      color="primary"
                      @click="marcarLido(item)"
                      size="small"
                      :disabled="item.lido"
                      >Marcar como lido</v-btn
                    >
                  </v-list-item>
                  <!-- <v-list-item>
                    <v-btn
                      prepend-icon="mdi-eye-outline"
                      variant="plain"
                      color="primary"
                      @click="toPage(item.id)"
                      size="small"
                      
                      >Visualizar</v-btn
                    >
                  </v-list-item> -->
                </v-list>
              </v-menu>
            </div>
          </td>
        </tr>
      </tbody>
      <tfoot>
        <tr>
          <td colspan="7">
            <v-pagination
              v-model="page"
              :length="totalPages"
              :total-visible="4"
            ></v-pagination>
          </td>
        </tr>
      </tfoot>
    </v-table>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from "vue";
import handleErrors from "@/helpers/HandleErrors";
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import { useAuthStore } from "@/store/auth.store";
import { useShareNotificacaoStore } from "@/store/share/notificacao.store";
import moment from "moment";
//DATA
const page = ref(1);
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const isBusy = ref(false);
const totalPages = ref(1);
const notificacaoStore = useShareNotificacaoStore();
const items = ref([]);
const filter = ref({
  usuarioId: useAuthStore().user.id,
  naoLido: false,
  dataIni: null,
  dataFim: null,
});

//VUE METHODS
onMounted(async () => {
  await clearFilters();
});

watch(page, () => getItems());

//METHODS
async function clearFilters() {
  page.value = 1;
  filter.value = {
    usuarioId: useAuthStore().user.id,
    naoLido: false,
    dataIni: null,
    dataFim: null,
  };

  await getItems();
}

function toPage(item) {
    router.push({name: useAuthStore().sistema.nome + item.entidade +'Cadastro', query: {id: item.entidadeId}})
}

async function marcarLido(item) {
    await useAuthStore().marcarNotificao(item.id);
    item.lido = true;
}


async function getItems() {
  try {
    isBusy.value = true;
    if (
      (filter.value.dataIni && !filter.value.dataFim) ||
      (filter.value.dataFim && !filter.value.dataIni)
    )
      throw new TypeError("Ambas as datas devem ser informadas!");
    else if (moment(filter.value.dataFim) < moment(filter.value.dataIni))
      throw new TypeError("A data fim deve ser maior que a data início!");

    let response = await notificacaoStore.getPaginate(
      page.value,
      pageSize,
      filter.value
    );

    items.value = response.items;
    totalPages.value = response.totalPages;
  } catch (error) {
    console.log("getItems.error:", error.response);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}
</script>
