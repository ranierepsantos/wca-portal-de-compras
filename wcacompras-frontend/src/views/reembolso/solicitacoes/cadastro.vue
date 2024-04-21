<template>
  <div>
    <bread-crumbs
      title="Solicitação"
      :show-button="false"
      :buttons="formButtons"
      @aprovar-click="aprovarReprovarOpen()"
      @salvar-click="salvar()"
      @registrarpgto-click="abrirDepositoForm()"
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
                  item-title="nome"
                  item-value="id"
                  variant="outlined"
                  color="primary"
                  v-model="solicitacao.clienteId"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                  v-if="solicitacao.id == 0"
                ></v-select>
                <v-text-field
                  label="Cliente"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                  v-model="solicitacao.cliente.nome"
                  :readonly="true"
                  bg-color="#f2f2f2"
                  v-else
                ></v-text-field>
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
                  :readonly="solicitacao.id > 0"
                  :bg-color="solicitacao.id > 0? '#f2f2f2':''"
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
            <v-row v-show="solicitacao.id > 0">
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
                  label="Centro de Custo"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.centroCustoNome"
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
              <v-col v-show="solicitacao.id == 0 && !isColaborador">
                <v-select
                  label="Centro de Custo"
                  :items="listCentroCusto"
                  density="compact"
                  item-title="nome"
                  item-value="id"
                  variant="outlined"
                  color="primary"
                  v-model="solicitacao.centroCustoId"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                  
                ></v-select>
              </v-col>
              <v-col v-show="solicitacao.id == 0 && isColaborador">
                <v-text-field
                  label="Centro de Custo"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.centroCustoNome"
                  :readonly="true"
                  bg-color = "#f2f2f2"
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
                  :readonly="isReadonly"
                  :bg-color = 'isReadonly ? "#f2f2f2":"" '
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
                  :readonly="isReadonly"
                  :bg-color = 'isReadonly ? "#f2f2f2":"" '
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
                  :readonly="isReadonly"
                  :bg-color = 'isReadonly ? "#f2f2f2":"" '
                ></v-text-field>
              </v-col>
              <v-col v-show="solicitacao.tipoSolicitacao == 2">
                <v-text-field-money
                  label-text="Valor Solicitação"
                  v-model="solicitacao.valorAdiantamento"
                  color="primary"
                  :number-decimal="2"
                  prefix="R$"
                  :readonly="isReadonly"
                  :bg-color = 'isReadonly ? "#f2f2f2":"" '
                ></v-text-field-money>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>
      </v-card>

      <v-card
        style="margin-top: 20px"
        v-show="showSecaoDespesa"
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
              v-show="despesaCanAdd"
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
                <th class="text-center text-grey" colspan="2">DESCRICAO</th>
                <th class="text-center text-grey">VALOR</th>
                <th class="text-center text-grey">STATUS</th>
                <th class="text-center text-grey"></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, index) in solicitacao.despesa.sort(compararValor('dataEvento', 'asc'))" :key="index">
                <td class="text-center">
                  {{ moment(item.dataEvento).format("DD/MM/YYYY") }}
                </td>
                <td class="text-center">
                  {{ getDespesaTipo(item.tipoDespesaId).nome }}
                </td>
                <td class="text-center" colspan="2">
                  {{
                    getDespesaTipo(item.tipoDespesaId).tipo == 1
                      ? item.razaoSocial +
                        " : " +
                        item.numeroFiscal
                      : item.origem + " > " + item.destino
                  }}
                </td>
                <td class="text-right">
                  {{ formatToCurrencyBRL(parseFloat(item.valor)) }}
                </td>
                <td v-if="!item.aprovada">
                  <v-icon icon="mdi-eye-off-outline" variant="plain"
                            color="gray" title="à conferir"></v-icon>
                </td>
                <td v-else>
                  <v-icon :icon="item.aprovada == 1 ? 'mdi-eye-check-outline' : 'mdi-eye-remove-outline'" variant="plain"
                            :color="item.aprovada == 1 ? 'success' : 'error'"
                            :title="item.aprovada == 1 ? 'conferido' : 'reprovado'"></v-icon>
                </td>
                <td class="text-right">
                  <v-btn
                    icon="mdi-text-box-search-outline"
                    size="smaller"
                    variant="plain"
                    color="primary"
                    @click="editarDespesa(item)"
                    title="Visualizar"
                    v-show ="despesaCanView && !despesaCanEdit"
                  ></v-btn>
                  <v-btn
                    icon="mdi-lead-pencil"
                    size="smaller"
                    variant="plain"
                    color="primary"
                    @click="editarDespesa(item)"
                    title="Editar"
                    :disabled="isBusy"
                    v-show ="!despesaCanView || despesaCanEdit"
                  ></v-btn>
                  <v-btn
                    icon="mdi-delete"
                    variant="plain"
                    color="error"
                    @click="removerDespesa(item)"
                    :disabled="isBusy"
                    v-show="!despesaCanView || (despesaCanEdit && authStore.user.id == solicitacao.colaboradorId)"
                  >
                  </v-btn>
                </td>
              </tr>
            </tbody>
            <tfoot>
              <tr>
                <td class="text-right" colspan="5"><b>TOTAL:</b></td>
                <td class="text-right">
                  {{ formatToCurrencyBRL(calcularTotalDespesa()) }}
                </td>
                <td></td>
              </tr>
              <tr v-show="solicitacao.tipoSolicitacao == 2">
                <td class="text-right" colspan="5"><b>SALDO:</b></td>
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
        min-width="1024"
        
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
          @save-click="salvarDespesa($event)"
          :read-only="!(despesaCanEdit && !(despesa.aprovada == 1))"
          :can-aprove="solicitacao.status == 5 && despesa.aprovada !== 1 && authStore.hasPermissao('wca_aprovacao')"
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
      <!-- FORM PARA REGISTRAR PAGAMENTO -->
      <v-dialog
        v-model="openDepositoForm"
        max-width="500"
        :absolute="false"
        persistent
      >
        <registrar-pagamento-form 
          :data="dadosDeposito" 
          @close-form="openDepositoForm = false"
          @registrar-pagamento = "registrarPagto($event)"
          />
      </v-dialog>
    </v-container>
  </div>
