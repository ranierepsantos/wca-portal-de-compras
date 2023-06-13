<template>
  <div>
    <Breadcrumbs title="Usuário" :show-button="false" />
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isBusy"
    ></v-progress-linear>
    <v-container class="justify-center">
      <v-card>
        <v-card-text>
          <v-form @submit.prevent="salvar()" ref="userForm">
            <usuario-form :user="usuario" :list-filial="filiais"></usuario-form>
                <v-row style="margin-top: 3px;">
                  <v-col>
                    <v-select
                      label="Perfil"
                      :model-value="getSistemaPerfil(authStore.sistema.id)"
                      :items="listPerfil"
                      item-title="text"
                      item-value="value"
                      variant="outlined"
                      color="primary"
                      :rules="[(v) => !!v || 'Perfil é obrigatório']"
                      density="compact"
                      @update:model-value="setPerfilUsuario($event)"
                    ></v-select>
                  </v-col>
                </v-row>
                
                <box-transfer 
                  :list-origem="clientes" 
                  :list-destino="usuario.cliente"
                  list-origem-titulo = "Selecione os clientes"
                  list-destino-titulo = "Clientes do usuário"
                />

                <box-transfer 
                  :list-origem="tipos" 
                  :list-destino="usuario.tipoFornecimento"
                  list-origem-titulo = "Selecione as categorias"
                  list-destino-titulo = "Categorias do usuário"
                  style="margin-bottom: 5px;"
                />
            
            <v-row style="margin-top: 5px;">
              <v-col class="text-right">
                <v-btn variant="outlined" color="primary" @click="router.go(-1)"
                  >Cancelar</v-btn
                >
                <v-btn color="primary" type="submit" class="ml-3">Salvar</v-btn>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>
      </v-card>
    </v-container>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, inject } from "vue";
import userService from "@/services/user.service";
import perfilService from "@/services/perfil.service";
import filialService from "@/services/filial.service";
import clienteService from "@/services/cliente.service";
import handleErrors from "@/helpers/HandleErrors";
import { useAuthStore } from "@/store/auth.store";
import { useRoute } from "vue-router";
import router from "@/router"
import Breadcrumbs from "@/components/breadcrumbs.vue";
import tipoFornecimentoService from "@/services/tipofornecimento.service";
import usuarioForm from "@/components/usuarioForm.vue";
import boxTransfer from "@/components/boxTransfer.vue";

//DATA
const isBusy = ref(false);
const listPerfil = ref([]);
const filiais = ref([]);
const clientes = ref([]);
const swal = inject("$swal");
const route = useRoute();
const usuario = ref({
  id: 0,
  nome: "",
  email: "",
  ativo: true,
  filialid: null,
  perfilid: null,
  cliente: [],
  tipoFornecimento: [],
  usuarioSistemaPerfil: []
});
let filialUsuario = 0;
const authStore = useAuthStore();
const userForm = ref(null);
const tipos = ref([]);

//VUE METHODS
onMounted(async () => {
  clearData();
  await getFilialToList();
  await getPerfilToList();
  await getTipoFornecimentoToList();
  if (parseInt(route.query.id) > 0) {
    await getUsuario(route.query.id);
  }
  isBusy.value = false;
});

watch(
  () => usuario.value.filialid,
  async (filialid, oldValue) => {
    clientes.value = [];
    await getClienteToList(filialid);
    clientesListRemove();
    if (filialid != filialUsuario) {
      usuario.value.cliente = [];
      filialUsuario = filialid;
    }
  }
);

//METHODS
function setPerfilUsuario(perfilId) {
  let index = -1;
  if ( usuario.value.usuarioSistemaPerfil.length > 0) {
    index = usuario.value.usuarioSistemaPerfil.findIndex(c => c.sistemaId == authStore.sistema.id)
  }
  
  if (index != -1) {
    usuario.value.usuarioSistemaPerfil[index].perfilId = perfilId
  }else {
    usuario.value.usuarioSistemaPerfil.push({
      "sistemaId": authStore.sistema.id,
      "perfilId": perfilId 
    });
  }
}

function clientesListRemove(removerTodos = false) {
  if (removerTodos == true) clientes.value.splice(0, clientes.value.length);
  else {
    usuario.value.cliente.forEach((cliente) => {
      let index = clientes.value.findIndex((c) => c.value == cliente.value);
      if (index > -1) clientes.value.splice(index, 1);
    });
  }
}

function getSistemaPerfil(sistemaId) {
  let perfilUsuario = undefined
  if (usuario.value.usuarioSistemaPerfil != undefined && usuario.value.usuarioSistemaPerfil.length> 0)
    perfilUsuario = usuario.value.usuarioSistemaPerfil.filter(c => c.sistemaId == sistemaId)[0]
  
  return perfilUsuario == undefined? null: perfilUsuario.perfilId;
  
}

async function salvar() {
  try {
    let { valid } = await userForm.value.validate();
    if (valid) {
      let data = usuario.value;
      if (data.id == 0) {
        await userService.create(data);
      } else {
        await userService.update(data);
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
    }
  } catch (error) {
    console.log("usuários.error:", error);
    handleErrors(error);
  }
}

async function clearData() {
  usuario.value = {
    id: 0,
    nome: "",
    email: "",
    ativo: true,
    filialid: authStore.user.filial,
    perfilid: null,
    cliente: [],
    tipoFornecimento: [],
    usuarioSistemaPerfil: []
  };
  filialUsuario = authStore.user.filial;
  await getClienteToList(filialUsuario);
}

async function getClienteToList(filial) {
  try {
    let response = await clienteService.toList(filial);
    clientes.value = response.data;
  } catch (error) {
    console.log("getClienteToList.error:", error);
    handleErrors(error);
  }
}

async function getPerfilToList() {
  try {
    let response = await perfilService.toList();
    listPerfil.value = response.data;
  } catch (error) {
    console.log("getPerfilToList.error:", error);
    handleErrors(error);
  }
}

async function getFilialToList() {
  try {
    let response = await filialService.toList();
    filiais.value = response.data;
  } catch (error) {
    console.log("getFilialToList.error:", error);
    handleErrors(error);
  }
}

async function getTipoFornecimentoToList() {
  try {
    let response = await tipoFornecimentoService.toList();
    tipos.value = response.data;
  } catch (error) {
    console.log("getTipoFornecimentoToList.error:", error);
    handleErrors(error);
  }
}

async function getUsuario(usuarioId) {
  try {
    isBusy.value = true;
    let response = await userService.getById(usuarioId);
    filialUsuario = response.data.filialid;
    usuario.value = response.data;
    tiposListRemove();
    clientesListRemove();
  } catch (error) {
    console.log("getUsuario.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}

function tiposListRemove(removerTodos = false) {
  if (removerTodos == true) tipos.value.splice(0, clientes.value.length);
  else {
    usuario.value.tipoFornecimento.forEach((tipo) => {
      let index = tipos.value.findIndex((c) => c.value == tipo.value);
      if (index > -1) tipos.value.splice(index, 1);
    });
  }
}

</script>

<style scoped>
.v-list {
  height: 145px;
  /* or any height you want */
  overflow-y: auto;
}

.v-icon:hover {
  cursor: pointer;
}
</style>
