<template>
    <div>
        <bread-crumbs title="Fornecedores" :show-button="false" />
        <v-progress-linear color="primary" indeterminate :height="5" v-if="isBusy"></v-progress-linear>
        <v-container class="justify-center" v-else>
            <v-card class="mx-auto">
                <v-card-text>
                    <v-form ref="fornecedorForm" lazy-validation>
                        <v-row>
                            <v-col cols="8">
                                <v-text-field label="Nome" v-model="fornecedor.nome" type="text" required
                                    variant="outlined" color="primary" density="compact"
                                    :rules="[(v) => !!v || 'Campo é obrigatório']"></v-text-field>
                            </v-col>
                            <v-col>
                                <v-text-field label="CNPJ" v-model="fornecedor.cnpj" type="text" required
                                    density="compact" variant="outlined" color="primary"
                                    :rules="[(v) => !!v || 'Campo é obrigatório']" v-maska="'##.###.###/####-##'">
                                </v-text-field>
                            </v-col>

                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field label="Inscrição Estadual" v-model="fornecedor.inscricaoEstadual"
                                    type="text" required density="compact" variant="outlined" color="primary"
                                    v-maska="'###.###.###.###'"
                                    :rules="[(v) => !!v || 'Campo é obrigatório']"></v-text-field>
                            </v-col>
                            <v-col>
                                <v-select label="Matriz/Filial" :items="filiais" variant="outlined" color="primary"
                                    item-title="text" item-value="value" v-model="fornecedor.filialId"
                                    :disabled="(authStore.user.filial != 1)" density="compact"
                                    :rules="[(v) => !!v || 'Filial é obrigatório']"></v-select>
                            </v-col>
                            <v-col>
                                <v-checkbox v-model="fornecedor.ativo" color="primary" label="Ativo?" density="compact"
                                    v-show="fornecedor.id > 0">
                                </v-checkbox>
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
                                <v-text-field label="Endereço" v-model="fornecedor.endereco" type="text"
                                    variant="outlined" color="primary" density="compact" maxlength="150">
                                </v-text-field>
                            </v-col>
                            <v-col cols="2">
                                <v-text-field label="Numero" v-model="fornecedor.numero" type="text" variant="outlined"
                                    color="primary" density="compact" maxlength="10">
                                </v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col cols="2">
                                <v-text-field label="Cep" v-model="fornecedor.cep" type="text" variant="outlined"
                                    color="primary" density="compact" v-maska="'#####-###'">
                                </v-text-field>
                            </v-col>
                            <v-col>
                                <v-text-field label="Cidade" v-model="fornecedor.cidade" type="text" variant="outlined"
                                    color="primary" density="compact" maxlength="100">
                                </v-text-field>
                            </v-col>
                            <v-col cols="2">
                                <v-text-field label="UF" v-model="fornecedor.uf" type="text" variant="outlined"
                                    color="primary" density="compact" v-maska="'AA'">
                                </v-text-field>
                            </v-col>
                        </v-row>
                        <!-- CONTATOS -->
                        <v-row>
                            <v-col>
                                <h3 class="text-left text-grey">Contatos</h3>
                                <small class="text-error" v-show="!isContatoValido">Deve haver pelo menos 1 contato que
                                    aprova requisição</small>
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
                                <tr v-for="item in fornecedor.fornecedorContatos" :key="item.id">
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
                                <v-text-field label="Nome" v-model="fornecedorContato.nome" type="text" required
                                    density="compact" variant="outlined" color="primary"
                                    :rules="[(v) => !!v || 'Campo é obrigatório']">
                                </v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field label="Email" v-model="fornecedorContato.email" type="email" required
                                    density="compact" variant="outlined" color="primary" :rules="emailRules">
                                </v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col>
                                <v-text-field label="Telefone" v-model="fornecedorContato.telefone" type="text" required
                                    density="compact" variant="outlined" color="primary"
                                    v-maska="['(##) #####-####', '(##) ####-####']">
                                </v-text-field>
                            </v-col>
                            <v-col>
                                <v-text-field label="Celular" v-model="fornecedorContato.celular" type="text" required
                                    density="compact" variant="outlined" color="primary"
                                    v-maska="['(##) #####-####', '(##) ####-####']">
                                </v-text-field>
                            </v-col>
                            <v-col>
                                <v-checkbox v-model="fornecedorContato.aprovaPedido" label="Aprova Pedido?"
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
import fornecedorService from '@/services/fornecedor.service';
import filialService from "@/services/filial.service";
import { mask } from "maska"

// VARIABLES
const authStore = useAuthStore()
const swal = inject("$swal")
const route = useRoute()
const isBusy = ref(true)
const fornecedorForm = ref(null)
const contatoForm = ref(null)
const contatoDialog = ref(false)
const dialogTitle = "Novo Contato"
const fornecedor = ref({
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
    fornecedorContatos: []
});
const fornecedorContato = ref({
    id: 0,
    fornecedorId: null,
    nome: "",
    email: "",
    telefone: "",
    celular: "",
    aprovaPedido: false
});
const filiais = ref([]);
const fornecimentos = ref([]);
const emailRules = ref([
    (v) => !!v || "Campo é obrigatório",
    (v) => /.+@.+\..+/.test(v) || "E-mail deve ser válido",
]);

const isContatoValido = ref(true);

//VUE FUNCTIONS

onMounted(async () =>
{
    await getFilialToList()

    fornecedor.value.filialId = authStore.user.filial;
    if (parseInt(route.query.id) > 0)
    {
        await getfornecedor(route.query.id)
    }
    isBusy.value = false;
});

//METHODS
function clearContato()
{
    fornecedorContato.value = {
        id: 0,
        fornecedorId: fornecedor.value.id,
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
        fornecedorContato.value = { ...contato }
    }
    contatoDialog.value = true
}

async function getfornecedor(fornecedorId)
{
    try
    {
        isBusy.value = true
        let response = await fornecedorService.getById(fornecedorId);
        fornecedor.value = response.data;
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
        let index = fornecedor.value.fornecedorContatos.findIndex(c => { return c.id == contato.id })
        if (index > -1)
        {
            fornecedor.value.fornecedorContatos.splice(index, 1);
        }
    }
}

async function salvar()
{
    try
    {
        isBusy.value = true
        isContatoValido.value = fornecedor.value.fornecedorContatos.find(c => c.aprovaPedido == true) != undefined;
        const { valid } = await fornecedorForm.value.validate()
        if (valid && isContatoValido.value)
        {
            if (fornecedor.value.id == 0)
            {
                await fornecedorService.create(fornecedor.value)
            }
            else
                await fornecedorService.update(fornecedor.value)

            swal.fire({
                toast: true,
                icon: "success",
                index: "top-end",
                title: "Sucesso!",
                text: "Dados salvos com sucesso!",
                showConfirmButton: false,
                timer: 2000,
            })
            router.push({ name: "fornecedores" })
        }

    } catch (error)
    {
        console.log("fornecedor.cadastro.salvar.erro", error)
        handleErrors(error)
    } finally
    {
        isBusy.value = false
    }
}

async function salvarContato()
{
    const { valid } = await contatoForm.value.validate()
    if (valid)
    {
        let index = -1;
        if (fornecedor.value.id > 0)
        {
            index = fornecedor.value.fornecedorContatos.findIndex(c => { return c.id == fornecedorContato.value.id })
            if (index > -1)
            {
                fornecedor.value.fornecedorContatos[index] = { ...fornecedorContato.value }
            }
        }
        if (index < 0)
            fornecedor.value.fornecedorContatos.push({ ...fornecedorContato.value })
        closeContatoDialog()
    }

}
</script>
