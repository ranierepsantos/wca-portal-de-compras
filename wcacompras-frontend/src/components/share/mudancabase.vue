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
          v-model="dataModel.dataAlteracao"
          :readonly="!createMode || isReadOnly"
          :bg-color="!createMode || isReadOnly ? '#f2f2f2' : ''"
        ></v-text-field>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.clienteDestinoId"
          :combo-items="listClientes"
          :select-mode="dataModel.solicitacaoId == 0"
          :text-field-value="dataModel.clienteDestinoNome"
          label-text="Cliente Destino"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :disabled="!clienteSelected"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col >
            <v-textarea
              variant="outlined"
              label="Descricão da Mudança"
              class="text-primary"
              :readOnly = "isReadOnly"
              :bg-color="isReadOnly ? '#f2f2f2' : ''"
              v-model="dataModel.observacao"
            >
            </v-textarea>
        </v-col>
    </v-row>
    <v-row>
        <v-col >
            <box-transfer 
              :list-origem="listItensMudanca" 
              :list-destino="dataModel.itensMudanca"
              list-origem-titulo = "Selecione os Itens de Mudança"
              list-destino-titulo = "Itens de Mudança"
              :show-search-text="false"
              v-show="!isReadOnly"
            />
            <v-textarea
              variant="outlined"
              label="Items de Mudança"
              class="text-primary"
              :readOnly = "isReadOnly"
              :bg-color="isReadOnly ? '#f2f2f2' : ''"
              :model-value="getItensMudancaDescricao()"
              v-show="isReadOnly"
              rows="2"
            >
            
            </v-textarea>
        </v-col>
    </v-row>
  </div>
</template>

<script setup>
import {
  MudancaBase,
} from "@/store/share/solicitacao.store";
import boxTransfer from "@/components/boxTransfer.vue";
import selectText from "../selectText.vue";

const props = defineProps({
  dataModel: {
    type: Object,
    default: function () {
      return new MudancaBase();
    },
  },
  dataAdmissao: { type: String, default: "" },
  createMode: { type: Boolean, default: true },
  isReadOnly: { type: Boolean, default: false },
  listClientes: {
    type: Array,
    default: function () {
      return [];
    },
  },
  clienteSelected: { type: Boolean, default: false },
  listItensMudanca: {
    type: Array,
    default: function () {
      return [];
    },
  }
});

function getItensMudancaDescricao () {
  let text = "";
  props.dataModel.itensMudanca.forEach(item => {
    text += item.text + "; "
  });
  return text.trimEnd();

}
</script>
