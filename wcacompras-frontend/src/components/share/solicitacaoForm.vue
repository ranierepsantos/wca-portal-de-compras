<template>
  <div>
    <v-row>
      <v-col cols="4" v-show="comboTipoShow">
        <select-text
          v-model="solicitacao.solicitacaoTipoId"
          :combo-items="TipoSolicitacao"
          :select-mode="solicitacao.id == 0"
          :text-field-value="getTextFromListByCodigo(TipoSolicitacao, solicitacao.solicitacaoTipoId)"
          label-text="Tipo Solicitação"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="solicitacao.clienteId"
          :combo-items="listClientes"
          :select-mode="solicitacao.id == 0"
          :text-field-value="getTextFromListByCodigo(listClientes, solicitacao.clienteId)"
          label-text="Cliente"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <select-text
          v-model="solicitacao.funcionarioId"
          :combo-items="listFuncionarios"
          :select-mode="solicitacao.id == 0"
          :text-field-value="getTextFromListByCodigo(listFuncionarios, solicitacao.funcionarioId)"
          label-text="Funcionário"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <select-text
          v-model="solicitacao.centroCustoId"
          :combo-items="listCentroCustos"
          :select-mode="solicitacao.id == 0"
          :text-field-value="getTextFromListByCodigo(listCentroCustos, solicitacao.centroCustoId)"
          label-text="Centro de Custo"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
      </v-row>
      <v-row>
      <v-col>
        <select-text
          v-model="solicitacao.responsavelId"
          :combo-items="listResponsavel"
          :select-mode="solicitacao.id != 0"
          :text-field-value="getTextFromListByCodigo(listResponsavel, solicitacao.responsavelId)"
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
  descricaoLabel: {type: String, default: "Observação"},
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

  comboTipoShow: {type: Boolean, default: true},
});

</script>
