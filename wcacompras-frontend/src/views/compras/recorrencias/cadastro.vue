<template>
    <div>
        <bread-crumbs :title="pageTitle" :show-button="false" :custom-button-show="true" custom-button-text="Salvar"
            @customClick="salvar()" />
        <v-form ref="formCadastro">
            <v-row>
                <v-col>
              <v-select label="Filiais" v-model="filterFilial" :items="filiais" density="compact"
                item-title="text" item-value="value" variant="outlined" color="primary"
                :hide-details="true"
                clearable>
              </v-select>
            </v-col>
                <v-col >
                    <v-select label="Clientes" v-model="recorrencia.clienteId" :items="clientes" density="compact"
                        item-title="nome" item-value="id" variant="outlined" color="primary"
                        :rules="[(v) => !!v || 'Campo obrigatório']"
                        :disabled="filterFilial == null"
                        ></v-select>
                </v-col>
                <v-col >
                    <v-select label="Fornecedor" v-model="recorrencia.fornecedorId" :items="fornecedores" density="compact"
                        item-title="text" item-value="value" variant="outlined" color="primary"
                        :rules="[(v) => !!v || 'Campo obrigatório']"
                        :disabled="recorrencia.clienteId == null"></v-select>
                </v-col>
                <v-col cols="2">
                    <v-select label="Destino:" v-model="recorrencia.destino" :items="destinos" density="compact"
                        item-title="text" item-value="value" variant="outlined" color="primary"></v-select>
                </v-col>
            </v-row>
            <v-row>
                <v-col :cols="authStore.hasPermissao('requisicao_local_entrega') ? 10 : 12">
                    <v-text-field label="Local Entrega" :model-value="localEntrega" placeholder="Local de entrega do Pedido"
                        density="compact" variant="outlined" color="primary" disabled>
                    </v-text-field>
                </v-col>

                <v-col v-show="authStore.hasPermissao('requisicao_local_entrega')">
                    <v-btn color="primary" @click="openEnderecoForm = true" :disabled="recorrencia.clienteId == null">Alterar
                        Local</v-btn>
                </v-col>
            </v-row>
            <v-row class="mt-1">
                <v-col cols="3">
                    <v-select label="Tipo" v-model="recorrencia.tipoRecorrencia" :items="tipoRecorrencia" density="compact"
                        item-title="text" item-value="value" variant="outlined" color="primary"
                        @update:modelValue="recorrencia.dia = null"></v-select>
                </v-col>
                <v-col cols="2">
                    <v-select label="Dia" v-model="recorrencia.dia"
                        :items="recorrencia.tipoRecorrencia == 0 ? diasDaSemana : diasDoMes" density="compact"
                        item-title="text" item-value="value" variant="outlined" color="primary"
                        :rules="[(v) => v != null || 'Campo obrigatório']"></v-select>
                </v-col>
                <v-col>
                    <v-text-field label="Nome" v-model="recorrencia.nome" placeholder="Informe o nome para recorrência"
                        density="compact" variant="outlined" color="info" :rules="[(v) => !!v || 'Campo obrigatório']">
                    </v-text-field>
                </v-col>

            </v-row>
            <v-row v-show="recorrencia.clienteId > 0">
                <v-col cols="10" class="text-left">
                    <div>
                        <span class="text-grey">Entregas: <small>Legenda: (M - Manhã, T - Tarde, N - Noite)</small></span>
                        <br />
                        <span v-for="(dia, index) in diasDaSemana" :key="dia"
                            :class="index > 0 ? 'text-primary ml-2' : 'text-primary'"
                            v-show="recorrencia.periodoEntrega[dia.value].selected && recorrencia.periodoEntrega[dia.value].periodo.length > 0">
                            {{ dia.text }}:&nbsp;
                            <v-avatar color="info" size="x-small"
                                v-for="periodo in recorrencia.periodoEntrega[dia.value].periodo">{{ periodo.slice(0, 1)
                                }}</v-avatar>
                        </span>
                    </div>
                </v-col>
                <v-col>
                    <v-btn color="primary" @click="openPeriodoForm = true">Alterar Período</v-btn>
                </v-col>
            </v-row>
            <v-divider class="mt-3 mb-2"></v-divider>

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
                        <v-text-field density="compact" variant="outlined" type="number" color="primary" class="ml-12"
                            v-model="item.quantidade" min="0" @change="adicionarRemoverProduto(item)" :hide-details="true">
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
                        <v-text-field density="compact" variant="outlined" type="number" color="primary" class="sm ml-12"
                            v-model="item.quantidade" min="0" @change="adicionarRemoverProduto(item)" :hide-details="true">
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

        <v-dialog v-model="openPeriodoForm" max-width="700" :absolute="false" persistent>
            <v-form ref="periodoForm">
                <v-card>
                    <v-card-title class="text-primary text-h5 text-left mb-2 mt-2">
                        Confirmar Período de Entrega
                    </v-card-title>
                    <v-card-text>
                        <v-row>
                            <v-col>
                                <periodo v-model:dia-selecionado="recorrencia.periodoEntrega" />
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col class="text-right">
                                <v-btn color="primary" @click="openPeriodoForm = false">Confirmar</v-btn>
                            </v-col>
                        </v-row>
                    </v-card-text>
                </v-card>
            </v-form>
        </v-dialog>
        <!-- ALTERAR LOCAL DE ENTREGA -->
        <v-dialog v-model="openEnderecoForm" max-width="900" :absolute="false" persistent>
            <v-form>
                <v-card>
                    <v-card-title class="text-primary text-h5 text-left mb-2 mt-2">
                        Alterar Local de Entrega
                    </v-card-title>
                    <v-card-text>
                        <v-row>
                            <v-col>
                                <endereco v-model:data="recorrencia" />
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col class="text-right">
                                <v-btn color="primary" @click="openEnderecoForm = false">Confirmar</v-btn>
                            </v-col>
                        </v-row>
                    </v-card-text>
                </v-card>
            </v-form>
        </v-dialog>
    </div>
