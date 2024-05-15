<template>
  <div>
    <Breadcrumbs
      :title="getPageTitle(solicitacao.solicitacaoTipoId)"
      :show-button="false"
      :buttons="[{ text: 'Salvar', icon: '', event: 'salvar-click', disabled: isBusy.save }]"
      @salvar-click="salvar()"
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
              :list-clientes="clienteList"
              :descricao-label="getObservacaoLabelDescricao(solicitacao.solicitacaoTipoId)"
              :list-funcionarios="funcionarioList"
              :list-centro-custos="centrosCustoList"
              :combo-tipo-show="comboTipoShow"
              :descricao-show="![2,4].includes(solicitacao.solicitacaoTipoId)"
            >
              <desligamento
                :data-model="solicitacao.desligamento"
                :create-mode="true"
                :data-admissao="solicitacao.funcionarioDataAdmissao"
                v-if="solicitacao.solicitacaoTipoId == 1"
              />
              <Comunicado v-else-if="solicitacao.solicitacaoTipoId == 2" 
                :data-model="solicitacao.comunicado"
                :create-mode="true"
                />
              <Ferias v-else-if="solicitacao.solicitacaoTipoId == 3" 
              :data-model="solicitacao.ferias"
              :create-mode="true"/>
              <Mudancabase v-else-if="solicitacao.solicitacaoTipoId == 4" 
              :data-model="solicitacao.mudancaBase" 
              :list-clientes="getClienteDestinoList()"
              :list-itens-mudanca="listItensMudanca"
              :cliente-selected="solicitacao.clienteId && solicitacao.clienteId > 0"
              :create-mode="true"/>
            </SolicitacaoForm>

            <table-file-upload 
              :anexos="solicitacao.anexos" 
              :combo-items="tableUploadItems"
              @change-status="changeFieldStatus($event)"
            />
          </v-form>
        </v-card-text>
      </v-card>
    </v-container>
  </div>
</template>

<script setup>
import Breadcrumbs from "@/components/breadcrumbs.vue";
import SolicitacaoForm from "@/components/share/solicitacaoForm.vue";
import desligamento from "@/components/share/desligamento.vue";
import { useShareClienteStore } from "@/store/share/cliente.store";
import { Solicitacao } from "@/store/share/solicitacao.store";
import { onBeforeMount, ref,inject } from "vue";
import Comunicado from "@/components/share/comunicado.vue";
import Ferias from "@/components/share/ferias.vue";
import Mudancabase from "@/components/share/mudancabase.vue";
import tableFileUpload from "@/components/share/tableFileUpload.vue";
import handleErrors from "@/helpers/HandleErrors";
import { useAuthStore } from "@/store/auth.store";
import { useShareSolicitacaoStore } from "@/store/share/solicitacao.store";
import { watch } from "vue";
import { useRoute } from "vue-router";
import { getObservacaoLabelDescricao, getPageTitle } from "@/helpers/share/data";
import router from "@/router";
import {useShareFuncionarioStore} from "@/store/share/funcionario.store"
import moment from "moment";
import { useShareUsuarioStore } from "@/store/share/usuario.store";

const isBusy = ref({
  form: true,
  save: false,
});
const solicitacao = ref(new Solicitacao());
const clienteList = ref([]);
const comboTipoShow = ref(true);
const funcionarioList = ref([]);
const centrosCustoList = ref([]);
const route = useRoute();
const mForm = ref(null);
const swal = inject("$swal");
const permissao = ref("")
const tableUploadItems = ref([{text: "Outros"}])
const listItensMudanca = ref([])
//VUE FUNCTIONS
onBeforeMount(async () => {
  try {
    solicitacao.value.solicitacaoTipoId = null;
    if (route.path.includes("desligamento")) {
      solicitacao.value.solicitacaoTipoId = 1;
      permissao.value = 'desligamento' 
      
      await useShareSolicitacaoStore().listarStatusSolicitacao();
      await useShareSolicitacaoStore().listarMotivosDemissao();

      tableUploadItems.value.push({text: "Ficha EPI"})
    } else if (route.path.includes("comunicado")) {
      solicitacao.value.solicitacaoTipoId = 2;
      permissao.value = 'comunicado' 
      await useShareSolicitacaoStore().getListaAssuntos();

    } else if (route.path.includes("ferias")) {
      solicitacao.value.solicitacaoTipoId = 3;
      permissao.value = 'ferias' 
      await useShareSolicitacaoStore().getTipoFerias();

    } else if (route.path.includes("mudancabase")) {
      console.info("mudança-de-base")
      solicitacao.value.solicitacaoTipoId = 4;
      permissao.value = 'mudancabase' 
      listItensMudanca.value = await useShareSolicitacaoStore().getListaItensMudanca();
    }

    if (solicitacao.value.solicitacaoTipoId) comboTipoShow.value = false;

    clienteList.value = await useShareClienteStore().toComboList(0, useAuthStore().user.id);
    
  } catch (error) {
    console.debug("create.beforeMount.error", error);
    handleErrors(error);
  } finally {
    isBusy.value.form = false;
  }
});

