<template>
    <div>
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
   
  defineProps({
    dataModel: {
      type: Object,
      default: function () {
        return new Ferias();
      },
    },
    createMode: { type: Boolean, default: true },
  });

  const tipoFerias = useShareSolicitacaoStore().tipoFerias;
</script>
  