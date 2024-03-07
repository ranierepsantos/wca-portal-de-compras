<template>
  <div>
    <v-row>
      <v-col cols="4">
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
      <v-col v-show="!createMode">
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
    </v-row>
    <v-row v-show="!createMode">
        <v-col>
        <select-text
          v-model="dataModel.statusFichaEpi"
          :combo-items="listFieldStatus"
          combo-item-title="text"
          combo-item-value="value"
          :select-mode="!createMode && !isReadOnly"
          :text-field-value="
            getTextFromListByCodigo(
              listFieldStatus,
              dataModel.statusFichaEpi
            )
          "
          label-text="Ficha EPI"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.statusExameDemissional"
          :combo-items="listFieldStatus"
          combo-item-title="text"
          combo-item-value="value"
          :select-mode="!createMode && !isReadOnly"
          :text-field-value="
            getTextFromListByCodigo(
              listFieldStatus,
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

defineProps({
  dataModel: {
    type: Object,
    default: function () {
      return new Desligamento();
    },
  },
  createMode: { type: Boolean, default: true },
  isReadOnly: { type: Boolean, default: false },
});
const listMotivoDemissao = useShareSolicitacaoStore().motivosDemissao;
const listAvisoPrevioStatus = useShareSolicitacaoStore().avisoPrevioStatus;
const listFieldStatus = useShareSolicitacaoStore().fieldStatus;

function getMotivoDemissaoText(motivoId) {
  let _motivo = listMotivoDemissao.find((x) => x.id == motivoId);
  return _motivo ? _motivo.motivo : "tipo desconhecido";
}
</script>
