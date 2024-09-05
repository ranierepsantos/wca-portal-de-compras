<template>
  <div>
    <Breadcrumbs
      :title="getPageTitle(solicitacao.solicitacaoTipoId)"
      :show-button="false"
      :buttons="getButtons()"      
      @salvar-click="salvar()"
      @aprovar-click ="openAprovacaoForm=true"
      @nofiticar-click="openNotificacao=true"
      @finalizar-click="FinalizarSolicitacao()"
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
              :list-funcionarios ="[]"
              :list-centro-custos="[]"
              :combo-tipo-show="comboTipoShow"
              :list-responsavel="responsavelList"
              :is-read-only="modeReadOnly"
            >
              <desligamento
                :data-model="solicitacao.desligamento"
                :create-mode="false"
                v-if="solicitacao.solicitacaoTipoId == 1"
                :is-read-only="modeReadOnly"
                :data-admissao="solicitacao.funcionarioDataAdmissao"
              />
              <Comunicado v-else-if="solicitacao.solicitacaoTipoId == 2"
                :data-model="solicitacao.comunicado"
                :create-mode="false"
                :is-read-only="true"
              />
              <Ferias v-else-if="solicitacao.solicitacaoTipoId == 3"
                :data-model="solicitacao.ferias"
                :create-mode="false" />
              <Mudancabase v-else-if="solicitacao.solicitacaoTipoId == 4" 
              :data-model="solicitacao.mudancaBase" 
              :list-clientes="[]"
              :list-centro-custos="[]"
              :list-itens-mudanca="listItensMudanca"
              :cliente-selected="solicitacao.clienteId && solicitacao.clienteId > 0"
              :create-mode="false"
              :is-read-only="true"
              />
              <vaga v-else-if="solicitacao.solicitacaoTipoId == 5"
                :list-documento-complementar="listEntidade['documentocomplementar']"
                :is-read-only="true"
                :data-model="solicitacao.vaga"
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
              :eventos="solicitacao.historico"
              style="margin-top: 15px"
            />
          </v-form>
        </v-card-text>
      </v-card>
      <v-card class="mx-auto">
        
      </v-card>
    </v-container>
    <v-dialog
      v-model="openNotificacao"
      max-width="800"
      :absolute="false"
      persistent
    >
      <NotificacaoEnvio 
        :usuario-list="responsavelList" 
        @closeForm="openNotificacao=false"
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
          :title="getPageTitle(solicitacao.solicitacaoTipoId) + ' - Aprovar / Reprovar'"
          @aprovar-click="aprovarReprovar(true, $event)"
          @reprovar-click="aprovarReprovar(false, $event)"
          @close-form="openAprovacaoForm = false"
          :is-running-event="isRunningEvent"
          reprovar-title="Reprovar"
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
import { useShareEntidadeAuxiliarStore } from "@/store/share/entidadesauxiliares.store";


const tableUploadItems = ref([{text: "Outros"}])
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
const openNotificacao = ref(false)
const openAprovacaoForm = ref(false)
const isRunningEvent = ref(false)
const permissao = ref("")
const entidadeStore = useShareEntidadeAuxiliarStore();

const listEntidade = ref({
  documentocomplementar: [],
  escala : [],
  escolaridade : [],
  funcao : [],
  gestor : [],
  horario: [],
  motivocontratacao: [],
  sexo: [],
  tipocontrato: [],
  tipofaturamento: [],
})


