<template>
  <div>
    <v-row>
      <v-col v-show="solicitacao.id > 0" class="text-right">
        <v-btn
          :color="solicitacao.status.color"
          variant="tonal"
          class="text-center"
        >
          {{ solicitacao.status.statusIntermediario }}</v-btn
        >
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="4" v-show="comboTipoShow">
        <select-text
          v-model="solicitacao.solicitacaoTipoId"
          :combo-items="TipoSolicitacao"
          :select-mode="solicitacao.id == 0"
          :text-field-value="
            getTextFromListByCodigo(
              TipoSolicitacao,
              solicitacao.solicitacaoTipoId
            )
          "
          label-text="Tipo Solicitação"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="solicitacao.clienteId"
          :combo-items="listClientes"
          :select-mode="solicitacao.id == 0"
          :text-field-value="solicitacao.clienteNome"
          label-text="Cliente"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <select-text
          v-model="solicitacao.responsavelId"
          :combo-items="listResponsavel"
          :select-mode="solicitacao.id != 0 && !isReadOnly"
          :text-field-value="
            getTextFromListByCodigo(listResponsavel, solicitacao.responsavelId)
          "
          label-text="Responsável"
          v-show="solicitacao.id != 0"
        ></select-text>
      </v-col>
    </v-row>
    <slot></slot>
    <v-row>
      <v-col>
        <v-textarea
          variant="outlined"
          :label="descricaoLabel"
          class="text-primary"
          v-model="solicitacao.descricao"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
          v-show="descricaoShow"
        >
        </v-textarea>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import { Solicitacao } from "@/store/share/solicitacao.store";
import { TipoSolicitacao, getTextFromListByCodigo } from "@/helpers/share/data";
import selectText from "../selectText.vue";

const props = defineProps({
  solicitacao: {
    type: Object,
    default: function () {
      return new Solicitacao();
    },
  },
  descricaoLabel: { type: String, default: "Observação" },
  descricaoShow: { type: Boolean, default: true },
  listClientes: {
    type: Array,
    default: function () {
      return [];
    },
  },
  listFuncionarios: {
    type: Array,
    default: function () {
      return [];
    },
  },
  listCentroCustos: {
    type: Array,
    default: function () {
      return [];
    },
  },
  listResponsavel: {
    type: Array,
    default: function () {
      return [];
    },
  },

  comboTipoShow: { type: Boolean, default: true },
  isReadOnly: { type: Boolean, default: false },
});
</script>
