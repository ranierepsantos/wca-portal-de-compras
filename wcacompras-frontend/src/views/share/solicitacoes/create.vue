<template>
  <div>
    <Breadcrumbs :title="getBreadCrumbsTitle()" :show-button="false" 
    :buttons="formButtons"
    @salvar-click="salvar()"
    />
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isBusy"
    ></v-progress-linear>
    <v-card>
      <v-card-text>
        <v-form>
          <SolicitacaoForm
            :solicitacao="solicitacao"
            :list-clientes="clienteList"
            :descricao-label="getDescricaoObservacao()"
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
          
          <table-file-upload :anexos="solicitacao.anexos"/>
            
        </v-form>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup>
import Breadcrumbs from "@/components/breadcrumbs.vue";
import SolicitacaoForm from "@/components/share/solicitacaoForm.vue";
import desligamento from "@/components/share/desligamento.vue";
import { useShareClienteStore } from "@/store/share/cliente.store";
import { Solicitacao } from "@/store/share/solicitacao.store";
import { onBeforeMount, ref } from "vue";
import Comunicado from "@/components/share/comunicado.vue";
import Ferias from "@/components/share/ferias.vue";
import Mudancabase from "@/components/share/mudancabase.vue";
import tableFileUpload from "@/components/share/tableFileUpload.vue";
import handleErrors from "@/helpers/HandleErrors";
import { useAuthStore } from "@/store/auth.store";
import { useShareSolicitacaoStore} from "@/store/share/solicitacao.store"

const isBusy = ref(false);
const solicitacao = ref(new Solicitacao());
const clienteList = ref([]);
const formButtons = ref([]);

//VUE FUNCTIONS
onBeforeMount(async () => {
  clienteList.value = await useShareClienteStore().toComboList();
  formButtons.value.push({ text: "Salvar", icon: "", event: "salvar-click" });
});



//FUNCTIONS
function getBreadCrumbsTitle() {
  switch (solicitacao.value.solicitacaoTipoId) {
    case 1:
      return "Solicitação de Desligamento";
      break;
    case 2:
      return "Comunicado";
      break;
    case 3:
      return "Solicitação de Férais";
      break;
    case 4:
      return "Solicitação de Mudança de Base";
    default:
      return "Solicitação";
  }
}

function getDescricaoObservacao() {
  switch (solicitacao.value.solicitacaoTipoId) {
    case 1:
      return "Observação do Desligamento";
      break;
    case 2:
      return "Considerações";
      break;
    case 3:
      return "Observação Férias";
      break;
    case 4:
      return "Descrição da Mudança";
    default:
      return "Observação";
  }
}

async function salvar() {
  
  try {
    let data = {...solicitacao.value}
    data.funcionarioId =1;
    data.gestorId =1;
    data.notificarUsuarioIds = []
    data.usuarioCriador = useAuthStore().user.nome;

    await useShareSolicitacaoStore().add(data);

  } catch (error) {
    console.debug(error)
    handleErrors(error)
  }

}

</script>
