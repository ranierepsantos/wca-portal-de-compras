<template>
  <div>
    <v-row>
      <v-col>
        <select-text
          v-model="dataModel.tipoContratoId"
          :combo-items="listTipoContrato"
          :select-mode="!isReadOnly"
          label-text="Tipo Contrato"
          :text-field-value="dataModel.tipoContratoNome"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :button-show="!isReadOnly"
          button-title="Adicionar Tipo Contrato"
          @button-click="$emit('selectButtonClick', 'TipoContrato')"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.tipoFaturamentoId"
          :combo-items="listTipoFaturamento"
          :select-mode="!isReadOnly"
          label-text="Tipo Faturamento"
          :text-field-value="dataModel.tipoFaturamentoNome"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :button-show="!isReadOnly"
          button-title="Adicionar Tipo Faturamento"
          @button-click="$emit('selectButtonClick', 'TipoFaturamento')"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-textarea
          variant="outlined"
          label="Endereço Cliente"
          class="text-primary"
          v-model="dataModel.enderecoCliente"
          rows="2"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-textarea>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <select-text
          v-model="dataModel.gestorId"
          :combo-items="listGestor"
          :select-mode="!isReadOnly"
          label-text="Gestor"
          :text-field-value="dataModel.gestorNome"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :button-show="!isReadOnly"
          button-title="Adicionar Gestor"
          @button-click="$emit('selectButtonClick', 'Gestor')"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.funcaoId"
          :combo-items="listFuncao"
          :select-mode="!isReadOnly"
          label-text="Função"
          :text-field-value="dataModel.funcaoNome"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :button-show="!isReadOnly"
          button-title="Adicionar Função"
          @button-click="$emit('selectButtonClick', 'Funcao')"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-text-field
          variant="outlined"
          label="Quantidade Vagas"
          class="text-primary"
          v-model="dataModel.quantidadeVagas"
          density="compact"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          type="number"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-text-field>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.sexoId"
          :combo-items="listSexo"
          :select-mode="!isReadOnly"
          label-text="Sexo"
          :text-field-value="dataModel.sexoNome"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
      <v-col>
        <v-text-field
          variant="outlined"
          label="Idade Miníma"
          class="text-primary"
          v-model="dataModel.idadeMinima"
          density="compact"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          type="number"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-text-field>
      </v-col>
      <v-col>
        <v-text-field
          variant="outlined"
          label="Idade máxima"
          class="text-primary"
          v-model="dataModel.idadeMaxima"
          density="compact"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          type="number"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-textarea
          variant="outlined"
          label="Indicação Profissional"
          class="text-primary"
          v-model="dataModel.indicacao"
          rows="2"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-textarea>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <select-text
          v-model="dataModel.motivoContratacaoId"
          :combo-items="listMotivoContratacao"
          :select-mode="!isReadOnly"
          label-text="Motivo Contratação"
          :text-field-value="dataModel.motivoContratacaoNome"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.permiteFumante"
          :combo-items="optSimNao"
          :select-mode="!isReadOnly"
          label-text="Permite Fumante"
          :text-field-value="dataModel.permiteFumante == 1 ? 'Sim' : 'Não'"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.escolaridadeId"
          :combo-items="listEscolaridade"
          :select-mode="!isReadOnly"
          label-text="Escolaridade"
          :text-field-value="dataModel.escolaridadeNome"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :button-show="!isReadOnly"
          button-title="Adicionar Escolaridade"
          @button-click="$emit('selectButtonClick', 'Escolaridade')"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-textarea
          variant="outlined"
          label="Justificativa"
          class="text-primary"
          v-model="dataModel.justificativaContratacao"
          rows="2"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-textarea>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-text-field
          variant="outlined"
          label="Local residência"
          class="text-primary"
          v-model="dataModel.localResidencia"
          density="compact"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-text-field>
      </v-col>
      <v-col>
        <v-text-field
          variant="outlined"
          label="Experiência profissional"
          class="text-primary"
          v-model="dataModel.experienciaProfissinal"
          density="compact"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-textarea
          variant="outlined"
          label="Descrição atividades"
          class="text-primary"
          v-model="dataModel.descricaoAtividades"
          rows="2"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-textarea>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <select-text
          v-model="dataModel.temCNH"
          :combo-items="optSimNao"
          :select-mode="!isReadOnly"
          label-text="Tem CNH"
          :text-field-value="dataModel.temCNH == 1 ? 'Sim' : 'Não'"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.categoriaCNH"
          :combo-items="[
            { value: 'A', text: 'A' },
            { value: 'B', text: 'B' },
            { value: 'C', text: 'C' },
            { value: 'D', text: 'D' },
          ]"
          :select-mode="!isReadOnly"
          label-text="Categoria CNH"
          :text-field-value="dataModel.categoriaCNH"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.temValeTransporte"
          :combo-items="optSimNao"
          :select-mode="!isReadOnly"
          label-text="Vale Transporte"
          :text-field-value="dataModel.temValeTransporte == 1 ? 'Sim' : 'Não'"
        ></select-text>
      </v-col>
      <v-col>
        <v-text-field-money
          label-text="Valor Transporte"
          v-model="dataModel.valorValeTransporte"
          color="primary"
          :number-decimal="2"
          prefix="R$"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        ></v-text-field-money>
      </v-col>
      <v-col>
        <v-text-field
          variant="outlined"
          label="Quant. dias Vale Transporte"
          class="text-primary"
          v-model="dataModel.diasValeTransporte"
          density="compact"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          type="number"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-text-field
          variant="outlined"
          label="Refeição"
          class="text-primary"
          v-model="dataModel.refeicao"
          density="compact"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-text-field>
      </v-col>
      <v-col>
        <v-text-field
          variant="outlined"
          label="Outros benefícios"
          class="text-primary"
          v-model="dataModel.outrosBeneficios"
          density="compact"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-text-field-money
          label-text="Salário base"
          v-model="dataModel.salarioBase"
          color="primary"
          :number-decimal="2"
          prefix="R$"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        ></v-text-field-money>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.temInsalubridade"
          :combo-items="optSimNao"
          :select-mode="!isReadOnly"
          label-text="Insalubridade"
          :text-field-value="dataModel.temInsalubridade == 1 ? 'Sim' : 'Não'"
        ></select-text>
      </v-col>
      <v-col>
        <v-text-field-money
          label-text="Percentual Insalubridade"
          v-model="dataModel.percentualInsalubridade"
          color="primary"
          :number-decimal="2"
          sufix="%"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        ></v-text-field-money>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.temPericulosidade"
          :combo-items="optSimNao"
          :select-mode="!isReadOnly"
          label-text="Periculosidade"
          :text-field-value="dataModel.temPericulosidade == 1 ? 'Sim' : 'Não'"
        ></select-text>
      </v-col>
      <v-col>
        <v-text-field-money
          label-text="Percentual Periculosidade"
          v-model="dataModel.percentualPericulosidade"
          color="primary"
          :number-decimal="2"
          sufix="%"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        ></v-text-field-money>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="4">
        <select-text
          v-model="dataModel.escalaId"
          :combo-items="listEscala"
          :select-mode="!isReadOnly"
          label-text="Escala"
          :text-field-value="dataModel.escalaNome"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :button-show="!isReadOnly"
          button-title="Adicionar Escala"
          @button-click="$emit('selectButtonClick', 'Escala')"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.horarioId"
          :combo-items="listHorario"
          :select-mode="!isReadOnly"
          label-text="Horário"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :text-field-value="dataModel.horarioNome"
          :button-show="!isReadOnly"
          button-title="Adicionar Horário"
          @button-click="$emit('selectButtonClick','Horario')"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-text-field
          label="Data Início Prevista"
          v-model="dataModel.dataInicioPrevista"
          type="Date"
          variant="outlined"
          class="text-primary"
          density="compact"
          :readOnly="isReadOnly && statusSolicitacao == 3"
          :bg-color="isReadOnly && statusSolicitacao == 3 ? '#f2f2f2' : ''"
        ></v-text-field>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.temCopiaAdmissaoCliente"
          :combo-items="optSimNao"
          :select-mode="!isReadOnly"
          label-text="Cópia Admissao Cliente"
          :text-field-value="dataModel.temCopiaAdmissaoCliente == 1 ? 'Sim' : 'Não'"
        ></select-text>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.temIntegracaoCliente"
          :combo-items="optSimNao"
          :select-mode="!isReadOnly"
          label-text="Integração Cliente"
          :text-field-value="dataModel.temIntegracaoCliente == 1 ? 'Sim' : 'Não'"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-text-field
          variant="outlined"
          label="Horário Integração"
          class="text-primary"
          v-model="dataModel.horarioIntegracao"
          density="compact"
          :field-rules="[(v) => !!v || 'Campo é obrigatório']"
          :readOnly="isReadOnly"
          :bg-color="isReadOnly ? '#f2f2f2' : ''"
        >
        </v-text-field>
      </v-col>
      <v-col>
        <select-text
          v-model="dataModel.integracaoDiasSemanas"
          :combo-items="listDiasSemana"
          :select-mode="!isReadOnly"
          label-text="Dias da Semana"
          :text-field-value="integracaoDiasSemanaText"
          :is-multiple="true"
        ></select-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <box-transfer
          :list-origem="listDocumentoComplementar"
          :list-destino="dataModel.documentoComplementares"
          list-origem-titulo="Documentos Complementares"
          list-destino-titulo="Documentos Complementares"
          :show-search-text="false"
          :is-read-only="isReadOnly"
          :plus-button-show="!isReadOnly"
          plus-button-title="Adicionar Documento Complementar"
          @plus-click="$emit('selectButtonClick', 'DocumentoComplementar')"
          v-show="!isReadOnly"
        />
        <v-textarea
              variant="outlined"
              label="Documentos Complementares"
              class="text-primary"
              :readOnly = "!createMode"
              :bg-color="!createMode ? '#f2f2f2' : ''"
              :model-value="getDocumentosComplementaresDescricao()"
              v-show="isReadOnly"
              rows="2"
        ></v-textarea>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-textarea
          variant="outlined"
          label="Andamento Recrutamento"
          class="text-primary"
          v-model="dataModel.anotacoes"
          :readOnly="isAndamentoReadOnly"
          :bg-color="isAndamentoReadOnly ? '#f2f2f2' : ''"
        >
        </v-textarea>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import { computed } from "vue";