</template>
  
<script setup>
import { ref, onMounted, watch, inject, computed } from "vue";
import recorrenciaService from "@/services/recorrencia.service";
import handleErrors from "@/helpers/HandleErrors"
import BreadCrumbs from "@/components/breadcrumbs.vue";
import { useAuthStore } from "@/store/auth.store";
import { useRoute } from "vue-router";
import fornecedorService from "@/services/fornecedor.service";
import { compararValor, tipoRecorrencia, diasDaSemana, diasDoMes } from "@/helpers/functions"
import router from "@/router";
import clienteService from "@/services/cliente.service";
import Periodo from '@/components/Periodo.vue';
import Endereco from "@/components/endereco.vue";
import filialService from "@/services/filial.service";

//DATA

const periodoEntrega = {
    0: { periodo: [], selected: false },
    1: { periodo: [], selected: false },
    2: { periodo: [], selected: false },
    3: { periodo: [], selected: false },
    4: { periodo: [], selected: false },
    5: { periodo: [], selected: false },
    6: { periodo: [], selected: false }
}
const authStore = useAuthStore();
const route = useRoute();
const isBusy = ref(false);
const formCadastro = ref(null);
const recorrencia = ref({
    id: 0,
    nome: "",
    ativo: true,
    filialId: null,
    clienteId: null,
    fornecedorId: null,
    usuarioId: authStore.user.id,
    tipoRecorrencia: 0,
    destino: 0,
    dia: null,
    recorrenciaProdutos: [],
    recorrenciaLogs: [],
    localEntrega: "",
    periodoEntrega: {...periodoEntrega},
    endereco: "",
    numero: "",
    cep: "",
    cidade: "",
    uf: null
});
const clientes = ref([]);
const fornecedores = ref([]);
const destinos = ref([
    { value: 0, text: "Outros" },
    { value: 1, text: "Diretoria" },
])
const produtos = ref([]);
const swal = inject("$swal");
const filterFilial = ref(null);
const filiais = ref([])
const filter = ref("");
const hasProduto = ref(true);
const pageTitle = ref("Recorrência")
const openPeriodoForm = ref(false)
const openEnderecoForm = ref(false)
let clienteHasChange = true;
//VUE METHODS
onMounted(async () => {
    let recorrenciaId = route.params.codigo;
    recorrencia.value.UsuarioId = authStore.user.id;
  
    await getFiliaisByUser();
    await getClienteListByUser()
    if (recorrenciaId != undefined && recorrenciaId > 0) {
        pageTitle.value += " - Edição"
        clienteHasChange = false
        await getRecorrencia(recorrenciaId)
    } else {
        pageTitle.value += " - Cadastro"
    }

});

watch(() => recorrencia.value.fornecedorId, async (fornecedorId, oldFornecedor) => {
    if (oldFornecedor != null && oldFornecedor != fornecedorId)
        recorrencia.value.recorrenciaProdutos = []
        produtos.value = []
        if (fornecedorId)  await getProdutosToList(fornecedorId);
})

watch(() => recorrencia.value.clienteId, async (clienteId) => {
    if (clienteHasChange == true) {
        let cliente = clientes.value.find(c => c.id == clienteId);
        recorrencia.value.endereco = ""
        recorrencia.value.numero =""
        recorrencia.value.cep = ""
        recorrencia.value.cidade = ""
        recorrencia.value.uf = ""
        recorrencia.value.filialId = null
        recorrencia.value.periodoEntrega = {...periodoEntrega}
        if (cliente) {
            recorrencia.value.endereco = cliente.endereco
            recorrencia.value.numero = cliente.numero
            recorrencia.value.cep = cliente.cep
            recorrencia.value.cidade = cliente.cidade
            recorrencia.value.uf = cliente.uf
            recorrencia.value.filialId = cliente.filialId
            if (cliente.filialId != filterFilial.value) await getFornecedorToList(cliente.filialId);

            if (typeof cliente.periodoEntrega === Object)
                recorrencia.value.periodoEntrega = JSON.parse(cliente.periodoEntrega)
            openPeriodoForm.value = true
        }
        
    }
    clienteHasChange = true
})


