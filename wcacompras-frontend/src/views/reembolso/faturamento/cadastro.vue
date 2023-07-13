<template>
  <div>
    <bread-crumbs
      title="Faturamento"
      :show-button="false"
      :buttons="formButtons"
      @aprovar-click="openAprovacaoForm = true"
      @salvar-click="salvar()"
    />
    <v-container class="justify-center">
      <v-card>
        <v-card-text>
          <v-form>
            <v-row>
              <v-col>
                <v-select
                  label="Cliente"
                  :items="clientes"
                  density="compact"
                  item-title="text"
                  item-value="value"
                  variant="outlined"
                  color="primary"
                  v-model="faturamento.clienteId"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                ></v-select>
              </v-col>
              <v-col cols="4" v-show="faturamento.status > 0">
                <v-btn
                  :color="faturamentoStore.getStatus(faturamento.status).color"
                  variant="tonal"
                  class="text-center"
                >
                  {{
                    faturamentoStore.getStatus(faturamento.status).text
                  }}</v-btn
                >
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>
      </v-card>
      <v-card
        style="margin-top: 10px"
        v-show="'1,2'.includes(faturamento.status) == false"
      >
        <v-card-title>
          <v-breadcrumbs>
            <div class="text-h6 text-primary">Filtros</div>
          </v-breadcrumbs>
        </v-card-title>
        <v-card-text>
          <v-row>
            <v-col>
              <v-select
                label="Colaborador"
                :items="colaboradores"
                density="compact"
                item-title="text"
                item-value="value"
                variant="outlined"
                color="primary"
                v-model="filtro.colaborador"
                :rules="[(v) => !!v || 'Campo obrigatório']"
              ></v-select>
            </v-col>
            <v-col>
              <v-select
                label="Gestor"
                :items="gestores"
                density="compact"
                item-title="text"
                item-value="value"
                variant="outlined"
                color="primary"
                v-model="filtro.gestor"
                :rules="[(v) => !!v || 'Campo obrigatório']"
              ></v-select>
            </v-col>
            <v-col cols="2">
              <v-text-field
                label="Período Inicial"
                type="date"
                variant="outlined"
                color="primary"
                density="compact"
                v-model="filtro.periodoInicial"
              ></v-text-field>
            </v-col>
            <v-col cols="2">
              <v-text-field
                label="Período Final"
                type="date"
                variant="outlined"
                color="primary"
                density="compact"
                v-model="filtro.periodoFinal"
              ></v-text-field>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
      <!-- SOLICITAÇÕES DO FATURAMENTO  -->
      <v-card style="margin-top: 20px">
        <v-card-title>
          <v-breadcrumbs>
            <div class="text-h6 text-primary">
              Items {{ faturamento.status == 2 ? "Faturados" : " a Faturar" }}
            </div>
          </v-breadcrumbs>
        </v-card-title>
        <v-card-text v-show="false"> </v-card-text>
        <!-- SOLICITAÇÕES DO FATURAMENTO  -->
        <v-card-text>
          <v-table class="elevation-2">
            <thead>
              <tr>
                <th class="text-center text-grey">#</th>
                <th class="text-center text-grey">DATA</th>
                <th class="text-center text-grey">COLABORADOR</th>
                <th class="text-center text-grey">GESTOR</th>
                <th class="text-center text-grey">VALOR</th>
                <th
                  class="text-center text-grey"
                  v-show="faturamento.status != 2"
                >
                  <div>
                    <v-btn
                      variant="plain"
                      :color="selecionarTodosItens ? 'primary' : 'secondary'"
                      :icon="
                        selecionarTodosItens
                          ? 'mdi-radiobox-marked'
                          : 'mdi-radiobox-blank'
                      "
                      @click="selecionarTodos()"
                    ></v-btn>
                    <span @click="selecionarTodos()">SELECIONAR TODOS</span>
                  </div>
                </th>
              </tr>
            </thead>
            <tbody>
              <template
                v-for="(item, index) in faturamento.faturamentoItems.sort(
                  compararValor('id')
                )"
                :key="index"
                v-if="faturamento.status == 2"
              >
                <tr>
                  <td class="text-center">{{ item.solicitacao.id }}</td>
                  <td class="text-center">
                    {{
                      moment(item.solicitacao.dataSolicitacao).format(
                        "DD/MM/YYYY"
                      )
                    }}
                  </td>
                  <td class="text-left">{{ item.solicitacao.colaborador }}</td>
                  <td class="text-left">{{ item.solicitacao.gestor }}</td>
                  <td class="text-right">
                    {{
                      formatToCurrencyBRL(parseFloat(item.solicitacao.valor))
                    }}
                  </td>
                </tr>
              </template>
              <template
                v-for="(item, index) in items.sort(compararValor('id'))"
                :key="item.id"
                v-else
              >
                <tr>
                  <td class="text-center">{{ item.id }}</td>
                  <td class="text-center">
                    {{ moment(item.dataSolicitacao).format("DD/MM/YYYY") }}
                  </td>
                  <td class="text-left">{{ item.colaborador }}</td>
                  <td class="text-left">{{ item.gestor }}</td>
                  <td class="text-right">
                    {{ formatToCurrencyBRL(parseFloat(item.valor)) }}
                  </td>
                  <td class="text-center">
                    <v-btn
                      variant="plain"
                      :color="item.selected ? 'primary' : 'secondary'"
                      :title="item.selected ? 'Desmarcar' : 'Marcar'"
                      :icon="
                        item.selected
                          ? 'mdi-radiobox-marked'
                          : 'mdi-radiobox-blank'
                      "
                      @click="checkUncheckItem(item)"
                    >
                    </v-btn>
                  </td>
                </tr>
              </template>
            </tbody>
            <tfoot>
              <tr>
                <td class="text-right" colspan="4"><b>TOTAL:</b></td>
                <td class="text-right">
                  {{ formatToCurrencyBRL(valorFaturamento) }}
                </td>
                <td v-show="faturamento.status != 2"></td>
              </tr>
            </tfoot>
          </v-table>
        </v-card-text>
      </v-card>
      <!-- FORM PARA APROVAR / REJEITAR FATURAMENTO -->
      <v-dialog
        v-model="openAprovacaoForm"
        max-width="700"
        :absolute="false"
        persistent
      >
        <aprovar-rejeitar-form
          title="APROVAR / REJEITAR - FATURAMENTO"
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
import { ref, inject, computed, watch, onMounted } from "vue";
import aprovarRejeitarForm from "@/components/aprovarRejeitarForm.vue";
import { useAuthStore } from "@/store/auth.store";
import { formatToCurrencyBRL, compararValor } from "@/helpers/functions";
import { useClienteStore } from "@/store/reembolso/cliente.store";
import { Evento, useSolicitacaoStore } from "@/store/reembolso/solicitacao.store";
import { useUsuarioStore } from "@/store/reembolso/usuario.store";
import {
  Faturamento,
  FaturamentoEvento,
  FaturamentoItem,
  useFaturamentoStore,
} from "@/store/reembolso/faturamento.store";
import moment from "moment";
import { useRoute } from "vue-router";
import router from "@/router";
import handleErrors from "@/helpers/HandleErrors";
import { Solicitacao } from "@/store/reembolso/solicitacao.store";

