<template>
  <div>
    <Breadcrumbs
      :title="getPageTitle(solicitacao.solicitacaoTipoId)"
      :show-button="false"
      :buttons="buttons"
      @salvar-click="salvar()"
      @aprovar-click="openAprovacaoForm = true"
      @nofiticar-click="openNotificacao = true"
      @finalizar-click="FinalizarSolicitacao()"
      @gerar-pdf-click="gerarPDF()"
      @cancelar-click="cancelarSolicitacao()"
      @editar-click="openModeEdicaoForm=true"
      @reabrir-click="openReabrirSolicitacao=true"
    />
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isBusy.save || isBusy.form"
    ></v-progress-linear>
    <v-container class="justify-center" v-show="!isBusy.form">
      <v-card class="mx-auto">
        <v-card-text>
          <v-form ref="mForm" lazy-validation>
            <SolicitacaoForm
              :solicitacao="solicitacao"
              :list-clientes="[]"
              :descricao-label="
                getObservacaoLabelDescricao(solicitacao.solicitacaoTipoId)
              "
              :list-funcionarios="[]"
              :list-centro-custos="[]"
              :combo-tipo-show="comboTipoShow"
              :list-responsavel="responsavelList"
              :list-status="useShareSolicitacaoStore().statusSolicitacao.filter(q => 'info, warning'.includes(q.color))"
              :is-read-only="modeReadOnly "
              :is-descricao-read-only="descricaoReadOnly"
            >
              <desligamento
                :data-model="solicitacao.desligamento"
                :create-mode="false"
                :edit-mode="isEditMode"
                v-if="solicitacao.solicitacaoTipoId == 1"
                :is-read-only="modeReadOnly"
                :data-admissao="solicitacao.funcionarioDataAdmissao"
                :list-funcionarios="funcionarioList"
              />
              <Comunicado
                v-else-if="solicitacao.solicitacaoTipoId == 2"
                :data-model="solicitacao.comunicado"
                :create-mode="isEditMode"
                :list-funcionarios="funcionarioList"
              />
              <Ferias
                v-else-if="solicitacao.solicitacaoTipoId == 3"
                :data-model="solicitacao.ferias"
                :create-mode="isEditMode"
                :list-funcionarios="funcionarioList"
              />
              <Mudancabase
                v-else-if="solicitacao.solicitacaoTipoId == 4"
                :data-model="solicitacao.mudancaBase"
                :list-clientes="[]"
                :list-centro-custos="[]"
                :list-itens-mudanca="listItensMudanca"
                :cliente-selected="
                  solicitacao.clienteId && solicitacao.clienteId > 0
                "
                :create-mode="isEditMode"
                :is-read-only="modeReadOnly"
                :list-funcionarios="funcionarioList"
              />
              <vaga
                v-else-if="solicitacao.solicitacaoTipoId == 5"
                :list-documento-complementar="listEntidade['documentocomplementar']"
                :list-escala="listEntidade['escala']"
                :list-escolaridade="listEntidade['escolaridade']"
                :list-funcao="listEntidade['funcao']"
                :list-gestor="listEntidade['gestor']"
                :list-horario="listEntidade['horario']"
                :list-motivo-contratacao="listEntidade['motivocontratacao']"
                :list-sexo="listEntidade['sexo']"
                :list-tipo-contrato="listEntidade['tipocontrato']"
                :list-tipo-faturamento="listEntidade['tipofaturamento']"
                :is-read-only="!isEditMode"
                :data-model="solicitacao.vaga"
                :status-solicitacao="solicitacao.statusSolicitacaoId"
                :is-andamento-read-only="modeReadOnly"
                @select-button-click="abrirCadastroAuxiliar($event)"

                
              />
            </SolicitacaoForm>
            <v-card>
              <v-card-text>
                <table-file-upload
                  :anexos="solicitacao.anexos"
                  :is-read-only="modeReadOnly"
                  :combo-items="tableUploadItems"
                  @change-status="changeFieldStatus($event)"
                />
              </v-card-text>
            </v-card>
            <Historico
              :eventos="solicitacao.historico.sort(compararValor('dataHora','desc'))"
              style="margin-top: 15px"
            />
          </v-form>
        </v-card-text>
      </v-card>
      <v-card class="mx-auto"> </v-card>
    </v-container>
    <v-dialog v-model="entidadeDialog" max-width="700" :absolute="false">
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
    <v-dialog
      v-model="openNotificacao"
      max-width="800"
      :absolute="false"
      persistent
    >
      <NotificacaoEnvio
        :usuario-list="responsavelList"
        @closeForm="openNotificacao = false"
        :entidade="permissao.charAt(0).toUpperCase() + permissao.slice(1)"
        :entidade-id="solicitacao.id"
      />
    </v-dialog>
    <!-- FORM PARA APROVAR / REJEITAR PEDIDO -->
    <v-dialog
      v-model="openAprovacaoForm"
      max-width="700"
      :absolute="false"
      persistent
    >
      <aprovar-rejeitar-form
        :title="
          getPageTitle(solicitacao.solicitacaoTipoId) + ' - Aprovar / Reprovar'
        "
        @aprovar-click="aprovarReprovar(true, $event)"
        @reprovar-click="aprovarReprovar(false, $event)"
        @close-form="openAprovacaoForm = false"
        :is-running-event="isRunningEvent"
        reprovar-title="Reprovar"
      />
    </v-dialog>
    <!--FORM PARA INFORMAR MOTIVO DA EDIÇÃO E HABILITAR MODO EDIÇÃO-->
    <v-dialog
      v-model="openModeEdicaoForm"
      max-width="700"
      :absolute="false"
      persistent
    >
      <aprovar-rejeitar-form
        :title="
          getPageTitle(solicitacao.solicitacaoTipoId) + ' - Habilitar Edição'
        "
        @aprovar-click="openModeEdicaoForm = false"
        aprovar-color="secondary"
        reprovar-color="success"
        @reprovar-click="habilitarEdicao($event)"
        @close-form="openModeEdicaoForm = false"
        :is-running-event="isRunningEvent"
        reprovar-title="Habilitar"
        aprovar-title="Cancelar"
      />
    </v-dialog>
    <!-- FORM PARA REABRIR SOLICITAÇÃO-->
    <v-dialog
      v-model="openReabrirSolicitacao"
      max-width="700"
      :absolute="false"
      persistent
    >
      <aprovar-rejeitar-form
        :title="
          getPageTitle(solicitacao.solicitacaoTipoId) + ' - Reabrir Solicitação'
        "
        @aprovar-click="openReabrirSolicitacao = false"
        aprovar-color="secondary"
        reprovar-color="success"
        @reprovar-click="reabrirSolicitacao($event)"
        @close-form="openReabrirSolicitacao = false"
        :is-running-event="isRunningEvent"
        reprovar-title="Reabrir"
        aprovar-title="Cancelar"
      />
    </v-dialog>
  </div>