import boxTransfer from "@/components/boxTransfer.vue";
import selectText from "@/components/selectText.vue";
import { Vaga } from "@/store/share/solicitacao.store";
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";

const props = defineProps({
  dataModel: {
    type: Object,
    default: function () {
      return new Vaga();
    },
  },
  statusSolicitacao:
  {
    type: Number,
    default: 0
  },
  isReadOnly: { type: Boolean, default: false },
  listDocumentoComplementar: {
    type: Array,
    default: function () {
      return [];
    },
  },
  listEscala: {
    type: Array,
    default: function () {
      return [];
    },
  },
  listEscolaridade: {
    type: Array,
    default: function () {
      return [];
    },
  },
  listFuncao: {
    type: Array,
    default: function () {
      return [];
    },
  },
  listGestor: {
    type: Array,
    default: function () {
      return [];
    },
  },
  listHorario: {
    type: Array,
    default: function () {
      return [];
    },
  },
  listMotivoContratacao: {
    type: Array,
    default: function () {
      return [];
    },
  },
  listSexo: {
    type: Array,
    default: function () {
      return [];
    },
  },
  listTipoContrato: {
    type: Array,
    default: function () {
      return [];
    },
  },
  listTipoFaturamento: {
    type: Array,
    default: function () {
      return [];
    },
  },
  isAndamentoReadOnly: {type: Boolean, default: false}
});

const listDiasSemana = [
  { value: "Segunda", text: "Segunda" },
  { value: "Terça", text: "Terça" },
  { value: "Quarta", text: "Quarta" },
  { value: "Quinta", text: "Quinta" },
  { value: "Sexta", text: "Sexta" },
  { value: "Sabádo", text: "Sabádo" },
];

const optSimNao = [
  { value: 0, text: 'Não' },
  { value: 1, text: 'Sim' },
];

const integracaoDiasSemanaText = computed(() => {
  if (props.dataModel.integracaoDiasSemana)
    return props.dataModel.integracaoDiasSemana.join(", ");
  else return "";
});

function getDocumentosComplementaresDescricao () {
  let text = "";
  props.dataModel.documentoComplementares.forEach(item => {
    text += item.text + "; "
  });
  return text.trimEnd();

}
</script>
