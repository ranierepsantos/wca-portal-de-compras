<template>
    <div>
        <bread-crumbs title="Faturamento" :show-button="false" @novo-click="editar('novo')" 
        :buttons="formButtons"
        />
        <!--FILTROS - INÍCIO-->
        <v-row v-show="!isLoading.form">
            <v-col v-show="isMatriz">
                <v-select
                label="Filiais"
                v-model="filter.filialId"
                :items="filiais"
                density="compact"
                item-title="text"
                item-value="value"
                variant="outlined"
                color="primary"
                :hide-details="true"
                ></v-select>
            </v-col>
            <v-col>
                <v-select
                label="Clientes"
                v-model="filter.clienteId"
                :items="clientes"
                density="compact"
                item-title="text"
                item-value="value"
                variant="outlined"
                color="primary"
                :hide-details="true"
                ></v-select>
            </v-col>
        </v-row>
        <v-row v-show="!isLoading.form">
            <v-col cols="3">
                <v-select
                label="Status"
                v-model="filter.status"
                :items="faturamentoStore.faturamentoStatus"
                density="compact"
                item-title="status"
                item-value="id"
                variant="outlined"
                color="primary"
                :hide-details="true"
                ></v-select>
            </v-col>
            <v-col cols="2">
                <v-text-field
                label="Data Início"
                v-model="filter.dataIni"
                type="date"
                variant="outlined"
                color="primary"
                density="compact"
                ></v-text-field>
            </v-col>
            <v-col cols="2">
                <v-text-field
                label="Data Fim"
                v-model="filter.dataFim"
                type="date"
                variant="outlined"
                color="primary"
                density="compact"
                ></v-text-field>
            </v-col>
            <v-col class="text-right">
                <v-btn
                color="primary"
                variant="outlined"
                class="text-capitalize"
                @click="getItems()"
                >
                <b>Aplicar Filtros</b>
                </v-btn>
                &nbsp;
                <v-btn
                color="info"
                variant="outlined"
                class="text-capitalize"
                @click="clearFilters()"
                >
                <b>Limpar Filtros</b>
                </v-btn>
            </v-col>
        </v-row>
        <!--FILTROS - FIM-->
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isLoading.form || isLoading.busy"></v-progress-linear>
        <v-table class="elevation-2" v-show="!isLoading.form">
            <thead>
                <tr>
                    <th class="text-center text-grey">#</th>
                    <th class="text-center text-grey">DATA</th>
                    <th class="text-left text-grey">CLIENTE</th>
                    <th class="text-center text-grey">VALOR</th>
                    <th class="text-left text-grey">STATUS</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in faturamentos" :key="item.id">
                    <td class="text-center">{{item.id}}</td>
                    <td class="text-center">{{ moment(item.dataCriacao).format("DD/MM/YYYY") }}</td>
                    <td class="text-left">
                        {{ item.clienteNome }}
                    </td>
                    <td class="text-right">{{ formatToCurrencyBRL(item.valor) }}</td>
                    <td class="text-left">
                        <v-btn :color="faturamentoStore.getStatus(item.status).color" variant="tonal"
                            density="compact" class="text-center"> {{
                                faturamentoStore.getStatus(item.status).status
                            }}</v-btn>    
                    </td>
                    <td class="text-right">
                        <div class="text-center">
                            <v-menu open-on-hover>
                                <template v-slot:activator="{ props }">
                                    <v-btn
                                        icon="mdi-dots-vertical"
                                        color="primary"
                                        v-bind="props"
                                        variant="plain"
                                    >
                                    </v-btn>
                                </template>
                                <v-list>
                                    <v-list-item>
                                        <v-btn
                                            prepend-icon="mdi-text-box-search-outline"
                                            variant="plain"
                                            color="primary"
                                            @click="editar(item.id)"
                                            size="small"
                                            :disabled="isBusy"
                                        >Visualizar</v-btn>
                                    </v-list-item>
                                    <v-list-item>
                                        <v-btn
                                            prepend-icon="mdi-history"
                                            variant="plain"
                                            color="primary"
                                            @click="showHistorico(item)"
                                            size="small"
                                            :disabled="isBusy"
                                        >Histórico</v-btn>
                                    </v-list-item>
                                </v-list>
                            </v-menu>
                        </div>                        
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="8">
                        <v-pagination v-model="page" :length="totalPages" :total-visible="4"></v-pagination>
                    </td>
                </tr>
            </tfoot>
        </v-table>
        <!--DIALOG PARA EXIBIR HISTÓRICO -->
        <v-dialog v-model="openHistorico" max-width="900" max-height="500" scrollable>
            <historico :eventos="faturamentoEventos" @close-click="openHistorico=false"/>
        </v-dialog>
    </div>