</template>

<script setup>
import Breadcrumbs from "@/components/breadcrumbs.vue";
import SolicitacaoForm from "@/components/share/solicitacaoForm.vue";
import desligamento from "@/components/share/desligamento.vue";
import vaga from "@/components/share/vaga.vue";
import { Solicitacao } from "@/store/share/solicitacao.store";
import { ref } from "vue";
import Comunicado from "@/components/share/comunicado.vue";
import Ferias from "@/components/share/ferias.vue";
import Mudancabase from "@/components/share/mudancabase.vue";
import tableFileUpload from "@/components/share/tableFileUpload.vue";
import handleErrors from "@/helpers/HandleErrors";
import { useAuthStore } from "@/store/auth.store";
import { useShareSolicitacaoStore } from "@/store/share/solicitacao.store";
import { watch } from "vue";
import { useRoute } from "vue-router";
import { onBeforeMount } from "vue";
import {
  getObservacaoLabelDescricao,
  getPageTitle,
} from "@/helpers/share/data";
import { useShareUsuarioStore } from "@/store/share/usuario.store";
import router from "@/router";
import { inject } from "vue";
import Historico from "@/components/reembolso/historico.vue";
import NotificacaoEnvio from "@/components/share/notificacaoEnvio.vue";
import aprovarRejeitarForm from "@/components/aprovarRejeitarForm.vue";
import moment from "moment";
import { computed } from "vue";
import { EntidadeAuxiliar, useShareEntidadeAuxiliarStore } from "@/store/share/entidadesauxiliares.store";
import { useShareFuncionarioStore } from "@/store/share/funcionario.store";
import { compararValor } from "@/helpers/functions";

