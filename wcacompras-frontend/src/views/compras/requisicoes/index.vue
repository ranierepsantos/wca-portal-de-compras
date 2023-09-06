<template>
    <div>
        <bread-crumbs :title="(hasPermissionAprovador ? 'Requisições' : 'Minhas Requisições')"
            @novoClick="router.push({ name: 'requisicaoCadastro' })" :show-button="authStore.hasPermissao('requisicao')"
            :buttons="headerButtons"
            @gerar-relatorio-click="gerarRelatorio()"
            />
        <v-row>
            <v-col>
                <v-select label="Filiais" v-model="filter.filial" :items="filiais" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"
                    :hide-details="true"></v-select>
            </v-col>
            <v-col >
                <v-select label="Clientes" v-model="filter.clienteId" :items="clientes" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"
                    :hide-details="true"></v-select>
            </v-col>
            <v-col >
                <v-select label="Fornecedor" v-model="filter.fornecedorId" :items="fornecedores" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"
                    :hide-details="true"></v-select>
            </v-col>
            <v-col v-show="hasPermissionAprovador">
                <v-select label="Usuário" v-model="filter.usuarioId" :items="usuarios" density="compact"
                    item-title="text" item-value="value" variant="outlined" color="primary"
                    :hide-details="true"></v-select>
            </v-col>
            
        </v-row>
        <v-row>
            <v-col cols="2">
                <v-select label="Status" v-model="filter.status" :items="status" density="compact" item-title="text"
                    item-value="value" variant="outlined" color="primary" :hide-details="true"></v-select>
            </v-col>
            <v-col>
                <v-text-field label="Data Início" v-model="filter.dataInicio" type="date"
                variant="outlined" color="primary"
                density="compact"></v-text-field>
            </v-col>
            <v-col>
                <v-text-field label="Data Fim" v-model="filter.dataFim" type="date"
                variant="outlined" color="primary"
                density="compact"></v-text-field>
            </v-col>
            <v-col class="text-right">
                <v-btn color="primary" variant="outlined" class="text-capitalize" @click="getItems()">
                    <!-- <v-icon :icon="customButtonIcon" v-if="customButtonIcon != ''"></v-icon> -->
                    <b>Aplicar Filtros</b>
                </v-btn>
                &nbsp;
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
                    <th class="text-left text-grey" v-show="hasPermissionAprovador">USUÁRIO</th>
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
                    <th class="text-left text-grey" v-show="hasPermissionAprovador">{{ item.usuario.text }}
                    </th>
                    <td class="text-center">{{ new Date(item.dataCriacao).toLocaleDateString() }}</td>
                    <td class="text-left">{{ item.cliente.nome }}</td>
                    <td class="text-left">{{ item.fornecedor.text }}</td>
                    <td class="text-right">{{ parseFloat(item.valorTotal.toFixed(2)).toLocaleString("pt-BR", {style: "currency", currency: "BRL"}) }}</td>
                    <td class="text-center"><v-btn :color="getStatus(item.status).color" variant="tonal"
                            density="compact" class="text-center"> {{
                                getStatus(item.status).text
                            }}</v-btn></td>
                    <td class="text-right">
                        <v-btn icon="mdi-content-copy" size="smaller" variant="plain" color="info" title="Duplicar"
                            @click="duplicar(item)" v-show="authStore.hasPermissao('requisicao')"
                            :disabled="getStatus(item.status).text == 'Cancelado' || isBusy"></v-btn>
                        <v-btn icon="mdi-lead-pencil" size="smaller" variant="plain" color="primary"
                            @click="editar(item.id)" title="Editar" :disabled="isBusy"></v-btn>
                        <v-btn icon="mdi-close-circle-outline" size="smaller" variant="plain" color="error"
                            @click="remove(item)" title="Cancelar" v-show="authStore.hasPermissao('requisicao')"
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
import { ref, onMounted, watch, inject, computed } from "vue";
import requisicaoService from "@/services/requisicao.service";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import { useAuthStore } from "@/store/auth.store";
import clienteService from "@/services/cliente.service";
import userService from "@/services/user.service";
import fornecedorService from "@/services/fornecedor.service";
import { realizarDownload, status } from "@/helpers/functions"
import moment from "moment";
import filialService from "@/services/filial.service";

