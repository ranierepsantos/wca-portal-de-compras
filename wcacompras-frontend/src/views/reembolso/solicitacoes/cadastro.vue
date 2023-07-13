<template>
  <div>
    <bread-crumbs
      title="Solicitação"
      :show-button="false"
      :buttons="formButtons"
      @aprovar-click="openAprovacaoForm = true"
      @salvar-click="salvar()"
      @registrarpgto-click ="registrarPagto()"
    />
    <v-container class="justify-center">
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
                ></v-select>
              </v-col>
              <v-col v-show="solicitacao.id > 0">
                <v-btn
                  :color="solicitacaoStore.getStatus(solicitacao.status).color"
                  variant="tonal"
                  
                  class="text-center"
                >
                  {{ solicitacaoStore.getStatus(solicitacao.status).text }}</v-btn
                >
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-text-field
                  label="Colaborador"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                  v-model="solicitacao.colaborador"
                  :readonly="true"
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Gestor Responsável"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                  v-model="solicitacao.gestor"
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-text-field
                  label="Cargo"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.cargo"
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Local Projeto"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.localProjeto"
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
                  v-model="solicitacao.valor"
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
          (solicitacao.tipoSolicitacao == 2 && solicitacao.status != 0)
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
                <th class="text-center text-grey">NOTA/CUPOM FISCAL</th>
                <th class="text-left text-grey">RAZÃO SOCIAL</th>
                <th class="text-center text-grey">VALOR</th>
                <th class="text-center text-grey"></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, index) in solicitacao.despesas" :key="index">
                <td class="text-center">
                  {{ moment(item.dataEvento).format("DD/MM/YYYY") }}
                </td>
                <td class="text-center">
                  {{ despesaTipoStore.getById(item.tipoDespesaId).nome }}
                </td>
                <td class="text-center">{{ item.nroFiscal }}</td>
                <td class="text-left">{{ item.razaoSocial }}</td>

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
                    :disabled="isBusy || !canEdit"
                  ></v-btn>
                  <v-btn icon="mdi-delete" variant="plain" color="error" @click="removerDespesa(item)" :disabled="isBusy || !canEdit">
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
                      solicitacao.valor - calcularTotalDespesa()
                    )
                  }}
                </td>
              </tr>
            </tfoot>
          </v-table>
        </v-card-text>

        
      </v-card>
      <!-- HISTORICO DA SOLICITAÇÃO -->
      <historico :eventos="solicitacao.eventos.sort(compararValor('dataEvento', 'desc'))" style="margin-top: 15px;" v-show="solicitacao.id != 0"/>
      <!-- FORM PARA CADASTRO DE DESPESA -->
      <v-dialog
        v-model="openDespesaForm"
        max-width="900"
        :absolute="false"
        persistent
      >
        <v-card>
          <v-breadcrumbs>
            <div class="text-h6 text-primary">Detalhamento de Despesa</div>
            <v-spacer></v-spacer>
            <v-btn
              color="primary"
              variant="outlined"
              class="text-capitalize"
              @click="openDespesaForm = false"
            >
              <b>Cancelar</b>
            </v-btn>

            <v-btn
              color="primary"
              variant="outlined"
              class="text-capitalize"
              @click="salvarDespesa()"
              style="margin-left: 5px"
            >
              <b>Salvar</b>
            </v-btn>
          </v-breadcrumbs>
          <div class="text-center">
            <despesa-form
              :despesa="despesa"
              :combo-tipo-despea="despesaTipoStore.toComboList()"
              @change-image="(image) => (despesa.comprovanteImage = image)"
            ></despesa-form>
          </div>
        </v-card>
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
import { Transacao, useUsuarioStore, IDPERFILCOLABORADOR } from "@/store/reembolso/usuario.store";
import {
  Despesa,
  Solicitacao,
  useSolicitacaoStore,
  Evento
} from "@/store/reembolso/solicitacao.store";
import moment from "moment";
import router from "@/router";
import { useRoute } from "vue-router";
import { useDespesaTipoStore } from "@/store/reembolso/despesaTipo.store";
import { computed } from "vue";
import {compararValor} from "@/helpers/functions"
import historico from "@/components/reembolso/historico.vue";
import { Usuario } from "@/store/reembolso/usuario.store";

