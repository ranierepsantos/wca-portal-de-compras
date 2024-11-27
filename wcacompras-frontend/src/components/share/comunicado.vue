<template>
  <div>
    <v-row>
      <v-col>
        <select-text
          v-model="dataModel.funcionarioId"
          :combo-items="listFuncionarios"
          :select-mode="createMode"
          :text-field-value="dataModel.funcionarioNome"
          label-text="Funcionário"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
      <v-col cols="2">
        <v-text-field
          variant="outlined"
          label="eSocial Matrícula"
          class="text-primary"
          v-model="dataModel.eSocialMatricula"
          :readOnly="true"
          bg-color="#f2f2f2"
          density="compact"
        >
        </v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <select-text
          v-model="dataModel.centroCustoId"
          :combo-items="listCentroCustos"
          :select-mode="false"
          :text-field-value="dataModel.centroCustoNome"
          label-text="Centro de Custo"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
    </v-row>
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
          :text-field-value="dataModel.assuntoNome"
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
import { watch } from "vue";
import moment from "moment";

const props = defineProps({
  dataModel: {
    type: Object,
    default: function () {
      return new Comunicado();
    },
  },
  createMode: { type: Boolean, default: true },
  listCentroCustos: {
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
});
const listAssuntos = useShareSolicitacaoStore().assuntos;

watch(() => props.dataModel.funcionarioId, () => {
  
  let oFunc = props.listFuncionarios.find(q => q.value == props.dataModel.funcionarioId)
    props.dataModel.centroCustoNome = null
    props.dataModel.centroCustoId = null
    props.dataModel.eSocialMatricula =null
    props.dataModel.funcionarioDataAdmissao =null
  if (oFunc) {
    props.dataModel.centroCustoNome = oFunc.centroCustoNome
    props.dataModel.centroCustoId = oFunc.centroCustoId
    props.dataModel.eSocialMatricula = oFunc.eSocialMatricula
    props.dataModel.funcionarioDataAdmissao = moment(oFunc.dataAdmissao).format("YYYY-MM-DD");
  }
});
</script>
