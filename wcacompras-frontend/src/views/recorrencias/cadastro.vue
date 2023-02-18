<template>
    <div>
        <bread-crumbs :title="pageTitle" :show-button="false" :custom-button-show="true"
            custom-button-text="Salvar" @customClick="salvar()" />
            <v-form ref="formCadastro">
                <v-row>
                    <v-col cols="5">
                        <v-select label="Clientes" v-model="recorrencia.clienteId" :items="clientes" density="compact"
                            item-title="nome" item-value="id" variant="outlined" color="primary"
                             :rules="[(v) => !!v || 'Campo obrigatório']"></v-select>
                    </v-col>
                    <v-col cols="5">
                        <v-select label="Fornecedor" v-model="recorrencia.fornecedorId" :items="fornecedores" density="compact"
                            item-title="text" item-value="value" variant="outlined" color="primary" 
                            :rules="[(v) => !!v || 'Campo obrigatório']"></v-select>
                    </v-col>
                    <v-col cols="2">
                        <v-select label="Destino:" v-model="recorrencia.destino" :items="destinos" density="compact"
                            item-title="text" item-value="value" variant="outlined" color="primary"
                            ></v-select>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="3">
                        <v-select label="Tipo" v-model="recorrencia.tipoRecorrencia" :items="tipoRecorrencia" density="compact"
                            item-title="text" item-value="value" variant="outlined" color="primary"
                             @update:modelValue="recorrencia.dia = null"></v-select>
                    </v-col>
                    <v-col cols="2">
                        <v-select label="Dia" v-model="recorrencia.dia" :items="recorrencia.tipoRecorrencia == 0? diasDaSemana: diasDoMes" density="compact"
                            item-title="text" item-value="value" variant="outlined" color="primary"
                             :rules="[(v) => v != null || 'Campo obrigatório']"></v-select>
                    </v-col>
                    <v-col :cols="recorrencia.id > 0? '5':'7'">
                        <v-text-field label="Nome" v-model="recorrencia.nome" placeholder="Informe o nome para recorrência" density="compact"
                            variant="outlined" color="info" :rules="[(v) => !!v || 'Campo obrigatório']">
                        </v-text-field>
                    </v-col>
                    <v-col cols="2" v-show="recorrencia.id > 0">
                        <v-checkbox v-model="recorrencia.ativo" label="Ativo" color="primary"></v-checkbox>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="5">
                        <v-text-field label="Pesquisar Produto" v-model="filter" placeholder="Nome ou Código" density="compact"
                            variant="outlined" color="info">
                        </v-text-field>
                    </v-col>
                </v-row>
            </v-form>
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <small class="text-error" v-show="!hasProduto">Adicione pelo menos 1 produto</small>
        <v-table class="elevation-2">
            <thead>
                <tr>
                    <th class="text-center text-grey">CÓDIGO</th>
                    <th class="text-center text-grey">PRODUTO</th>
                    <th class="text-center text-grey">QUANTIDADE</th>
                    <th class="text-center text-grey">U. M.</th>
                    <th class="text-center text-grey">REMOVER</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in recorrencia.recorrenciaProdutos" :key="item.id">
                    <td class="text-left">{{ item.codigo }}</td>
                    <td class="text-left">{{ item.nome }}</td>
                    <td class="text-left">
                        <v-text-field density="compact" variant="outlined" type="number" color="primary"
                             class="ml-12" v-model="item.quantidade" min="0"
                            @change="adicionarRemoverProduto(item)" :hide-details="true">
                        </v-text-field>
                    </td>
                    <td class="text-center">{{ item.unidadeMedida }}</td>
                    <td class="text-center">
                        <v-btn icon="mdi-package-variant-minus" variant="plain" color="red" title="Remover Produto"
                            @click="removeProdutoRecorrencia(item)"></v-btn>
                    </td>
                </tr>

                <tr v-for="item in produtos" :key="item.id" class="text-grey">
                    <td class="text-left">{{ item.codigo }}</td>
                    <td class="text-left">{{ item.nome }}</td>
                    <td class="text-left">
                        <v-text-field density="compact" variant="outlined" type="number" color="primary"
                             class="sm ml-12" v-model="item.quantidade" min="0"
                            @change="adicionarRemoverProduto(item)" :hide-details="true">
                        </v-text-field>
                    </td>
                    <td class="text-center">{{ item.unidadeMedida }}</td>
                    <td class="text-center">
                        <!-- <v-btn icon="mdi-package-variant-plus" variant="plain" color="success"
                            :disabled="(isNaN(item.quantidade) ? 0 : item.quantidade) == 0"
                            @click="adicionarProdutorecorrencia(item)" title="Incluir Produto"></v-btn> -->
                    </td>
                </tr>
            </tbody>
        </v-table>
    </div>
</template>
  
<script setup>
import { ref, onMounted, watch, inject } from "vue";
import recorrenciaService from "@/services/recorrencia.service";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import { useAuthStore } from "@/store/auth.store";
import { useRoute } from "vue-router";
import fornecedorService from "@/services/fornecedor.service";
import { compararValor, tipoRecorrencia, diasDaSemana, diasDoMes } from "@/helpers/functions"
import router from "@/router";
import clienteService from "@/services/cliente.service";

