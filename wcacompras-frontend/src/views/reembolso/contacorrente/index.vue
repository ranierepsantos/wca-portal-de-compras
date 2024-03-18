<template>
    <div>
        <bread-crumbs title="Contas" :showButton="false"/>
        <v-row>
            <v-col cols="6">
                <v-text-field label="Pesquisar" placeholder="(Nome)" v-model="filter" density="compact"
                    variant="outlined" color="info">
                </v-text-field>
            </v-col>
        </v-row>
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <v-table class="elevation-2">
            <thead>
                <tr>
                    <th class="text-left text-grey">NOME</th>
                    <th class="text-right text-grey">SALDO</th>
                    <!-- <th></th> -->
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in contas" :key="item.usuarioId">
                    <td class="text-left">
                        {{ item.usuarioNome }}
                    </td>
                    <td :class="'text-right ' + (item.saldo >= 0 ?'text-success':'text-error')">{{ formatToCurrencyBRL(item.saldo) }}</td>
                    <!-- <td class="text-right">
                        <v-btn
                        prepend-icon="mdi-text-box-search-outline"
                        variant="plain"
                        color="primary"
                        size="small"
                        :disabled="isBusy"
                      >Transações</v-btn>
                    </td> -->
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="2">
                        <v-pagination v-model="page" :length="totalPages" :total-visible="4"></v-pagination>
                    </td>
                </tr>
            </tfoot>
        </v-table>
    </div>
</template>
  
<script setup>
import { ref, onMounted, watch } from "vue";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import { useAuthStore } from "@/store/auth.store";
import { useClienteStore } from "@/store/reembolso/cliente.store";
import { useUsuarioStore } from "@/store/reembolso/usuario.store";
import { useContaStore } from "@/store/reembolso/conta.store";
import { formatToCurrencyBRL } from "@/helpers/functions";
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

//VUE METHODS
onMounted(async () =>
{
    let filialUsuario =(await useUsuarioStore().getFiliais(authStore.user.id))[0]
    authStore.user.filial = filialUsuario.value

    meusClientes.value = await clienteStore.ListByUsuario(authStore.user.id)
    meusClientes.value = meusClientes.value.map(x =>{ return x.id})

    meusCentroDeCusto.value = await useUsuarioStore().getCentrosdeCusto(authStore.user.id)
    meusCentroDeCusto.value = meusCentroDeCusto.value.map(x =>{ return x.id})

    await getItems();
});

watch(page, () => getItems());
watch(filter, () => getItems());

//METHODS
async function getItems()
{
    try
    {
        isBusy.value = true;

        let filtros = {
            filialId: authStore.user.filial.id,
            usuarioNome: filter.value,
            centroCustoIds: meusCentroDeCusto.value,
            clientesIds: meusClientes.value
        }
        let response = await useContaStore().getPaginate(page.value, pageSize, filtros)
        contas.value = response.items;
        totalPages.value = response.totalPages;
    } catch (error)
    {
        console.log("clientes.getItems.error:", error.response);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}
</script>
  
  