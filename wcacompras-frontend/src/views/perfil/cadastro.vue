<template>
  <div>
    <bread-crumbs title="Perfil" :show-button="false" />
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isBusy"
    ></v-progress-linear>
    <v-container class="justify-center">
      <v-card class="mx-auto" max-width="980">
        <v-card-text>
          <v-form ref="form" v-model="valid" lazy-validation>
            <v-text-field
              density="compact"
              variant="outlined"
              v-model="perfil.nome"
              :counter="50"
              :rules="nameRules"
              label="Perfil"
              required
              :readonly="isBlocked"
              :bg-color="isBlocked ? '#f2f2f2' : ''"
            >
            </v-text-field>

            <v-textarea
              v-model="perfil.descricao"
              label="Descrição"
              variant="outlined"
              density="compact"
            >
            </v-textarea>

            <v-checkbox
              v-model="perfil.ativo"
              color="primary"
              label="Ativo?"
              density="compact"
              v-show="perfil.id > 0"
            >
            </v-checkbox>
            <v-expansion-panels v-model="panel" multiple>
              <v-expansion-panel >
                <v-expansion-panel-title>
                    <h2 class="text-primary">Permissões</h2>
                    &nbsp;&nbsp;
                    <small class="text-error" v-show="!isPermissaoValid">
                        Selecione pelo menos 1 permissão
                    </small>
                </v-expansion-panel-title>
                <v-expansion-panel-text>
                  <div v-for="permissao in permissoesList" :key="permissao.id">
                    <v-switch
                      v-model="perfil.permissao"
                      color="primary"
                      :value="{
                        id: permissao.id,
                        nome: permissao.nome,
                        regra: permissao.regra,
                      }"
                      :hide-details="true"
                    >
                      <template v-slot:label>
                        {{ permissao.nome }}
                        &nbsp;-&nbsp;
                        <small>{{ permissao.descricao }}</small>
                      </template>
                    </v-switch>
                    <v-divider class="mt-5"></v-divider>
                  </div>
                </v-expansion-panel-text>
              </v-expansion-panel>
              
              <v-expansion-panel v-show="authStore.sistema.id == 2">
                    <v-expansion-panel-title>
                        <h2 class="text-primary">Tipos de Despesas do Perfil</h2>
                    </v-expansion-panel-title>
                    <v-expansion-panel-text>
                    <box-transfer
                      :list-origem="listTipoDespesa"
                      :list-destino="perfil.tiposDespesa"
                      list-origem-titulo="Selecione os tipos de despesa"
                      list-destino-titulo="Tipos de Despesa do Perfil"
                    />
                    </v-expansion-panel-text>
                </v-expansion-panel>
              
            </v-expansion-panels>
          </v-form>
        </v-card-text>
        <v-card-text class="text-right">
          <v-btn
            color="primary"
            variant="outlined"
            class="mr-4"
            @click="router.go(-1)"
          >
            Cancelar
          </v-btn>
          <v-btn color="primary" class="mr-4" @click="salvar"> Salvar </v-btn>
        </v-card-text>
      </v-card>
    </v-container>
  </div>
</template>