//DATA
const authStore = useAuthStore();
const route = useRoute();
const isBusy = ref(false);
const formCadastro = ref(null);
const recorrencia = ref({
    id: 0,
    nome: "",
    ativo: true,
    filialId: authStore.user.filial,
    clienteId: null,
    fornecedorId: null,
    usuarioId: authStore.user.id,
    tipoRecorrencia: 0,
    destino: 0,
    dia: null,
    recorrenciaProdutos: [],
    recorrenciaLogs:[]
});
const clientes = ref([]);
const fornecedores = ref([]);
const destinos = ref([
    { value: 0, text: "Outros" },
    { value: 1, text: "Diretoria" },
])
const produtos = ref([]);
const swal = inject("$swal");
const filter = ref("");
const hasProduto = ref(true);
const pageTitle = ref("Recorrência")

//VUE METHODS
onMounted(async () =>
{
    let recorrenciaId = route.params.codigo;
    console.log('Recorrência: ',recorrenciaId)
    recorrencia.value.UsuarioId = authStore.user.id;
    await getClienteListByUser()
    await getFornecedorToList(authStore.user.filial)
    if (recorrenciaId !=undefined && recorrenciaId > 0) {
        pageTitle.value += " - Edição"
        await getRecorrencia(recorrenciaId)
    }else {
        pageTitle.value += " - Cadastro"
    }
    
});

watch(() => recorrencia.value.fornecedorId, async (fornecedorId, oldFornecedor) =>
{
    if (oldFornecedor !=null  && oldFornecedor !=fornecedorId)
        recorrencia.value.recorrenciaProdutos = []

    await getProdutosToList(fornecedorId)

})

watch(filter, async () =>
{
    if (recorrencia.value.fornecedorId != null && recorrencia.value.fornecedorId > 0)
    {
        await getProdutosToList(recorrencia.value.fornecedorId)
    }
})

//METHODS

function adicionarProdutoRecorrencia(item)
{
    let index = recorrencia.value.recorrenciaProdutos.findIndex(p => p.codigo == item.codigo)
    if (index == -1)
    {
        let produto = { ...item }
        delete produto.id
        recorrencia.value.recorrenciaProdutos.push(produto)
        
        if (!hasProduto.value) 
            hasProduto.value = true;
        
        produtoRemoveFromList(item)
        ordenarRecorrenciaProdutos()
    }
}

function adicionarRemoverProduto(item)
{
    if (item.quantidade > 0)
    {
        adicionarProdutoRecorrencia(item)
    } else
    {
        removeProdutoRecorrencia(item)
    }

}

async function getClienteListByUser() 
{
    try
    {
        let response = await clienteService.getListByAuthenticatedUser();
        clientes.value = response.data;
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

async function getProdutosToList(fornecedorId)
{
    try
    {
        isBusy.value = true;
        let response = await fornecedorService.produtoPaginate(fornecedorId, 99999, 1, filter.value);
        produtos.value = response.data.items;

        for (let index = 0; index < recorrencia.value.recorrenciaProdutos.length; index++)
        {
            let item = recorrencia.value.recorrenciaProdutos[index];
            produtoRemoveFromList(item)
        }
    } catch (error)
    {
        console.log("getUsuarioToList.error:", error);
        handleErrors(error)
    } finally
    {
        isBusy.value = false
    }
}

async function getRecorrencia(id)
{
    try
    {
        isBusy.value = true;
        let response = await recorrenciaService.getById(id);
        if (response.data)
            recorrencia.value = response.data;
    } catch (error)
    {
        console.log("getUsuarioToList.error:", error);
        handleErrors(error)
    } finally
    {
        isBusy.value = false
    }
}

function ordenarRecorrenciaProdutos() 
{
    if (recorrencia.value.recorrenciaProdutos.length > 0)
        recorrencia.value.recorrenciaProdutos.sort(compararValor("nome"))
}

function produtoRemoveFromList(item)
{
    let index = produtos.value.findIndex(p => p.codigo == item.codigo)
    if (index != -1)
        produtos.value.splice(index, 1)
}

async function removeProdutoRecorrencia(item)
{
    let index = recorrencia.value.recorrenciaProdutos.findIndex(p => p.codigo == item.codigo)
    if (index != -1)
    {
        recorrencia.value.recorrenciaProdutos.splice(index, 1)
        await getProdutosToList(recorrencia.value.fornecedorId)
        ordenarRecorrenciaProdutos()
    }
    calcularOrcamentoTotais()
}

async function salvar()
{
    try
    {
        isBusy.value = true;
        let { valid } = await formCadastro.value.validate();
        hasProduto.value = recorrencia.value.recorrenciaProdutos.length > 0
        
        if (valid && hasProduto.value) {
            
            if (recorrencia.value.id > 0)
                await recorrenciaService.update(recorrencia.value)
            else
                await recorrenciaService.create(recorrencia.value)

            swal.fire({
                toast: true,
                icon: "success",
                index: "top-end",
                title: "Sucesso!",
                text: "Dados salvos com sucesso!",
                showConfirmButton: false,
                timer: 2000,
            })
            router.push({ name: "recorrencias" })
        }
    } catch (error)
    {
        console.log("salvar.error:", error);
        handleErrors(error)
    } finally
    {
        isBusy.value = false
    }
}

</script>

<style scoped>
table .v-text-field {
    width: 80px;
}
</style>