<template>
  <div>
    <Breadcrumbs
      :title="getPageTitle(solicitacao.solicitacaoTipoId)"
      :show-button="false"
      :buttons="formButtons"
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
            >
              <desligamento
                :data-model="solicitacao.desligamento"
                :create-mode="true"
                v-show="solicitacao.solicitacaoTipoId == 1"
              />
              <Comunicado v-show="solicitacao.solicitacaoTipoId == 2" />
              <Ferias v-show="solicitacao.solicitacaoTipoId == 3" />
              <Mudancabase v-show="solicitacao.solicitacaoTipoId == 4" />
            </SolicitacaoForm>

            <table-file-upload :anexos="solicitacao.anexos" />
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
import api from "@/services/share/shareApi";
import { getObservacaoLabelDescricao, getPageTitle } from "@/helpers/share/data";
import { useShareUsuarioStore } from "@/store/share/usuario.store";
import router from "@/router";

const isBusy = ref({
  form: true,
  save: false,
});
const solicitacao = ref(new Solicitacao());
const clienteList = ref([]);
const formButtons = ref([]);
const comboTipoShow = ref(true);
const funcionarioList = ref([]);
const centrosCustoList = ref([]);
const route = useRoute();
const mForm = ref(null);
const swal = inject("$swal");
const permissao = ref("")
//VUE FUNCTIONS
onBeforeMount(async () => {
  try {
    solicitacao.value.solicitacaoTipoId = null;
    if (route.path.includes("desligamento")) {
      solicitacao.value.solicitacaoTipoId = 1;
      permissao.value = 'desligamento' 
    } else if (route.path.includes("comunicado")) {
      solicitacao.value.solicitacaoTipoId = 2;
      permissao.value = 'comunicado' 
    } else if (route.path.includes("ferias")) {
      solicitacao.value.solicitacaoTipoId = 3;
      permissao.value = 'ferias' 
    } else if (route.path.includes("mudancabase")) {
      solicitacao.value.solicitacaoTipoId = 4;
      permissao.value = 'mudancabase' 
    }

    if (solicitacao.value.solicitacaoTipoId) comboTipoShow.value = false;

    clienteList.value = await useShareClienteStore().toComboList();
    formButtons.value.push({ text: "Salvar", icon: "", event: "salvar-click" });
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
        funcionarioList.value = (
          await api.get(
            `/Funcionario/ListByClienteToCombo?ClienteId=${clienteId}`
          )
        ).data;

        //Trazer centros de custo
        //centrosCustoList.value = await useShareUsuarioStore().getListByCliente(clienteId)
        
      }  
    } catch (error) {
      handleErrors(error);
    }
  }
);

//FUNCTIONS
async function salvar() {
  try {
    isBusy.value.save = true;
    const { valid } = await mForm.value.validate();

    if (valid) {
      let data = { ...solicitacao.value };
      data.notificarUsuarioIds = [];
      data.usuarioCriador = useAuthStore().user.nome;

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

      router.push({name: "shareDesligamento"})
    }
  } catch (error) {
    console.debug(error);
    handleErrors(error);
  }finally {
    isBusy.value.save = false;
  }
}
</script>
