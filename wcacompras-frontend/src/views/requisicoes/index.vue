<template>
    <div>
        <bread-crumbs :title="(hasRequisicaoAllUsersPermission ? 'Requisições' : 'Minhas Requisições')"
            @novoClick="router.push({ name: 'requisicaoCadastro' })" custom-button-text="Exportar Dados"
            @customClick="editar('novo')" :custom-button-show="false" />
        <v-row>
            <v-col cols="3">
                <v-select label="Clientes" v-model="filter.clienteId" :items="clientes" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"
                    :hide-details="true"></v-select>
            </v-col>
            <v-col cols="4">
                <v-select label="Fornecedor" v-model="filter.fornecedorId" :items="fornecedores" density="compact"
                    item-title="nome" item-value="id" variant="outlined" color="primary"
                    :hide-details="true"></v-select>
            </v-col>
            <v-col cols="3" v-show="hasRequisicaoAllUsersPermission">
                <v-select label="Usuário" v-model="filter.usuarioId" :items="usuarios" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"
                    :hide-details="true"></v-select>
            </v-col>
            <v-col cols="2">
                <v-select label="Status" v-model="filter.status" :items="status" density="compact" item-title="text"
                    item-value="value" variant="outlined" color="primary" :hide-details="true"></v-select>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="12" class="text-right">
                <v-btn color="info" variant="outlined" class="text-capitalize" @click="clearFilters()">
                    <!-- <v-icon :icon="customButtonIcon" v-if="customButtonIcon != ''"></v-icon> -->
                    <b>Limpar Filtros</b>
                </v-btn>
            </v-col>

        </v-row>
        <br>
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <v-table class="elevation-2">
            <thead>
                <tr>
                    <th class="text-center text-grey">PEDIDO</th>
                    <th class="text-left text-grey" v-show="hasRequisicaoAllUsersPermission">USUÁRIO</th>
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
                    <th class="text-left text-grey" v-show="hasRequisicaoAllUsersPermission">{{ item.usuario.text }}
                    </th>
                    <td class="text-center">{{ new Date(item.dataCriacao).toLocaleDateString() }}</td>
                    <td class="text-left">{{ item.cliente.nome }}</td>
                    <td class="text-left">{{ item.fornecedor.text }}</td>
                    <td class="text-right">{{ item.valorTotal.toFixed(2) }}</td>
                    <td class="text-center"><v-btn :color="getStatus(item.status).color" variant="tonal"
                            density="compact" class="text-center"> {{
                                getStatus(item.status).text
                            }}</v-btn></td>
                    <td class="text-right">
                        <v-btn icon="mdi-content-copy" size="smaller" variant="plain" color="info" title="Duplicar"
                            @click="duplicar(item)"
                            :disabled="getStatus(item.status).text == 'Cancelado' || isBusy"></v-btn>
                        <v-btn icon="mdi-lead-pencil" size="smaller" variant="plain" color="primary"
                            @click="editar(item.id)" title="Editar" :disabled="isBusy"></v-btn>
                        <v-btn icon="mdi-close-circle-outline" size="smaller" variant="plain" color="error"
                            @click="remove(item)" title="Cancelar"
                            :disabled="getStatus(item.status).text.toLowerCase() == 'cancelado' || getStatus(item.status).text.toLowerCase() == 'finalizado' || isBusy"></v-btn>
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
import { status } from "@/helpers/functions"

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

//VUE METHODS
onMounted(async () =>
{
    hasRequisicaoAllUsersPermission.value = authStore.hasPermissao('requisicao_all_users')
    let filial = 0
    if (!hasRequisicaoAllUsersPermission.value)
    {
        filter.value.usuarioId = authStore.user.id;
        filial = authStore.user.filial;
        await getClienteListByUser();
    }
    if (hasRequisicaoAllUsersPermission.value)
    {
        await getClienteToList(filial)
    }
    await getFornecedorToList(filial)
    await getUsuarioToList();
    await getItems();
});

watch(page, () => getItems());
watch(filter.value, () => getItems());

//METHODS
function clearFilters()
{
    filter.value.clienteId = null
    filter.value.fornecedorId = null
    filter.value.usuarioId = hasRequisicaoAllUsersPermission.value ? null : authStore.user.id
    filter.value.status = -1
}
async function duplicar(item)
{
    try
    {
        let options = {
            title: "Confirmar Duplicação",
            text: `Deseja realmente duplicar a requisição: #${item.id}?`,
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
        }

        let response = await swal.fire(options);
        if (response.isConfirmed)
        {
            isBusy.value = true;
            let response = await requisicaoService.duplicate(item.id, authStore.user.id);
            console.log('duplicar.response', response);
            await getItems()
            if (response.data.message == "")
            {
                swal.fire({
                    toast: true,
                    icon: "success",
                    position: "top-end",
                    title: "Sucesso!",
                    html: `Requisição duplicada com sucesso!`,
                    showConfirmButton: false,
                    timer: 2000,
                })
            } else
            {
                swal.fire({
                    icon: "warning",
                    title: "Atenção!",
                    html: `Requisição duplicada, mas alguns produtos não foram encontrados!<br/><br/> ${response.data.message}`,
                    showConfirmButton: true
                })
            }

        }
    } catch (error)
    {
        console.log("requisicoes.remove.error:", error);
        handleErrors(error)
    } finally { isBusy.value = false; }
}
function editar(id)
{
    router.push({ name: "requisicaoEdicao", params: { requisicao: id } })
}

async function getClienteListByUser() 
{
    try
    {
        let response = await clienteService.getListByAuthenticatedUser();
        let list = response.data;
        list.forEach(elem => {
            clientes.value.push({text: elem.nome, value: elem.id })
        });
        
    } catch (error)
    {
        console.log("getClienteListByUser.error:", error);
        handleErrors(error)
    }
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

async function remove(item)
{
    try
    {
        let options = {
            title: "Confirmar Cancelamento",
            text: `Deseja realmente cancelar a requisição: #${item.id}?`,
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
        }

        let response = await swal.fire(options);
        if (response.isConfirmed)
        {
            isBusy.value = true;
            await requisicaoService.remove(item.id);
            await getItems()

            swal.fire({
                toast: true,
                icon: "success",
                position: "top-end",
                title: "Sucesso!",
                text: "Cancelamento realizado!",
                showConfirmButton: false,
                timer: 2000,
            })
        }
    } catch (error)
    {
        console.log("requisicoes.remove.error:", error);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}

</script>
