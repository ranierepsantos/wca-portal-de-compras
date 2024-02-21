<template>
  <div>
    <Breadcrumbs
      :title="getPageTitle(solicitacao.solicitacaoTipoId)"
      :show-button="false"
      :buttons="formButtons"
      @salvar-click="salvar()"
      @nofiticar-click="openNotificacao=true"
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
              :descricao-label="
                getObservacaoLabelDescricao(solicitacao.solicitacaoTipoId)
              "
              :list-funcionarios="funcionarioList"
              :list-gestores="funcionarioList"
              :combo-tipo-show="comboTipoShow"
              :list-responsavel="responsavelList"
            >
              <desligamento
                :data-model="solicitacao.desligamento"
                :create-mode="false"
                v-show="solicitacao.solicitacaoTipoId == 1"
              />
              <Comunicado v-show="solicitacao.solicitacaoTipoId == 2" />
              <Ferias v-show="solicitacao.solicitacaoTipoId == 3" />
              <Mudancabase v-show="solicitacao.solicitacaoTipoId == 4" />
            </SolicitacaoForm>

            <table-file-upload :anexos="solicitacao.anexos" />

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
      <NotificacaoEnvio :usuario-list="responsavelList" @closeForm="openNotificacao=false"></NotificacaoEnvio>
    </v-dialog>
  </div>
</template>

<script setup>
import Breadcrumbs from "@/components/breadcrumbs.vue";
import SolicitacaoForm from "@/components/share/solicitacaoForm.vue";
import desligamento from "@/components/share/desligamento.vue";
import { useShareClienteStore } from "@/store/share/cliente.store";
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
import api from "@/services/share/shareApi";
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

const isBusy = ref({
  form: true,
  save: false,
});
const solicitacao = ref(new Solicitacao());
const clienteList = ref([]);
const formButtons = ref([]);
const comboTipoShow = ref(true);
const funcionarioList = ref([]);
const gestorList = ref([]);
const responsavelList = ref([]);
const route = useRoute();
const mForm = ref(null);
const swal = inject("$swal");
const openNotificacao = ref(false)
//VUE FUNCTIONS
onBeforeMount(async () => {
  try {
    clienteList.value = await useShareClienteStore().toComboList();
    await getById(route.query.id);
    formButtons.value.push({ text: "Notificar", icon: "", event: "nofiticar-click" });
    formButtons.value.push({ text: "Salvar", icon: "", event: "salvar-click" });
    comboTipoShow.value = false;
  } catch (error) {
    console.debug("edit.beforeMount.error", error);
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
      gestorList.value = [];
      if (clienteId) {
        funcionarioList.value = (
          await api.get(
            `/Funcionario/ListByClienteToCombo?ClienteId=${clienteId}`
          )
        ).data;

        //Trazer responsáveis por permissão
        responsavelList.value = await useShareUsuarioStore().getListByCliente(
          clienteId
        );
        //trazer gestor por perfil
        gestorList.value = responsavelList.value;
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
      data.usuarioAtualizador = useAuthStore().user.nome;

      if (data.responsavelId && data.responsavelId > 0) {
        let status = useShareSolicitacaoStore().statusSolicitacao.find(
          (x) => x.status.toLowerCase() == "em andamento"
        );
        if (status && data.statusSolicitacaoId != status.id)
          data.statusSolicitacaoId = status.id;
      }

      await useShareSolicitacaoStore().update(data);

      await swal.fire({
        toast: true,
        icon: "success",
        index: "top-end",
        title: "Sucesso!",
        text: "Dados salvos com sucesso!",
        showConfirmButton: false,
        timer: 2000,
      });

      router.push({ name: "shareDesligamento" });
    }
  } catch (error) {
    handleErrors(error);
  } finally {
    isBusy.value.save = false;
  }
}

async function getById(id) {
  try {
    solicitacao.value = await useShareSolicitacaoStore().getById(id);
  } catch (error) {
    console.debug("getById->", error);
    handleErrors(error);
  }
}
</script>
