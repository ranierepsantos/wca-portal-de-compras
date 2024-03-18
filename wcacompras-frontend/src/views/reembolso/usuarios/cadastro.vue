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
            <usuario-form
              :user="usuario"
              :list-filial="filiais"
              @email-change="searchByEmail"
            ></usuario-form>
            <v-card elevation="2">
              <v-tabs v-model="tab" color="primary" align-tabs="start">
                <v-tab value="1" v-show="authStore.sistema.id == 1"
                  >COMPRAS</v-tab
                >
                <v-tab value="2" v-show="authStore.sistema.id == 2"
                  >REEMBOLSO</v-tab
                >
              </v-tabs>
              <v-card-text>
                <v-window v-model="tab">
                  <v-window-item :value="1" style="padding: 2px" v-show="false">
                    <v-row style="margin-top: 3px">
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
                      list-origem-titulo="Selecione os clientes"
                      list-destino-titulo="Clientes do usuário"
                    />

                    <box-transfer
                      :list-origem="tipos"
                      :list-destino="usuario.tipoFornecimento"
                      list-origem-titulo="Selecione as categorias"
                      list-destino-titulo="Categorias do usuário"
                      style="margin-bottom: 5px"
                    />
                  </v-window-item>

                  <v-window-item :value="2" style="padding: 2px">
                    <v-row style="margin-top: 3px">
                      <v-col>
                        <v-select
                          label="Filial"
                          :model-value="getFilialUsuario()"
                          :items="filiais"
                          item-title="text"
                          item-value="value"
                          variant="outlined"
                          color="primary"
                          :rules="[(v) => !!v || 'Filial é obrigatória']"
                          density="compact"
                          @update:model-value="setFilialUsuario($event)"
                          :disabled="!isMatriz"
                        ></v-select>
                      </v-col>
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
                    <v-row v-show="isColaborador">
                      <v-col>
                        <v-select
                          label="Cliente"
                          :items="clientes"
                          variant="outlined"
                          color="primary"
                          item-title="text"
                          item-value="value"
                          density="compact"
                          v-model="colaboradorClienteId"
                          :rules="
                            isColaborador
                              ? [(v) => !!v || 'Cliente é obrigatório']
                              : []
                          "
                        ></v-select>
                      </v-col>
                      <v-col v-show="isColaborador">
                        <v-select
                          label="Centro de Custo"
                          :items="centrosDeCusto"
                          variant="outlined"
                          color="primary"
                          item-title="nome"
                          item-value="centroCustoId"
                          v-model="
                            usuario.usuarioReembolsoComplemento.centroCustoId
                          "
                          density="compact"
                          :rules="
                            isColaborador
                              ? [(v) => !!v || 'Centro de Custo é obrigatório']
                              : []
                          "
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
                          :rules="
                            isColaborador
                              ? [(v) => !!v || 'Cargo é obrigatório']
                              : []
                          "
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
                          :rules="
                            isColaborador
                              ? [(v) => !!v || 'Gestor é obrigatório']
                              : []
                          "
                        ></v-select>
                      </v-col>
                    </v-row>

                    <box-transfer
                      :list-origem="clientes"
                      :list-destino="usuario.cliente"
                      list-origem-titulo="Selecione os clientes"
                      list-destino-titulo="Clientes do usuário"
                      v-show="!isColaborador"
                    />
                    <br />
                    <box-transfer
                      :list-origem="centros"
                      :list-destino="usuario.centroCusto"
                      list-origem-titulo="Selecione o(s) Centro(s) de Custo"
                      list-destino-titulo="Centro(s) de Custo do usuário"
                      v-show="!isColaborador"
                    />
                  </v-window-item>
                </v-window>
              </v-card-text>
            </v-card>

            <v-row style="margin-top: 5px">
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
import { ref, onMounted, watch, inject, computed } from "vue";
import perfilService from "@/services/perfil.service";
import handleErrors from "@/helpers/HandleErrors";
import { useAuthStore } from "@/store/auth.store";
import { useRoute } from "vue-router";
import router from "@/router";
import Breadcrumbs from "@/components/breadcrumbs.vue";
import usuarioForm from "@/components/usuarioForm.vue";
import boxTransfer from "@/components/boxTransfer.vue";
import {
  Usuario,
  useUsuarioStore,
  IDPERFILGESTOR,
  IDPERFILCOLABORADOR,
} from "@/store/reembolso/usuario.store";
import { useClienteStore } from "@/store/reembolso/cliente.store";
import filialService from "@/services/filial.service";

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
const authStore = useAuthStore();
const userForm = ref(null);
const tipos = ref([]);
const usuarioStore = useUsuarioStore();
const colaboradorClienteId = ref(null);
const isMatriz = ref(false);
const centrosDeCusto = ref([]);
const centros = ref([]);
const clientesUsuarios = ref([])

