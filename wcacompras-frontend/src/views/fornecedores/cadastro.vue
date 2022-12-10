<template>
    <div>
        <bread-crumbs title="Fornecedores" :show-button="false" />
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <v-container class="justify-center">
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
import tipoFornecimentoService from "@/services/tipofornecimento.service";
import router from "@/router"
import handleErrors from "@/helpers/HandleErrors"
import fornecedorService from '@/services/fornecedor.service';
import filialService from "@/services/filial.service";


// VARIABLES
const authStore = useAuthStore()
const swal = inject("$swal")
const route = useRoute()
const isBusy = ref(false)
const fornecedorForm = ref(null)
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
    tipoFornecimentoId: 1
});
const filiais = ref([]);
const fornecimentos = ref([]);

//VUE FUNCTIONS

onMounted(async () =>
{
    await getFilialToList()
    await getTipoFornecimentoToList()

    fornecedor.value.filialId = authStore.user.filial;
    if (parseInt(route.query.id) > 0)
    {
        await getfornecedor(route.query.id)
    }
});

//METHODS
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


async function salvar()
{
    try
    {
        isBusy.value = true
        const { valid } = await fornecedorForm.value.validate()
        if (valid)
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
</script>
