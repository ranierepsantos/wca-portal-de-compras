<template>
    <div>
        <bread-crumbs title="Clientes" :show-button="false" />
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isLoading.form"></v-progress-linear>
        <v-container class="justify-center" v-show="!isLoading.form">
            <v-card class="mx-auto">
                <v-card-text>
                    <v-form ref="clienteForm" lazy-validation>
                        <cliente-dados-basicos-form :cliente="cliente" :filiais = "filiais" :combo-filial-disabled="authStore.user.filial !== 1"/>
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
                    <v-btn color="primary" variant="outlined" class="mr-4" @click="router.go(-1)" :disabled="isLoading.save">
                        Cancelar
                    </v-btn>
                    <v-btn color="primary" class="mr-4" @click="salvar" :disabled="isLoading.save">
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
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import ClienteDadosBasicosForm from '@/components/clienteDadosBasicosForm.vue';
import {Cliente, useClienteStore } from '@/store/reembolso/cliente.store';
import { useFilialStore } from '@/store/reembolso/filial.store';
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
//VUE FUNCTIONS

onMounted(async () => {
    try {
        isLoading.value.form = true
        authStore.user.filial = (await useUsuarioStore().getFiliais(authStore.user.id))[0].value;
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
        
        filiais.value = await useFilialStore().toComboList();

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
</script>