watch(filter, async () => {
    if (recorrencia.value.fornecedorId != null && recorrencia.value.fornecedorId > 0) {
        await getProdutosToList(recorrencia.value.fornecedorId)
    }
})

watch(() => filterFilial.value, async (novoValor) => {
  clientes.value = []
  recorrencia.value.clienteId = null
  recorrencia.value.fornecedorId = null
    
  await getClienteListByUser(novoValor ? [novoValor]: [])
  await getFornecedorToList(novoValor ? [novoValor]: []);
  
})

const localEntrega = computed(() => {
    let localEntrega = ""
    if (recorrencia.value.endereco && recorrencia.value.endereco?.trim() != "")
        localEntrega = `${recorrencia.value.endereco}, ${recorrencia.value.numero} - ${recorrencia.value.cep} - ${recorrencia.value.cidade} / ${recorrencia.value.uf}`
    return localEntrega
})


//METHODS

function adicionarProdutoRecorrencia(item) {
    let index = recorrencia.value.recorrenciaProdutos.findIndex(p => p.codigo == item.codigo)
    if (index == -1) {
        let produto = { ...item }
        delete produto.id
        recorrencia.value.recorrenciaProdutos.push(produto)

        if (!hasProduto.value)
            hasProduto.value = true;

        produtoRemoveFromList(item)
        ordenarRecorrenciaProdutos()
    }
}

function adicionarRemoverProduto(item) {
    if (item.quantidade > 0) {
        adicionarProdutoRecorrencia(item)
    } else {
        removeProdutoRecorrencia(item)
    }

}

async function getClienteListByUser(filial = []) {
  try {
      let  response = await clienteService.getListByAuthenticatedUser(filial.length > 0? filial : null);
      clientes.value = response.data;
  } catch (error) {
    console.log("getClienteListByUser.error:", error);
    handleErrors(error);
  }
}

async function getFornecedorToList(filial) {
    try {
        let response = await fornecedorService.toList(filial);
        fornecedores.value = response.data;
    } catch (error) {
        console.log("getUsuarioToList.error:", error);
        handleErrors(error)
    }
}

async function getProdutosToList(fornecedorId) {
    try {
        isBusy.value = true;
        let response = await fornecedorService.produtoPaginate(fornecedorId, 99999, 1, filter.value);
        produtos.value = response.data.items;

        for (let index = 0; index < recorrencia.value.recorrenciaProdutos.length; index++) {
            let item = recorrencia.value.recorrenciaProdutos[index];
            produtoRemoveFromList(item)
        }
    } catch (error) {
        console.log("getUsuarioToList.error:", error);
        handleErrors(error)
    } finally {
        isBusy.value = false
    }
}

async function getRecorrencia(id) {
    try {
        isBusy.value = true;
        let response = await recorrenciaService.getById(id);
        if (response.data){
            let data = response.data
            if (data.periodoEntrega == null || data.periodoEntrega.trim() == "") {
                data.periodoEntrega = {...periodoEntrega}
            } else {
                data.periodoEntrega = JSON.parse(data.periodoEntrega)
            }
            recorrencia.value = data;
        }
            
    } catch (error) {
        console.log("getUsuarioToList.error:", error);
        handleErrors(error)
    } finally {
        isBusy.value = false
    }
}

function ordenarRecorrenciaProdutos() {
    if (recorrencia.value.recorrenciaProdutos.length > 0)
        recorrencia.value.recorrenciaProdutos.sort(compararValor("nome"))
}

function produtoRemoveFromList(item) {
    let index = produtos.value.findIndex(p => p.codigo == item.codigo)
    if (index != -1)
        produtos.value.splice(index, 1)
}

async function removeProdutoRecorrencia(item) {
    let index = recorrencia.value.recorrenciaProdutos.findIndex(p => p.codigo == item.codigo)
    if (index != -1) {
        recorrencia.value.recorrenciaProdutos.splice(index, 1)
        await getProdutosToList(recorrencia.value.fornecedorId)
        ordenarRecorrenciaProdutos()
    }
}

async function salvar() {
    try {
        isBusy.value = true;
        let { valid } = await formCadastro.value.validate();
        hasProduto.value = recorrencia.value.recorrenciaProdutos.length > 0

        if (valid && hasProduto.value) {

            let data = { ...recorrencia.value }
            data.periodoEntrega = JSON.stringify(data.periodoEntrega);

            if (recorrencia.value.id > 0)
                await recorrenciaService.update(data)
            else
                await recorrenciaService.create(data)

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
    } catch (error) {
        console.log("salvar.error:", error);
        handleErrors(error)
    } finally {
        isBusy.value = false
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
        console.log("recorrencia.getFiliaisByUser.error:", error.response);
        handleErrors(error)
    } finally
    {
        isBusy.value = false;
    }
}

</script>

<style scoped>
table .v-text-field {
    width: 80px;
}
</style>