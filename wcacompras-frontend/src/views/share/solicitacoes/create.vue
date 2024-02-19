<template>
  <div>
    <Breadcrumbs :title="getBreadCrumbsTitle()" :show-button="false" />
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

          <v-text-field
            label="Arquivo"
            type="file"
            variant="outlined"
            color="primary"
            density="compact"
          ></v-text-field>
          <v-table> 
            <thead>
                <tr>
                    <th class="text-left text-grey">Arquivo</th>
                    <th class="text-center text-grey">Ações</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in solicitacao.anexos" :key="item.id">
                    <td class="text-left">
                        <v-icon icon="mdi-file-document-check-outline"></v-icon>
                        &nbsp;{{ item.descricao }}
                    </td>
                    <td class="text-center">
                        <v-btn icon="mdi-file-download-outline" variant="plain" color="primary"
                        @click="downloadFile(item.caminhoArquivo, item.descricao)"
                        ></v-btn>
                        <v-btn variant="plain" color="error"
                            icon="mdi-trash-can-outline"
                            title="Exluir">
                        </v-btn>
                    </td>
                </tr>
            </tbody>
          </v-table>
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
import axios from "axios";
import { realizarDownload } from "@/helpers/functions";

const isBusy = ref(false);
const solicitacao = ref(new Solicitacao());
const clienteList = ref([]);

//VUE FUNCTIONS
onBeforeMount(async () => {
  clienteList.value = await useShareClienteStore().toComboList();
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

function downloadFile(url, nomeArquivo) {
            axios({
                url: url, // Download File URL Goes Here
                method: 'GET',
                responseType: 'blob',
                headers: {
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': ' GET, PUT, POST, DELETE, OPTIONS',
                    'Access-Control-Allow-Headers': 'Origin, Content-Type, X-Auth-Token',
                    'Access-Control-Allow-Credentials': 'false',
                },
            }).then(async (res) => {
                await realizarDownload(res,nomeArquivo)
            });
        }
</script>