const listItensMudanca = ref([])
//VUE FUNCTIONS
onBeforeMount(async () => {
  try {
    await useShareSolicitacaoStore().listarStatusSolicitacao();
    comboTipoShow.value = false;
    if (route.path.includes("desligamento")) {
      permissao.value = 'desligamento' 
      await useShareSolicitacaoStore().listarStatusSolicitacao();
      await useShareSolicitacaoStore().listarMotivosDemissao();

      tableUploadItems.value.push({text: "Ficha EPI"})
    } else if (route.path.includes("comunicado")) {
      permissao.value = 'comunicado' 
      await useShareSolicitacaoStore().getListaAssuntos();

    } else if (route.path.includes("ferias")) {
      permissao.value = 'ferias' 
      await useShareSolicitacaoStore().getTipoFerias();
    } else if (route.path.includes("mudancabase")) {
      permissao.value = 'mudancabase' 
      listItensMudanca.value = await useShareSolicitacaoStore().getListaItensMudanca();
    }else if (route.path.includes("vaga")) {
      listEntidade.value["documentocomplementar"] = await entidadeStore.getToComboList("DocumentoComplementar");
      permissao.value = 'vaga' 
    }
    
    await getById(route.query.id);

    if (solicitacao.value.solicitacaoTipoId == 1) {
      tableUploadItems.value.push({text: "Apontamento"})
      let dias = moment(solicitacao.value.desligamento.dataDemissao).diff(solicitacao.value.funcionarioDataAdmissao, "days");
      if (dias > 90 )
        tableUploadItems.value.push({text: "Exame demissional"})
        
      tableUploadItems.value.push({text: "Ficha EPI"})
    } else if (solicitacao.value.solicitacaoTipoId == 2) {
    } else if (solicitacao.value.solicitacaoTipoId == 3) {
    } else if (solicitacao.value.solicitacaoTipoId == 4)
      ItensMudancaListRemove()
    else if (solicitacao.value.solicitacaoTipoId == 5)
      documentoComplementarListRemove()
    
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
        responsavelList.value = await useShareUsuarioStore()
                                      .getListByCliente(clienteId);
      }
    } catch (error) {
      handleErrors(error);
    }
  }
);

const modeReadOnly = computed(() => {
  return solicitacao.value.status.status.toLowerCase() == 'concluído' || solicitacao.value.status.autorizar
})


