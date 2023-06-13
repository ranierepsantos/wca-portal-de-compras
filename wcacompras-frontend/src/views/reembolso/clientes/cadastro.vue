<template>
    <div>
        <bread-crumbs title="Clientes" :show-button="false" />
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <v-container class="justify-center">
            <v-card class="mx-auto">
                <v-card-text>
                    <v-form ref="clienteForm" lazy-validation>
                        <cliente-dados-basicos-form :cliente="cliente" :filiais = "filiais"/>
                        <v-divider class="mt-2"></v-divider>
                        <!-- ORÇAMENTOS -->
                        <v-row class="mt-2">
                            <v-col>
                                <v-row>
                                    <v-col><v-checkbox v-model="cliente.naoUltrapassarLimite" label="Não ultrapassar o valor limite" color="primary" :hide-details="true" density="compact"></v-checkbox>
                                </v-col>
                                    <v-col cols="4"><v-text-field-money label-text="" v-model="cliente.valorLimite" color="primary" prefix="R$"
                                    :number-decimal="2" :hide-details="true" density="compact" :disabled="!cliente.naoUltrapassarLimite"></v-text-field-money></v-col>
                                </v-row>
                            </v-col>
                        </v-row>
                        <v-divider class="mt-2"></v-divider>
                        <!-- CONTATOS -->
                        <v-row class="mt-2">
                            <v-col>
                                <h3 class="text-left text-grey">CONTATOS</h3>
                                <small class="text-error" v-show="!isContatoValido">
                                    Deve haver pelo menos 1 contato que aprova solicitação
                                </small>
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
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import Periodo from '@/components/Periodo.vue';
import ClienteDadosBasicosForm from '@/components/clienteDadosBasicosForm.vue';

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
    naoUltrapassarLimitePorRequisicao: false,
    valorLimiteRequisicao: 0
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
const isContatoValido= ref(true)
const contatoDialog = ref(false)
const dialogTitle = "Novo Contato"
const filiais = ref([]);
const emailRules = ref([
    (v) => !!v || "Campo é obrigatório",
    (v) => /.+@.+\..+/.test(v) || "E-mail deve ser válido",
]);
const clienteContatoId = ref(0);

//VUE FUNCTIONS

onMounted(async () => {
    await getFilialToList()
    cliente.value.filialId = authStore.user.filial;
    if (parseInt(route.query.id) > 0) {
        await getCliente(1)
    }
});

//METHODS
function clearContato() {
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

function closeContatoDialog() {
    contatoDialog.value = false
    clearContato();
}

function editarContato(contato) {
    clearContato();
    if (contato != null) {
        clienteContato.value = { ...contato }
    }
    contatoDialog.value = true
}

async function getCliente(clienteId) {
    try {
        isBusy.value = true
        
        let data = {
            id: 1, 
            nome: "Cliente Reembolso A",
            cnpj: "01.123.123/0001-01" , 
            ativo: true,
            inscricaoEstadual: "01.123.123-1",
            endereco: "Rua Cliente A",
            numero: "123",
            cep: "01234-567",
            cidade: "São Paulo",
            uf: "SP",
            filialId: 1,
            naoUltrapassarLimite: true,
            valorLimite: 5000.00
        }
        cliente.value = data;
    } catch (error) {
        console.log("getCliente.error:", error);
        handleErrors(error)
    } finally {
        isBusy.value = false
    }
}

async function getFilialToList() {
    try {
        let response = await filialService.toList();
        filiais.value = response.data;
    } catch (error) {
        console.log("getFilialToList.error:", error);
        handleErrors(error)
    }
}

async function removerContato(contato) {
    let options = {
        title: "Confirma Exclusão?",
        text: "Deseja realmente excluir o contato: " + contato.nome + "?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Sim",
        cancelButtonText: "Não",
    }

    let response = await swal.fire(options);
    if (response.isConfirmed) {
        let index = cliente.value.clienteContatos.findIndex(c => { return c.id == contato.id })
        if (index > -1) {
            cliente.value.clienteContatos.splice(index, 1);
        }
    }
}

async function salvar() {
    try {
        isBusy.value = true
        const { valid } = await clienteForm.value.validate()
        isContatoValido.value = cliente.value.clienteContatos.find(c => c.aprovaPedido == true) != undefined;
        
        let data = {...cliente.value}
        if (valid) {

            // //alterar os id's negativos do contato
            // cliente.value.clienteContatos.forEach(contato => {
            //     if (contato.id < 0)
            //         contato.id = 0;
            // })

            // if (cliente.value.id == 0) {

            //     await clienteService.create(data)
            // }
            // else
            //     await clienteService.update(data)

            swal.fire({
                toast: true,
                icon: "success",
                index: "top-end",
                title: "Sucesso!",
                text: "Dados salvos com sucesso!",
                showConfirmButton: false,
                timer: 2000,
            })
            router.push({ name: "reembolsoClientes" })
        }

    } catch (error) {
        console.log("cliente.cadastro.salvar.erro", error)
        handleErrors(error)
    } finally {
        isBusy.value = false
    }
}

async function salvarContato() {
    const { valid } = await contatoForm.value.validate()
    if (valid) {
        let index = -1;
        if (clienteContato.value.id != 0) {
            index = cliente.value.clienteContatos.findIndex(c => { return c.id == clienteContato.value.id })
            if (index > -1) {
                cliente.value.clienteContatos[index] = { ...clienteContato.value }
            }
        }
        if (index == -1){
            clienteContatoId += -1
            clienteContato.value.id = clienteContatoId;
            cliente.value.clienteContatos.push({ ...clienteContato.value })
        }
            
        closeContatoDialog()
    }
}

</script>
