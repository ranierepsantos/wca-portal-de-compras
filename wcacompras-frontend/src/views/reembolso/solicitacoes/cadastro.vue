<template>
  <div>
    <bread-crumbs
      title="Solicitação"
      :show-button="false"
      :buttons="formButtons"
      @aprovar-click="openAprovacaoForm = true"
      @salvar-click="salvar()"
      @registrarpgto-click="registrarPagto()"
    />
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-if="isBusy"
    ></v-progress-linear>
    <v-container class="justify-center" v-else>
      <v-card>
        <v-card-text>
          <v-form>
            <v-row>
              <v-col cols="6">
                <v-select
                  label="Cliente"
                  :items="clientes"
                  density="compact"
                  item-title="text"
                  item-value="value"
                  variant="outlined"
                  color="primary"
                  v-model="solicitacao.clienteId"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                ></v-select>
              </v-col>
              <v-col>
                <v-select
                  label="Tipo Solicitação"
                  :items="solicitacaoStore.tipoSolicitacao"
                  density="compact"
                  item-title="text"
                  item-value="value"
                  variant="outlined"
                  color="primary"
                  v-model="solicitacao.tipoSolicitacao"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                  :disabled="solicitacao.id > 0"
                ></v-select>
              </v-col>
              <v-col v-show="solicitacao.id > 0">
                <v-btn
                  :color="solicitacaoStore.getStatus(solicitacao.status).color"
                  variant="tonal"
                  class="text-center"
                >
                  {{
                    solicitacaoStore.getStatus(solicitacao.status).status
                  }}</v-btn
                >
              </v-col>
            </v-row>
            <v-row
              v-show="
                solicitacao.id > 0 
              "
            >
              <v-col>
                <v-text-field
                  label="Colaborador"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                  v-model="solicitacao.colaboradorNome"
                  :readonly="true"
                  bg-color="#f2f2f2"
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Gestor Responsável"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.gestorNome"
                  :readonly="true"
                  bg-color="#f2f2f2"
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col
                v-show="
                  solicitacao.id > 0 
                "
              >
                <v-text-field
                  label="Cargo"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.colaboradorCargo"
                  bg-color="#f2f2f2"
                  :readonly="true"
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Local Projeto"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.projeto"
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col cols="6">
                <v-text-field
                  label="Objetivo Solicitação"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.objetivo"
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Período Inicial"
                  type="date"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.periodoInicial"
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Período Final"
                  type="date"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.periodoFinal"
                ></v-text-field>
              </v-col>
              <v-col v-show="solicitacao.tipoSolicitacao == 2">
                <v-text-field-money
                  label-text="Valor Solicitação"
                  v-model="solicitacao.valorAdiantamento"
                  color="primary"
                  :number-decimal="2"
                  prefix="R$"
                ></v-text-field-money>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>
      </v-card>

      <v-card
        style="margin-top: 20px"
        v-show="
          solicitacao.tipoSolicitacao == 1 ||
          (solicitacao.tipoSolicitacao == 2 && solicitacao.status != 1)
        "
      >
        <v-card-title>
          <v-breadcrumbs>
            <div class="text-h6 text-primary">Detalhamento de Despesas</div>
            <v-spacer></v-spacer>
            <v-btn
              color="primary"
              variant="outlined"
              class="text-capitalize"
              @click="openDespesaForm = true"
              v-show="canEdit"
            >
              <b>Nova Despesa</b>
            </v-btn>
          </v-breadcrumbs>
        </v-card-title>

        <v-card-text>
          <v-table class="elevation-2">
            <thead>
              <tr>
                <th class="text-center text-grey">DATA</th>
                <th class="text-center text-grey">TIPO</th>
                <th class="text-center text-grey">DESCRICAO</th>
                <th class="text-center text-grey">VALOR</th>
                <th class="text-center text-grey"></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, index) in solicitacao.despesa" :key="index">
                <td class="text-center">
                  {{ moment(item.dataEvento).format("DD/MM/YYYY") }}
                </td>
                <td class="text-center">
                  {{ getDespesaTipo(item.tipoDespesaId).nome }}
                </td>
                <td class="text-center">
                  {{
                    getDespesaTipo(item.tipoDespesaId).tipo == 1
                      ? "Cupom/Nota:" +
                        item.numeroFiscal +
                        " / " +
                        item.razaoSocial
                      : item.origem + " -> " + item.destino
                  }}
                </td>
                <td class="text-right">
                  {{ formatToCurrencyBRL(parseFloat(item.valor)) }}
                </td>
                <td class="text-right">
                  <v-btn
                    icon="mdi-lead-pencil"
                    size="smaller"
                    variant="plain"
                    color="primary"
                    @click="editarDespesa(item)"
                    title="Editar"
                    :disabled="isBusy"
                  ></v-btn>
                  <v-btn
                    icon="mdi-delete"
                    variant="plain"
                    color="error"
                    @click="removerDespesa(item)"
                    :disabled="isBusy"
                  >
                  </v-btn>
                </td>
              </tr>
            </tbody>
            <tfoot>
              <tr>
                <td class="text-right" colspan="4"><b>TOTAL:</b></td>
                <td class="text-right">
                  {{ formatToCurrencyBRL(calcularTotalDespesa()) }}
                </td>
                <td></td>
              </tr>
              <tr v-show="solicitacao.tipoSolicitacao == 2">
                <td class="text-right" colspan="4"><b>SALDO:</b></td>
                <td></td>
                <td class="text-right">
                  {{
                    formatToCurrencyBRL(
                      solicitacao.valorAdiantamento - calcularTotalDespesa()
                    )
                  }}
                </td>
              </tr>
            </tfoot>
          </v-table>
        </v-card-text>
      </v-card>
      <!-- HISTORICO DA SOLICITAÇÃO -->
      <historico
        :eventos="
          solicitacao.solicitacaoHistorico.sort(
            compararValor('dataEvento', 'desc')
          )
        "
        style="margin-top: 15px"
        v-show="solicitacao.id != 0"
      />
      <!-- FORM PARA CADASTRO DE DESPESA -->
      <v-dialog
        v-model="openDespesaForm"
        max-width="900"
        :absolute="false"
        persistent
      >
        <despesa-form
          :despesa="despesa"
          :combo-tipo-despea="despesaTipos"
          @change-image="(image) => (despesa.imagePath = image)"
          @cancela-click="
            () => {
              limparDadosDespesa();
              openDespesaForm = false;
            }
          "
          @save-click="salvarDespesa()"
        ></despesa-form>
      </v-dialog>
      <!-- FORM PARA APROVAR / REJEITAR PEDIDO -->
      <v-dialog
        v-model="openAprovacaoForm"
        max-width="700"
        :absolute="false"
        persistent
      >
        <aprovar-rejeitar-form
          title="APROVAR / REJEITAR - SOLICITAÇÃO"
          @aprovar-click="aprovarReprovar(true, $event)"
          @reprovar-click="aprovarReprovar(false, $event)"
          @close-form="openAprovacaoForm = false"
          :is-running-event="isRunningEvent"
        />
      </v-dialog>
    </v-container>
  </div>
