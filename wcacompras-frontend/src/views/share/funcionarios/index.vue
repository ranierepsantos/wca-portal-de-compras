<template>
    <div>
        <bread-crumbs title="Funcionários" @novoClick="editar('novo')" />
        <v-row>
            <v-col cols="6">
                <v-text-field label="Pesquisar" placeholder="(Nome ou Código Funcionário)" v-model="filter.termo" density="compact"
                    variant="outlined" color="info">
                </v-text-field>
            </v-col>
        </v-row>
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <v-table class="elevation-2">
            <thead>
                <tr>
                    <th class="text-left text-grey">COD. FUNCIONÁRIO</th>
                    <th class="text-left text-grey">NOME</th>
                    <th class="text-left text-grey">CLIENTE</th>
                    <th class="text-left text-grey">CENTRO DE CUSTO</th>
                    <th class="text-left text-grey">DDD</th>
                    <th class="text-left text-grey">CELULAR</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in funcionarios" :key="item.id">
                    <td class="text-left">{{ item.codigoFuncionario }}</td>
                    <td class="text-left">{{ item.nome }}</td>
                    <td class="text-left">{{ item.clienteNome }}</td>
                    <td class="text-left">{{ item.centroCustoNome }}</td>
                    <td class="text-left">{{ item.dddCelular }}</td>
                    <td class="text-left">{{ item.numeroCelular }}</td>
                    <td class="text-right">
                        <v-btn icon="mdi-lead-pencil" variant="plain" color="primary" @click="editar(item.id)"></v-btn>
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="5">
                        <v-pagination v-model="page" :length="totalPages" :total-visible="4"></v-pagination>
                    </td>
                </tr>
            </tfoot>
        </v-table>
    </div>
</template>
  
<script setup>
import { ref, onMounted, watch, inject } from "vue";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import { useAuthStore } from "@/store/auth.store";
import { useShareFuncionarioStore } from "@/store/share/funcionario.store";
import { useShareClienteStore } from "@/store/share/cliente.store";
//DATA
const page = ref(1);
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const isBusy = ref(false);
const totalPages = ref(1);
const funcionarioStore = useShareFuncionarioStore();
const funcionarios = ref([]);
const filter = ref({
    termo: "",
    clienteIds: [0],
    centroCustoIds: null
});
const authStore = useAuthStore();

//VUE METHODS
onMounted(async () =>
{
    authStore.user.filial = authStore.sistema.filial.value

    let list = await useShareClienteStore().toComboList(0, useAuthStore().user.id);
    if (list.length > 0)
    {
        filter.value.clienteIds = list.map(x => x.value);
    }
    await getItems();
});

watch(page, () => getItems());
watch(() => filter.value.termo, () => getItems());

//METHODS
function editar(id)
{
    router.push({ name: "shareFuncionarioCadastro", query: { id: id } })
}

async function getItems()
{
    try
    {
        isBusy.value = true;
        let response = await funcionarioStore.getPaginate(page.value, pageSize, filter.value)
        
        funcionarios.value = response.items;
        totalPages.value = response.totalPages;
    } catch (error)
    {
        console.log("getItems.error:", error.response);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}

</script>
  
  