<script setup>
import { onMounted, ref, inject, computed } from "vue";
import perfilService from "@/services/perfil.service";
import handleErrors from "../../helpers/HandleErrors";
import { useRoute } from "vue-router";
import breadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import { useAuthStore } from "@/store/auth.store";
import {
  IDPERFILGESTOR,
  IDPERFILCOLABORADOR,
} from "@/store/reembolso/usuario.store";
import boxTransfer from "@/components/boxTransfer.vue";
import { useDespesaTipoStore } from "@/store/reembolso/despesaTipo.store";
// VARIABLES
const route = useRoute();
const authStore = useAuthStore();
const despesaTipoStore = useDespesaTipoStore();
const swal = inject("$swal");
const valid = ref(true);
const isBusy = ref(false);
const form = ref(null);
const nameRules = ref([
  (v) => !!v || "Perfil é obrigatório",
  (v) => (v && v.length <= 50) || "Perfil deve ter até 50 caracteres",
]);
const permissoesList = ref([]);
const isPermissaoValid = ref(true);
const perfil = ref({
  id: 0,
  nome: "",
  descricao: "",
  ativo: true,
  sistemaId: authStore.sistema.id,
  permissao: [],
});
let routeName = "";
const panel = ref([0,1]);
const listTipoDespesa = ref([])
//VUE METHODS
onMounted(async () => {
  
  await getPermissoes();
  let tiposDespesa = await despesaTipoStore.toComboList(false);
  listTipoDespesa.value = tiposDespesa.map(m => ({value: m.id, text: m.nome}))

  if (parseInt(route.query.id) > 0) {
    await getPerfil(route.query.id);
  }
  
  //set routeName to Perfil
  if (authStore.sistema.id == 1)
    //compras
    routeName = "perfil";
  else if (authStore.sistema.id == 2)
    //reembolso
    routeName = "reembolsoPerfil";
  else if (authStore.sistema.id == 3)
    //share
    routeName = "sharePerfil";
});

const isBlocked = computed(() => {
  return (
    perfil.value.id == IDPERFILCOLABORADOR || perfil.value.id == IDPERFILGESTOR
  );
});

// METHODS
async function getPerfil(perfilId) {
  try {
    isBusy.value = true;
    let response = await perfilService.getWithPermissions(perfilId);
    let data = response.data;
    
    //reembolso - carregar os tipos de despesas relacionados ao perfil
    if (authStore.sistema.id ==2) {
        let list  = await despesaTipoStore.getListByPerfil(data.id);
        data.tiposDespesa = list.map(m => ({value: m.id , text: m.nome}))
    }
    perfil.value = data;
    tipoDespesaListRemove()
  } catch (error) {
    console.log("getPerfil.error", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}

async function getPermissoes() {
  try {
    isBusy.value = true;
    let response = await perfilService.permissaoAll();
    permissoesList.value = response.data;
  } catch (error) {
    console.log("getPermissoes.error", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}

async function salvar() {
  try {
    isBusy.value = true;
    isPermissaoValid.value = perfil.value.permissao.length > 0;
    const { valid } = await form.value.validate();
    if (valid && isPermissaoValid.value) {
      
      let tiposDespesa = perfil.value.tiposDespesa || []

      if (perfil.value.id == 0)
      {
           let response =  await perfilService.create(perfil.value);       
           perfil.value.id = response.data.id;
      }else {
            await perfilService.update(perfil.value);
      }

      if (authStore.sistema.id == 2) {
        //REEMBOLSO
        let perfilTiposDespesa = {
            perfilId: perfil.value.id,
            tipoDespesaIds: tiposDespesa.map(m => m.value)
        }
        await despesaTipoStore.relateToProfile(perfilTiposDespesa)
      }
    
      swal.fire({
        toast: true,
        icon: "success",
        position: "top-end",
        title: "Sucesso!",
        text: "Dados salvos com sucesso!",
        showConfirmButton: false,
        timer: 2000,
      });
      router.push({ name: routeName });
    }
  } catch (error) {
    console.log("perfil.cadastro.salvar.erro", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}

function reset() {
  form.value.reset();
}

function tipoDespesaListRemove(removerTodos = false) {
  if (authStore.sistema.id !== 2) return
  if (removerTodos == true) listTipoDespesa.value.splice(0, listTipoDespesa.value.length);
  else {
    perfil.value.tiposDespesa.forEach((cc) => {
      let index = listTipoDespesa.value.findIndex((c) => c.value == cc.value);
      if (index > -1) listTipoDespesa.value.splice(index, 1);
    });
  }
}
</script>

<style scoped>
.v-switch {
  height: 30px !important;
  /* margin-bottom: 5px !important; */
}
</style>
