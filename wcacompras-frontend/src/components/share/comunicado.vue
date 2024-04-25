<template>
  <div>
    <v-row>
      <v-col cols="2">
        <v-text-field
          label="Data Alteração"
          type="date"
          variant="outlined"
          color="primary"
          density="compact"
          v-model="dataModel.dataAlteracao"
          :rules="[(v) => !!v || 'Campo obrigatório']"
          :readonly="!createMode"
          :bg-color="!createMode ? '#f2f2f2' : ''"
        ></v-text-field>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.assuntoId"
          :combo-items="listAssuntos"
          combo-item-title="text"
          combo-item-value="value"
          :select-mode="createMode"
          :text-field-value="dataModel.assunto.nome"
          label-text="Assunto"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-textarea
          variant="outlined"
          label="Descrição Pedido"
          class="text-primary"
          v-model="dataModel.observacao"
          :readonly="!createMode"
          :bg-color="!createMode ? '#f2f2f2' : ''"
        >
        </v-textarea>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import {
  Comunicado,
  useShareSolicitacaoStore,
} from "@/store/share/solicitacao.store";

import selectText from "../selectText.vue";
import { getTextFromListByCodigo } from "@/helpers/share/data";

defineProps({
  dataModel: {
    type: Object,
    default: function () {
      return new Comunicado();
    },
  },
  createMode: { type: Boolean, default: true },
});
const listAssuntos = useShareSolicitacaoStore().assuntos;
</script>