</template>

<script setup>
import breadCrumbs from "@/components/breadcrumbs.vue";
import { ref, inject, onMounted } from "vue";
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import DespesaForm from "@/components/reembolso/despesaForm.vue";
import aprovarRejeitarForm from "@/components/aprovarRejeitarForm.vue";
import { useAuthStore } from "@/store/auth.store";
import handleErrors from "@/helpers/HandleErrors";
import { formatToCurrencyBRL } from "@/helpers/functions";
import { useClienteStore } from "@/store/reembolso/cliente.store";
import { Usuario, useUsuarioStore } from "@/store/reembolso/usuario.store";
import {
  Despesa,
  Solicitacao,
  useSolicitacaoStore,
  Evento,
} from "@/store/reembolso/solicitacao.store";
import moment from "moment";
import router from "@/router";
import { useRoute } from "vue-router";
import { useDespesaTipoStore } from "@/store/reembolso/despesaTipo.store";
import { computed } from "vue";
import { compararValor } from "@/helpers/functions";
import historico from "@/components/reembolso/historico.vue";
import { Transacao, useContaStore } from "@/store/reembolso/conta.store";


const authStore = useAuthStore();
const clienteStore = useClienteStore();
const despesaTipoStore = useDespesaTipoStore();
const contaStore = useContaStore();
const route = useRoute();
const solicitacaoStore = useSolicitacaoStore();
const openDespesaForm = ref(false);
const openAprovacaoForm = ref(false);
const isRunningEvent = ref(false);
const isBusy = ref(false);
const clientes = ref([]);
const swal = inject("$swal");
const solicitacao = ref(new Solicitacao());
const despesa = ref(new Despesa());
const despesaTipos = ref([]);

const formButtons = ref([]);
const usuario = ref(new Usuario());

