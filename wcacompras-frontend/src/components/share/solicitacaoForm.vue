<template>
  <div>
    <v-row>
      <v-col v-show="solicitacao.id > 0 && !'1,2'.includes(solicitacao.statusSolicitacaoId) " class="text-right">
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
      <v-col cols="4" v-show="solicitacao.id > 0 && '1,2'.includes(solicitacao.statusSolicitacaoId)">
        <select-text
          v-model="solicitacao.statusSolicitacaoId"
          :combo-items="listStatus"
          combo-item-value="id"
          combo-item-title="statusIntermediario"
          :select-mode="'3,5,6,8'.includes(solicitacao.statusSolicitacaoId) == false"
          :text-field-value="solicitacao.status.statusIntermediario"
          label-text="Status Solicitação"
        ></select-text>
      </v-col>
      <v-col cols="4" v-show="solicitacao.id > 0 && '1,2'.includes(solicitacao.statusSolicitacaoId)">
        <select-text
          v-model="solicitacao.statusSolicitacaoId"
          :combo-items="listStatus"
          combo-item-value="id"
          combo-item-title="statusIntermediario"
          :select-mode="'3,5,6,8'.includes(solicitacao.statusSolicitacaoId) == false"
          :text-field-value="solicitacao.status.statusIntermediario"
          label-text="Status Solicitação"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <select-text
          v-model="solicitacao.responsavelId"
          :combo-items="listResponsavel"
          :select-mode="!isReadOnly"
          :text-field-value="solicitacao.responsavelNome"
          label-text="Responsável"
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
          :readOnly="isDescricaoReadOnly"
          :bg-color="isDescricaoReadOnly ? '#f2f2f2' : ''"
          v-show="descricaoShow"
        >
        </v-textarea>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import { Solicitacao } from "@/store/share/solicitacao.store";
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
  listStatus: {
    type: Array,
    default: function () {
      return [];
    },
  },

  comboTipoShow: { type: Boolean, default: true },
  isReadOnly: { type: Boolean, default: false },
  isDescricaoReadOnly: { type: Boolean, default: false },
});
</script>
