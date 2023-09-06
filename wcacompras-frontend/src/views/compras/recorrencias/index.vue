<template>
    <div>
        <bread-crumbs :title="(hasPermissionViewAllRecorrencia ? 'Recorrências' : 'Minhas Recorrências')"
            @novoClick="router.push({ name: 'recorrenciaCadastro' })" custom-button-text="Exportar Dados"
            @customClick="editar('novo')" :custom-button-show="false" />
        <v-row>
            <v-col>
                <v-select label="Filiais" v-model="filter.filial" :items="filiais" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"
                    :hide-details="true"></v-select>
            </v-col>
            <v-col>
                <v-select label="Clientes" v-model="filter.clienteId" :items="clientes" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"
                    :hide-details="true"></v-select>
            </v-col>
            <v-col>
                <v-select label="Fornecedor" v-model="filter.fornecedorId" :items="fornecedores" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"
                    :hide-details="true"></v-select>
            </v-col>
            <v-col v-show="hasPermissionViewAllRecorrencia">
                <v-select label="Usuário" v-model="filter.usuarioId" :items="usuarios" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"
                    :hide-details="true"></v-select>
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
                    <th class="text-center text-grey">CÓDIGO</th>
                    <th class="text-left text-grey" v-show="hasPermissionViewAllRecorrencia">USUÁRIO</th>
                    <th class="text-left text-grey">DESCRIÇÃO</th>
                    <th class="text-left text-grey">CLIENTE</th>
                    <th class="text-left text-grey">FORNECEDOR</th>
                    <th class="text-center text-grey">TIPO</th>
                    <th class="text-center text-grey">DIA</th>
                    <th class="text-center text-grey">ATIVO</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in recorrencias" :key="item.id">
                    <td class="text-center"> # {{ item.id }}</td>
                    <th class="text-left text-grey" v-show="hasPermissionViewAllRecorrencia">{{ item.usuario.text }}
                    </th>
                    <td class="text-left">{{ item.nome }}</td>
                    <td class="text-left">{{ item.cliente.text }}</td>
                    <td class="text-left">{{ item.fornecedor.text }}</td>
                    <td class="text-center">{{getTipoRecorrenciaText(item.tipoRecorrencia)}}</td>
                    <td class="text-center">{{item.tipoRecorrencia == 0? getDiaSemana(item.dia):item.dia}}</td>
                    <td class="text-center"><v-icon :icon="item.ativo ? 'mdi-check' : 'mdi-close'" variant="plain"
              :color="item.ativo ? 'success' : 'error'"></v-icon></td>
                    <td class="text-right">
                        <v-btn icon="mdi-lead-pencil" size="smaller" variant="plain" color="primary"
                            @click="editar(item.id)" title="Editar" :disabled="isBusy"></v-btn>

                            <v-btn icon="mdi-list-status" size="smaller" variant="plain" color="primary"
                            title="Histórico" :disabled="isBusy" @click="showHistoricoExecucao(item.id)"></v-btn>
    
                            <v-btn variant="plain" :color="item.ativo ? 'error' : 'success'" size="smaller"
                            :title="item.ativo ? 'Desativar' : 'Ativar'"
                            :icon="item.ativo ? 'mdi-close-circle-outline' : 'mdi-check-circle-outline'"
                            @click="enableDisable(item)" :disabled="isBusy">
                        </v-btn>
                    </td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="9">
                        <v-pagination v-model="page" :length="totalPages" :total-visible="4"></v-pagination>
                    </td>
                </tr>
            </tfoot>
        </v-table>

        <v-dialog v-model="showLogs" max-width="800" :absolute="false">
            <v-card>
                <v-card-title class="text-primary text-h5">Histórico de execução</v-card-title>
                <v-card-text>
                    <v-table class="elevation-1">
                        <thead>
                            <tr>
                                <th class="text-center text-grey">DATA/HORA</th>
                                <th class="text-center text-grey">STATUS</th>
                                <th class="text-left text-grey">LOG</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="item in logs" :key="item.id">
                                <td class="text-center"> {{moment(item.dataHora).format("DD/MM/YYYY HH:mm")}}</td>
                                <td :class="'text-center '+ (item.status.toLowerCase() == 'sucesso'?'text-success':'text-red')">{{ item.status.toUpperCase()}}</td>
                                <td class="text-left" v-html="item.log"></td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="9">
                                    <v-pagination v-model="logPage" :length="logTotalPages" :total-visible="4"></v-pagination>
                                </td>
                            </tr>
                        </tfoot>
                    </v-table>
                    <br>
                    <v-row>
                        <v-col class="text-right">
                            <v-btn variant="outlined" color="primary" @click="showLogs=false">Fechar</v-btn>
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions> </v-card-actions>
            </v-card>
        </v-dialog>
        
        
    </div>
</template>
  
<script setup>
import { ref, onMounted, watch, inject } from "vue";
import recorrenciaService from "@/services/recorrencia.service";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import { useAuthStore } from "@/store/auth.store";
import clienteService from "@/services/cliente.service";
import userService from "@/services/user.service";
import fornecedorService from "@/services/fornecedor.service";
import { tipoRecorrencia, diasDaSemana } from "@/helpers/functions";
import moment from "moment";
import filialService from "@/services/filial.service";