const openAprovacaoForm = ref(false);
const authStore = useAuthStore();
const solicitacaoStore = useSolicitacaoStore();
const clienteStore = useClienteStore();
const usuarioStore = useUsuarioStore();
const faturamentoStore = useFaturamentoStore();
const faturamento = ref(new Faturamento());
const isRunningEvent = ref(false);
const clientes = ref([]);
const gestores = ref([]);
const colaboradores = ref([]);
const items = ref([]);
const route = useRoute();
const swal = inject("$swal");
const filtro = ref({
  periodoInicial: moment().format("DD/MM/YYYY"),
  periodoFinal: moment().format("DD/MM/YYYY"),
});
const selecionarTodosItens = ref(false);
const formButtons = ref([]);
//VUE - FUNCTIONS
onMounted(async () => {
  clientes.value = clienteStore.toComboList(authStore.user.filialId);
  if (parseInt(route.query.id) > 0) {
    await getById(route.query.id);
    if (faturamento.value.status == 1)
      formButtons.value.push({
        text: "Aprovar / Reprovar",
        icon: "",
        event: "aprovar-click",
      });
  }

  if (!"1,2".includes(faturamento.value.status))
    formButtons.value.push({ text: "Salvar", icon: "", event: "salvar-click" });
});

watch(
  () => faturamento.value.clienteId,
  (newValue, oldValue) => {
    if (oldValue && newValue != oldValue)
      faturamento.value.faturamentoItems = [];

    items.value = solicitacaoStore.getToFaturamento(
      faturamento.value.clienteId
    );
    gestores.value = usuarioStore.toComboListGestorByCliente(
      faturamento.value.clienteId
    );
    colaboradores.value = usuarioStore.toComboListColaboradorByCliente(
      faturamento.value.clienteId
    );
  }
);

