<template>
    <div>
        <bread-crumbs title="Clientes" :show-button="false" />
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <v-container class="justify-center">
            <v-card class="mx-auto">
                <v-card-text>
                    <v-form ref="clienteForm" lazy-validation>
                        <v-row>
                            <v-col cols="8">
                                <v-text-field label="Nome" v-model="cliente.nome" type="text" required
                                    variant="outlined" color="primary" density="compact"
                                    :rules="[(v) => !!v || 'Campo é obrigatório']"></v-text-field>
                            </v-col>
                            <v-col>
                                <v-text-field label="CNPJ" v-model="cliente.cnpj" type="text" required density="compact"
                                    variant="outlined" color="primary" :rules="[(v) => !!v || 'Campo é obrigatório']"
                                    v-maska="'##.###.###/####-##'">
                                </v-text-field>
                            </v-col>

                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field label="Inscrição Estadual" v-model="cliente.inscricaoEstadual" type="text"
                                    required density="compact" variant="outlined" color="primary"
                                    v-maska="'###.###.###.###'"
                                    :rules="[(v) => !!v || 'Campo é obrigatório']"></v-text-field>
                            </v-col>
                            <v-col>
                                <v-select label="Matriz/Filial" :items="filiais" variant="outlined" color="primary"
                                    item-title="text" item-value="value" v-model="cliente.filialId"
                                    :disabled="(authStore.user.filial != 1)" density="compact"
                                    :rules="[(v) => !!v || 'Filial é obrigatório']"></v-select>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <h3 class="text-left text-grey">Endereço</h3>
                                <v-divider class="mt-3"></v-divider>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field label="Endereço" v-model="cliente.endereco" type="text" variant="outlined"
                                    color="primary" density="compact" maxlength="150">
                                </v-text-field>
                            </v-col>
                            <v-col cols="2">
                                <v-text-field label="Numero" v-model="cliente.numero" type="text" variant="outlined"
                                    color="primary" density="compact" maxlength="10">
                                </v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="2">
                                <v-text-field label="Cep" v-model="cliente.cep" type="text" variant="outlined"
                                    color="primary" density="compact" v-maska="'#####-###'">
                                </v-text-field>
                            </v-col>
                            <v-col>
                                <v-text-field label="Cidade" v-model="cliente.cidade" type="text" variant="outlined"
                                    color="primary" density="compact" maxlength="100">
                                </v-text-field>
                            </v-col>
                            <v-col cols="2">
                                <v-text-field label="UF" v-model="cliente.uf" type="text" variant="outlined"
                                    color="primary" density="compact" v-maska="'AA'">
                                </v-text-field>
                            </v-col>
                        </v-row>
                        <!-- ORÇAMENTOS -->
                        <v-row>
                            <v-col>
                                <h3 class="text-left text-grey">Orçamentos</h3>
                                <v-divider class="mt-3"></v-divider>
                            </v-col>
                        </v-row>
                        <v-row class="text-subtitle-2 text-grey">
                            <v-col class="text-left" cols="3">
                                Fornecimento
                            </v-col>
                            <v-col class="text-center" cols="2">
                                Valor Pedido
                            </v-col>
                            <v-col class="text-center" cols="2">
                                Quantidade Mês
                            </v-col>
                            <v-col class="text-center" cols="2">
                                Tolerância (%)
                            </v-col>
                            <v-col class="text-center" cols="3">
                                Aprovador por
                            </v-col>
                        </v-row>
                        <v-divider></v-divider>
                        <v-row class="mt-1" v-for="item in cliente.clienteOrcamentoConfiguracao"
                            :key="item.tipoFornecimentoId">
                            <v-col class="text-left" cols="3">
                                {{ getTipoFornecimentoNome(item.tipoFornecimentoId) }}
                            </v-col>
                            <v-col class="text-left" cols="2">
                                <div class="v-input__control">
                                    <div class="v-field v-field--active v-field--no-label v-field--variant-outlined "
                                        role="textbox">
                                        <div class="v-field__field" data-no-activator="">
                                            <money3 v-model.number="item.valorPedido" v-bind="moneyConfig"
                                                class="v-field__input"
                                                style="padding-top:4px;min-height: 40px !important;" />
                                        </div>
                                        <!----><!---->
                                        <div class="v-field__outline">
                                            <div class="v-field__outline__start"></div><!---->
                                            <div class="v-field__outline__end"></div><!---->
                                        </div>
                                    </div>
                                </div>
                            </v-col>
                            <v-col class="text-left" cols="2">
                                <v-text-field v-model="item.quantidadeMes" density="compact" variant="outlined"
                                    color="primary" :hide-details="true" type="number" class="text-left" min="0" />
                            </v-col>
                            <v-col class="text-left" cols="2">
                                <v-text-field v-model="item.tolerancia" density="compact" variant="outlined"
                                    color="primary" :hide-details="true" type="number" min="0" max="100" />
                            </v-col>
                            <v-col class="text-left" cols="3">
                                <v-select v-model="item.aprovadoPor" :items="aprovadorList" density="compact"
                                    item-title="text" item-value="value" variant="outlined" color="primary"></v-select>
                            </v-col>
                        </v-row>

                        <!-- CONTATOS -->
                        <v-row>
                            <v-col>
                                <h3 class="text-left text-grey">Contatos</h3>
                                <v-divider class="mt-3"></v-divider>
                            </v-col>
                        </v-row>
                        <v-table class="elevation-2">
                            <thead>
                                <tr>
                                    <th class="text-left text-grey">NOME</th>
                                    <th class="text-left text-grey">E-MAIL</th>
                                    <th class="text-center text-grey">TELEFONE</th>
                                    <th class="text-center text-grey">CELULAR</th>
                                    <th class="text-center text-grey">APROVA?</th>
                                    <th class="text-right"> <v-btn density="compact" variant="outlined" color="primary"
                                            class="text-capitalize" @click="editarContato(null)">Novo</v-btn>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="item in cliente.clienteContatos" :key="item.id">
                                    <td class="text-left">
                                        {{ item.nome }}
                                    </td>
                                    <td class="text-left">{{ item.email }}</td>
                                    <td class="text-right">
                                        {{ mask(item.telefone, '["(##) #####-####", "(##) ####-####"]') }}
                                    </td>
                                    <td class="text-right">
                                        {{ mask(item.celular, '["(##) #####-####", "(##) ####-####"]') }}
                                    </td>
                                    <td class="text-center">
                                        <v-icon :icon="item.aprovaPedido ? 'mdi-check' : 'mdi-close'" variant="plain"
                                            :color="item.aprovaPedido ? 'success' : 'error'"></v-icon>
                                    </td>
                                    <td class="text-right">
                                        <v-btn icon="mdi-lead-pencil" variant="plain" color="primary"
                                            @click="editarContato(item)"></v-btn>
                                        <v-btn icon="mdi-delete" variant="plain" color="error"
                                            @click="removerContato(item)"></v-btn>
                                    </td>
                                </tr>
                            </tbody>
                        </v-table>
                    </v-form>
                </v-card-text>
                <v-card-text class="text-right">
                    <v-btn color="primary" variant="outlined" class="mr-4" @click="router.go(-1)">
                        Cancelar
                    </v-btn>
                    <v-btn color="primary" class="mr-4" @click="salvar">
                        Salvar
                    </v-btn>
                </v-card-text>
            </v-card>
        </v-container>
        <v-dialog v-model="contatoDialog" max-width="700" :absolute="false">
            <v-card>
                <v-card-title class="text-primary text-h5">
                    {{ dialogTitle }}
                </v-card-title>
                <v-card-text>
                    <v-form @submit.prevent="salvarContato()" ref="contatoForm">
                        <v-row>
                            <v-col>
                                <v-text-field label="Nome" v-model="clienteContato.nome" type="text" required
                                    density="compact" variant="outlined" color="primary"
                                    :rules="[(v) => !!v || 'Campo é obrigatório']">
                                </v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field label="Email" v-model="clienteContato.email" type="email" required
                                    density="compact" variant="outlined" color="primary" :rules="emailRules">
                                </v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field label="Telefone" v-model="clienteContato.telefone" type="text" required
                                    density="compact" variant="outlined" color="primary"
                                    v-maska="['(##) #####-####', '(##) ####-####']">
                                </v-text-field>
                            </v-col>
                            <v-col>
                                <v-text-field label="Celular" v-model="clienteContato.celular" type="text" required
                                    density="compact" variant="outlined" color="primary"
                                    v-maska="['(##) #####-####', '(##) ####-####']">
                                </v-text-field>
                            </v-col>
                            <v-col>
                                <v-checkbox v-model="clienteContato.aprovaPedido" label="Aprova Pedido?"
                                    color="primary"></v-checkbox>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col class="text-right">
                                <v-btn variant="outlined" color="primary" @click="closeContatoDialog()">Cancelar</v-btn>
                                <v-btn color="primary" type="submit" class="ml-3">Salvar</v-btn>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-card-text>
                <v-card-actions> </v-card-actions>
            </v-card>
        </v-dialog>
    </div>