</template>

<script setup>
import breadCrumbs from "@/components/breadcrumbs.vue";
import registrarPagamentoForm from "@/components/reembolso/registrarPagamentoForm.vue";

import { ref, inject, onMounted } from "vue";
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import DespesaForm from "@/components/reembolso/despesaForm.vue";
import aprovarRejeitarForm from "@/components/aprovarRejeitarForm.vue";
import { useAuthStore } from "@/store/auth.store";
import handleErrors from "@/helpers/HandleErrors";
import { formatToCurrencyBRL } from "@/helpers/functions";
import { useClienteStore } from "@/store/reembolso/cliente.store";
import { IDPERFILCOLABORADOR, Usuario, useUsuarioStore } from "@/store/reembolso/usuario.store";
import {
  Despesa,
  Solicitacao,
  useSolicitacaoStore,
} from "@/store/reembolso/solicitacao.store";
import moment from "moment";
import router from "@/router";
import { useRoute } from "vue-router";
import { useDespesaTipoStore } from "@/store/reembolso/despesaTipo.store";
import { computed } from "vue";
import { compararValor } from "@/helpers/functions";
import historico from "@/components/reembolso/historico.vue";
import { Transacao, useContaStore } from "@/store/reembolso/conta.store";
import { watch } from "vue";


