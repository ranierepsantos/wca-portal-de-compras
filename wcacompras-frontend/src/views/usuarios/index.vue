<template>
  <div>
    <v-breadcrumbs>
      <template v-slot:prepend>
        <v-icon size="large" icon="mdi-arrow-left" color="primary"></v-icon>
      </template>
      <div class="text-h4 text-primary">Usuários</div>
      <v-spacer></v-spacer>
      
      <v-btn color="primary" variant="outlined" class="text-capitalize">
        <b>Novo</b>

        <v-dialog
          v-model="dialog"
          activator="parent"
          max-width="700"
          :absolute="false"
        >
          <v-card>
            <v-card-title class="text-primary text-h5">
              {{ dialogTitle }}
            </v-card-title>
            <v-card-text>
              <v-form @submit.prevent="salvar()" ref="userForm">
                <v-row>
                  <v-col>
                    <v-text-field
                      label="Nome"
                      v-model="usuario.nome"
                      type="text"
                      required
                      variant="outlined"
                      color="primary"
                      :rules="[(v) => !!v || 'Nome é obrigatório']"
                    ></v-text-field>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-text-field
                      label="Email"
                      v-model="usuario.email"
                      type="email"
                      required
                      variant="outlined"
                      color="primary"
                      :rules="emailRules"
                    ></v-text-field>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-select
                      label="Perfil"
                      v-model="usuario.perfilid"
                      :items="listPerfil"
                      item-title="text"
                      item-value="value"
                      variant="outlined"
                      color="primary"
                      :rules="[(v) => !!v || 'Perfil é obrigatório']"
                    ></v-select>
                  </v-col>
                  <v-col>
                    <v-select
                      label="Filial"
                      :items="filiais"
                      variant="outlined"
                      color="primary"
                      item-title="text"
                      item-value="value"
                      v-model="usuario.filialid"
                      :disabled="(authStore.user.filial != 1)"
                      :rules="[(v) => !!v || 'Filial é obrigatório']"
                    ></v-select>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="6">
                    <v-select
                      label="Cliente"
                      disabled
                      
                      :items="[
                        'California',
                        'Colorado',
                        'Florida',
                        'Georgia',
                        'Texas',
                        'Wyoming',
                      ]"
                      variant="outlined"
                      color="primary"
                      :rules="[(v) => !!v || 'Cliente é obrigatório']"
                    ></v-select>
                  </v-col>
                  <v-col cols="6">
                    <v-checkbox
                      v-show="usuario.id > 0"
                      v-model="usuario.ativo"
                      label="Ativo"
                      color="primary"
                    ></v-checkbox>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col class="text-right">
                    <v-btn
                      variant="outlined"
                      color="primary"
                      @click="closeDialog()"
                      >Cancelar</v-btn
                    >
                    <v-btn color="primary" type="submit" class="ml-3"
                      >Salvar</v-btn
                    >
                  </v-col>
                </v-row>
              </v-form>
            </v-card-text>
            <v-card-actions> </v-card-actions>
          </v-card>
        </v-dialog>
      </v-btn>
    </v-breadcrumbs>
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isBusy"
    ></v-progress-linear>
    <v-table class="elevation-2">
      <thead>
        <tr>
          <th class="text-left text-grey">NOME</th>
          <th class="text-left text-grey">PERFIL</th>
          <th class="text-center text-grey">ATIVO</th>
          <th></th>
        </tr>
      </thead>
      <!-- <tbody v-if="isBusy">
        <tr class="text-center">
          <td colspan="4">
            <v-progress-circular
              color="primary"
              indeterminate
              :size="40"
            ></v-progress-circular>
          </td>
        </tr>
      </tbody> -->

      <tbody>
        <tr v-for="user in users" :key="user.id">
          <td class="text-left">
            <v-icon icon="mdi-account-circle-outline"></v-icon>
            &nbsp;{{ user.nome }}
          </td>
          <td class="text-left">{{ getPerfilName(user.perfilid) }}</td>
          <td class="text-center">
            <v-icon
              :icon="user.ativo ? 'mdi-check':'mdi-close'"
              variant="plain"
              :color="user.ativo ? 'success':'error'"
            ></v-icon>
          </td>
          <td class="text-right">
            <v-btn
              icon="mdi-lead-pencil"
              variant="plain"
              color="primary"
              @click="editar(user)"
            ></v-btn>
            <v-btn
              icon="mdi-delete"
              variant="plain"
              color="error"
              @click="remove(user)"
            ></v-btn>
          </td>
        </tr>
      </tbody>
      <tfoot>
        <tr>
          <td colspan="4">
            <v-pagination
              v-model="page"
              :length="totalPages"
              :total-visible="4"
            ></v-pagination>
          </td>
        </tr>
      </tfoot>
    </v-table>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, inject } from "vue";
