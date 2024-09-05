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
          label="Data Admissão"
          type="date"
          variant="outlined"
          color="primary"
          density="compact"
          v-model="dataModel.funcionarioDataAdmissao"
          :rules="[(v) => !!v || 'Campo obrigatório']"
          :readonly="true"
          bg-color="#f2f2f2"
        ></v-text-field>
      </v-col>
      <v-col cols="2">
        <v-text-field
          label="Data Demissão"
          type="date"
          variant="outlined"
          color="primary"
          density="compact"
          v-model="dataModel.dataDemissao"
          :rules="[(v) => !!v || 'Campo obrigatório']"
          :readonly="!createMode"
          :bg-color="!createMode ? '#f2f2f2' : ''"
        ></v-text-field>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.motivoDemissaoId"
          :combo-items="listMotivoDemissao"
          combo-item-title="motivo"
          combo-item-value="id"
          :select-mode="createMode"
          :text-field-value="getMotivoDemissaoText(dataModel.motivoDemissaoId)"
          label-text="Motivo Demissão"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
        <v-col>
            <v-checkbox v-model="dataModel.temContratoExperiencia" 
                        label="Contrato de Experiência?"
                        color="primary" :disabled="!createMode">
            </v-checkbox>
        </v-col>
      <v-col>
        <select-text
          v-model="dataModel.statusAvisoPrevio"
          :combo-items="listAvisoPrevioStatus"
          combo-item-title="text"
          combo-item-value="value"
          :select-mode="createMode"
          :text-field-value="
            getTextFromListByCodigo(
              listAvisoPrevioStatus,
              dataModel.statusAvisoPrevio
            )
          "
          label-text="Aviso Prévio"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.statusFichaEpi"
          :combo-items="listFieldStatus"
          combo-item-title="text"
          combo-item-value="value"
          :select-mode="createMode"
          :text-field-value="
            getTextFromListByCodigo(
              listFieldStatus,
              dataModel.statusFichaEpi
            )
          "
          label-text="Ficha EPI"
        ></select-text>
        
      </v-col>
    </v-row>
    <v-row v-show="!createMode">
        <v-col>
          <select-text
          v-model="dataModel.statusApontamento"
          :combo-items="listFieldStatus"
          combo-item-title="text"
          combo-item-value="value"
          :select-mode="!createMode && !isReadOnly"
          :text-field-value="
            getTextFromListByCodigo(
              listFieldStatus,
              dataModel.statusApontamento
            )
          "
          label-text="Apontamento"
          
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.statusExameDemissional"
          :combo-items="listExameAdmissionalStatus"
          combo-item-title="text"
          combo-item-value="value"
          :select-mode="!createMode && !isReadOnly && dataModel.statusExameDemissional != 2"
          :text-field-value="
            getTextFromListByCodigo(
              listExameAdmissionalStatus,
              dataModel.statusExameDemissional
            )
          "
          label-text="Exame Demissional"
        ></select-text>
      </v-col>
      <v-col>
        <v-text-field
          label="Data de Crédito"
          type="date"
          variant="outlined"
          color="primary"
          density="compact"
          v-model="dataModel.dataCredito"
          :readonly="createMode || isReadOnly"
          :bg-color="createMode || isReadOnly ? '#f2f2f2' : ''"
          
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row v-show="!createMode">
      <v-col>
          <select-text
          v-model="dataModel.statusBeneficio"
          :combo-items="listFieldStatus"
          combo-item-title="text"
          combo-item-value="value"
          :select-mode="!createMode && !isReadOnly"
          :text-field-value="
            getTextFromListByCodigo(
              listFieldStatus,
              dataModel.statusBeneficio
            )
          "
          label-text="Beneficíos"
          
        ></select-text>
      </v-col>
      <v-col>
          <select-text
          v-model="dataModel.statusReembolso"
          :combo-items="listFieldStatus"
          combo-item-title="text"
          combo-item-value="value"
          :select-mode="!createMode && !isReadOnly"
          :text-field-value="
            getTextFromListByCodigo(
              listFieldStatus,
              dataModel.statusReembolso
            )
          "
          label-text="Reembolso"
          
        ></select-text>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import {
  Desligamento,
  useShareSolicitacaoStore,
} from "@/store/share/solicitacao.store";

import selectText from "../selectText.vue";
import {
  getTextFromListByCodigo,
} from "@/helpers/share/data";
import { watch } from "vue";
import moment from "moment";

const props = defineProps({
  dataModel: {
    type: Object,
    default: function () {
      return new Desligamento();
    },
  },
  createMode: { type: Boolean, default: true },
  isReadOnly: { type: Boolean, default: false },
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

const listMotivoDemissao = useShareSolicitacaoStore().motivosDemissao;
const listAvisoPrevioStatus = useShareSolicitacaoStore().avisoPrevioStatus;
const listFieldStatus = useShareSolicitacaoStore().fieldStatus;
const listExameAdmissionalStatus = useShareSolicitacaoStore().exameAdmissionalStatus;

function getMotivoDemissaoText(motivoId) {
  let _motivo = listMotivoDemissao.find((x) => x.id == motivoId);
  return _motivo ? _motivo.motivo : "tipo desconhecido";
}
</script>