const authStore = useAuthStore();
const clienteStore = useClienteStore();
const despesaTipoStore = useDespesaTipoStore();
const contaStore = useContaStore();
const route = useRoute();
const solicitacaoStore = useSolicitacaoStore();
const openDespesaForm = ref(false);
const openDepositoForm = ref(false);
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
const listCentroCusto = ref([])
const dadosDeposito = ref(
  {
        saldo: 0,
        dataDeposito: moment().format("YYYY-MM-DD"),
        valorDeposito: 0,
  }
)
//COMPUTED
const isReadonly = computed(() =>{

  return (solicitacao.value.id > 0 && solicitacao.value.status != 4 && solicitacao.value.status != 10)
      || (solicitacao.value.colaboradorId != authStore.user.id)
})
const showSecaoDespesa = computed(()=> {
  return solicitacao.value.tipoSolicitacao == 1 ||
        (solicitacao.value.tipoSolicitacao == 2 && solicitacao.value.status != 1)
})
const despesaCanAdd = computed(() => {
    
    /**
     * pode adicionar despesa quando:
     * colaborador = auth.user && (status == 3 || status == 4 || status ==10)
     * colaborador != auth.user && status == 5 && hasPermissao('despesa-aprovar'))
     */
    let can = (solicitacao.value.colaboradorId == authStore.user.id && solicitacao.value.id == 0) || 
              (solicitacao.value.colaboradorId == authStore.user.id && "3,4,10,12".includes(solicitacao.value.status)) 
    return can
  }
)

const despesaCanEdit = computed(() => {
    
    /**
     * pode adicionar despesa quando:
     * colaborador = auth.user && (status == 3 || status == 4 || status ==10)
     * colaborador != auth.user && status == 5 && hasPermissao('despesa-editar'))
     */
    let can = (solicitacao.value.colaboradorId == authStore.user.id && "3,4,10,12".includes(solicitacao.value.status)) ||
              (solicitacao.value.status == 5 && authStore.hasPermissao('despesa-editar'))
    return can
  }
)

const solicitacaoCanEdit = computed(() => solicitacao.value.colaboradorId == authStore.user.id && "1,3,4,10,12".includes(solicitacao.value.status) && authStore.hasPermissao("solicitacao"));

const despesaCanView = computed(() => solicitacao.value.colaboradorId != authStore.user.id || !(solicitacao.value.colaboradorId == authStore.user.id && "1,3,4,10,12".includes(solicitacao.value.status)));

const isColaborador = computed(() => {
  
    return (
      authStore.sistema.perfil.id == IDPERFILCOLABORADOR
    );
  
});


