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
          :rules="[(v) => !!v || 'Campo obrigatório']"
          :readonly="true"
          v-model="dataModel.dataAlteracao"
          bg-color="#f2f2f2"
        ></v-text-field>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.clienteDestinoId"
          :combo-items="[]"
          :select-mode="dataModel.solicitacaoId == 0"
          :text-field-value="dataModel.clienteDestinoNome"
          label-text="Cliente Destino"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
        <v-col>
            <box-transfer 
              :list-origem="listItensMudanca" 
              :list-destino="dataModel.itensMudanca"
              list-origem-titulo = "Selecione os Itens de Mudança"
              list-destino-titulo = "Itens de Mudança"
            />
        </v-col>
    </v-row>
  </div>
</template>

<script setup>
import {
  MudancaBase,
  useShareSolicitacaoStore,
} from "@/store/share/solicitacao.store";
import boxTransfer from "@/components/boxTransfer.vue";
import selectText from "../selectText.vue";
import { getTextFromListByCodigo } from "@/helpers/share/data";

defineProps({
  dataModel: {
    type: Object,
    default: function () {
      return new MudancaBase();
    },
  },
  dataAdmissao: { type: String, default: "" },
  createMode: { type: Boolean, default: true },
  isReadOnly: { type: Boolean, default: false },
});
const listItensMudanca = [];
</script>