const valorFaturamento = computed(() => {
  faturamento.value.valor = 0;
  faturamento.value.faturamentoItems.forEach((item) => {
    faturamento.value.valor += parseFloat(item.solicitacao.valor);
  });
  return faturamento.value.valor;
});

//FUNCTIONS

async function aprovarReprovar(isAprovado, comentario) {
  try {
    isRunningEvent.value = true;

    if (!isAprovado) faturamento.value.status = 3; //rejeitado
    else faturamento.value.status = 2; //aprovado

    openAprovacaoForm.value = false;

    let texto = `Solicitação  <b>${
      isAprovado ? "APROVADA" : "REJEITADA"
    }</b> por ${authStore.user.nome}. <br/> Comentário: ${comentario}`;

    faturamento.value.addEvento(
      new FaturamentoEvento(faturamento.value.id, authStore.user.nome, texto)
    );

    faturamentoStore.update(faturamento.value);

    //passa por cada solicitação e atualiza o status para Faturado
    if (isAprovado) {
      faturamento.value.faturamentoItems.forEach((fitem) => {
        let sitem = new Solicitacao(fitem.solicitacao);
        sitem.status = 6; //faturado
        sitem.addEvento(new Evento(sitem.id, authStore.user.nome,`Solicitação Faturada, nº faturamento: ${faturamento.value.id}`));
        solicitacaoStore.update(sitem);
      });
    }

    let mensagem =
      (isAprovado ? "Aprovação" : "Rejeição") + " realizada com sucesso!";

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

function checkUncheckItem(item) {
  item.selected = !item.selected;
  addRemove(item);
  selecionarTodosItens.value =
    faturamento.value.faturamentoItems.length == items.value.length;
}

function addRemove(solicitacao) {
  let index = faturamento.value.faturamentoItems.findIndex(
    (q) => q.solicitacao.id == solicitacao.id
  );

  if (solicitacao.selected) {
    if (index == -1) {
      let item = new FaturamentoItem();
      item.solicitacaoId = solicitacao.id;
      item.faturamentoId = faturamento.value.id;
      item.solicitacao = { ...solicitacao };
      faturamento.value.adicionarAlterarItem(item);
    }
  } else {
    if (index != -1)
      faturamento.value.removerItem(faturamento.value.faturamentoItems[index]);
  }
}

async function getById(faturamentoId) {
  try {
    //isBusy.value = true;
    let data = await faturamentoStore.getById(faturamentoId);
    faturamento.value = data;
  } catch (error) {
    console.log("getById.error:", error);
    handleErrors(error);
  } finally {
    //isBusy.value = false;
  }
}

async function salvar() {
  try {
    let data = faturamento.value;
    if (data.status == 0) {
      await faturamentoStore.add(data);
    } else {
      if (data.status == 3) data.status = 1;
      await faturamentoStore.update(data);
    }

    swal.fire({
      toast: true,
      icon: "success",
      position: "top-end",
      title: "Sucesso!",
      text: "Dados salvos com sucesso!",
      showConfirmButton: false,
      timer: 2000,
    });
    router.push({ name: "reembolsoFaturamento" });
  } catch (error) {
    console.log("usuários.error:", error);
    handleErrors(error);
  }
}

function selecionarTodos() {
  selecionarTodosItens.value = !selecionarTodosItens.value;
  for (let idx = 0; idx < items.value.length; idx++) {
    let item = items.value[idx];
    if (
      item.selected == undefined ||
      item.selected != selecionarTodosItens.value
    ) {
      item.selected = selecionarTodosItens.value;
      addRemove(item);
    }
  }
}
</script>