const tableUploadItems = ref([{ text: "Outros" }]);
const isBusy = ref({
  form: true,
  save: false,
});
const solicitacao = ref(new Solicitacao());
const comboTipoShow = ref(true);
const responsavelList = ref([]);
const route = useRoute();
const mForm = ref(null);
const swal = inject("$swal");
const openNotificacao = ref(false);
const openAprovacaoForm = ref(false);
const isRunningEvent = ref(false);
const permissao = ref("");
const entidadeStore = useShareEntidadeAuxiliarStore();
const buttons = ref([])
const funcionarioList = ref([])
const centrosCustoList = ref([])
const listEntidade = ref({
  documentocomplementar: [],
  escala: [],
  escolaridade: [],
  funcao: [],
  gestor: [],
  horario: [],
  motivocontratacao: [],
  sexo: [],
  tipocontrato: [],
  tipofaturamento: [],
});
const isEditMode = ref(false);
const listItensMudanca = ref([]);

const entidadeDialog = ref(false)
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
  {title: "Motivo Contratação", type: "MotivoContratacao"},
  {title: "Tipo de Contrato", type: "TipoContrato"},
  {title: "Tipo de Faturamento", type: "TipoFaturamento"},
])
const isSavingEntity = ref(false)
const openModeEdicaoForm = ref(false)
const openReabrirSolicitacao = ref(false)
let solicitacaoOriginal = null
//VUE FUNCTIONS
onBeforeMount(async () => {
  try {
    await useShareSolicitacaoStore().listarStatusSolicitacao();
    comboTipoShow.value = false;
    if (route.path.includes("desligamento")) {
      permissao.value = "desligamento";
      await useShareSolicitacaoStore().listarMotivosDemissao();

      tableUploadItems.value.push({ text: "Ficha EPI" });
    } else if (route.path.includes("comunicado")) {
      permissao.value = "comunicado";
      await useShareSolicitacaoStore().getListaAssuntos();
    } else if (route.path.includes("ferias")) {
      permissao.value = "ferias";
      await useShareSolicitacaoStore().getTipoFerias();
    } else if (route.path.includes("mudancabase")) {
      permissao.value = "mudancabase";
      listItensMudanca.value =
        await useShareSolicitacaoStore().getListaItensMudanca();
    } else if (route.path.includes("vaga")) {
      listEntidade.value["documentocomplementar"] = await entidadeStore.getToComboList("DocumentoComplementar");
      listEntidade.value["escala"] = await entidadeStore.getToComboList("Escala");
      listEntidade.value["escolaridade"] = await entidadeStore.getToComboList("Escolaridade");
      listEntidade.value["funcao"] = await entidadeStore.getToComboList("Funcao");
      listEntidade.value["gestor"] = await entidadeStore.getToComboList("Gestor");
      listEntidade.value["horario"] = await entidadeStore.getToComboList("Horario");
      listEntidade.value["motivocontratacao"]= await entidadeStore.getToComboList("MotivoContratacao");
      listEntidade.value["tipocontrato"] = await entidadeStore.getToComboList("TipoContrato");
      listEntidade.value["tipofaturamento"] = await entidadeStore.getToComboList("TipoFaturamento");
      listEntidade.value["sexo"] = [{value: 3, text: "Indiferente"},{value: 2, text: "Feminino"},{value: 1, text: "Masculino"}]
      permissao.value = "vaga";
    }

    await getById(route.query.id);

    if (solicitacao.value.solicitacaoTipoId == 1) {
      tableUploadItems.value.push({ text: "Apontamento" });
      let dias = moment(solicitacao.value.desligamento.dataDemissao).diff(
        solicitacao.value.funcionarioDataAdmissao,
        "days"
      );
      if (dias > 90) tableUploadItems.value.push({ text: "Exame demissional" });

      tableUploadItems.value.push({ text: "Ficha EPI" });
    } else if (solicitacao.value.solicitacaoTipoId == 2) {
    } else if (solicitacao.value.solicitacaoTipoId == 3) {
    } else if (solicitacao.value.solicitacaoTipoId == 4)
      ItensMudancaListRemove();
    else if (solicitacao.value.solicitacaoTipoId == 5)
      documentoComplementarListRemove();
  } catch (error) {
    console.error("edit.beforeMount.error", error);
    handleErrors(error);
  } finally {
    isBusy.value.form = false;
  }
});

watch(
  () => solicitacao.value.clienteId,
  async (clienteId) => {
    try {
      if (clienteId) {
        responsavelList.value = await useShareUsuarioStore().getListByCliente(
          clienteId
        );
        funcionarioList.value = await useShareFuncionarioStore().getToComboByCliente(clienteId, useAuthStore().user.id)
        //Trazer centros de custo
        centrosCustoList.value = await useShareUsuarioStore().getCentrosdeCusto(useAuthStore().user.id, clienteId)         
      }
    } catch (error) {
      handleErrors(error);
    }
  }
);

