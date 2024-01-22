<template>
    <div>
        <bread-crumbs title="Clientes" :show-button="false" />
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isLoading.form"></v-progress-linear>
        <v-container class="justify-center" v-show="!isLoading.form">
            <v-card class="mx-auto">
                <v-card-text>
                    <v-form ref="clienteForm" lazy-validation>
                        <cliente-dados-basicos-form :cliente="cliente" :filiais = "filiais" :combo-filial-disabled="!isMatriz"/>
                        <v-divider class="mt-2"></v-divider>
                        <!-- ORÇAMENTOS -->
                        <v-row class="mt-2">
                            <v-col>
                                <v-row>
                                    
                                    <v-col cols="4"><v-text-field-money label-text="Não ultrapassar o valor limite:" v-model="cliente.valorLimite" color="primary" prefix="R$"
                                    :number-decimal="2" :hide-details="true" density="compact"></v-text-field-money></v-col>
                                </v-row>
                            </v-col>
                        </v-row>
                        <v-divider class="mt-3 mb-3"></v-divider>
                        <!-- CENTRO DE CUSTO -->
                        <v-row>
                            <v-col>
                                <h3 class="text-left text-grey">CENTROS DE CUSTO</h3>
                                <!-- <small class="text-error" v-show="!isContatoValido">Deve haver pelo menos 1 contato que
                                    aprova requisição</small> -->
                                <v-divider class="mt-3"></v-divider>
                            </v-col>
                        </v-row>
                        <v-table class="elevation-2">
                            <thead>
                                <tr>
                                    <th class="text-left text-grey">CÓDIGO</th>
                                    <th class="text-left text-grey">NOME</th>
                                    <th class="text-right"> <v-btn density="compact" variant="outlined" color="primary"
                                            class="text-capitalize" @click="editarCentroCusto(null)">Adicionar</v-btn>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="item in cliente.centroCusto" :key="item.id">
                                    <td class="text-left">
                                        {{ item.centroCustoId }}
                                    </td>
                                    <td class="text-left">{{ item.nome }}</td>
                                    <td class="text-right">
                                        <v-btn icon="mdi-lead-pencil" variant="plain" color="primary" @click="editarCentroCusto(item)"></v-btn>
                                        <v-btn icon="mdi-delete" variant="plain" color="error"  @click="removerCentroCusto(item)"></v-btn>
                                    </td>
                                </tr>
                            </tbody>
                        </v-table>
                    </v-form>
                </v-card-text>
                <v-card-text class="text-right">
                    <v-btn color="primary" variant="outlined" class="mr-4" @click="router.go(-1)" :disabled="isLoading.save">
                        Cancelar
                    </v-btn>
                    <v-btn color="primary" class="mr-4" @click="salvar" :disabled="isLoading.save">
                        Salvar
                    </v-btn>
                </v-card-text>
            </v-card>
        </v-container>
        <v-dialog v-model="dialogCentroCustoOpen" max-width="700" :absolute="false">
            <v-card>
                <v-card-title class="text-primary text-h5">Centro de Custo</v-card-title>
                <v-card-text>
                    <v-form @submit.prevent="salvarCentroCusto()" ref="centroCustoForm">
                        <v-row>
                            <v-col>
                                <v-text-field label="Código" v-model="centroCusto.centroCustoId" type="number" required
                                    density="compact" variant="outlined" color="primary"
                                    :rules="[(v) => !!v || 'Campo é obrigatório']">
                                </v-text-field>
                            </v-col>
                            <v-col>
                                <v-text-field label="Nome" v-model="centroCusto.nome" type="text" required
                                    density="compact" variant="outlined" color="primary" :rules="[(v) => !!v || 'Campo é obrigatório']">
                                </v-text-field>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col class="text-right">
                                <v-btn variant="outlined" color="primary" @click="closeDialogCentroCusto()">Cancelar</v-btn>
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
import router from "@/router"
import handleErrors from "@/helpers/HandleErrors"
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import ClienteDadosBasicosForm from '@/components/clienteDadosBasicosReembolsoForm.vue';
import {Cliente, useClienteStore } from '@/store/reembolso/cliente.store';
import filialService from '@/services/filial.service';
import { useUsuarioStore } from '@/store/reembolso/usuario.store';