const authStore = useAuthStore();
const clienteStore = useClienteStore();
const despesaTipoStore = useDespesaTipoStore();
const route = useRoute();
const solicitacaoStore = useSolicitacaoStore();
const openDespesaForm = ref(false);
const openAprovacaoForm = ref(false);
const isRunningEvent = ref(false);
const isBusy = ref(false);
const clientes = clienteStore.toComboList();
const swal = inject("$swal");
const solicitacao = ref(new Solicitacao());
const despesa = ref(new Despesa());
const formButtons = ref([]);
const usuario = ref(new Usuario())
//VUE FUNCTIONS
onMounted(async () => {

  usuario.value =new Usuario(await useUsuarioStore().repository.filter(q => q.usuarioSistemaPerfil.filter(qy => qy.perfilId == IDPERFILCOLABORADOR).length> 0)[0])
  solicitacao.value.colaborador = usuario.value.nome;
  solicitacao.value.cargo = usuario.value.cargo
  solicitacao.value.gestor = (await useUsuarioStore().getById(usuario.value.usuarioGestor)).nome
  
  if (parseInt(route.query.id) > 0) {
    await getSolicitacao(route.query.id);
    if ("0,4,5".includes(solicitacao.value.status)){
      formButtons.value.push({
        text: "Aprovar / Reprovar",
        icon: "",
        event: "aprovar-click",
      });
    }
      
    if ((solicitacao.value.tipoSolicitacao==2 && solicitacao.value.status == 1) || 
        "4,5".includes(solicitacao.value.status)){
        formButtons.value.push({ text: "Registrar Pagamento", icon: "", event: "registrarpgto-click" });
    }
  }else {
    //checar se o usuario já possui adiantamento em aberto
    let adiantamento = await solicitacaoStore.getByTipoAndUsuario(2, usuario.value.nome, "0,1");
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
        router.push({name: "reembolsoSolicitacoes"})
        return;
    }
  }

  if (!"2,4,5,6".includes(solicitacao.value.status))
    formButtons.value.push({ text: "Salvar", icon: "", event: "salvar-click" });
    
});

const canEdit = computed(() => !"2,4,5,6".includes(solicitacao.value.status));

//FUNCTIONS