const modeReadOnly = computed(() => {
  return (
    solicitacao.value.status.status.toLowerCase() == "concluído" ||
    solicitacao.value.status.autorizar
  );
});

const descricaoReadOnly = computed(() => {
  return (
    solicitacao.value.status.statusIntermediario.toLowerCase() == "cancelado" ||
    !useAuthStore().hasPermissao(permissao.value + '-executar')
  );
});


//FUNCTIONS
async function aprovarReprovar(isAprovado, comentario) {
  isRunningEvent.value = true;
  try {
    let status = null;
    let texto = null;
    let permissaoNotificar = permissao.value;
    if (!isAprovado) {
      solicitacao.value.statusSolicitacaoId = 5; //reprovado
      permissaoNotificar += "-criar";
    } else {
      //checar através do status atual, qual é o proximo status
      if (solicitacao.value.status && solicitacao.value.status.proximoStatusId)
        solicitacao.value.statusSolicitacaoId =
          solicitacao.value.status.proximoStatusId;

      permissaoNotificar += "-executar";
    }
    status = useShareSolicitacaoStore().getStatus(
      solicitacao.value.statusSolicitacaoId
    );

    texto = `Solicitação  <b>${isAprovado ? "APROVADA" : "REPROVADA"}</b> por ${
      useAuthStore().user.nome
    }!`;
    if (isAprovado)
      texto += `<br/>Status alterado para <b>${status.statusIntermediario}</b>.`;

    if (comentario && comentario.trim() != "") {
      texto += `<br/>Comentário: ${comentario}`;
    }

    await AlterarStatus(solicitacao.value, status, texto, permissaoNotificar);

    openAprovacaoForm.value = false;

    let mensagem =
      (isAprovado ? "Aprovação" : "Reprovação") + " realizada com sucesso!";

    swal.fire({
      toast: true,
      icon: "success",
      index: "top-end",
      title: "Sucesso!",
      html: mensagem,
      showConfirmButton: false,
      timer: 4000,
    });

    router.push({
      name:
        "share" +
        permissao.value.charAt(0).toUpperCase() +
        permissao.value.slice(1),
    });
  } catch (error) {
    console.error("aprovarReprovar.error:", error);
    handleErrors(error);
  } finally {
    isRunningEvent.value = false;
  }
}

function gerarPDF() {
  try {
    const routeData = router.resolve({
      name: "shareVagaPdf",
      query: { id: solicitacao.value.id },
    });
    window.open(routeData.href, "_blank");
  } catch (error) {
    console.error(error);
    handleErrors(
      error,
      error.response.status == 404 ? "Não há comprovantes anexos!" : null
    );
  }
}

async function habilitarEdicao(comentario) {
  isRunningEvent.value = true;
  try {
    let texto = null;
    
    texto = `Solicitação teve o modo edição <b>HABILITADO</b> por ${useAuthStore().user.nome}!`;
    if (comentario && comentario.trim() != "") {
      texto += `<br/>Comentário: ${comentario}`;
    }
    let data ={
      solicitacaoId: solicitacao.value.id,
      evento: texto
    }
    await useShareSolicitacaoStore().adicionarHistorico( data)

    data.dataHora = new Date().toUTCString();
    solicitacao.value.historico.push(data)

    let button = buttons.value.find(q => q.text == "Editar")
    if (button)
      button.disabled = true
    isEditMode.value = true;
    openModeEdicaoForm.value = false

  } catch (error) {
    console.error("habilitarEdicao.error:", error);
    handleErrors(error);
  } finally {
    isRunningEvent.value = false;
  }
}