watch(
  () => solicitacao.value.clienteId,
  async (clienteId) => {
    try {
      funcionarioList.value = [];
      centrosCustoList.value = [];
      if (clienteId) {
        if (permissao.value = 'mudancabase')
          solicitacao.value.mudancaBase.clienteDestinoId = null;

        funcionarioList.value = await useShareFuncionarioStore().getToComboByCliente(clienteId, useAuthStore().user.id)

        //Trazer centros de custo
        centrosCustoList.value = await useShareUsuarioStore().getCentrosdeCusto(useAuthStore().user.id, clienteId)       
        
      }  
    } catch (error) {
      console.log('watch.clienteId', error)
      handleErrors(error);
    }
  }
);

watch(() => solicitacao.value.funcionarioId, () => {
  
  let oFunc = funcionarioList.value.find(q => q.value == solicitacao.value.funcionarioId)
  if (oFunc) {
    solicitacao.value.centroCustoNome = oFunc.centroCustoNome
    solicitacao.value.centroCustoId = oFunc.centroCustoId
    solicitacao.value.eSocialMatricula = oFunc.eSocialMatricula
    solicitacao.value.funcionarioDataAdmissao = moment(oFunc.dataAdmissao).format("YYYY-MM-DD");
  }
});

watch(() => solicitacao.value.ferias.tipoFeriasId, (newvalue) => {
  
  let oTipo = useShareSolicitacaoStore().tipoFerias.find(q =>  q.id == newvalue)
  if (oTipo && solicitacao.value.ferias.dataSaida) {
    console.debug(moment(solicitacao.value.ferias.dataSaida).add("days", oTipo.quantidadeDias))
    solicitacao.value.ferias.dataRetorno = moment(solicitacao.value.ferias.dataSaida).add("days",oTipo.quantidadeDias).format("YYYY-MM-DD")
  }
});

watch(() => solicitacao.value.ferias.dataSaida, () => {
  
  if (solicitacao.value.ferias.tipoFeriasId) 
  {
    let oTipo = useShareSolicitacaoStore().tipoFerias.find(q =>  q.id == solicitacao.value.ferias.tipoFeriasId)
    if (oTipo && solicitacao.value.ferias.dataSaida) {
      console.debug(moment(solicitacao.value.ferias.dataSaida).add("days", oTipo.quantidadeDias))
      solicitacao.value.ferias.dataRetorno = moment(solicitacao.value.ferias.dataSaida).add("days",oTipo.quantidadeDias).format("YYYY-MM-DD")
    }
  }
  
});

//FUNCTIONS

function changeFieldStatus(tipo) {
  if (tipo.toLowerCase() =="ficha epi")
    solicitacao.value.desligamento.statusFichaEpi = 2
  else if (tipo.toLowerCase() =="apontamento")
    solicitacao.value.desligamento.statusApontamento = 2
  else if (tipo.toLowerCase() =="exame demissional")
    solicitacao.value.desligamento.statusApontamento = 3
}

async function salvar() {
  try {
    isBusy.value.save = true;

    const { valid } = await mForm.value.validate();

    if (valid) {
      let data = { ...solicitacao.value };
      data.notificarUsuarioIds = [];
      data.usuarioCriador = useAuthStore().user.nome;
      data.regra = permissao.value


      await useShareSolicitacaoStore().add(data);
      await swal.fire({
        toast: true,
        icon: "success",
        index: "top-end",
        title: "Sucesso!",
        text: "Solicitação criada com sucesso!",
        showConfirmButton: false,
        timer: 2000,
      });

      router.push({ name: "share" + permissao.value.charAt(0).toUpperCase() + permissao.value.slice(1) });
    }
  } catch (error) {
    console.debug('salvar.error', error);
    handleErrors(error);
  }finally {
    isBusy.value.save = false;
  }
}
function getClienteDestinoList()
{
    let lista = clienteList.value.filter(q => q.value != solicitacao.value.clienteId) ?? []
    return lista;
}
</script>
