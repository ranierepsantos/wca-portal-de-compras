<template>
  <v-card
    max-width="450"
    class="mx-auto drop-zone"
    style="margin: auto; border-radius: 15px"
    :color="color"
  >
    <v-card-title>
      {{ cardTitle + " (" + listData.length + ")" }}
    </v-card-title>
    <draggable
      class="list-group"
      :list="listData"
      group="people"
      itemKey="name"
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
              <v-col>{{ element.nome }}</v-col>
              <v-col class="text-right">
                <v-btn
                  prepend-icon="mdi-arrow-right-circle-outline"
                  variant="plain"
                  color="primary"
                  size="small"
                >
                  Acessar
                </v-btn>
              </v-col>
            </v-row>
          </v-card-title>
          <v-card-text class="text-left">
            <v-row>
              <v-col>Cliente</v-col>
              <v-col>cliente</v-col>
            </v-row>
            <v-row>
              <v-col>Funcionário</v-col>
              <v-col>{{ element.id }}</v-col>
            </v-row>
            <v-row>
              <v-col>Responsável</v-col>
              <v-col>{{ element.list }}</v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </template>

      <template #footer>
        <v-pagination v-model="page" length="10"></v-pagination>
      </template>
    </draggable>
  </v-card>
</template>

<script setup>
import { ref } from "vue";
import draggable from "vuedraggable";

defineProps({
  listData: {
    type: Array,
    default: () => {
      return [];
    },
  },
  color: {
    type: String,
    default: "grey-lighten-1",
  },
  cardTitle: {
    type: String,
    default: "Título",
  },
  page: {
    type: Number,
    default: 1,
  },
});
</script>

<style scoped>
.drag-el {
  margin: 3px;
}
</style>
