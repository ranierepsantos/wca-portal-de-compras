<template>
  <div>
    <Breadcrumbs title="Usuário" :show-button="false" />
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isBusy || isSaving"
    ></v-progress-linear>
    <v-container class="justify-center">
      <v-card v-show="!isBusy">
        <v-card-text>
          <v-form @submit.prevent="salvar()" ref="userForm">
            <usuario-form :user="usuario" :list-filial="filiais"></usuario-form>
            <v-card elevation="2">
              <v-tabs v-model="tab" color="primary" align-tabs="start">
                <v-tab value="1" v-show="authStore.sistema.id == 1">COMPRAS</v-tab>
                <v-tab value="2" v-show="authStore.sistema.id ==2">REEMBOLSO</v-tab>
              </v-tabs>
              <v-card-text>
                <v-window v-model="tab">
                  <v-window-item :value="1" 
                    style="padding: 2px" 
                    v-show="false">
                    <v-row style="margin-top: 3px;">
                      <v-col>
                        <v-select
                          label="Perfil"
                          :model-value="getSistemaPerfil(1)"
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

                  </v-window-item>

                  <v-window-item :value="2" style="padding: 2px">
                    <v-row style="margin-top: 3px;">
                      <v-col>
                        <v-select
                          label="Perfil"
                          :model-value="getSistemaPerfil(2)"
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
                    <v-row v-show="isColaborador || isGestor">
                      <v-col>
                        <v-select
                          label="Cliente"
                          :items="clientes"
                          variant="outlined"
                          color="primary"
                          item-title="text"
                          item-value="value"
                          density="compact"
                          v-model ="colaboradorClienteId"
                        ></v-select>
                      </v-col>
                    </v-row>
                    <v-row v-show="isColaborador">
                      <v-col>
                        <v-text-field
                          label="Cargo"
                          v-model="usuario.usuarioReembolsoComplemento.cargo"
                          type="text"
                          variant="outlined"
                          color="primary"
                          density="compact"
                        ></v-text-field>
                      </v-col>
                      <v-col>
                        <v-select
                          label="Gestor"
                          :items="gestores"
                          variant="outlined"
                          color="primary"
                          item-title="text"
                          item-value="value"
                          v-model="usuario.usuarioReembolsoComplemento.gestorId"
                          density="compact"
                        ></v-select>
                      </v-col>
                    </v-row>
                    
                    <box-transfer 
                      :list-origem="clientes" 
                      :list-destino="usuario.cliente"
                      list-origem-titulo = "Selecione os clientes"
                      list-destino-titulo = "Clientes do usuário"
                      v-show="!isColaborador && !isGestor"
                    />

                  </v-window-item>
                </v-window>
              </v-card-text>
            </v-card>

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
import { ref, onMounted, watch, inject,computed } from "vue";
import perfilService from "@/services/perfil.service";
import filialService from "@/services/filial.service";
import handleErrors from "@/helpers/HandleErrors";
import { useAuthStore } from "@/store/auth.store";
import { useRoute } from "vue-router";
import router from "@/router"
import Breadcrumbs from "@/components/breadcrumbs.vue";
import usuarioForm from "@/components/usuarioForm.vue";
import boxTransfer from "@/components/boxTransfer.vue";
import { Usuario, useUsuarioStore, IDPERFILGESTOR, IDPERFILCOLABORADOR } from "@/store/reembolso/usuario.store";
import { useClienteStore } from "@/store/reembolso/cliente.store";

//DATA
const isBusy = ref(true);
const isSaving = ref(false);
const listPerfil = ref([]);
const filiais = ref([]);
const clientes = ref([]);
const gestores = ref([]);
const swal = inject("$swal");
const route = useRoute();
const tab = ref(null);
const usuario = ref(new Usuario());
let filialUsuario = 0;
const authStore = useAuthStore();
const userForm = ref(null);
const tipos = ref([]);
const usuarioStore = useUsuarioStore();
const colaboradorClienteId = ref(null)

//VUE METHODS
onMounted(async () => {
  tab.value = authStore.sistema.id
  clearData();
  await getFilialToList();
  await getPerfilToList();
  if (parseInt(route.query.id) > 0) {
    await getUsuario(route.query.id);
  }
  isBusy.value = false;
});

watch(
  () => usuario.value.filialId,
  async (filialid, oldValue) => {
    if (filialid != filialUsuario) {
      usuario.value.cliente = [];
      filialUsuario = filialid;
    }
    await getClienteToList(filialUsuario)
  }
);
watch(() => colaboradorClienteId.value, async(clienteId) => {
  
  let cliente = clientes.value.filter(q => q.value== clienteId)[0]
  if (cliente) 
  {
    usuario.value.cliente = []
    usuario.value.cliente.push(cliente)
  }
  await getGestorToList(clienteId)
})

const isColaborador = computed(() => {
  if (usuario.value.usuarioSistemaPerfil.length >0) 
    return usuario.value.usuarioSistemaPerfil[0].perfilId == IDPERFILCOLABORADOR
  else  {
    return false
  }
})

const isGestor = computed(() => {
  if (usuario.value.usuarioSistemaPerfil.length >0) 
    return usuario.value.usuarioSistemaPerfil[0].perfilId == IDPERFILGESTOR
  else  {
    return false
  }
    
})

//METHODS
async function setPerfilUsuario(perfilId) {
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
  if (perfilId == IDPERFILCOLABORADOR || perfilId == IDPERFILGESTOR) { 
    usuario.value.cliente = []
    await getClienteToList(filialUsuario)
  }
  
  if (perfilId != IDPERFILCOLABORADOR) {
    colaboradorClienteId.value = null
    gestores.value = []
  }
  clientesListRemove()
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
      isSaving.value = true;
      let data = usuario.value;
      if (data.id == 0) {
        await usuarioStore.add(data);
      } else {
        await usuarioStore.update(data);
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
      router.push({name: "reembolsoUsuarios"})
    }
  } catch (error) {
    console.log("usuários.error:", error);
    handleErrors(error);
  } finally { isSaving.value = false;}
}

async function clearData() {
  usuario.value = new Usuario();
  filialUsuario = authStore.user.filial;
  usuario.value.filialId = filialUsuario
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

async function getClienteToList(filial) {
  try {
    clientes.value = await useClienteStore().toComboList(filial)
    console.log("getClienteToList",clientes.value);
  } catch (error) {
    console.log("getClienteToList.error:", error);
    handleErrors(error);
  }
}

async function getGestorToList(clienteId) {
  try {
    gestores.value = await usuarioStore.reembolsoToListByClientePerfil(clienteId, IDPERFILGESTOR);
  } catch (error) {
    console.log("getGestorToList.error:", error);
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

async function getUsuario(usuarioId) {
  try {
    isBusy.value = true;
    usuario.value = await usuarioStore.getById(usuarioId);
    filialUsuario = usuario.value.filialId;
    if (usuario.value.usuarioSistemaPerfil[0].perfilId == IDPERFILCOLABORADOR ||
        usuario.value.usuarioSistemaPerfil[0].perfilId == IDPERFILGESTOR) {
      colaboradorClienteId.value = usuario.value.cliente[0].value
    }else {
      clientesListRemove()
    }
    
  } catch (error) {
    console.log("getUsuario.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
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
