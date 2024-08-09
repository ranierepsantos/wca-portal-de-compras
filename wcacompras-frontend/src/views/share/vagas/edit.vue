<template>
    <div>
      <Breadcrumbs
        title="Vagas"
        :show-button="false"
        :buttons="[{ text: 'Salvar', icon: '', event: 'salvar-click', disabled: isLoading.save }]"
        @salvar-click="salvar()"
      />
      <v-progress-linear
        color="primary"
        indeterminate
        :height="5"
        v-show="isLoading.save || isLoading.form"
      ></v-progress-linear>
      <v-container class="justify-center" v-show="!isLoading.form">
        <v-card class="mx-auto">
          <v-card-text>
            <v-form ref="mForm" lazy-validation>
              <v-row>
                <v-col>
                    <select-text
                        v-model="vaga.clienteId"
                        :combo-items="listCliente"
                        :select-mode="false"
                        label-text="Cliente"
                        :text-field-value ="vaga.clienteNome"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    ></select-text>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                    <select-text
                        v-model="vaga.tipoContratoId"
                        :combo-items="listTipoContrato"
                        :select-mode="false"
                        label-text="Tipo Contrato"
                        :text-field-value ="vaga.tipoContratoNome"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    ></select-text>
                </v-col>
                <v-col>
                    <select-text
                        v-model="vaga.tipoFaturamentoId"
                        :combo-items="listTipoFaturamento"
                        :select-mode="false"
                        label-text="Tipo Faturamento"
                        :text-field-value ="vaga.tipoFaturamentoNome"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    ></select-text>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-textarea
                    variant="outlined"
                    label="Endereço Cliente"
                    class="text-primary"
                    v-model="vaga.enderecoCliente"
                    rows="2"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                  >
                  </v-textarea>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                    <select-text
                        v-model="vaga.gestorId"
                        :combo-items="listGestor"
                        :select-mode="false"
                        label-text="Gestor"
                        :text-field-value ="vaga.gestorNome"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    ></select-text>
                </v-col>
                <v-col>
                    <select-text
                        v-model="vaga.funcaoId"
                        :combo-items="listFuncao"
                        :select-mode="false"
                        label-text="Função"
                        :text-field-value ="vaga.funcaoNome"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    ></select-text>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-text-field
                    variant="outlined"
                    label="Quantidade Vagas"
                    class="text-primary"
                    v-model="vaga.quantidadeVagas"
                    density="compact"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    type="number"
                  >
                  </v-text-field>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.sexoId"
                        :combo-items="listSexo"
                        :select-mode="false"
                        label-text="Sexo"
                        :text-field-value ="vaga.sexoNome"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    ></select-text>
                </v-col>
                <v-col>
                  <v-text-field
                    variant="outlined"
                    label="Idade Miníma"
                    class="text-primary"
                    v-model="vaga.idadeMinima"
                    density="compact"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    type="number"
                  >
                  </v-text-field>
                </v-col>
                <v-col>
                  <v-text-field
                    variant="outlined"
                    label="Idade máxima"
                    class="text-primary"
                    v-model="vaga.idadeMaxima"
                    density="compact"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    type="number"
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
                    v-model="vaga.indicacao"
                    rows="2"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                  >
                  </v-textarea>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <select-text
                        v-model="vaga.motivoContratacaoId"
                        :combo-items="listMotivoContratacao"
                        :select-mode="false"
                        label-text="Motivo Contratação"
                        :text-field-value ="vaga.motivoContratacaoNome"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    ></select-text>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.permiteFumante"
                        :combo-items="[{value: 0, text: 'Não'}, {value: 1, text: 'Sim'}]"
                        :select-mode="false"
                        label-text="Permite Fumante"
                        :text-field-value ="vaga.permiteFumante ==1 ? 'Sim': 'Não'"
                    ></select-text>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.escolaridadeId"
                        :combo-items="listEscolaridade"
                        :select-mode="false"
                        label-text="Escolaridade"
                        :text-field-value ="vaga.escolaridadeNome"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    ></select-text>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-textarea
                    variant="outlined"
                    label="Justificativa"
                    class="text-primary"
                    v-model="vaga.justificativaContratacao"
                    rows="2"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
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
                    v-model="vaga.localResidencia"
                    density="compact"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                  >
                  </v-text-field>
                </v-col>
                <v-col>
                  <v-text-field
                    variant="outlined"
                    label="Experiência profissional"
                    class="text-primary"
                    v-model="vaga.experienciaProfissinal"
                    density="compact"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
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
                    v-model="vaga.descricaoAtividades"
                    rows="2"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                  >
                  </v-textarea>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <select-text
                        v-model="vaga.temCNH"
                        :combo-items="[{value: 0, text: 'Não'},{value: 1, text: 'Sim'}]"
                        :select-mode="false"
                        label-text="Tem CNH"
                        :text-field-value ="vaga.temCNH == 1 ? 'Sim': 'Não'"
                    ></select-text>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.categoriaCNH"
                        :combo-items="[{value: 'A', text: 'A'},{value: 'B', text: 'B'},{value: 'C', text: 'C'},{value: 'D', text: 'D'}, ]"
                        :select-mode="false"
                        label-text="Categoria CNH"
                        :text-field-value ="vaga.categoriaCNH"
                    ></select-text>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.temValeTransporte"
                        :combo-items="[{value: 0, text: 'Não'},{value: 1, text: 'Sim'}]"
                        :select-mode="false"
                        label-text="Vale Transporte"
                        :text-field-value ="vaga.temValeTransporte == 1 ? 'Sim': 'Não'"
                    ></select-text>
                </v-col>
                <v-col>
                  <v-text-field-money
                    label-text="Valor Transporte"
                    v-model="vaga.valorValeTransporte"
                    color="primary"
                    :number-decimal="2"
                    prefix="R$"
                  ></v-text-field-money>
                </v-col>
                <v-col>
                  <v-text-field
                    variant="outlined"
                    label="Quant. dias Vale Transporte"
                    class="text-primary"
                    v-model="vaga.diasValeTransporte"
                    density="compact"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    type="number"
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
                    v-model="vaga.refeicao"
                    density="compact"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                  >
                  </v-text-field>
                </v-col>
                <v-col>
                  <v-text-field
                    variant="outlined"
                    label="Outros benefícios"
                    class="text-primary"
                    v-model="vaga.outrosBeneficios"
                    density="compact"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                  >
                  </v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-text-field-money
                    label-text="Salário base"
                    v-model="vaga.salarioBase"
                    color="primary"
                    :number-decimal="2"
                    prefix="R$"
                  ></v-text-field-money>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.temInsalubridade"
                        :combo-items="[{value: 0, text: 'Não'},{value: 1, text: 'Sim'}]"
                        :select-mode="false"
                        label-text="Insalubridade"
                        :text-field-value ="vaga.temInsalubridade == 1 ? 'Sim': 'Não'"
                    ></select-text>
                </v-col>
                <v-col>
                  <v-text-field-money
                    label-text="Percentual Insalubridade"
                    v-model="vaga.percentualInsalubridade"
                    color="primary"
                    :number-decimal="2"
                    sufix="%"
                  ></v-text-field-money>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.temPericulosidade"
                        :combo-items="[{value: 0, text: 'Não'},{value: 1, text: 'Sim'}]"
                        :select-mode="false"
                        label-text="Periculosidade"
                        :text-field-value ="vaga.temPericulosidade == 1 ? 'Sim': 'Não'"
                    ></select-text>
                </v-col>
                <v-col>
                  <v-text-field-money
                    label-text="Percentual Periculosidade"
                    v-model="vaga.percentualPericulosidade"
                    color="primary"
                    :number-decimal="2"
                    sufix="%"
                  ></v-text-field-money>
                </v-col>
              </v-row>
              <v-row>
                <v-col cols="4">
                  <select-text
                        v-model="vaga.escalaId"
                        :combo-items="listEscala"
                        :select-mode="false"
                        label-text="Escala"
                        :text-field-value ="vaga.escalaNome"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    ></select-text>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.horarioId"
                        :combo-items="listHorario"
                        :select-mode="false"
                        label-text="Horário"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                        :text-field-value ="vaga.horarioNome"
                    ></select-text>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-text-field
                    label="Data Início Prevista"
                    v-model="vaga.dataInicioPrevista"
                    type="Date"
                    variant="outlined"
                    class="text-primary"
                    density="compact"
                  ></v-text-field>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.temCopiaAdmissaoCliente"
                        :combo-items="[{value: 0, text: 'Não'},{value: 1, text: 'Sim'}]"
                        :select-mode="false"
                        label-text="Cópia Admissao Cliente"
                        :text-field-value ="vaga.temCopiaAdmissaoCliente == 1 ? 'Sim': 'Não'"
                    ></select-text>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.temIntegracaoCliente"
                        :combo-items="[{value: 0, text: 'Não'},{value: 1, text: 'Sim'}]"
                        :select-mode="false"
                        label-text="Integração Cliente"
                        :text-field-value ="vaga.temIntegracaoCliente == 1 ? 'Sim': 'Não'"
                    ></select-text>
                </v-col>
                
              </v-row>
              <v-row>
                <v-col>
                  <v-text-field
                    variant="outlined"
                    label="Horário Integração"
                    class="text-primary"
                    v-model="vaga.horarioIntegracao"
                    density="compact"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                  >
                  </v-text-field>
                </v-col>
                <v-col>
                  <v-select
                    label="Dias da Semana"
                    :items="['Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sabádo']"
                    multiple
                    variant="outlined"
                    density="compact"
                    v-model="vaga.integracaoDiasSemana"
                    :readOnly="true"
                  ></v-select>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <box-transfer
                    :list-origem="listDocumentoComplementar"
                    :list-destino="vaga.documentoComplementares"
                    list-origem-titulo="Documentos Complementares"
                    list-destino-titulo="Documentos Complementares"
                    :show-search-text="false"
                    :is-read-only = "true"
                  />
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-textarea
                    variant="outlined"
                    label="Andamentos"
                    class="text-primary"
                    v-model="vaga.anotacoes"
                    rows="2"
                    :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                  ></v-textarea>
                </v-col>
              </v-row>
            </v-form>
          </v-card-text>
        </v-card>
        <historico :eventos="vaga.vagaHistorico" />
      </v-container>
    </div>
  </template>
  
  <script setup>
  import { onBeforeMount, ref,inject } from "vue";
  import handleErrors from "@/helpers/HandleErrors";
  import router from "@/router";
  import moment from "moment";
  import { useRoute } from "vue-router";
  //Components
  import selectText from "@/components/selectText.vue"
  import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
  import boxTransfer from "@/components/boxTransfer.vue";
  import historico from "@/components/reembolso/historico.vue";
  import Breadcrumbs from "@/components/breadcrumbs.vue";
  //Stores
  import { useAuthStore } from "@/store/auth.store";
  import {Vaga, useShareVagaStore} from "@/store/share/vaga.store";
  import {useShareEntidadeAuxiliarStore} from "@/store/share/entidadesauxiliares.store"
  import { useShareClienteStore } from "@/store/share/cliente.store";
  import { useShareUsuarioStore } from "@/store/share/usuario.store";

  const isLoading = ref({
    form: true,
    save: false,
  });
  const route = useRoute();
  const mForm = ref(null);
  const swal = inject("$swal");
  const vaga = ref(new Vaga())
  const vagaStore = useShareVagaStore()
  const entidadeStore = useShareEntidadeAuxiliarStore();
  const listCliente = ref([]);
  const listDocumentoComplementar = ref([]);
  const listEscala = ref([]);
  const listEscolaridade = ref([]);
  const listFuncao = ref([]);
  const listGestor = ref([]);
  const listHorario = ref([]);
  const listMotivoContratacao = ref([]);
  const listSexo = ref([]);
  const listTipoContrato = ref([]);
  const listTipoFaturamento = ref([]);
  const listResponsavel = ref([]);
  //VUE FUNCTIONS
  onBeforeMount(async () => {
    try {
      
      if (isNaN(route.query.id))
        throw new Error("Parâmentro incorreto!");
        

      listCliente.value = await useShareClienteStore().toComboList(0, useAuthStore().user.id);
      listDocumentoComplementar.value = await entidadeStore.getToComboList("DocumentoComplementar");
      listEscala.value = await entidadeStore.getToComboList("Escala");
      listEscolaridade.value = await entidadeStore.getToComboList("Escolaridade");
      listFuncao.value = await entidadeStore.getToComboList("Funcao");
      listGestor.value = await entidadeStore.getToComboList("Gestor");
      listHorario.value = await entidadeStore.getToComboList("Horario");
      listMotivoContratacao.value = await entidadeStore.getToComboList("MotivoContratacao");
      listTipoContrato.value = await entidadeStore.getToComboList("TipoContrato");
      listTipoFaturamento.value = await entidadeStore.getToComboList("TipoFaturamento");
      listSexo.value = [{value: 3, text: "Indiferente"},{value: 2, text: "Feminino"},{value: 1, text: "Masculino"}]

      vaga.value = await vagaStore.getById(route.query.id)
      documentoComplementarListRemove()

      listResponsavel.value = await useShareUsuarioStore()
                                    .getListByCliente(vaga.value.clienteId);

    } catch (error) {
      console.debug("create.beforeMount.error", error);
      handleErrors(error);
    } finally {
      isLoading.value.form = false;
    }
  });
  
  
  //FUNCTIONS
  async function salvar() {
    try {
      isLoading.value.save = true;
  
      const { valid } = await mForm.value.validate();
  
      if (valid) {
        let data = {...vaga.value}
        data.UsuarioAtualizador = useAuthStore().user.nome;
        if (data.responsavelId && data.responsavelId > 0) {
            let status = vagaStore().statusSolicitacao.find(
                (x) => x.status.toLowerCase() == "em andamento"
            );
            if (status && data.statusSolicitacaoId != status.id)
                data.statusSolicitacaoId = status.id;
        }

        await vagaStore.update(data);

        await swal.fire({
          toast: true,
          icon: "success",
          index: "top-end",
          title: "Sucesso!",
          text: "Solicitação criada com sucesso!",
          showConfirmButton: false,
          timer: 2000,
        });

        router.push({ name: "shareVagas"});
        
      }
    } catch (error) {
      console.debug('salvar.error', error);
      handleErrors(error);
    }finally {
      isLoading.value.save = false;
    }
  }

  function documentoComplementarListRemove(removerTodos = false) {
  if (removerTodos == true) listDocumentoComplementar.value.splice(0, listDocumentoComplementar.value.length);
  else {
    vaga.value.documentoComplementares.forEach((cc) => {
      let index = listDocumentoComplementar.value.findIndex((c) => c.value == cc.value);
      if (index > -1) listDocumentoComplementar.value.splice(index, 1);
    });
  }
}

</script>
  