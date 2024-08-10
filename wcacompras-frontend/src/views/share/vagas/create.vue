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
                        :select-mode="true"
                        label-text="Cliente"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    ></select-text>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                    <select-text
                        v-model="vaga.tipoContratoId"
                        :combo-items="listEntidade['tipocontrato']"
                        :select-mode="true"
                        label-text="Tipo Contrato"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                        :button-show="true"
                        button-title="Adicionar Tipo de Contrato"
                        @button-click="abrirCadastroAuxiliar('TipoContrato')"
                    ></select-text>
                </v-col>
                <v-col>
                    <select-text
                        v-model="vaga.tipoFaturamentoId"
                        :combo-items="listEntidade['tipofaturamento']"
                        :select-mode="true"
                        label-text="Tipo Faturamento"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                        :button-show="true"
                        button-title="Adicionar Tipo de Faturamento"
                        @button-click="abrirCadastroAuxiliar('TipoFaturamento')"
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
                        :combo-items="listEntidade['gestor']"
                        :select-mode="true"
                        label-text="Gestor"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                        :button-show="true"
                        button-title="Adicionar Gestor"
                        @button-click="abrirCadastroAuxiliar('Gestor')"
                    ></select-text>
                </v-col>
                <v-col>
                    <select-text
                        v-model="vaga.funcaoId"
                        :combo-items="listEntidade['funcao']"
                        :select-mode="true"
                        label-text="Função"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                        :button-show="true"
                        button-title="Adicionar Função"
                        @button-click="abrirCadastroAuxiliar('Funcao')"
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
                        :select-mode="true"
                        label-text="Sexo"
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
                        :select-mode="true"
                        label-text="Motivo Contratação"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                    ></select-text>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.permiteFumante"
                        :combo-items="[{value: 0, text: 'Não'}, {value: 1, text: 'Sim'}]"
                        :select-mode="true"
                        label-text="Permite Fumante"
                    ></select-text>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.escolaridadeId"
                        :combo-items="listEntidade['escolaridade']"
                        :select-mode="true"
                        label-text="Escolaridade"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                        :button-show="true"
                        button-title="Adicionar Escolaridade"
                        @button-click="abrirCadastroAuxiliar('Escolaridade')"
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
                        :select-mode="true"
                        label-text="Tem CNH"
                    ></select-text>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.categoriaCNH"
                        :combo-items="[{value: 'A', text: 'A'},{value: 'B', text: 'B'},{value: 'C', text: 'C'},{value: 'D', text: 'D'}, ]"
                        :select-mode="true"
                        label-text="Categoria CNH"
                    ></select-text>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.temValeTransporte"
                        :combo-items="[{value: 0, text: 'Não'},{value: 1, text: 'Sim'}]"
                        :select-mode="true"
                        label-text="Vale Transporte"
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
                        :select-mode="true"
                        label-text="Insalubridade"
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
                        :select-mode="true"
                        label-text="Periculosidade"
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
                        :combo-items="listEntidade['escala']"
                        :select-mode="true"
                        label-text="Escala"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                        :button-show="true"
                        button-title="Adicionar Escala"
                        @button-click="abrirCadastroAuxiliar('Escala')"
                    ></select-text>
                </v-col>
                <v-col>
                    <select-text
                        v-model="vaga.horarioId"
                        :combo-items="listEntidade['horario']"
                        :select-mode="true"
                        label-text="Horário"
                        :field-rules="[(v) => !!v || 'Campo é obrigatório']"
                        :button-show="true"
                        :button-loading="false"
                        button-title="Adicionar Horário"
                        @button-click="abrirCadastroAuxiliar('Horario')"
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
                        :select-mode="true"
                        label-text="Cópia Admissao Cliente"
                    ></select-text>
                </v-col>
                <v-col>
                  <select-text
                        v-model="vaga.temIntegracaoCliente"
                        :combo-items="[{value: 0, text: 'Não'},{value: 1, text: 'Sim'}]"
                        :select-mode="true"
                        label-text="Integração Cliente"
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
                  ></v-select>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <box-transfer
                    :list-origem="listEntidade['documentocomplementar']"
                    :list-destino="vaga.documentoComplementares"
                    list-origem-titulo="Documentos Complementares"
                    list-destino-titulo="Documentos Complementares"
                    :show-search-text="false"
                    :plus-button-show="true"
                    plus-button-title="Adicionar Novo Documento Complementar"
                    @plus-click="abrirCadastroAuxiliar('DocumentoComplementar')"
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
      </v-container>
      <v-dialog v-model="dialog" max-width="700" :absolute="false">
        <v-card>
          <v-card-title class="text-primary text-h5">
            {{ dialogTitle }}
          </v-card-title>
          <v-card-text>
            <v-form @submit.prevent="salvarEntidade()" ref="entidadeForm">
              <v-row>
                <v-col>
                  <v-text-field label="Nome" v-model="entidade.nome" type="text" required variant="outlined" color="primary"
                    :rules="[(v) => !!v || 'Nome é obrigatório']" density="compact"></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col class="text-right">
                  <v-btn variant="outlined" color="primary" @click="fecharDialog()" :disabled="isSavingEntity">Cancelar</v-btn>
                  <v-btn color="primary" type="submit" class="ml-3" :disabled="isSavingEntity">Salvar</v-btn>
                  <v-fade-transition leave-absolute>
                    <v-progress-circular
                      v-show="isSavingEntity"
                      color="primary"
                      size="24"
                      :width="2"
                      indeterminate
                      class="ml-3"
                    ></v-progress-circular>
                  </v-fade-transition>
                </v-col>
              </v-row>
            </v-form>
          </v-card-text>
          <v-card-actions> </v-card-actions>
        </v-card>
      </v-dialog>
    </div>
  </template>
  
  <script setup>
  import Breadcrumbs from "@/components/breadcrumbs.vue";
  import { onBeforeMount, ref,inject } from "vue";
  import handleErrors from "@/helpers/HandleErrors";
  import router from "@/router";
  import moment from "moment";
  import selectText from "@/components/selectText.vue"
  import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
  import boxTransfer from "@/components/boxTransfer.vue";
  //Stores
  import { useAuthStore } from "@/store/auth.store";
  import {Vaga, useShareVagaStore} from "@/store/share/vaga.store";
  import {EntidadeAuxiliar, useShareEntidadeAuxiliarStore} from "@/store/share/entidadesauxiliares.store"
  import { useShareClienteStore } from "@/store/share/cliente.store";
  

  const isLoading = ref({
    form: true,
    save: false,
  });
  const mForm = ref(null);
  const swal = inject("$swal");
  const vaga = ref(new Vaga())
  const vagaStore = useShareVagaStore()
  const entidadeStore = useShareEntidadeAuxiliarStore();
  const listCliente = ref([]);
  const listMotivoContratacao = ref([]);
  const listSexo = ref([]);
  const dialog = ref(false)
  const dialogTitle = ref("")
  const entidadeForm = ref(null)
  const entidade = ref(new EntidadeAuxiliar());
  const entidadeTipo = ref(null)
  const entidadeTipos = ref([
    {title: "Documento Complementar", type: "DocumentoComplementar"},
    {title: "Escala", type: "Escala"},
    {title: "Escolaridade", type: "Escolaridade"},
    {title: "Função", type: "Funcao"},
    {title: "Gestor", type: "Gestor"},
    {title: "Horário", type: "Horario"},
    {title: "Tipo de Contrato", type: "TipoContrato"},
    {title: "Tipo de Faturamento", type: "TipoFaturamento"},
  ])
  const listEntidade = ref({
    documentocomplementar: [],
    escala : [],
    escolaridade : [],
    funcao : [],
    gestor : [],
    horario: [],
    tipocontrato: [],
    tipofaturamento: []
  })
  const isSavingEntity = ref(false)


  //VUE FUNCTIONS
  onBeforeMount(async () => {
    try {
      listCliente.value = await useShareClienteStore().toComboList(0, useAuthStore().user.id);
      listEntidade.value["documentocomplementar"] = await entidadeStore.getToComboList("DocumentoComplementar");
      listEntidade.value["escala"] = await entidadeStore.getToComboList("Escala");
      listEntidade.value["escolaridade"] = await entidadeStore.getToComboList("Escolaridade");
      listEntidade.value["funcao"] = await entidadeStore.getToComboList("Funcao");
      listEntidade.value["gestor"] = await entidadeStore.getToComboList("Gestor");
      listEntidade.value["horario"] = await entidadeStore.getToComboList("Horario");
      listMotivoContratacao.value = await entidadeStore.getToComboList("MotivoContratacao");
      listEntidade.value["tipocontrato"] = await entidadeStore.getToComboList("TipoContrato");
      listEntidade.value["tipofaturamento"] = await entidadeStore.getToComboList("TipoFaturamento");
      listSexo.value = [{value: 3, text: "Indiferente"},{value: 2, text: "Feminino"},{value: 1, text: "Masculino"}]
    } catch (error) {
      console.debug("create.beforeMount.error", error);
      handleErrors(error);
    } finally {
      isLoading.value.form = false;
    }
  });
  
  
  //FUNCTIONS
  function abrirCadastroAuxiliar(_entidade) {
    
    entidadeTipo.value = entidadeTipos.value.find(q =>  q.type == _entidade);

    entidade.value = new EntidadeAuxiliar();
    dialogTitle.value = `Novo(a) ${entidadeTipo.value.title}`;
    dialog.value  = true
  }
  
  function fecharDialog()
  {
    dialog.value = false;
    entidadeForm.value.reset()
    entidade.value = new EntidadeAuxiliar();
    entidadeTipo.value = null;
  }

  async function salvar() {
    try {
      isLoading.value.save = true;
  
      const { valid } = await mForm.value.validate();
  
      if (valid) {
        let data = {...vaga.value}
        data.usuarioCriador = useAuthStore().user.nome;
        await vagaStore.add(data);

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

  async function salvarEntidade()
  {
    try
    {
      let { valid } = await entidadeForm.value.validate();
      if (valid)
      {
        isSavingEntity.value = true
        let data = entidade.value;
        let response = null
        if (data.id == 0)
        {
          response = await useShareEntidadeAuxiliarStore().add(entidadeTipo.value.type, data);
        } else
        {
          response = await useShareEntidadeAuxiliarStore().update(entidadeTipo.value.type, data);
        }
        
        listEntidade.value[entidadeTipo.value.type.toLowerCase()] = await entidadeStore.getToComboList(entidadeTipo.value.type);

        fecharDialog();
        
        swal.fire({
          toast: true,
          icon: "success",
          position: "top-end",
          title: "Sucesso!",
          text: "Dados salvos com sucesso!",
          showConfirmButton: false,
          timer: 2000,
        })
      }
    } catch (error)
    {
      console.error("salvarEntidade.error:", error);
      handleErrors(error)
    }finally {
      isSavingEntity.value = false
    }
  }
  </script>
  