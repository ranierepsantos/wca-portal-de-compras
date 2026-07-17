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
            <v-row class="mt-3">
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
              :list-origem="filiais"
              :list-destino="usuario.filial"
              list-origem-titulo="Selecione a filial"
              list-destino-titulo="Filiais do usuário"
            />
            <box-transfer
              :list-origem="clientes"
              :list-destino="usuario.cliente"
              list-origem-titulo="Selecione os clientes"
              list-destino-titulo="Clientes do usuário"
            />

            <box-transfer
              :list-origem="tipos"
              :list-destino="usuario.tipoFornecimento"
              list-origem-titulo="Selecione as categorias de compra"
              list-destino-titulo="Categorias de compra do usuário"
              class="mb-5"
            />

            <v-row class="mt-5">
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
import {
  ref,
  onMounted,
  watch,
  inject,
  computed,
} from "vue";
import userService from "@/services/user.service";
import perfilService from "@/services/perfil.service";
import filialService from "@/services/filial.service";
import clienteService from "@/services/cliente.service";
import handleErrors from "@/helpers/HandleErrors";
import { useAuthStore } from "@/store/auth.store";
import { useRoute } from "vue-router";
import router from "@/router";
import Breadcrumbs from "@/components/breadcrumbs.vue";
import tipoFornecimentoService from "@/services/tipofornecimento.service";
import usuarioForm from "@/components/usuarioForm.vue";
import boxTransfer from "@/components/boxTransfer.vue";
import { Usuario } from "@/store/reembolso/usuario.store";

//DATA
const isBusy = ref(false);
const listPerfil = ref([]);
const filiais = ref([]);
const clientes = ref([]);
const swal = inject("$swal");
const route = useRoute();
const usuario = ref(new Usuario());
let filialUsuario = [];
const authStore = useAuthStore();
const userForm = ref(null);
const tipos = ref([]);

let filiaisIdAnteriores = [];
let requestId = 0;

//VUE METHODS
onMounted(async () => {
  clearData();
  await Promise.all([
    getFilialToList(),
    getPerfilToList(),
    getTipoFornecimentoToList(),
  ]);
  if (parseInt(route.query.id) > 0) {
    await getUsuario(route.query.id);
  }
  isBusy.value = false;
});

//COMPUTED
const userFilialIds = computed(() =>
  (usuario.value.filial || []).map((f) => f.value).sort(),
);

// watch(
//   () => usuario.value.filial,
//   async (newfilials) => {
//     if (newfilials.length > 0) {
//       let objA = JSON.parse(JSON.stringify(newfilials));
//       if (objA.length > 0) objA.forEach((e) => delete e.selected);
//       let objB = JSON.parse(JSON.stringify(filialUsuario));
//       if (objB.length > 0) objB.forEach((e) => delete e.selected);

//       if (JSON.stringify(objA) !== JSON.stringify(objB)) {
//         clientes.value = [];
//         await getClienteToList(
//           newfilials.map((p) => {
//             return p.value;
//           }),
//         );
//         clientes.value = removeFromList(clientes.value, new Set(usuario.value.cliente.map((t) => t.value)));
//         let listIds = newfilials.map((p) => {
//           return p.value;
//         });

//         let remove =
//           filialUsuario.length > 0
//             ? filialUsuario.filter((p) => !listIds.includes(p.value))
//             : [];

//         usuario.value.cliente = removeFromList(usuario.value.cliente, new Set(remove.map((p) => p.value)))
//         filialUsuario = JSON.parse(JSON.stringify(newfilials));
//       }
//     } else {
//       clientes.value = [];
//       usuario.value.cliente = removeFromList(usuario.value.cliente, new Set(filiais.value.map((q) => q.value)))
//     }
//   },
//   { deep: true },
// );

watch(userFilialIds, async (novosId) => {
  const currentRequest = ++requestId;

  if (novosId.length === 0) {
    clientes.value = [];
    usuario.value.cliente = [];
    filiaisIdAnteriores = [];
    return;
  }

  clientes.value = [];
  await getClienteToList(novosId);

  const removidos = filiaisIdAnteriores.filter((id) => !novosId.includes(id));

  const clientesRemovidos = new Set(usuario.value.cliente.filter((c) => removidos.includes(c.filialId)).map((c) => c.value) );

  usuario.value.cliente = removeFromList(
    usuario.value.cliente,
    new Set(clientesRemovidos)
  );

  // Descarta se outra mudança de filial já foi disparada nesse meio tempo
  if (currentRequest !== requestId) return;

  clientes.value = removeFromList(
    clientes.value,
    new Set(usuario.value.cliente.map((u) => u.value)),
  );
  filiaisIdAnteriores = novosId;
});



//METHODS
function setPerfilUsuario(perfilId) {
  let index = -1;
  if (usuario.value.usuarioSistemaPerfil.length > 0) {
    index = usuario.value.usuarioSistemaPerfil.findIndex(
      (c) => c.sistemaId == authStore.sistema.id,
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
}

function getSistemaPerfil(sistemaId) {
  let perfilUsuario = undefined;
  if (
    usuario.value.usuarioSistemaPerfil != undefined &&
    usuario.value.usuarioSistemaPerfil.length > 0
  )
    perfilUsuario = usuario.value.usuarioSistemaPerfil.filter(
      (c) => c.sistemaId == sistemaId,
    )[0];

  return perfilUsuario == undefined ? null : perfilUsuario.perfilId;
}

async function salvar() {
  try {
    if (isBusy.value) return;

    let { valid } = await userForm.value.validate();
    if (valid) {
      isBusy.value = true;
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
      router.push({ name: "comprasUsuarios" });
    }
  } catch (error) {
    console.log("usuários.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}

function clearData() {
  usuario.value = new Usuario();
  filialUsuario = [];
  getClienteToList(filialUsuario);
}

async function getClienteToList(filial) {
  try {
    if (filial.length > 0) {
      let response = await clienteService.toList(filial);
      clientes.value = response.data;
    }
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
    if (isBusy.value) return;

    isBusy.value = true;
    let response = await userService.getById(usuarioId);
    usuario.value = response.data;
    tipos.value = removeFromList(
      tipos.value,
      new Set(usuario.value.tipoFornecimento.map((t) => t.value)),
    );
    clientes.value = removeFromList(
      clientes.value,
      new Set(usuario.value.cliente.map((t) => t.value)),
    );
    filiais.value = removeFromList(
      filiais.value,
      new Set(usuario.value.filial.map((t) => t.value)),
    );
  } catch (error) {
    console.log("getUsuario.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}

function removeFromList(list, selecteds, removeAll = false) {
  if (removeAll) return list.splice(0, list.length);

  return list.filter((item) => !selecteds.has(item.value));
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