//FUNCTIONS
async function aprovarReprovar(isAprovado, comentario) {
  isRunningEvent.value = true;
  try
  {
    let status = null;
    let texto  = null;
    let permissaoNotificar = permissao.value
    if (!isAprovado) {
      solicitacao.value.statusSolicitacaoId = 5; //reprovado
      permissaoNotificar += '-criar'
    }
    else 
    {
      //checar através do status atual, qual é o proximo status
      if (solicitacao.value.status && solicitacao.value.status.proximoStatusId)
        solicitacao.value.statusSolicitacaoId = solicitacao.value.status.proximoStatusId

      permissaoNotificar += '-executar'
    }
    status = useShareSolicitacaoStore().getStatus(solicitacao.value.statusSolicitacaoId);
    
    texto = `Solicitação  <b>${ isAprovado ? "APROVADA" : "REPROVADA"}</b> por ${useAuthStore().user.nome}!`
    if (isAprovado)
      texto += `<br/>Status alterado para <b>${status.statusIntermediario}</b>.`

    if (comentario && comentario.trim() != ""){
      texto += `<br/>Comentário: ${comentario}`
    }
    
    await AlterarStatus(solicitacao.value, status, texto, permissaoNotificar);
    
    openAprovacaoForm.value = false;

    let mensagem = (isAprovado ? "Aprovação" : "Reprovação") + " realizada com sucesso!";
    
    swal.fire({
      toast: true,
      icon: "success",
      index: "top-end",
      title: "Sucesso!",
      html: mensagem,
      showConfirmButton: false,
      timer: 4000,
    });

    router.push({ name: "share" + permissao.value.charAt(0).toUpperCase() + permissao.value.slice(1) });
  } catch (error) {
    console.error("aprovarReprovar.error:", error);
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

      if (data.responsavelId && data.responsavelId > 0) {
        let status = useShareSolicitacaoStore().statusSolicitacao.find(
          (x) => x.status.toLowerCase() == "em andamento"
        );
        if (status && data.statusSolicitacaoId != status.id)
          data.statusSolicitacaoId = status.id;
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

      router.push({ name: "share" + permissao.value.charAt(0).toUpperCase() + permissao.value.slice(1) });
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
    data.status = useShareSolicitacaoStore().getStatus(data.statusSolicitacaoId);
    if (data.solicitacaoTipoId == 1 && data.statusSolicitacaoId !== 3) {
      let dias = moment(data.desligamento.dataDemissao).diff(data.funcionarioDataAdmissao, "days");
      data.desligamento.statusExameDemissional = dias <= 90 ? 3 : data.desligamento.statusExameDemissional
    }
    solicitacao.value = data;
  } catch (error) {
    console.error("getById.error:", error);
    handleErrors(error);
  }
}

function changeFieldStatus(tipo) {

  if (tipo.toLowerCase() =="ficha epi")
    solicitacao.value.desligamento.statusFichaEpi = 2
  else if (tipo.toLowerCase() =="apontamento")
    solicitacao.value.desligamento.statusApontamento = 2
  else if (tipo.toLowerCase() =="exame demissional")
    solicitacao.value.desligamento.statusApontamento = 3

}

function getButtons() {
  if (solicitacao.value.status.status.toLowerCase() == 'concluído')
      return []
  else if (solicitacao.value.status.autorizar) 
  {
    if (useAuthStore().hasPermissao(permissao.value + '-aprovar'))
      return [{ text: 'Aprovar/Reprovar', icon: '', event: 'aprovar-click', disabled: isBusy.value.save }]
    else
      return []
  } else if (useAuthStore().hasPermissao(permissao.value + '-executar') || useAuthStore().hasPermissao(permissao.value + '-finalizar')) 
  {
      let buttons = [{ text: 'Notificar', icon: '', event: 'nofiticar-click',disabled: isBusy.value.save }]
      //let buttons = []
      if (useAuthStore().hasPermissao(permissao.value + '-finalizar')){
        buttons.push({ text: 'Finalizar', icon: '', event: 'finalizar-click',disabled: isBusy.value.save })
      }
      buttons.push({ text: 'Salvar', icon: '', event: 'salvar-click', disabled: isBusy.value.save })
      return buttons;
  }

}

async function AlterarStatus(solicitacao, status, evento, notificarPermissao) {
  let notificarUsuario = []
    if (status.notifica == 1) //verificar se precisa notificar
      notificarUsuario = await useShareSolicitacaoStore().retornaUsuariosParaNotificar(status, solicitacao.clienteId, notificarPermissao)
    
    let solicitacaoStatus = {
      solicitacaoId: solicitacao.id,
      evento: evento,
      status: status,
      notificar: notificarUsuario
    };
    
    await useShareSolicitacaoStore().changeStatus(solicitacaoStatus);
}

async function FinalizarSolicitacao() {

  let options = {
    title: "Confirmação",
    html: "Confirma a finalização da(o) " + getPageTitle(solicitacao.value.solicitacaoTipoId)  + "?",
    icon: "question",
    showCancelButton: true,
    confirmButtonText: "Sim",
    cancelButtonText: "Não",
  };

  let response = await swal.fire(options);
  if (response.isConfirmed) {
    let texto = `Solicitação  <b>FINALIZADA</b> por ${useAuthStore().user.nome}!`
    let status = useShareSolicitacaoStore().getStatus(3)
    await AlterarStatus(solicitacao.value, status, texto, "")

    await swal.fire({
        toast: true,
        icon: "success",
        index: "top-end",
        title: "Sucesso!",
        text: "Solicitação finalizada com sucesso!",
        showConfirmButton: false,
        timer: 2000,
      });
    router.push({ name: "share" + permissao.value.charAt(0).toUpperCase() + permissao.value.slice(1) });
  }

}
function ItensMudancaListRemove(removerTodos = false) {
  if (removerTodos == true) listItensMudanca.value.splice(0, listItensMudanca.value.length);
  else {

    solicitacao.value.mudancaBase.itensMudanca.forEach((item) => {
      let index = listItensMudanca.value.findIndex((c) => c.value == item.value);
      if (index > -1) listItensMudanca.value.splice(index, 1);
    });
  }
}

function documentoComplementarListRemove(removerTodos = false) {
    if (removerTodos == true) listEntidade.value['documentocomplementar'].splice(0, listEntidade.value['documentocomplementar'].length);
    else {
      solicitacao.value.vaga.documentoComplementares.forEach((cc) => {
        let index = listEntidade.value['documentocomplementar'].findIndex((c) => c.value == cc.value);
        if (index > -1) listEntidade.value['documentocomplementar'].splice(index, 1);
      });
    }
  }

</script>