//DATA
const authStore = useAuthStore();
const page = ref(1);
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const isBusy = ref(false);
const totalPages = ref(1);
const logPage = ref(1);
const logPageSize = 10;
const logTotalPages = ref(1);
const recorrencias = ref([]);
const clientes = ref([]);
const filiais = ref ([]);
const logs = ref([]);
const showLogs = ref(false);
const fornecedores = ref([]);
const usuarios = ref([]);
const hasPermissionViewAllRecorrencia = ref(false)
const swal = inject("$swal");
const filter = ref({
    clienteId: null,
    fornecedorId: null,
    usuarioId: null
});
let idRecorrencia = 0;
//VUE METHODS
onMounted(async () =>
{
    hasPermissionViewAllRecorrencia.value = authStore.hasPermissao('recorrencias_view_others_users')
   
    await getFiliaisByUser()
    await getClienteToList()
    await getFornecedorToList()
    await getUsuarioToList();
    await getItems();
});

watch(page, () => getItems());
watch(logPage, () => getLogItems(idRecorrencia));
watch(()=>filter.value.filial, async () => {
    let _filiais = []
    if (filter.value.filial!=null)_filiais.push(filter.value.filial)
    filter.value.clienteId = null
    filter.value.fornecedorId = null
    
    await getClienteToList(_filiais)
    await getFornecedorToList(_filiais)
    await getUsuarioToList(_filiais)

})
watch(filter.value, () => getItems());

//METHODS
function clearFilters()
{
    filter.value.filial = null
    filter.value.clienteId = null
    filter.value.fornecedorId = null
    filter.value.usuarioId = hasPermissionViewAllRecorrencia.value ? null : authStore.user.id
    filter.value.status = -1
}
function editar(id)
{
    router.push({ name: "recorrenciaEdicao", params: { codigo: id } })
}

function getDiaSemana(dia) {
    return diasDaSemana.filter(t =>  t.value == dia)[0].text
}

async function getClienteToList(filial =[])
{
    try
    {
        if (filial.length > 0){
            let response = await clienteService.toList(filial);
            clientes.value = response.data;
        }else {
            let response = await clienteService.getListByAuthenticatedUser();
            let list = response.data;
            list.forEach(elem => {
                clientes.value.push({text: elem.nome, value: elem.id })
            });
        }
    } catch (error)
    {
        console.log("getClienteToList.error:", error);
        handleErrors(error)
    }
}

async function getFornecedorToList(filial = [])
{
    try
    {
        if (filial.length == 0)
            filial = filiais.value.map(p => {return p.value })

        let response = await fornecedorService.toList(filial);
        fornecedores.value = response.data;
    } catch (error)
    {
        console.log("getUsuarioToList.error:", error);
        handleErrors(error)
    }
}

async function getFiliaisByUser() {
    try
    {
        isBusy.value = true;
        let response = await filialService.getListByAuthenticatedUser()
        filiais.value = response.data;
        
    } catch (error)
    {
        console.log("recorrencias.getFiliaisByUser.error:", error.response);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}


async function getItems()
{
    try
    {
        isBusy.value = true;
        let response = await recorrenciaService.paginate(pageSize, page.value, filter.value);
        recorrencias.value = response.data.items;
        totalPages.value = response.data.totalPages;
    } catch (error)
    {
        console.log("recorrencias.getItems.error:", error.response);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}

async function showHistoricoExecucao(id) {
    logPage.value = 1
    await getLogItems(id);
    showLogs.value = true
}

async function getLogItems(recorrenciaId = idRecorrencia)
{
    try
    {
        idRecorrencia = recorrenciaId
        if (idRecorrencia > 0) {
            let response = await recorrenciaService.paginateLogs(idRecorrencia,logPageSize, logPage.value);
            logs.value = response.data.items;
            logTotalPages.value = response.data.totalPages;
        }
    } catch (error)
    {
        console.log("recorrencias.getLogItems.error:", error.response);
        handleErrors(error)
    } 
}
async function getUsuarioToList(filial =[])
{
    try
    {
        if (filial.length == 0)
            filial = filiais.value.map(p => {return p.value })

        let response = await userService.toList(filial);
        usuarios.value = response.data;
    } catch (error)
    {
        console.log("getUsuarioToList.error:", error);
        handleErrors(error)
    }
}

function getTipoRecorrenciaText(tipo) {
    return tipoRecorrencia.filter(t =>  t.value == tipo)[0].text
}

async function enableDisable(item)
{
    try
    {
        let text = item.ativo ? "Desativar" : "Ativar"

        let options = {
            title: text,
            text: `Deseja realmente ${text} a recorrência: #${item.id}?`,
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
        }

        let response = await swal.fire(options);
        if (response.isConfirmed)
        {
            isBusy.value = true;
            item.ativo = !item.ativo
            await recorrenciaService.enabledDisabled({id: item.id, ativo: item.ativo});
            await getItems()

            swal.fire({
                toast: true,
                icon: "success",
                position: "top-end",
                title: "Sucesso!",
                text: `Recorrência ${item.ativo ? "inativada" : "ativada"} com sucesso!`,
                showConfirmButton: false,
                timer: 2000,
            })
        }
    } catch (error)
    {
        console.log("recorrencias.remove.error:", error);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}

</script>