//DATA
const authStore = useAuthStore();
const page = ref(1);
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const isBusy = ref(false);
const totalPages = ref(1);
const requisicoes = ref([]);
const clientes = ref([]);
const filiais = ref ([]);
const fornecedores = ref([]);
const usuarios = ref([]);
const hasPermissionAprovador = ref(false)
const swal = inject("$swal");
const filter = ref({
    filial: [],
    clienteId: null,
    fornecedorId: null,
    usuarioId: null,
    status: -1,
    dataInicio: null,
    dataFim: null
});

//VUE METHODS
const headerButtons = computed(() => {
    let buttons = [];
    buttons.push({ text: 'Gerar relatório', icon: 'mdi-microsoft-excel', event: 'GerarRelatorioClick' })
    return buttons;
})

onMounted(async () =>
{
    hasPermissionAprovador.value = authStore.hasPermissao('aprova_requisicao') || authStore.hasPermissao('aprova_requisicao_cliente')
    if (!hasPermissionAprovador.value)
    {
        filter.value.usuarioId = authStore.user.id;
    }
    await getFiliaisByUser();
    await getClienteToList();
    await getFornecedorToList()
    await getUsuarioToList();
    await getItems();
});

watch(page, () => getItems());
watch(()=>filter.value.filial, async () => {
    let _filiais = []
    if (filter.value.filial!=null)_filiais.push(filter.value.filial)
    filter.value.clienteId = null
    filter.value.fornecedorId = null
    filter.value.usuarioId = hasPermissionAprovador.value ? null : authStore.user.id
    await getClienteToList(_filiais)
    await getFornecedorToList(_filiais)
    await getUsuarioToList(_filiais)

})

//watch(filter.value, () => getItems());

//METHODS
function clearFilters()
{
    filter.value.filial = null
    filter.value.clienteId = null
    filter.value.fornecedorId = null
    filter.value.usuarioId = hasPermissionAprovador.value ? null : authStore.user.id
    filter.value.status = -1
    filter.value.dataInicio = null
    filter.value.dataFim = null
    getItems();
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

async function gerarRelatorio()
{
    try
    {
        isBusy.value = true;
        let response = await requisicaoService.gerarRelatorio(filter.value);
        console.log("gerarRelatorio", response)
        if (response.status == 200) {
            let nomeArquivo = `requisicao_relatorio_${moment().format("DDMMYYYY_HHmmSS")}.xlsx`
            await new Promise(r => setTimeout(r, 1000));
            realizarDownload(response, nomeArquivo, response.headers.getContentType());
        }
        
    } catch (error)
    {
        console.log("requisicoes.gerarRelatorio.error:", error.response);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}

async function getClienteToList(filial = [])
{
    try
    {
        //se tiver permissao para visualizar requisição de todos os usuários, lista todos os clientes
        if (authStore.hasPermissao('requisicao_all_users') || filial.length > 0){
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

async function getFiliaisByUser() {
    try
    {
        isBusy.value = true;
        let response = await filialService.getListByAuthenticatedUser()
        filiais.value = response.data;
        
    } catch (error)
    {
        console.log("clientes.getFiliaisByUser.error:", error.response);
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

        let response =  null;
        
        if (authStore.hasPermissao("requisicao_all_users")){
            let filtro = {...filter.value }

            if (filtro.filial.length == 0)
                filtro.filial = filiais.value.map(p => {return p.value })
        
                response = await requisicaoService.paginate(pageSize, page.value, filtro);
        }else
            response = await requisicaoService.paginateByUserContext(pageSize, page.value, filter.value);
        
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

async function getUsuarioToList(filial = [])
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

async function getFornecedorToList(filial =[])
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