</template>

<script setup>
import { onMounted, ref, inject } from 'vue';
import { useRoute } from "vue-router";
import { useAuthStore } from "@/store/auth.store";
import breadCrumbs from '@/components/breadcrumbs.vue';
import tipoFornecimentoService from "@/services/tipofornecimento.service";
import router from "@/router"
import handleErrors from "@/helpers/HandleErrors"
import clienteService from '@/services/cliente.service';
import filialService from "@/services/filial.service";
import { mask } from "maska"


// VARIABLES
//const mask = new Mask({ mask: [, "(##) ####-####"] })
const authStore = useAuthStore()
const swal = inject("$swal")
const route = useRoute()
const isBusy = ref(false)
const clienteForm = ref(null)
const contatoForm = ref(null)
const cliente = ref({
    id: 0,
    nome: "",
    cnpj: "",
    inscricaoEstadual: "",
    endereco: "",
    numero: "",
    cep: "",
    cidade: "",
    uf: "",
    ativo: true,
    filialId: null,
    clienteContatos: [],
    clienteOrcamentoConfiguracao: []
});
const clienteContato = ref({
    id: 0,
    clienteId: 0,
    nome: "",
    email: "",
    telefone: "",
    celular: "",
    aprovaPedido: false
})
const contatoDialog = ref(false)
const dialogTitle = "Novo Contato"
const filiais = ref([]);
const fornecimentos = ref([]);
const aprovadorList = [
    { text: "WCA", value: 0 },
    { text: "Cliente", value: 1 }
]
const moneyConfig = {
    masked: false,
    prefix: '',
    suffix: '',
    thousands: '',
    decimal: '.',
    precision: 2,
    disableNegative: true,
    disabled: false,
    min: 0.00,
    max: null,
    allowBlank: false,
    minimumNumberOfCharacters: 3,
}
const emailRules = ref([
    (v) => !!v || "Campo é obrigatório",
    (v) => /.+@.+\..+/.test(v) || "E-mail deve ser válido",
]);
//VUE FUNCTIONS

