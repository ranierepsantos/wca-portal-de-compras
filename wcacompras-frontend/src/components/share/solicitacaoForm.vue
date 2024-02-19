<template>
  <div>
    <v-row>
      <v-col cols="4">
        <select-text
          v-model="solicitacao.solicitacaoTipoId"
          :combo-items="TipoSolicitacao"
          :select-mode="solicitacao.id == 0"
          :text-field-value="
            getTextFromListByCodigo(TipoSolicitacao, solicitacao.solicitacaoTipoId)
          "
          label-text="Tipo Solicitação"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="solicitacao.clienteId"
          :combo-items="listClientes"
          :select-mode="solicitacao.id == 0"
          text-field-value="Carapicuiba"
          label-text="Cliente"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <select-text
          v-model="solicitacao.funcionarioId"
          :combo-items="listFuncionarios"
          :select-mode="solicitacao.id == 0"
          text-field-value="Carapicuiba"
          label-text="Funcionário"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <select-text
          v-model="solicitacao.gestorId"
          :combo-items="listGestores"
          :select-mode="solicitacao.id == 0"
          text-field-value="Carapicuiba"
          label-text="Gestor"
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
              v-model="solicitacao.Observacao"
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

defineProps({
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
  listGestores: {
    type: Array,
    default: function () {
      return [];
    },
  },
});

//FUNCTIONS
</script>
