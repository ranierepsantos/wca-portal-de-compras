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

        <v-dialog v-model="dialog" activator="parent" max-width="700">
          <v-card>
            <v-card-title class="text-primary text-h5">
              {{ dialogTitle }}
            </v-card-title>
            <v-card-text>
              <v-form @submit.prevent="salvar()">
                <v-row>
                  <v-col>
                    <v-text-field
                      label="Nome"
                      v-model="usuario.nome"
                      type="text"
                      required
                      variant="outlined"
                      color="primary"
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
                    ></v-select>
                  </v-col>
                  <v-col>
                    <v-select
                      label="Filial"
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
                      disabled
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
                    ></v-select>
                  </v-col>
                  <v-col cols="6">
                    <v-checkbox
                      v-show="usuario.id.trim() != ''"
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
            <v-btn
              icon="mdi-check"
              variant="plain"
              color="success"
              v-if="user.ativo"
            ></v-btn>
            <v-btn
              icon="mdi-close"
              variant="plain"
              color="error"
              v-else
            ></v-btn>
          </td>
          <td class="text-right">
            <v-btn
              icon="mdi-lead-pencil"
              variant="plain"
              color="primary"
              @click="editar(user)"
            ></v-btn>
            <v-btn icon="mdi-delete" variant="plain" color="error"></v-btn>
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
import { ref, onMounted, watch } from "vue";
import userService from "../../services/user.service";
import perfilService from "../../services/perfil.service";

//DATA
const page = ref(1);
const pageSize = 2;
const isBusy = ref(false);
const totalPages = ref(1);
const users = ref([]);
const listPerfil = ref([]);
const dialogTitle = ref("Novo Usuário");
const dialog = ref(false);
const usuario = ref({
  id: "",
  nome: "",
  email: "",
  ativo: true,
  clienteid: "",
  filialid: "",
  perfilid: "",
});

//VUE METHODS
onMounted(async () => {
  await getListPerfil();
  await getItems();
});

watch(page, () => {
  getItems();
});

//METHODS
function editar(item) {
  usuario.value = { ...item };
  dialogTitle.value = "Edição de Usuário";
  dialog.value = true;
}

function closeDialog() {
  dialog.value = false;
  clearData();
}

async function salvar() {
  try {
    let data = usuario.value;
    data.clienteid = data.perfilid;
    data.filialid = data.perfilid;
    let response = null;
    if (data.id.trim() == "") {
      await userService.create(data);
    } else {
      await userService.update(data);
    }
    await getItems();
    closeDialog();
  } catch (error) {
    console.log(error);
  }
}

function clearData() {
  dialogTitle.value = "Novo Usuário";
  usuario.value = {
    id: "",
    nome: "",
    email: "",
    ativo: true,
    clienteid: null,
    filialid: null,
    perfilid: "",
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
    console.log(error);
  }
}

async function getItems() {
  try {
    isBusy.value = true;
    let response = await userService.paginate(pageSize, page.value, "");
    users.value = response.data.items;
    totalPages.value = response.data.totalPages;
  } catch (error) {
    console.log(error);
  } finally {
    isBusy.value = false;
  }
}
</script>