//VUE FUNCTIONS
onMounted(async () => {
  try {
    isBusy.value = true;

    usuario.value = await useUsuarioStore().getById(authStore.user.id);
    clientes.value = await clienteStore.ListByUsuario(usuario.value.id);
    despesaTipos.value = await despesaTipoStore.toComboList();

    if (parseInt(route.query.id) > 0) {
      await getSolicitacao(route.query.id);
      if ("1,5,6".includes(solicitacao.value.status)) {
        formButtons.value.push({
          text: "Aprovar / Reprovar",
          icon: "",
          event: "aprovar-click",
        });
      }
      if (
        solicitacao.value.tipoSolicitacao == 2 &&
        solicitacao.value.status == 2
      ) {
        formButtons.value.push({
          text: "Registrar Pagamento",
          icon: "",
          event: "registrarpgto-click",
        });
      }
    } else {
      //checar se o usuario já possui adiantamento em aberto
      let adiantamento = await solicitacaoStore.getByTipoAndUsuario(
        2,
        authStore.user.id,
        [1, 2, 3, 4, 5]
      );
      if (adiantamento.length > 0) {
        swal.fire({
          toast: true,
          icon: "warning",
          index: "top-end",
          title: "Atenção!",
          text: "Você possui adiantamento em aberto, novas solicitações não poderão ser feitas!",
          showConfirmButton: false,
          timer: 3000,
        });
        router.push({ name: "reembolsoSolicitacoes" });
        return;
      }
      solicitacao.value.colaboradorId = usuario.value.id;
      solicitacao.value.colaborador = usuario.value.nome;
      solicitacao.value.colaboradorCargo =
        usuario.value.usuarioReembolsoComplemento.cargo;
      solicitacao.value.gestorId =
        usuario.value.usuarioReembolsoComplemento.gestorId;
      solicitacao.value.gestor = solicitacao.value.gestor =
        solicitacaoStore.getUsuarioSolicitacao(
          usuario.value.usuarioReembolsoComplemento.gestorId
        ).text;
    }

    // if (!"2,4,5,6".includes(solicitacao.value.status))
    formButtons.value.push({ text: "Salvar", icon: "", event: "salvar-click" });
  } catch (error) {
    console.log("onMounted.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
});

const canEdit = computed(() => !"2,4,5,6".includes(solicitacao.value.status));

//FUNCTIONS

async function aprovarReprovar(isAprovado, comentario) {
  try {
    debugger;
    isRunningEvent.value = true;

    /**
      1 - Solicitado
      2 - Aguardando Depósito
      3 - Prestar Contas
      4 - Rejeitado
      5 - Aguardando Conferência
      6 - Aguardando Aprovação Cliente
      7 - Aguardando Faturamento
      8 - Faturado
      9 - Cancelado
    **/
    if (!isAprovado) solicitacao.value.status = 4; //rejeitado
    else {
      //tipoSolicitacao: Adiantamento, Status: Solicitado
      if (
        solicitacao.value.tipoSolicitacao == 2 &&
        solicitacao.value.status == 1
      )
        solicitacao.value.status = 2; //2 - Aguardando Depósito
      //Status: 5 - Aguardando conferência
      else if (solicitacao.value.status == 5)
        solicitacao.value.status = 6; //6 - Aguardando Aprovação Cliente
      //Status: 6 - Aguardando Aprovação Cliente
      else if (solicitacao.value.status == 6) {
        solicitacao.value.status = 7; //7 - Aguardando Faturamento
        // Se o tipo de solicitação for reembolso - enviar para aguardando depósito
        if (solicitacao.value.tipoSolicitacao == 1)
          solicitacao.value.status = 2; //2 - Aguardando Depósito
      }
    }
    let reembolsoStatus = solicitacaoStore.getStatus(solicitacao.value.status);
    let texto = `Solicitação  <b>${
      isAprovado ? "APROVADA" : "REJEITADA"
    }</b> por ${authStore.user.nome}, status alterado para <b>${
      reembolsoStatus.status
    }</b>. <br/> Comentário: ${comentario}`;

    let solicitacaoStatus = {
      solicitacaoId: solicitacao.value.id,
      evento: texto,
      status: reembolsoStatus,
    };

    await solicitacaoStore.changeStatus(solicitacaoStatus);

    openAprovacaoForm.value = false;

    let mensagem =
      (isAprovado ? "Aprovação" : "Rejeição") + " realizada com sucesso!";
    //mensagem += reembolsoStatus.notifica !="" ? `<br/> Notificação enviada para ${reembolsoStatus.notifica}!`:""

    swal.fire({
      toast: true,
      icon: "success",
      index: "top-end",
      title: "Sucesso!",
      html: mensagem,
      showConfirmButton: false,
      timer: 4000,
    });
  } catch (error) {
    console.log("aprovarReprovar.error:", error);
    handleErrors(error);
  } finally {
    isRunningEvent.value = false;
  }
}

function calcularTotalDespesa() {
  let valorTotalDespesa = 0;

  solicitacao.value.despesa.forEach((item) => {
    valorTotalDespesa += parseFloat(item.valor);
  });
  
  solicitacao.value.valorDespesa = valorTotalDespesa;
  return valorTotalDespesa;
}

function editarDespesa(item) {
  despesa.value = { ...item };
  openDespesaForm.value = true;
}

function getDespesaTipo(id) {
  return despesaTipos.value.find((q) => q.id == id);
}

async function getSolicitacao(solicitacaoId) {
  try {
    let data = await solicitacaoStore.getById(solicitacaoId);
    data.periodoInicial = data.periodoInicial.split("T")[0];
    data.periodoFinal = data.periodoFinal.split("T")[0];
    data.despesa.forEach((element) => {
      element.dataEvento = element.dataEvento.split("T")[0];
    });
    solicitacao.value = data;
  } catch (error) {
    console.log("getSolicitacao.error:", error);
    handleErrors(error);
  }
}

function limparDadosDespesa() {
  despesa.value = new Despesa();
}

async function salvar() {
  try {
    let data = { ...solicitacao.value };
    //checar se o status esta aguardando despesa e se há despesa lançadas
    //verificar se finalizou o cadastro de despesas
    if (data.tipoSolicitacao == 1) {
      data.status = 5; //Aguardando Conferência
    }
    if (
      data.tipoSolicitacao == 2 &&
      data.status == 3 &&
      data.despesa.length > 0
    ) {
      let options = {
        title: "Confirmação",
        html: "Finalizou o cadastro de despesa? <br/> Ao confirmar não poderá realizar alterações!",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Sim",
        cancelButtonText: "Não",
      };

      let response = await swal.fire(options);
      if (response.isConfirmed) {
        data.status = 5; //5 - aguardando conferência
      }
    }
    if (data.id == 0) await solicitacaoStore.add(data);
    else {
      if (data.status == 4) data.status = 5; //5 - aguardando conferência
      await solicitacaoStore.update(data);
    }

    swal.fire({
      toast: true,
      icon: "success",
      index: "top-end",
      title: "Sucesso!",
      text: "Dados salvos com sucesso!",
      showConfirmButton: false,
      timer: 2000,
    });
    router.push({ name: "reembolsoSolicitacoes" });
  } catch (error) {
    console.log("solicitacao.cadastro.salvar.erro", error);
    handleErrors(error);
  } finally {
  }
}

async function registrarPagto() {
  try {
    let valor =
      solicitacao.value.tipoSolicitacao == 2
        ? solicitacao.value.valorAdiantamento
        : solicitacao.value.valorDespesa;

    let options = {
      title: "Confirmação",
      text: `Confirma o registro de pagamento no valor de ${formatToCurrencyBRL(
        valor
      )}?`,
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Sim",
      cancelButtonText: "Não",
    };

    let response = await swal.fire(options);

    if (response.isConfirmed) {
      let transacao = new Transacao(
        `Crédito - solicitação ${solicitacao.value.id}`,
        "+",
        valor
      );

      await contaStore.addTransacao(solicitacao.value.colaboradorId, transacao);

      // se tipo = 1 - reembolso, enviar para 7 - aguardando faturamento, senão, 3 - prestar contas
      let status = solicitacao.value.tipoSolicitacao == 1 ? 7 : 3;

      let novoStatus = solicitacaoStore.getStatus(status);
      
      let texto = `${authStore.user.nome} registrou pagamento ao colaborador no valor ${formatToCurrencyBRL(
        valor
      )}!`;
      

      let solicitacaoStatus = {
        solicitacaoId: solicitacao.value.id,
        evento: texto,
        status: novoStatus,
      };

      await solicitacaoStore.changeStatus(solicitacaoStatus);

      swal.fire({
        toast: true,
        icon: "success",
        index: "top-end",
        title: "Sucesso!",
        text: "Registro realizado com sucesso!",
        showConfirmButton: false,
        timer: 2000,
      });
    }
  } catch (error) {
    console.log("solicitacao.cadastro.registrarPagamento.erro", error);
    handleErrors(error);
  }
}

function salvarDespesa() {
  solicitacao.value.salvarDespesa(despesa.value);
  limparDadosDespesa();
  openDespesaForm.value = false;
}

async function removerDespesa(item) {
  let options = {
    title: "Confirma Exclusão?",
    text: "Deseja realmente excluir o despesa: " + item.nroFiscal + "?",
    icon: "question",
    showCancelButton: true,
    confirmButtonText: "Sim",
    cancelButtonText: "Não",
  };

  let response = await swal.fire(options);
  if (response.isConfirmed) {
    solicitacao.value.removerDespesa(item);
  }
}
</script>
