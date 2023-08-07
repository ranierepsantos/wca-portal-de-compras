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
                                    
                                    <v-col cols="4"><v-text-field-money label-text="Não ultrapassar o valor limite:" v-model="cliente.valorLimite" color="primary" prefix="R$"
                                    :number-decimal="2" :hide-details="true" density="compact"></v-text-field-money></v-col>
                                </v-row>
                            </v-col>
                        </v-row>
                        <v-divider class="mt-2"></v-divider>
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
    </div>
</template>

<script setup>
import { onMounted, ref, inject } from 'vue';
import { useRoute } from "vue-router";
import { useAuthStore } from "@/store/auth.store";
import breadCrumbs from '@/components/breadcrumbs.vue';
import router from "@/router"
import handleErrors from "@/helpers/HandleErrors"
import filialService from "@/services/filial.service";
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import ClienteDadosBasicosForm from '@/components/clienteDadosBasicosForm.vue';
import {Cliente, useClienteStore } from '@/store/reembolso/cliente.store';

// VARIABLES
const authStore = useAuthStore()
const clienteStore = useClienteStore()
const swal = inject("$swal")
const route = useRoute()
const isBusy = ref(false)
const clienteForm = ref(null)
const cliente = ref(new Cliente());
const filiais = ref([]);
//VUE FUNCTIONS

onMounted(async () => {
    await getFilialToList()
    cliente.value.filialId = authStore.user.filial;
    if (parseInt(route.query.id) > 0) {
        await getCliente(route.query.id)
    }
});

async function getCliente(clienteId) {
    try {
        isBusy.value = true
        let data = await clienteStore.getClienteById(clienteId)
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

async function salvar() {
    try {
        isBusy.value = true
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
        isBusy.value = false
    }
}
</script>
