<template>
    <div>
        <bread-crumbs  title="Perfil" :show-button="false"/>
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <v-container class="justify-center">
            <v-card class="mx-auto" max-width="980">
                <v-card-text>
                    <v-form ref="form" v-model="valid" lazy-validation>
                        <v-text-field density="compact" variant="outlined" v-model="perfil.nome" :counter="50"
                            :rules="nameRules" label="Perfil" required>
                        </v-text-field>

                        <v-textarea v-model="perfil.descricao" label="Descrição" variant="outlined" density="compact">
                        </v-textarea>

                        <v-checkbox v-model="perfil.ativo" color="primary" label="Ativo?" density="compact"
                            v-show="perfil.id > 0">
                        </v-checkbox>
                        <v-row>
                            <v-col class="text-left">
                                <h2 class="text-primary">Permissões</h2>
                                <small class="text-error" v-show="!isPermissaoValid">Selecione pelo menos 1
                                    permissão</small>
                            </v-col>
                        </v-row>

                        <div v-for="permissao in permissoesList" :key="permissao.id">
                            <v-switch v-model="perfil.permissao" color="primary"
                                :value="{ id: permissao.id, nome: permissao.nome, regra: permissao.regra }"
                                :hide-details="true">
                                <template v-slot:label>
                                    {{ permissao.nome }}
                                    &nbsp;-&nbsp;
                                    <small>{{ permissao.descricao }}</small>
                                </template>
                            </v-switch>
                            <v-divider class="mt-5"></v-divider>
                        </div>
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
import { onMounted, ref, inject } from "vue"
import perfilService from "@/services/perfil.service";
import handleErrors from "../../helpers/HandleErrors"
import { useRoute } from "vue-router";
import breadCrumbs from '@/components/breadcrumbs.vue';
import router from "@/router"

// VARIABLES
let route = useRoute();
let swal = inject("$swal");
let valid = ref(true);
let isBusy = ref(false)
let form = ref(null);
let nameRules = ref([
    v => !!v || 'Perfil é obrigatório',
    v => (v && v.length <= 50) || 'Perfil deve ter até 50 caracteres',
]);
let permissoesList = ref([]);
let isPermissaoValid = ref(true);
let perfil = ref({
    id: 0,
    nome: '',
    descricao: '',
    ativo: true,
    permissao: []
})

//VUE METHODS
onMounted(async () =>
{
    if (parseInt(route.query.id) > 0)
    {
        await getPerfil(route.query.id)
    }
    await getPermissoes();
})

// METHODS
async function getPerfil(perfilId)
{
    try
    {
        isBusy.value = true
        let response = await perfilService.getWithPermissions(perfilId);
        perfil.value = response.data;
    } catch (error)
    {
        console.log('getPerfil.error', error)
        handleErrors(error);
    } finally
    {
        isBusy.value = false
    }
}

async function getPermissoes()
{
    try
    {
        isBusy.value = true
        let response = await perfilService.permissaoAll();
        permissoesList.value = response.data;
    } catch (error)
    {
        console.log('getPermissoes.error', error)
        handleErrors(error);
    } finally
    {
        isBusy.value = false
    }
}

async function salvar()
{
    try
    {
        isBusy.value = true
        isPermissaoValid.value = perfil.value.permissao.length > 0
        const { valid } = await form.value.validate()
        if (valid && isPermissaoValid.value)
        {
            if (perfil.value.id == 0)
            {

                await perfilService.create(perfil.value)
            }
            else
                await perfilService.update(perfil.value)

            swal.fire({
                toast: true,
                icon: "success",
                position: "top-end",
                title: "Sucesso!",
                text: "Dados salvos com sucesso!",
                showConfirmButton: false,
                timer: 2000,
            })
            router.push({ name: "perfil" })
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

function reset()
{
    form.value.reset()
}


</script>

<style scoped>
.v-switch {
    height: 30px !important;
    /* margin-bottom: 5px !important; */
}
</style>