//VUE METHODS
onMounted(async () => {
  tab.value = authStore.sistema.id;
  let filiaisUsuario = await usuarioStore.getFiliais();
  if (filiaisUsuario.length > 0) {
    isMatriz.value = filiaisUsuario[0].text.toLowerCase() == "matriz";
    authStore.user.filial = filiaisUsuario[0].value;
  }
  await getFilialToList();
  await getPerfilToList();
  clearData();
  if (parseInt(route.query.id) > 0) {
    await getUsuario(route.query.id);
  }
  isBusy.value = false;
});

//quando perfil = COLABORADOR
watch(
  () => colaboradorClienteId.value,
  async (clienteId, oldClienteId) => {
    let cliente = clientes.value.filter((q) => q.value == clienteId)[0];
    if (cliente) {
      usuario.value.cliente = [];
      if (oldClienteId && oldClienteId != clienteId) {
        usuario.value.usuarioReembolsoComplemento.gestorId = null;
        usuario.value.usuarioReembolsoComplemento.centroCustoId = null;
      }
      centrosDeCusto.value = cliente.centroCusto;
      usuario.value.cliente.push(cliente);
    }
    await getGestorToList(clienteId);
  }
);

watch(
  () => usuario.value.cliente,
  async (newClientes) => {
    if (isGestor) {
      if (newClientes.length > 0) {
        let objA = JSON.parse(JSON.stringify(newClientes));
        if (objA.length > 0) objA.forEach((e) => delete e.selected);
        let objB = JSON.parse(JSON.stringify(clientesUsuarios.value));
        if (objB.length > 0) objB.forEach((e) => delete e.selected);

        if (JSON.stringify(objA) !== JSON.stringify(objB)) {
          //carregar os centros de custos
          centros.value = await useClienteStore().ListCentrosDeCusto(newClientes.map((p) => { return p.value;}));
          
          //remover o que estiver no usuario
          centroCustoListRemove();

          //pega a lista de id dos clientes selecionados
          let listIds = newClientes.map((p) =>{ return p.value})
          console.log("clientes selecionados: ",listIds)

          //pega a lista de ids que foram removidos
          let listToRemove = clientesUsuarios.value.length > 0 ? clientesUsuarios.value.filter(p => !listIds.includes(p.value)):[]
          console.log("clientes removidos: ",listToRemove)

          // remover os centros de custo do cliente removido da lista do usuario
          usuarioRemoveCentroCusto(listToRemove.map(p => {return p.value}))
          
          clientesUsuarios.value = JSON.parse(JSON.stringify(newClientes));
        }
      } else {
        centros.value = [];
        usuarioRemoveCentroCusto(clientesUsuarios.value.map(p => {return p.value}))
        clientesUsuarios.value =[]
      }
    }
  },
  { deep: true }
);

const isColaborador = computed(() => {
  if (usuario.value.usuarioSistemaPerfil.length > 0)
    return (
      usuario.value.usuarioSistemaPerfil[0].perfilId == IDPERFILCOLABORADOR
    );
  else {
    return false;
  }
});

const isGestor = computed(() => {
  if (usuario.value.usuarioSistemaPerfil.length > 0)
    return usuario.value.usuarioSistemaPerfil[0].perfilId == IDPERFILGESTOR;
  else {
    return false;
  }
});