async function aprovarReprovar(isAprovado, comentario) {
  try {
    isRunningEvent.value = true;

    /**
      { value: -1, text: "Todos" },
      { value: 0, text: "Solicitado", color: "warning", notifica: "cliente" },
      { value: 1, text: "Aguardando Prestação de Contas", color: "warning", notifica: "usuário" },
      { value: 2, text: "Aguardando faturamento", color: "info", notifica: "WCA" },
      { value: 3, text: "Rejeitado", color: "error", notifica: "usuario" },
      { value: 4, text: "Aguardando conferência", color: "info", notifica: "WCA" },
      { value: 5, text: "Aguardando aprovação cliente", color: "info", notifica: "cliente" },
      { value: 6, text: "Faturado", color: "success", notifica: "" },
    **/
    if (!isAprovado) solicitacao.value.status = 3; //rejeitado
    else {
      //tipoSolicitacao: Adiantamento, Status: Solicitado
      if (solicitacao.value.tipoSolicitacao == 2 && solicitacao.value.status == 0)
        solicitacao.value.status = 1; //1- Aguardando Prestação de Contas  
      //Status: 4 - Aguardando conferência
      else if (solicitacao.value.status == 4)
        solicitacao.value.status = 5; //5 - Aguardando Aprovação Cliente
      //Status: 5 - Aguardando Aprovação Cliente
      else if (solicitacao.value.status == 5){
        solicitacao.value.status = 2; //2 - Aguardando Faturamento
        if (solicitacao.value.tipoSolicitacao == 2)
        {
          //lançar valor das despesa na conta corrente do usuário
          let transacao = new Transacao(`Despesas da solicitação ${solicitacao.value.id}`, "-", calcularTotalDespesa())
          usuario.value.contaCorrente.adicionarTransacao(transacao)
          useUsuarioStore().update(usuario.value);
        }
      }
    }
    let reembolsoStatus = solicitacaoStore.getStatus(solicitacao.value.status);
    let texto = `Solicitação  <b>${isAprovado ? 'APROVADA': 'REJEITADA'}</b>, status alterado para <b>${reembolsoStatus.text.toUpperCase()}</b>. <br/> Comentário: ${comentario}`;
    let evento = new Evento(solicitacao.value.id, authStore.user.nome, texto)
    
    solicitacao.value.addEvento(evento)
    solicitacaoStore.update(solicitacao.value);
    openAprovacaoForm.value = false;

    let mensagem = (isAprovado ? "Aprovação" : "Rejeição") + " realizada com sucesso!"
    mensagem += reembolsoStatus.notifica !="" ? `<br/> Notificação enviada para ${reembolsoStatus.notifica}!`:""

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

  solicitacao.value.despesas.forEach((item) => {
    valorTotalDespesa += parseFloat(item.valor);
  });
  if (solicitacao.value.tipoSolicitacao == 1 ) solicitacao.value.valor = valorTotalDespesa;
  return valorTotalDespesa;
}

function editarDespesa(item) {
  despesa.value = { ...item };
  openDespesaForm.value = true;
}

function getSolicitacao(solicitacaoId) {
  try {
    isBusy.value = true;
    let data = solicitacaoStore.getById(solicitacaoId);
    solicitacao.value = data;
  } catch (error) {
    console.log("getSolicitacao.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}

function limparDadosDespesa() {
  despesa.value = new Despesa();
}

async function salvar() {
  try {
    isBusy.value = true;
    let data = { ...solicitacao.value };
    //checar se o status esta aguardando despesa e se há despesa lançadas
    //verificar se finalizou o cadastro de despesas 
    if (data.tipoSolicitacao == 1) {
      data.status = 4 //Aguardando Conferência
    }
    if (data.tipoSolicitacao==2 && data.status == 1 &&  data.despesas.length > 0) {
      let options = {
            title: "Confirmação",
            text: "Finalizou o cadastro de despesa? O status será alterado!",
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
        }

        let response = await swal.fire(options);
        if (response.isConfirmed)
        {
          data.status = 4;
        }
    }
    if (data.id == 0) solicitacaoStore.add(data);
    else {
      if (data.status == 3) data.status = 4
      solicitacaoStore.update(data);
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
    isBusy.value = false;
  }
}

async function registrarPagto() {

  
  let options = {
            title: "Confirmação",
            text: `Confirma o registro de pagamento no valor de ${formatToCurrencyBRL(solicitacao.value.valor)}?`,
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
        }

  let response = await swal.fire(options);

  if (response.isConfirmed){
    let transacao = new Transacao(`Adiantamento solicitação ${solicitacao.value.id}`, "+", solicitacao.value.valor)
    usuario.value.contaCorrente.adicionarTransacao(transacao)

    useUsuarioStore().update(usuario.value);

    let texto = `Pagamento realizado ao colaborador no valor ${formatToCurrencyBRL(solicitacao.value.valor)}!`
    let evento = new Evento(solicitacao.value.id, authStore.user.nome, texto);
    solicitacao.value.addEvento(evento)
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
}


function salvarDespesa() {
  solicitacao.value.salvarDespesa(despesa.value)
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
    }

    let response = await swal.fire(options);
    if (response.isConfirmed) {
        solicitacao.value.removerDespesa(item)
    }
}
</script>