//VUE FUNCTIONS
onMounted(async () => {
  try {
    isBusy.value = true;
    usuario.value = await useUsuarioStore().getById(authStore.user.id);
    clientes.value = await clienteStore.ListByUsuario(usuario.value.id);
    despesaTipos.value = await despesaTipoStore.toComboList();

    if (parseInt(route.query.id) > 0) {
      await getSolicitacao(route.query.id);
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
      solicitacao.value.colaboradorCargo = usuario.value.usuarioReembolsoComplemento.cargo;
      if (clientes.value.length > 0) {
        let centroCusto = clientes.value[0].centroCusto.find(q => q.centroCustoId == usuario.value.usuarioReembolsoComplemento.centroCustoId)
        solicitacao.value.centroCustoId = centroCusto ? centroCusto.id: null
        solicitacao.value.centroCustoNome = centroCusto? centroCusto.nome: ""
      }
      
      if (clientes.value.length ==1)
        solicitacao.value.clienteId = clientes.value[0].id

    }
    carregarBotoes()
      
  } catch (error) {
    console.log("onMounted.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
});

watch(() => solicitacao.value.clienteId, async (newValue,oldValue) => {
  
  if (newValue != oldValue) {
    let cliente = clientes.value.find(q => q.id == newValue)
    listCentroCusto.value = await useUsuarioStore().getCentrosdeCusto(authStore.user.id, cliente.id)
    if (!isColaborador.value)
      solicitacao.value.centroCustoId = null

  }
  
})


//FUNCTIONS

function aprovarReprovarOpen() {
    let semConferencia = solicitacao.value.despesa.filter(q => q.aprovada==null || q.aprovada==0)
    if (semConferencia.length > 0 && solicitacao.value.status == 5) {
      swal.fire({
        toast: true,
        icon: "warning",
        index: "top-end",
        title: "Atenção!",
        text: "Há despesas sem conferência!",
        showConfirmButton: false,
        timer: 4000,
      });
      return 
    }
    openAprovacaoForm.value = true
}

async function aprovarReprovar(isAprovado, comentario) {
  try {
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
      10 - Pré Cadastro
      11 - Aguardando Depósito (somente adiantamento, quando valor da despesa > valor adiantamento)
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
      else if (solicitacao.value.status == 5){
        solicitacao.value.status = 6; //6 - Aguardando Aprovação Cliente
        //21.03.2024 - Luciano solicitou para adicionar débito quando cliente aprovar
        // let transacao = new Transacao(
        //   `Débito - solicitação ${solicitacao.value.id}`,
        //   "-",
        //   solicitacao.value.valorDespesa
        // );
        // contaStore.addTransacao(solicitacao.value.colaboradorId, transacao);

      }
      //Status: 6 - Aguardando Aprovação Cliente
      else if (solicitacao.value.status == 6) {
        solicitacao.value.status = 7; //7 - Aguardando Faturamento

        //Adicionar o valor da despesa com débito do colaborador
        let transacao = new Transacao(
          `Débito - solicitação ${solicitacao.value.id}`,
          "-",
          solicitacao.value.valorDespesa
        );
        contaStore.addTransacao(solicitacao.value.colaboradorId, transacao);


        // Se o tipo de solicitação for reembolso - enviar para aguardando depósito
        if (solicitacao.value.tipoSolicitacao == 1)
          solicitacao.value.status = 2; //2 - Aguardando Depósito
        else (solicitacao.value.tipoSolicitacao == 2 && (solicitacao.value.valorAdiantamento - calcularTotalDespesa()) < 0)
          solicitacao.value.status = 11; //11 - Aguardando Depósito

      }
    }
    let reembolsoStatus = solicitacaoStore.getStatus(solicitacao.value.status);
    let texto = `Solicitação  <b>${
      isAprovado ? "APROVADA" : "REJEITADA"
    }</b> por ${authStore.user.nome}, status alterado para <b>${
      reembolsoStatus.status
    }</b>. <br/> Comentário: ${comentario}`;
    
    let notificarUsuario = await retornaUsuariosParaNotificar(reembolsoStatus)
    
    let solicitacaoStatus = {
      solicitacaoId: solicitacao.value.id,
      evento: texto,
      status: reembolsoStatus,
      notificar: notificarUsuario
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
    router.push({ name: "reembolsoSolicitacoes" });
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
  valorTotalDespesa = valorTotalDespesa.toFixed(2);
  solicitacao.value.valorDespesa = valorTotalDespesa;
  return valorTotalDespesa;
}

function editarDespesa(item) {
  despesa.value = new Despesa({ ...item });
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
    
    // verificar se o limite foi excedido
    if (hasValorLimiteExcedido(data)) {
      let options = {
        title: "Atenção",
        html: "O valor limite definido para o cliente foi atingido! <br/> Esta solicitação será abortada",
        icon: "warning",
        showCancelButton: false
      };

      await swal.fire(options);
      return
    }

    //checar se o status esta aguardando prestação de contas e se há despesa lançadas
    //verificar se finalizou o cadastro de despesas
    if (data.tipoSolicitacao == 2 && (data.status == 3 || data.status == 12) && data.despesa.length > 0) {
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
    
    //trocar os id despesa negativos por 0
    data.despesa.forEach(d => {
      if (d.id < 0) d.id = 0
    });
    
    // verificar se haverá notificação
    let statusAtual = 0 

    if (data.status == 4) {
      statusAtual = data.statusAnterior
    }else {
      if (data.id == 0 || data.status == 10) //status 10 - pré cadastro
      {
        if (data.tipoSolicitacao == 2 ) 
          data.status = 1; //1 - solicitado
        else 
          data.status = 5; //5 - aguardando conferência
      }
      statusAtual = data.status
    }

    let status = solicitacaoStore.getStatus(statusAtual) 
    let notificar = await retornaUsuariosParaNotificar(status);
    
    data.notificar = notificar;


    if (data.id == 0) {
      await solicitacaoStore.add(data);
    } else
      await solicitacaoStore.update(data);
    
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

async function abrirDepositoForm() {
  
  dadosDeposito.value.saldo = 0
  let conta = await useContaStore().getByUsuarioId(solicitacao.value.colaboradorId)
  if (conta) {
    dadosDeposito.value.saldo = conta.saldo
  }

  let valorDeposito = solicitacao.value.tipoSolicitacao == 2
                      ? solicitacao.value.valorAdiantamento
                      : solicitacao.value.valorDespesa;

  if (solicitacao.value.status == 11)
    valorDeposito = calcularTotalDespesa() - solicitacao.value.valorAdiantamento


  dadosDeposito.value.valorDeposito = valorDeposito
        
  openDepositoForm.value = true

}

async function registrarPagto(dados) {
  try {
    openDepositoForm.value = false
    let options = {
        title: "Confirmar",
        text: `Confirmar depósito na data ${moment(dados.dataDeposito).format("DD/MM/YYYY")}, no valor: ${formatToCurrencyBRL(dados.valorDeposito)}?`,
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Sim",
        cancelButtonText: "Não",
    }

    let response = await swal.fire(options);
    if (response.isConfirmed) {
      
      let transacao = new Transacao(
        `Crédito - solicitação ${solicitacao.value.id}`,
        "+",
        dados.valorDeposito
      );

      await contaStore.addTransacao(solicitacao.value.colaboradorId, transacao);

      // se tipo = 1 - reembolso, enviar para 7 - aguardando faturamento, senão, 3 - prestar contas
      let status = solicitacao.value.tipoSolicitacao == 1 ? 7 : 3;

      //Status: 11 - Aguardando Depósito, usado somente quando há diferença entre despesa e valor adiantado
      if (solicitacao.value.status == 11)
        status = 7 //Aguardando Faturamento

      let novoStatus = solicitacaoStore.getStatus(status);
      
      let texto = `${authStore.user.nome} registrou pagamento ao colaborador.
                  <br/> Depositado em: ${moment(dados.dataDeposito).format("DD/MM/YYYY")}
                  <br/> Valor: ${formatToCurrencyBRL(dados.valorDeposito)}.`;
      
      let notificarUsuario = await retornaUsuariosParaNotificar(novoStatus);

      let solicitacaoStatus = {
        solicitacaoId: solicitacao.value.id,
        evento: texto,
        status: novoStatus,
        notificar: notificarUsuario,
        dataDeposito: dados.dataDeposito,
        valorDeposito: dados.valorDeposito
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
      router.push({ name: "reembolsoSolicitacoes" });
    }
  } catch (error) {
    console.log("solicitacao.cadastro.registrarPagamento.erro", error);
    handleErrors(error);
  }
}

async function salvarDespesa() {
  try {
    let tipoDespesa = despesaTipos.value.find(q =>  q.id == despesa.value.tipoDespesaId)
    let hasDespesa = tipoDespesa.tipo == 1 ? await solicitacaoStore.checarSeDespesaExiste(despesa.value): false;
    if (hasDespesa) {
      limparDadosDespesa();
      openDespesaForm.value = false;
      swal.fire({
          icon: "warning",
          index: "top-end",
          title: "Atenção!",
          text: "Já existe despesa cadastrada com esta nota e cnpj!",
          showConfirmButton: true
        });
        return 
    }
    if (solicitacao.value.id == 0){
      solicitacao.value.status = 10 // pré cadastro
      solicitacao.value.despesa.push({...despesa.value})
      solicitacao.value.notificar = []
      let response = await solicitacaoStore.add(solicitacao.value)
      console.log(response)
      solicitacao.value = new Solicitacao(response.data)
    }else {
      despesa.value.solicitacaoId = solicitacao.value.id  
      despesa.value = despesa.value.id <= 0 ? (await solicitacaoStore.criarDespesa(despesa.value)): (await solicitacaoStore.atualizarDespesa(despesa.value))
      solicitacao.value.salvarDespesa(despesa.value);
    }
    limparDadosDespesa();
    openDespesaForm.value = false;
  } catch (error) {
    console.error("salvarDespesa.error", error)
    handleErrors(error)
  }
    
}

async function removerDespesa(item) {
  let options = {
    title: "Confirma Exclusão?",
    text: "Deseja realmente excluir o despesa?",
    icon: "question",
    showCancelButton: true,
    confirmButtonText: "Sim",
    cancelButtonText: "Não",
  };
  try {
    let response = await swal.fire(options);
    if (response.isConfirmed) {
      await solicitacaoStore.excluirDespesa(item)
      solicitacao.value.removerDespesa(item);
    }  
  } catch (error) {
    console.error("removerDespesa.error", error)
    handleErrors(error)
  }
  
}
function carregarBotoes() {

  let statusSolicitacao = solicitacaoStore.getStatus(solicitacao.value.status);

  let wcaAprova = statusSolicitacao.autorizar == true && statusSolicitacao.notifica == 1 && authStore.hasPermissao("wca_aprovacao")
  let clienteAprova = statusSolicitacao.autorizar == true && statusSolicitacao.notifica == 2 && authStore.hasPermissao("cliente_aprovacao")
  let registraPagamento = (solicitacao.value.status == 2 || solicitacao.value.status == 11) && authStore.hasPermissao("solicitacaoregistrarpagamento")
  
  let podeSalvar = solicitacao.value.id == 0  || solicitacaoCanEdit.value


  if ((wcaAprova || clienteAprova) && solicitacao.value.id > 0 && solicitacao.value.status != 10) {
    formButtons.value.push({
      text: "Aprovar / Reprovar",
      icon: "",
      event: "aprovar-click",
    });
  }

  if (registraPagamento) {
    formButtons.value.push({
      text: "Registrar Pagamento",
      icon: "",
      event: "registrarpgto-click",
    });
  }

  if (podeSalvar)
      formButtons.value.push({ text: "Enviar para Aprovação", icon: "", event: "salvar-click" });

}

function hasValorLimiteExcedido(data) {
  //1. pegar informações do cliente
  let _cliente = clientes.value.find(q => q.id == data.clienteId)
  if (_cliente && _cliente.valorLimite > 0) {
    let valorSolicitacao = data.tipoSolicitacaoId == 1? data.valorDespesa: data.valorAdiantamento
    return (parseFloat(valorSolicitacao) + parseFloat(_cliente.valorUtilizadoMes)) > parseFloat(_cliente.valorLimite);
  }
  return true
}

async function retornaUsuariosParaNotificar(status) {
  let list = []
  if (status.notifica == 1) // wca
  {
    let notificaList = await useUsuarioStore().getUsuarioToNotificacaoByCliente(solicitacao.value.clienteId, "wca_aprovacao")
    list = notificaList.map(q => {return q.value})
  }else if (status.notifica == 2 && solicitacao.value.centroCustoId) //gestores
    list = (await useUsuarioStore().getUsuarioToNotificacaoByCentroDeCusto(solicitacao.value.centroCustoId))
  else if (status.notifica == 3) //colaborador
    list.push(solicitacao.value.colaboradorId)

  return list;
}


</script>