import userService from "@/services/user.service";
import perfilService from "@/services/perfil.service";
import filialService from "@/services/filial.service";
import handleErrors from "@/helpers/HandleErrors"
import { useAuthStore } from "@/store/auth.store";

//DATA
const page = ref(1);
const pageSize = 5;
const isBusy = ref(false);
const totalPages = ref(1);
const users = ref([]);
const listPerfil = ref([]);
const filiais = ref([]);
const dialogTitle = ref("Novo Usuário");
const dialog = ref(false);
const swal = inject("$swal");
const usuario = ref({
  id: 0,
  nome: "",
  email: "",
  ativo: true,
  clienteid: null,
  filialid: null,
  perfilid: null,
});
const authStore = useAuthStore();
const emailRules = ref([
  (v) => !!v || "E-mail é obrigatório",
  (v) => /.+@.+\..+/.test(v) || "E-mail deve ser válido",
]);
const userForm = ref(null)

//VUE METHODS
onMounted(async () => {
  clearData();
  await getListFilial();
  await getListPerfil();
  await getItems();
});

watch(page, () => {
  getItems();
});

//METHODS
async function remove(item) {
  try {
    let options = {
      title: "Confirma Exclusão?",
      text: "Deseja realmente excluir o usuário: " + item.nome + "?",
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Sim",
      cancelButtonText: "Não",
    }

    let response = await swal.fire(options);
    if (response.isConfirmed)
    {
      await userService.remove(item.id);
      debugger
      if (users.value.length == 1)
      {
        page.value --;
      } else
      {
        await this.getItems()  
      }
      
      swal.fire({
        toast: true,
        icon: "success",
        position: "top-end",
        title: "Sucesso!",
        text: "Exclusão realizada!",
        showConfirmButton: false,
        timer: 2000,
      })
    }
  } catch (error) {
    console.log("usuários.error:",error);
    handleErrors(error)
  }
  
}
function editar(item) {
  usuario.value = { ...item };
  dialogTitle.value = "Edição de Usuário";
  dialog.value = true;
}

function closeDialog() {
  dialog.value = false;
  userForm.value.reset()
  clearData();
}

async function salvar() {
  try
  {
   let { valid } = await userForm.value.validate();
    if (valid)
    {
      let data = usuario.value;
      data.clienteid = data.perfilid;
      data.filialid = data.perfilid;
      if (data.id == 0)
      {
        await userService.create(data);
      } else
      {
        await userService.update(data);
      }
      await getItems();
      closeDialog();
      swal.fire({
          toast: true,
          icon: "success",
          position: "top-end",
          title: "Sucesso!",
          text: "Dados salvos com sucesso!",
          showConfirmButton: false,
          timer: 2000,
      })
      
    }
  } catch (error)
  {
    console.log("usuários.error:",error);
    handleErrors(error)
  }
}

function clearData() {
  dialogTitle.value = "Novo Usuário";
  usuario.value = {
    id: 0,
    nome: "",
    email: "",
    ativo: true,
    clienteid: null,
    filialid: authStore.user.filial,
    perfilid: null,
  };
}

function getPerfilName(perfilId) {
  let perfil = listPerfil.value.filter((p) => p.value == perfilId)[0];
  return perfil.text;
}

async function getListPerfil() {
  try {
    let response = await perfilService.toList();
    listPerfil.value = response.data;
  } catch (error) {
    console.log("getListPerfil.error:",error);
    handleErrors(error)
  }
}

async function getListFilial() {
  try {
    let response = await filialService.toList();
    filiais.value = response.data;
  } catch (error) {
    console.log("getListFilial.error:",error);
    handleErrors(error)
  }
}

async function getItems() {
  try {
    isBusy.value = true;
    let response = await userService.paginate(pageSize, page.value, "");
    users.value = response.data.items;
    totalPages.value = response.data.totalPages;
  } catch (error) {
    console.log("usuários.error:", error.response);
    handleErrors(error)
  } finally {
    isBusy.value = false;
  }
}
</script>

