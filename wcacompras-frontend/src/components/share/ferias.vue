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
      <v-col cols="2">
        <v-text-field
          label="Data Saída"
          type="date"
          variant="outlined"
          color="primary"
          density="compact"
          v-model="dataModel.dataSaida"
          :rules="[(v) => !!v || 'Campo obrigatório']"
          :readonly="!createMode"
          :bg-color="!createMode ? '#f2f2f2' : ''"
        ></v-text-field>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.tipoFeriasId"
          :combo-items="tipoFerias"
          combo-item-title="descricao"
          combo-item-value="id"
          :select-mode="createMode"
          :text-field-value="dataModel.tipoFerias.descricao"
          label-text="Tipo Férias"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
      <v-col cols="2">
        <v-text-field
          label="Data Retorno"
          type="date"
          variant="outlined"
          color="primary"
          density="compact"
          v-model="dataModel.dataRetorno"
          :rules="[(v) => !!v || 'Campo obrigatório']"
          :readonly="!createMode"
          :bg-color="!createMode ? '#f2f2f2' : ''"
        ></v-text-field>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import {
  Ferias,
  useShareSolicitacaoStore,
} from "@/store/share/solicitacao.store";
import selectText from "../selectText.vue";
import { watch } from "vue";
import moment from "moment";

const props = defineProps({
  dataModel: {
    type: Object,
    default: function () {
      return new Ferias();
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

const tipoFerias = useShareSolicitacaoStore().tipoFerias;

watch(
  () => props.dataModel.funcionarioId,
  () => {
    let oFunc = props.listFuncionarios.find(
      (q) => q.value == props.dataModel.funcionarioId
    );
    props.dataModel.centroCustoNome = null;
    props.dataModel.centroCustoId = null;
    props.dataModel.eSocialMatricula = null;
    props.dataModel.funcionarioDataAdmissao = null;
    if (oFunc) {
      props.dataModel.centroCustoNome = oFunc.centroCustoNome;
      props.dataModel.centroCustoId = oFunc.centroCustoId;
      props.dataModel.eSocialMatricula = oFunc.eSocialMatricula;
      props.dataModel.funcionarioDataAdmissao = moment(
        oFunc.dataAdmissao
      ).format("YYYY-MM-DD");
    }
  }
);
</script>