async function reabrirSolicitacao(comentario) {
  isRunningEvent.value = true;
  try {
    let texto = null;
    let status = useShareSolicitacaoStore().getStatus(1); //pendente

    texto = `Reabertura de solicitação por ${useAuthStore().user.nome}!`;
    if (comentario && comentario.trim() != "") {
      texto += `<br/>Comentário: ${comentario}`;
    }
    let data ={
      solicitacaoId: solicitacao.value.id,
      evento: texto
    }
    await AlterarStatus(solicitacao.value, status, texto, "nao-notificar");

    data.dataHora = new Date().toUTCString();
    solicitacao.value.historico.push(data)
    await getById(solicitacao.value.id);
    openReabrirSolicitacao.value = false
    
  } catch (error) {
    console.error("reabrirSolicitacao.error:", error);
    handleErrors(error);
  } finally {
    isRunningEvent.value = false;
  }
}
async function salvar() {
  try {
    isBusy.value.save = true;
    const { valid } = await mForm.value.validate();

    if (valid) {
      let data = { ...solicitacao.value };
      data.notificarUsuarioIds = [];
      data.usuarioAtualizador = useAuthStore().user.nome;

      if (solicitacaoOriginal.responsavelId != data.responsavelId) {
        data.notificarUsuarioIds.push(data.responsavelId)
      }

      await useShareSolicitacaoStore().update(data);

      swal.fire({
        toast: true,
        icon: "success",
        index: "top-end",
        title: "Sucesso!",
        text: "Dados salvos com sucesso!",
        showConfirmButton: false,
        timer: 2000,
      });

      router.push({
        name:
          "share" +
          permissao.value.charAt(0).toUpperCase() +
          permissao.value.slice(1),
      });
    }
  } catch (error) {
    console.error("salvar.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value.save = false;
  }
}

async function getById(id) {
  try {
    let data = await useShareSolicitacaoStore().getById(id);
    data.status = useShareSolicitacaoStore().getStatus(
      data.statusSolicitacaoId
    );
    if (data.solicitacaoTipoId == 1 && data.statusSolicitacaoId !== 3) {
      let dias = moment(data.desligamento.dataDemissao).diff(
        data.funcionarioDataAdmissao,
        "days"
      );
      data.desligamento.statusExameDemissional =
        dias <= 90 ? 3 : data.desligamento.statusExameDemissional;
    }
    solicitacaoOriginal = {...data }
    solicitacao.value = data;
    getButtons()
  } catch (error) {
    console.error("getById.error:", error);
    handleErrors(error);
  }
}

function changeFieldStatus(tipo) {
  if (tipo.toLowerCase() == "ficha epi")
    solicitacao.value.desligamento.statusFichaEpi = 2;
  else if (tipo.toLowerCase() == "apontamento")
    solicitacao.value.desligamento.statusApontamento = 2;
  else if (tipo.toLowerCase() == "exame demissional")
    solicitacao.value.desligamento.statusApontamento = 3;
}

function getButtons() {
  buttons.value = []
  if (solicitacao.value.status.statusIntermediario.toLowerCase() == "concluído") {
    if (solicitacao.value.solicitacaoTipoId == 5 && 
          (useAuthStore().hasPermissao(permissao.value + "-executar") ||
           useAuthStore().hasPermissao(permissao.value + "-finalizar")
          )
      )
      buttons.value.push({ text: "Imprimir", icon: "", event: "gerar-pdf-click" });

      if (useAuthStore().hasPermissao(permissao.value + "-executar")) 
      {
        buttons.value.push({ text: "Reabrir", icon: "", event: "reabrir-click" });
        buttons.value.push({ text: "Salvar", icon: "", event: "salvar-click" });
      }
      
    
  } else if ("cancelado,reprovado".includes(solicitacao.value.status.statusIntermediario.toLowerCase())) {
    if (solicitacao.value.solicitacaoTipoId == 5)
      buttons.value.push({ text: "Imprimir", icon: "", event: "gerar-pdf-click" });
    
  } else if (solicitacao.value.status.autorizar) {
    if (useAuthStore().hasPermissao(permissao.value + "-aprovar"))
      buttons.value.push({ text: "Aprovar/Reprovar", icon: "", event: "aprovar-click" });
    
  } else if (
    useAuthStore().hasPermissao(permissao.value + "-executar") ||
    useAuthStore().hasPermissao(permissao.value + "-finalizar")
  ) {
    buttons.value.push({ text: "Notificar", icon: "", event: "nofiticar-click" });
    if (useAuthStore().hasPermissao(permissao.value + "-finalizar")) {
      buttons.value.push({ text: "Finalizar", icon: "", event: "finalizar-click" });
    }
    if (solicitacao.value.solicitacaoTipoId == 5)
    {
      buttons.value.push({ text: "Imprimir", icon: "", event: "gerar-pdf-click" });
      buttons.value.push({ text: "Cancelar", icon: "", event: "cancelar-click" });
    }
    buttons.value.push({ text: "Editar", icon: "", event: "editar-click" });
    buttons.value.push({ text: "Salvar", icon: "", event: "salvar-click" });
    
  }
  
}

async function AlterarStatus(solicitacao, status, evento, notificarPermissao) {
  try {
    isBusy.value.save = true;
    let notificarUsuario = [];

    if ((solicitacao.criadoPor || 0) > 0 && status.status.toLowerCase() =="concluído" && status.notifica == 1)
      notificarUsuario.push(solicitacao.criadoPor)
    else if (status.notifica == 1)
      //verificar se precisa notificar
      notificarUsuario =
        await useShareSolicitacaoStore().retornaUsuariosParaNotificar(
          status,
          solicitacao.clienteId,
          notificarPermissao
        );

    let solicitacaoStatus = {
      solicitacaoId: solicitacao.id,
      evento: evento,
      status: status,
      notificar: notificarUsuario,
    };

    await useShareSolicitacaoStore().changeStatus(solicitacaoStatus);
  } catch (error) {
    console.error("alterarStatus.error:", error);
    handleErrors(error)
  }finally {
    isBusy.value.save = false
  }
  
}

async function FinalizarSolicitacao() {
  let options = {
    title: "Confirmação",
    html:
      "Confirma a finalização da(o) " +
      getPageTitle(solicitacao.value.solicitacaoTipoId) +
      "?",
    icon: "question",
    showCancelButton: true,
    confirmButtonText: "Sim",
    cancelButtonText: "Não",
  };

  let response = await swal.fire(options);
  if (response.isConfirmed) {
    let texto = `Solicitação  <b>FINALIZADA</b> por ${
      useAuthStore().user.nome
    }!`;
    let status = useShareSolicitacaoStore().getStatus(3);
    await AlterarStatus(solicitacao.value, status, texto, "naonotifica");

    await swal.fire({
      toast: true,
      icon: "success",
      index: "top-end",
      title: "Sucesso!",
      text: "Solicitação finalizada com sucesso!",
      showConfirmButton: false,
      timer: 2000,
    });
    router.push({
      name:
        "share" +
        permissao.value.charAt(0).toUpperCase() +
        permissao.value.slice(1),
    });
  }
}
function ItensMudancaListRemove(removerTodos = false) {
  if (removerTodos == true)
    listItensMudanca.value.splice(0, listItensMudanca.value.length);
  else {
    solicitacao.value.mudancaBase.itensMudanca.forEach((item) => {
      let index = listItensMudanca.value.findIndex(
        (c) => c.value == item.value
      );
      if (index > -1) listItensMudanca.value.splice(index, 1);
    });
  }
}

function documentoComplementarListRemove(removerTodos = false) {
  if (removerTodos == true)
    listEntidade.value["documentocomplementar"].splice(
      0,
      listEntidade.value["documentocomplementar"].length
    );
  else {
    solicitacao.value.vaga.documentoComplementares.forEach((cc) => {
      let index = listEntidade.value["documentocomplementar"].findIndex(
        (c) => c.value == cc.value
      );
      if (index > -1)
        listEntidade.value["documentocomplementar"].splice(index, 1);
    });
  }
}

async function cancelarSolicitacao() {
  let options = {
    title: "Confirmação",
    html:
      "Confirma cancelamento da(o) " +
      getPageTitle(solicitacao.value.solicitacaoTipoId) +
      "?",
    icon: "question",
    showCancelButton: true,
    confirmButtonText: "Sim",
    cancelButtonText: "Não",
  };

  let response = await swal.fire(options);
  if (response.isConfirmed) {
    let texto = `Solicitação  <b>CANCELADA</b> por ${
      useAuthStore().user.nome
    }!`;
    let status = useShareSolicitacaoStore().statusSolicitacao.find(
      (p) => p.statusIntermediario.toLowerCase() == "cancelado"
    );
    if (status) {
      await AlterarStatus(solicitacao.value, status, texto, "naonotifica");

      await swal.fire({
        toast: true,
        icon: "success",
        index: "top-end",
        title: "Sucesso!",
        text: "Solicitação cancelada com sucesso!",
        showConfirmButton: false,
        timer: 2000,
      });
      router.push({
        name:
          "share" +
          permissao.value.charAt(0).toUpperCase() +
          permissao.value.slice(1),
      });
    }
  }
}
function abrirCadastroAuxiliar(_entidade) {
    entidadeTipo.value = entidadeTipos.value.find(q =>  q.type == _entidade);

    entidade.value = new EntidadeAuxiliar();
    dialogTitle.value = `Novo(a) ${entidadeTipo.value.title}`;
    entidadeDialog.value  = true
  }
  
  function fecharDialog()
  {
    entidadeDialog.value = false;
    entidadeForm.value.reset()
    entidade.value = new EntidadeAuxiliar();
    entidadeTipo.value = null;
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
