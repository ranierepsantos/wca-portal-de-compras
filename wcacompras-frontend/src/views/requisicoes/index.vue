<template>
    <div>
        <bread-crumbs :title="(hasRequisicaoAllUsersPermission ? 'Requisições' : 'Minhas Requisições')"
            @novoClick="editar('novo')" custom-button-text="Exportar Dados" @customClick="editar('novo')"
            :custom-button-show="true" />
        <v-row>
            <v-col cols="3">
                <v-select label="Clientes" v-model="filter.clienteId" :items="clientes" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"></v-select>
            </v-col>
            <v-col cols="3">
                <v-select label="Fornecedor" v-model="filter.fornecedorId" :items="fornecedores" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"></v-select>
            </v-col>
            <v-col cols="3" v-show="hasRequisicaoAllUsersPermission">
                <v-select label="Usuário" v-model="filter.usuarioId" :items="usuarios" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"></v-select>
            </v-col>
            <v-col cols="3">
                <v-select label="Status" v-model="filter.status" :items="status" density="compact" item-title="text"
                    item-value="value" variant="outlined" color="primary"></v-select>
            </v-col>
        </v-row>
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <v-table class="elevation-2">
            <thead>
                <tr>
                    <th class="text-center text-grey">PEDIDO</th>
                    <th class="text-center text-grey">DATA</th>
                    <th class="text-left text-grey">CLIENTE</th>
                    <th class="text-left text-grey">FORNECEDOR</th>
                    <th class="text-center text-grey">VALOR</th>
                    <th class="text-center text-grey">STATUS</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in requisicoes" :key="item.id">
                    <td class="text-center"> # {{ item.id }}</td>
                    <td class="text-center">{{ new Date(item.dataCriacao).toLocaleDateString() }}</td>
                    <td class="text-left">{{ item.cliente.text }}</td>
                    <td class="text-left">{{ item.fornecedor.text }}</td>
                    <td class="text-right">{{ item.valorTotal.toFixed(2) }}</td>
                    <td class="text-center"><v-btn :color="getStatus(item.status).color" variant="tonal"
                            density="compact" class="text-center"> {{
                                    getStatus(item.status).text
                            }}</v-btn></td>
                    <td class="text-right">
                        <v-btn icon="mdi-dots-vertical" variant="plain" color="primary" disabled title="Editar"></v-btn>
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="7">
                        <v-pagination v-model="page" :length="totalPages" :total-visible="4"></v-pagination>
                    </td>
                </tr>
            </tfoot>
        </v-table>
    </div>
</template>
  
<script setup>
import { ref, onMounted, watch, inject } from "vue";
import requisicaoService from "@/services/requisicao.service";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import { useAuthStore } from "@/store/auth.store";
import clienteService from "@/services/cliente.service";
import userService from "@/services/user.service";
import fornecedorService from "@/services/fornecedor.service";

//DATA
const authStore = useAuthStore();
const page = ref(1);
const pageSize = 10;
const isBusy = ref(false);
const totalPages = ref(1);
const requisicoes = ref([]);
const clientes = ref([]);
const fornecedores = ref([]);
const usuarios = ref([]);
const hasRequisicaoAllUsersPermission = ref(false)
const swal = inject("$swal");
const filter = ref({
    clienteId: null,
    fornecedorId: null,
    usuarioId: null,
    status: -1
});

let status = [
    { value: -1, text: "Todos" },
    { value: 0, text: "Aguardando", color: "warning" },
    { value: 1, text: "Aprovado", color: "success" },
    { value: 2, text: "Rejeitado", color: "error" }
]

//VUE METHODS
onMounted(async () =>
{
    hasRequisicaoAllUsersPermission.value = authStore.hasPermissao('requisicao_all_users')
    if (!hasRequisicaoAllUsersPermission.value)
    {
        filter.value.usuarioId = authStore.user.id;
    }
    await getClienteToList(authStore.user.filial)
    await getFornecedorToList(authStore.user.filial)
    await getUsuarioToList();
    await getItems();
});

watch(page, () => getItems());
watch(filter.value, () => getItems());

//METHODS
async function enableDisable(item)
{
    try
    {
        let text = item.ativo ? "Desativar" : "Ativar"

        let options = {
            title: text,
            text: `Deseja realmente ${text.toLowerCase()} o fornecedor: ${item.nome}?`,
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
        }

        let response = await swal.fire(options);
        if (response.isConfirmed)
        {
            let data = { ...item }
            data.ativo = !data.ativo
            await fornecedorService.update(data);
            await this.getItems()

            swal.fire({
                toast: true,
                icon: "success",
                position: "top-end",
                title: "Sucesso!",
                text: "Alteração realizada!",
                showConfirmButton: false,
                timer: 2000,
            })
        }
    } catch (error)
    {
        console.log("fornecedores.enableDisable.error:", error);
        handleErrors(error)
    }
}


function editar(id)
{
    router.push({ name: "requisicaoCadastro", query: { id: id } })
}

async function getClienteToList(filial)
{
    try
    {
        let response = await clienteService.toList(filial);
        clientes.value = response.data;
    } catch (error)
    {
        console.log("getClienteToList.error:", error);
        handleErrors(error)
    }
}

async function getItems()
{
    try
    {
        isBusy.value = true;
        let response = await requisicaoService.paginate(pageSize, page.value, filter.value);
        requisicoes.value = response.data.items;
        totalPages.value = response.data.totalPages;
    } catch (error)
    {
        console.log("requisicoes.getItems.error:", error.response);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}

function getStatus(codigo)
{
    return status.filter(s => s.value == codigo)[0]
}

async function getUsuarioToList()
{
    try
    {
        let response = await userService.toList();
        usuarios.value = response.data;
    } catch (error)
    {
        console.log("getUsuarioToList.error:", error);
        handleErrors(error)
    }
}

async function getFornecedorToList(filial)
{
    try
    {
        let response = await fornecedorService.toList(filial);
        fornecedores.value = response.data;
    } catch (error)
    {
        console.log("getUsuarioToList.error:", error);
        handleErrors(error)
    }
}
</script>
