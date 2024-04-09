<template>
  <div>
    <bread-crumbs title="Contas" :showButton="false" />
    <v-row>
      <v-col cols="6">
        <v-text-field
          label="Pesquisar"
          placeholder="(Nome)"
          v-model="filter"
          density="compact"
          variant="outlined"
          color="info"
        >
        </v-text-field>
      </v-col>
    </v-row>
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isBusy"
    ></v-progress-linear>
    <v-table class="elevation-2">
      <thead>
        <tr>
          <th class="text-left text-grey">NOME</th>
          <th class="text-right text-grey">SALDO</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in contas" :key="item.usuarioId">
          <td class="text-left">
            {{ item.usuarioNome }}
          </td>
          <td
            :class="
              'text-right ' + (item.saldo >= 0 ? 'text-success' : 'text-error')
            "
          >
            {{ formatToCurrencyBRL(item.saldo) }}
          </td>
          <td class="text-right">
            <v-btn
              prepend-icon="mdi-text-box-search-outline"
              variant="plain"
              color="primary"
              size="small"
              :disabled="isBusy"
              @click="exibirTransacoes(item)"
              >Transações
            </v-btn>
          </td>
        </tr>
      </tbody>
      <tfoot>
        <tr>
          <td colspan="3">
            <v-pagination
              v-model="page"
              :length="totalPages"
              :total-visible="4"
            ></v-pagination>
          </td>
        </tr>
      </tfoot>
    </v-table>
    <v-dialog
      v-model="openTransacoes"
      :absolute="false"
      scrollable
      width="auto"
    >
      <v-card>
        <v-card-title>
          <v-row>
            <v-col>
              <h5>{{ conta.usuarioNome }}</h5>
            </v-col>
            <v-col cols="4" class="text-right">
              <h5>
                Saldo:
                <span :class="'text-' + (conta.saldo < 0 ? 'red' : 'green')">{{
                  formatToCurrencyBRL(conta.saldo)
                }}</span>
              </h5>
            </v-col>
            <v-col cols="1" class="text-right">
          <v-btn
            variant="plain"
            color="grey"
            @click="openTransacoes = false"
            icon="mdi-close-circle-outline"
          ></v-btn>
        </v-col>
          </v-row>
        </v-card-title>
        <v-card-text>
          <v-table class="elevation-2">
            <thead>
              <tr>
                <th class="text-center text-grey">DATA</th>
                <th class="text-center text-grey">DESCRIÇÃO</th>
                <th class="text-center text-grey">SALDO ANTERIOR</th>
                <th class="text-center text-grey">ENTRADA</th>
                <th class="text-center text-grey">SAÍDA</th>
                <th class="text-center text-grey">SALDO</th>
              </tr>
            </thead>
            <tbody>
              <template v-for="item in conta.transacoes" :key="item.id">
                <tr
                  :class="item.operador == '+' ? 'text-success' : 'text-error'"
                >
                  <td class="text-center">
                    {{ moment(item.dataHora).format("DD/MM/YYYY") }}
                  </td>
                  <td class="text-left">{{ item.descricao }}</td>
                  <td class="text-right text-black">
                    {{ formatToCurrencyBRL(item.saldoAnterior) }}
                  </td>
                  <td class="text-right">
                    {{
                      item.operador == "+"
                        ? formatToCurrencyBRL(item.valor)
                        : ""
                    }}
                  </td>
                  <td class="text-right">
                    {{
                      item.operador == "-"
                        ? formatToCurrencyBRL(item.valor)
                        : ""
                    }}
                  </td>
                  <td class="text-right text-black">
                    <b>{{ formatToCurrencyBRL(item.saldo) }}</b>
                  </td>
                </tr>
              </template>
            </tbody>
          </v-table>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from "vue";
import handleErrors from "@/helpers/HandleErrors";
import BreadCrumbs from "@/components/breadcrumbs.vue";
import { useAuthStore } from "@/store/auth.store";
import { useClienteStore } from "@/store/reembolso/cliente.store";
import { useUsuarioStore } from "@/store/reembolso/usuario.store";
import { ContaCorrente, useContaStore } from "@/store/reembolso/conta.store";
import { formatToCurrencyBRL } from "@/helpers/functions";
import moment from "moment";
//DATA
const page = ref(1);
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const isBusy = ref(false);
const totalPages = ref(1);
const clienteStore = useClienteStore();
const contas = ref([]);
const meusClientes = ref([]);
const meusCentroDeCusto = ref([]);
const filter = ref("");
const authStore = useAuthStore();
const conta = ref(new ContaCorrente());
const openTransacoes = ref(false);
//VUE METHODS
onMounted(async () => {
  let filialUsuario = (
    await useUsuarioStore().getFiliais(authStore.user.id)
  )[0];
  authStore.user.filial = filialUsuario.value;

  meusClientes.value = await clienteStore.ListByUsuario(authStore.user.id);
  meusClientes.value = meusClientes.value.map((x) => {
    return x.id;
  });

  meusCentroDeCusto.value = await useUsuarioStore().getCentrosdeCusto(
    authStore.user.id
  );
  meusCentroDeCusto.value = meusCentroDeCusto.value.map((x) => {
    return x.id;
  });

  await getItems();
});

watch(page, () => getItems());
watch(filter, () => getItems());

//METHODS
async function getItems() {
  try {
    isBusy.value = true;

    let filtros = {
      filialId: authStore.user.filial.id,
      usuarioNome: filter.value,
      centroCustoIds: meusCentroDeCusto.value,
      clientesIds: meusClientes.value,
    };
    let response = await useContaStore().getPaginate(
      page.value,
      pageSize,
      filtros
    );
    contas.value = response.items;
    totalPages.value = response.totalPages;
  } catch (error) {
    console.log("clientes.getItems.error:", error.response);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}

function exibirTransacoes(item) {
  conta.value = item;
  openTransacoes.value = true;
}
</script>
