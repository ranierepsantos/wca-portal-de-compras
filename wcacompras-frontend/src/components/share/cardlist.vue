<template>
  <v-card
    max-width="450"
    class="mx-auto drop-zone"
    style="margin: auto; border-radius: 15px"
    :color="color"
  >
    <v-card-title>
      {{ cardTitle + " (" + paginationData.totalCount + ")" }}
    </v-card-title>
    <draggable
      class="list-group"
      :list="paginationData.items"
      :group="cardTitle"
      :itemKey="cardTitle.toLowerCase()"
      @start="drag=false" 
      @end="drag=false" 
    >
      <template #item="{ element }">
        <v-card
          width="430"
          class="drag-el mx-auto"
          :key="element.id"
          style="margin-top: 5px; margin-bottom: 5px; border-radius: 15px"
        >
          <v-card-title class="text-left">
            <v-row>
              <v-col>{{ getPageTitle(element.solicitacaoTipoId) }}</v-col>
              <v-col class="text-right">
                <v-btn
                  prepend-icon="mdi-arrow-right-circle-outline"
                  variant="plain"
                  color="primary"
                  size="small"
                  @click="toPage(element)"
                >
                  Acessar
                </v-btn>
              </v-col>
            </v-row>
          </v-card-title>
          <v-card-text class="text-left">
            <v-row>
              <v-col :cols="cols">Cliente</v-col>
              <v-col>{{element.clienteNome}}</v-col>
            </v-row>
            <v-row v-if="element.solicitacaoTipoId != 5">
              <v-col :cols="cols">Funcionário</v-col>
              <v-col v-if="element.solicitacaoTipoId == 1">{{ element.desligamento.funcionarioNome }}</v-col>
              <v-col v-else-if="element.solicitacaoTipoId == 2">{{ element.comunicado.funcionarioNome }}</v-col>
              <v-col v-else-if="element.solicitacaoTipoId == 3">{{ element.ferias.funcionarioNome }}</v-col>
              <v-col v-else-if="element.solicitacaoTipoId == 4">{{ element.mudancaBase.funcionarioNome }}</v-col>
            </v-row>
            <v-row v-else>
              <v-col :cols="cols">Função</v-col>
              <v-col>{{ element.vaga.funcaoNome }}</v-col>
            </v-row>
            <v-row>
              <v-col :cols="cols">Responsável</v-col>
              <v-col>{{ element.responsavelNome }}</v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </template>

      <template #footer>
        <v-pagination
              v-model="page"
              :length="paginationData.totalPages"
              :total-visible="4"
        ></v-pagination>
      </template>
    </draggable>
  </v-card>
</template>

<script setup>
import { getPageTitle, getShareRouteName } from "@/helpers/share/data";
import router from "@/router";
import { ref, watch } from "vue";
import draggable from "vuedraggable";

defineProps({
  paginationData: {
    type: Object,
    default: {
      currentPage: 1,
      totalPages: 1, 
      pageSize: process.env.VUE_APP_PAGE_SIZE,
      totalCount: 0,
      items: [],
      hasPrevious: false,
      hasNext:  false
    },
  },
  color: {
    type: String,
    default: "grey-lighten-1",
  },
  cardTitle: {
    type: String,
    default: "Título",
  }
});
const emit = defineEmits(['pageChange'])
const cols = 4;
const page = ref(1);

watch(page, () => emit("pageChange", page.value));


function toPage(item) {
  router.push({ name: `${getShareRouteName(item.solicitacaoTipoId)}Cadastro`, query: { id: item.id } });
}

</script>

<style scoped>
.drag-el {
  margin: 3px;
}
</style>