</template>
  
<script setup>
import { ref, onMounted, watch, inject } from "vue";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import moment from "moment";
import { useFaturamentoStore } from "@/store/reembolso/faturamento.store";
import { useClienteStore } from "@/store/reembolso/cliente.store";
import historico from "@/components/reembolso/historico.vue";
import { formatToCurrencyBRL } from "@/helpers/functions";
import { useAuthStore } from "@/store/auth.store";
import filialService from "@/services/filial.service";
import { useUsuarioStore } from "@/store/reembolso/usuario.store";
//DATA
const authStore = useAuthStore();
const page = ref(1);
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const isBusy = ref(false);
const totalPages = ref(1);
const faturamentos = ref([]);
const clientes = ref([]);
const filiais = ref([]);
const filter = ref({
  filialId: null,
  clienteId: null,
  status: null,
  dataIni: null,
  dataFim: null,
  usuarioId: null
});
const faturamentoStore = useFaturamentoStore()
const faturamentoEventos = ref([])
const openHistorico =ref(false)
const isLoading = ref({
  form: true,
  busy: false,
});
const isMatriz= ref(false)
const formButtons= ref([])
//VUE METHODS
onMounted(async () =>
{
    try {
        isLoading.value.form = true
        await getFiliaisToList();
        let filialUsuario = (await useUsuarioStore().getFiliais(authStore.user.id))[0];
        isMatriz.value = filialUsuario.text.toLowerCase() == "matriz";
        authStore.user.filial = filialUsuario.value;
        await clearFilters();
        if (authStore.hasPermissao("faturamento")) {
            formButtons.value.push({ text: "Novo", icon: "", event: "novo-click" });
        }

    } catch (error) {
        console.debug(error)
    }finally {
        isLoading.value.form = false
    }
});

watch(page, () => getItems());


//METHODS
async function clearFilters() {
  try {
    isLoading.busy =true
    filter.value = {
      filialId: null,
      clienteId: null,
      status: null,
      dataIni: null,
      dataFim: null,
      usuarioId:null
    };
    if (!isMatriz.value) {
      filter.value.filialId = authStore.user.filial;
      filter.value.usuarioId = authStore.user.id;
      await getClientesToList(filter.value.filialId, filter.value.usuarioId);
    } else {
      await getClientesToList(filter.value.filialId);
    }
    await getItems();
  } catch (error) {
    console.error(error)

  }finally {
    isLoading.busy =false
  }
}

function editar(id)
{
    router.push({ name: "reembolsoFaturamentoCadastro", query: { id: id } })
}

async function getFiliaisToList() {
  try {
    let response = await filialService.toList();
    filiais.value = response.data;
  } catch (error) {
    console.log("getFiliaisToList.error:", error.response);
    handleErrors(error);
  }
}

async function getClientesToList(filialId = 0, usuarioId = 0) {
  try {
    clientes.value = await useClienteStore().toComboList(filialId, usuarioId);
  } catch (error) {
    console.log("getClientesToList.error:", error.response);
    handleErrors(error);
  }
}

async function getItems()
{
    try
    {
        isLoading.value.busy = true;
        if ((filter.value.dataIni && !filter.value.dataFim ) || (filter.value.dataFim && !filter.value.dataIni))
        throw new TypeError("Ambas as datas devem ser informadas!")
        else if (moment(filter.value.dataFim) < moment(filter.value.dataIni)) 
        throw new TypeError("A data fim deve ser maior que a data início!")
        
        let response = await faturamentoStore.getPaginate(page.value, pageSize, filter.value)
        faturamentos.value = response.items;
        totalPages.value = response.totalPages;
    } catch (error)
    {
        console.log("faturamento.getItems.error:", error);
        handleErrors(error)
    } finally
    {
        isLoading.value.busy = false;
    }
}

async function showHistorico(item)
{
    faturamentoEventos.value = item.faturamentoHistorico
    openHistorico.value = true
}

</script>
  
  