// VARIABLES
const authStore = useAuthStore()
const clienteStore = useClienteStore()
const swal = inject("$swal")
const route = useRoute()
const clienteForm = ref(null)
const cliente = ref(new Cliente());
const filiais = ref([]);
const isLoading = ref({
    form: true,
    save: false
})
const isMatriz = ref(false)

const centroCusto = ref({
    centroCustoId: 0,
    clienteId: 0,
    nome: ''
})
const dialogCentroCustoOpen = ref(false)
const centroCustoForm = ref(null)

//VUE FUNCTIONS

onMounted(async () => {
    try {
        isLoading.value.form = true
        let filialUsuario =(await useUsuarioStore().getFiliais(authStore.user.id))[0]
        isMatriz.value = filialUsuario.text.toLowerCase() =="matriz"
        authStore.user.filial = filialUsuario.value;
        cliente.value.filialId = authStore.user.filial
        await getFilialToList()
        if (parseInt(route.query.id) > 0) {
            await getCliente(route.query.id)
        }       
    } catch (error) {
        console.error(error)
    } finally {
        isLoading.value.form = false 
    }
});

async function getCliente(clienteId) {
    try {
        
        let data = await clienteStore.getClienteById(clienteId)
        cliente.value = data;
    } catch (error) {
        console.log("getCliente.error:", error);
        handleErrors(error)
    }
}

async function getFilialToList() {
    try {
        let response = await  filialService.toList();

        filiais.value = response.data

    } catch (error) {
        console.log("getFilialToList.error:", error);
        handleErrors(error)
    }
}

async function salvar() {
    try {
        isLoading.value.save = true
        const { valid } = await clienteForm.value.validate()
        let data = {...cliente.value}
        if (valid) {
            if (cliente.value.id == 0) {
                await clienteStore.addCliente(data)
            }
            else
                await clienteStore.updateCliente(data)

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
        isLoading.value.save = false
    }
}

async function salvarCentroCusto() {
    const { valid } = await centroCustoForm.value.validate()
    if (valid)
    {
        let index = -1;
        if (centroCusto.value.centroCustoId != 0)
        {
            index = cliente.value.centroCusto.findIndex(c => { return c.centroCustoId == centroCusto.value.centroCustoId })
            if (index > -1)
            {
                cliente.value.centroCusto[index] = { ...centroCusto.value }
            }
        }
        if (index < 0) {
            cliente.value.centroCusto.push({ ...centroCusto.value })
        }

        closeDialogCentroCusto()
    }
}

function closeDialogCentroCusto() {
    dialogCentroCustoOpen.value = false
    clearCentroCustoData()
}

function clearCentroCustoData() {
    centroCusto.value = {
        centroCustoId: 0,
        clienteId: 0,
        nome: ''
    }
}

function editarCentroCusto(item) {
    if (!item)
        clearCentroCustoData()
    else 
        centroCusto.value = {...item};
    dialogCentroCustoOpen.value = true
}

async function removerCentroCusto(item) {
    let options = {
        title: "Confirma Exclusão?",
        text: "Deseja realmente excluir o centro de custo: " + item.centroCustoId + ' - ' + item.nome + "?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Sim",
        cancelButtonText: "Não",
    }

    let response = await swal.fire(options);
    if (response.isConfirmed) {
        let index = cliente.value.centroCusto.findIndex(c => { return c.centroCustoId == item.centroCustoId })
        if (index > -1) {
            cliente.value.centroCusto.splice(index, 1);
        }
    }
}
</script>