//METHODS
function getFilialUsuario() {
  if (usuario.value.filial.length > 0) {
    return usuario.value.filial[0].value;
  }
  return null;
}

async function setFilialUsuario(filialId) {
  let _filial = filiais.value.find((q) => q.value == filialId);
  if (_filial) {
    usuario.value.filial = [];
    usuario.value.filial.push(_filial);
    usuario.value.cliente = [];
    colaboradorClienteId.value = null;
    await getClienteToList(_filial.value);
  }
}

async function setPerfilUsuario(perfilId) {
  let index = -1;
  if (usuario.value.usuarioSistemaPerfil.length > 0) {
    index = usuario.value.usuarioSistemaPerfil.findIndex(
      (c) => c.sistemaId == authStore.sistema.id
    );
  }

  if (index != -1) {
    usuario.value.usuarioSistemaPerfil[index].perfilId = perfilId;
  } else {
    usuario.value.usuarioSistemaPerfil.push({
      sistemaId: authStore.sistema.id,
      perfilId: perfilId,
    });
  }
  if (perfilId == IDPERFILCOLABORADOR || perfilId == IDPERFILGESTOR) {
    usuario.value.cliente = [];
    await getClienteToList(getFilialUsuario());
  }

  if (perfilId != IDPERFILCOLABORADOR) {
    colaboradorClienteId.value = null;
    gestores.value = [];
  }
  clientesListRemove();
}

function getSistemaPerfil(sistemaId) {
  let perfilUsuario = undefined;
  if (
    usuario.value.usuarioSistemaPerfil != undefined &&
    usuario.value.usuarioSistemaPerfil.length > 0
  )
    perfilUsuario = usuario.value.usuarioSistemaPerfil.filter(
      (c) => c.sistemaId == sistemaId
    )[0];

  return perfilUsuario == undefined ? null : perfilUsuario.perfilId;
}

async function salvar() {
  try {
    let { valid } = await userForm.value.validate();
    if (valid) {
      isSaving.value = true;
      let data = {...usuario.value};
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
      router.push({ name: "reembolsoUsuarios" });
    }
  } catch (error) {
    console.log("usuários.error:", error);
    handleErrors(error);
  } finally {
    isSaving.value = false;
  }
}

async function clearData() {
  usuario.value = new Usuario();
  clientesUsuarios.value = []
  if (!isMatriz.value) setFilialUsuario(authStore.user.filial);
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

function centroCustoListRemove(removerTodos = false) {
  if (removerTodos == true) centros.value.splice(0, centros.value.length);
  else {
    usuario.value.centroCusto.forEach((cc) => {
      let index = centros.value.findIndex((c) => c.value == cc.value);
      if (index > -1) centros.value.splice(index, 1);
    });
  }
}

function usuarioRemoveCentroCusto(clientesToRemove = []) {
  let removeList = usuario.value.centroCusto.filter(q =>  clientesToRemove.includes(q.clienteId));
  removeList.forEach(r => {
    let index = usuario.value.centroCusto.findIndex(q =>  q.value == r.value);
    usuario.value.centroCusto.splice(index, 1);
  })
}

async function getClienteToList(filial) {
  try {
    clientes.value = await useClienteStore().toComboList(filial);
  } catch (error) {
    console.log("getClienteToList.error:", error);
    handleErrors(error);
  }
}

async function getGestorToList(clienteId) {
  try {
    gestores.value = await usuarioStore.reembolsoToListByClientePerfil(
      clienteId,
      IDPERFILGESTOR
    );
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
    await getClienteToList(getFilialUsuario());

    if (
      usuario.value.usuarioSistemaPerfil[0].perfilId == IDPERFILCOLABORADOR &&
      usuario.value.cliente.length > 0
    ) {
      colaboradorClienteId.value = usuario.value.cliente[0].value;
    } else {
      clientesListRemove();
      centroCustoListRemove();
    }
  } catch (error) {
    console.log("getUsuario.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}

function searchByEmail(email) {
  console.log(email);
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