onMounted(async () =>
{
    await getFilialToList()
    await getTipoFornecimentoToList()
    console.log("route.query.id", route.query.id)
    cliente.value.filialId = authStore.user.filial;
    if (parseInt(route.query.id) > 0)
    {
        await getCliente(route.query.id)
    }

    fornecimentos.value.forEach(item =>
    {
        let orcamento = {
            "id": 0,
            "clienteId": 0,
            "tipoFornecimentoId": item.value,
            "valorPedido": 0.00,
            "quantidadeMes": 0,
            "tolerancia": 0,
            "aprovadoPor": 0
        }
        let hasTipo = cliente.value.clienteOrcamentoConfiguracao.filter(o => o.tipoFornecimentoId == orcamento.tipoFornecimentoId)[0] != undefined
        if (!hasTipo) cliente.value.clienteOrcamentoConfiguracao.push(orcamento);
    })

});

//METHODS
function clearContato()
{
    clienteContato.value = {
        id: 0,
        clienteId: cliente.value.id,
        nome: "",
        email: "",
        telefone: "",
        celular: "",
        aprovaPedido: false
    }
}
function closeContatoDialog()
{
    contatoDialog.value = false
    clearContato();
}

function editarContato(contato)
{
    clearContato();
    if (contato != null)
    {
        clienteContato.value = { ...contato }
    }
    contatoDialog.value = true
}

async function getCliente(clienteId)
{
    try
    {
        isBusy.value = true
        let response = await clienteService.getById(clienteId);
        cliente.value = response.data;
    } catch (error)
    {
        console.log("getTipoFornecimentoToList.error:", error);
        handleErrors(error)
    } finally
    {
        isBusy.value = false
    }
}

async function getFilialToList()
{
    try
    {
        let response = await filialService.toList();
        filiais.value = response.data;
    } catch (error)
    {
        console.log("getFilialToList.error:", error);
        handleErrors(error)
    }
}

function getTipoFornecimentoNome(tipoId)
{
    let tipo = fornecimentos.value.filter(t => t.value == tipoId)
    if (tipo.length > 0)
    {
        return tipo[0].text
    }
    return ""
}

async function getTipoFornecimentoToList() 
{
    try
    {
        let response = await tipoFornecimentoService.toList();
        fornecimentos.value = response.data;
    } catch (error)
    {
        console.log("getTipoFornecimentoToList.error:", error);
        handleErrors(error)
    }
}

async function removerContato(contato)
{
    let options = {
        title: "Confirma Exclusão?",
        text: "Deseja realmente excluir o contato: " + contato.nome + "?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Sim",
        cancelButtonText: "Não",
    }

    let response = await swal.fire(options);
    if (response.isConfirmed)
    {
        let index = cliente.value.clienteContatos.findIndex(c => { return c.id == contato.id })
        if (index > -1)
        {
            cliente.value.clienteContatos.splice(index, 1);
        }
    }
}

async function salvar()
{
    try
    {
        isBusy.value = true
        const { valid } = await clienteForm.value.validate()
        if (valid)
        {
            if (cliente.value.id == 0)
            {

                await clienteService.create(cliente.value)
            }
            else
                await clienteService.update(cliente.value)

            swal.fire({
                toast: true,
                icon: "success",
                index: "top-end",
                title: "Sucesso!",
                text: "Dados salvos com sucesso!",
                showConfirmButton: false,
                timer: 2000,
            })
            router.push({ name: "clientes" })
        }

    } catch (error)
    {
        console.log("perfil.cadastro.salvar.erro", error)
        handleErrors(error)
    } finally
    {
        isBusy.value = false
    }
}

function salvarContato()
{
    if (clienteContato.value.id > 0)
    {
        let index = cliente.value.clienteContatos.findIndex(c => { return c.id == clienteContato.value.id })
        if (index > -1)
        {
            cliente.value.clienteContatos[index] = { ...clienteContato.value }
        }
    } else
    {
        cliente.value.clienteContatos.push({ ...clienteContato.value })
    }
    closeContatoDialog()
}

</script>
