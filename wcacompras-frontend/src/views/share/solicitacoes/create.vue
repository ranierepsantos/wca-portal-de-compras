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
              :combo-tipo-show="comboTipoShow"
              :descricao-show="![2,4].includes(solicitacao.solicitacaoTipoId)"
            >
              <desligamento
                :data-model="solicitacao.desligamento"
                :create-mode="true"
                v-if="solicitacao.solicitacaoTipoId == 1"
                :list-funcionarios="funcionarioList"
                :list-centro-custos="centrosCustoList"
              />
              <Comunicado v-else-if="solicitacao.solicitacaoTipoId == 2" 
                :data-model="solicitacao.comunicado"
                :create-mode="true"
                :list-funcionarios="funcionarioList"
                :list-centro-custos="centrosCustoList"
              />
              <Ferias v-else-if="solicitacao.solicitacaoTipoId == 3" 
                :data-model="solicitacao.ferias"
                :create-mode="true"
                :list-funcionarios="funcionarioList"
                :list-centro-custos="centrosCustoList"
              />
              <Mudancabase v-else-if="solicitacao.solicitacaoTipoId == 4" 
              :data-model="solicitacao.mudancaBase" 
              :list-funcionarios="funcionarioList"
              :list-centro-custos="centrosCustoList"
              :list-clientes="getClienteDestinoList()"
              :list-itens-mudanca="listItensMudanca"
              :cliente-selected="solicitacao.clienteId && solicitacao.clienteId > 0"
              :create-mode="true"/>
              <vaga v-else-if="solicitacao.solicitacaoTipoId == 5"
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
                :is-read-only="false"
                :data-model="solicitacao.vaga"
                @select-button-click="abrirCadastroAuxiliar($event)"
              />
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
import { EntidadeAuxiliar, useShareEntidadeAuxiliarStore } from "@/store/share/entidadesauxiliares.store";
import vaga from "@/components/share/vaga.vue";
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
const dialog = ref(false)
const dialogTitle = ref("")
const entidadeForm = ref(null)

const entidadeStore = useShareEntidadeAuxiliarStore();
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
const isSavingEntity = ref(false)

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
      solicitacao.value.solicitacaoTipoId = 5;
      permissao.value = 'vaga' 
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
        if (permissao.value != 'vaga') 
        {
          if (permissao.value == 'mudancabase')
            solicitacao.value.mudancaBase.clienteDestinoId = null;

          solicitacao.value.funcionarioId = null
          funcionarioList.value = await useShareFuncionarioStore().getToComboByCliente(clienteId, useAuthStore().user.id)
          //Trazer centros de custo
          centrosCustoList.value = await useShareUsuarioStore().getCentrosdeCusto(useAuthStore().user.id, clienteId)         
        }else {
          let _cliente = await useShareClienteStore().getClienteById(clienteId)
          if (_cliente) {
            let _endereco  = _cliente.endereco 
            _endereco += _cliente.numero && _cliente.numero.trim() == '' ?'': `, ${_cliente.numero}\n`
            _endereco += _cliente.cep && _cliente.cep.trim() =='' ?'': `${_cliente.cep} - `
            _endereco += _cliente.cidade && _cliente.cidade.trim() =='' ?'': `${_cliente.cidade}`
            _endereco += _cliente.uf && _cliente.uf.trim() =='' ?'': ` - ${_cliente.uf}`
            solicitacao.value.vaga.enderecoCliente = _endereco;

          }

        }
        
      }  
    } catch (error) {
      console.log('watch.clienteId', error)
      handleErrors(error);
    }
  }
);

watch(() => solicitacao.value.ferias.tipoFeriasId, (newvalue) => {
  
  let oTipo = useShareSolicitacaoStore().tipoFerias.find(q =>  q.id == newvalue)
  if (oTipo && solicitacao.value.ferias.dataSaida) {
    solicitacao.value.ferias.dataRetorno = moment(solicitacao.value.ferias.dataSaida).add("days",oTipo.quantidadeDias).format("YYYY-MM-DD")
  }
});

watch(() => solicitacao.value.ferias.dataSaida, () => {
  
  if (solicitacao.value.ferias.tipoFeriasId) 
  {
    let oTipo = useShareSolicitacaoStore().tipoFerias.find(q =>  q.id == solicitacao.value.ferias.tipoFeriasId)
    if (oTipo && solicitacao.value.ferias.dataSaida) {
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
      console.debug("router.push", { name: "share" + permissao.value.charAt(0).toUpperCase() + permissao.value.slice(1) })
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

function abrirCadastroAuxiliar(_entidade) {
    console.log(_entidade)
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
