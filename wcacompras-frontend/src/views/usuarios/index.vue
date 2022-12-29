<template>
  <div>
    <Breadcrumbs title="Usuários" @novoClick="dialog = true;" />
    <v-row>
      <v-col cols="6">
        <v-text-field label="Pesquisar" placeholder="(Nome)" v-model="filter" density="compact" variant="outlined"
          color="info">
        </v-text-field>
      </v-col>
    </v-row>
    <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
    <v-table class="elevation-2">
      <thead>
        <tr>
          <th class="text-left text-grey">NOME</th>
          <th class="text-left text-grey">PERFIL</th>
          <th class="text-left text-grey" v-show="authStore.user.filial == 1">FILIAL</th>
          <th class="text-center text-grey">ATIVO</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in users" :key="user.id">
          <td class="text-left">
            <v-icon icon="mdi-account-circle-outline"></v-icon>
            &nbsp;{{ user.nome }}
          </td>
          <td class="text-left">{{ getPerfilName(user.perfilid) }}</td>
          <td class="text-left" v-show="authStore.user.filial == 1">{{ user.filial?.nome }}</td>
          <td class="text-center">
            <v-icon :icon="user.ativo ? 'mdi-check' : 'mdi-close'" variant="plain"
              :color="user.ativo ? 'success' : 'error'"></v-icon>
          </td>
          <td class="text-right">
            <v-btn icon="mdi-lead-pencil" variant="plain" color="primary" @click="editar(user)"></v-btn>
            <v-btn variant="plain" :color="user.ativo ? 'error' : 'success'"
              :title="user.ativo ? 'Desativar' : 'Ativar'"
              :icon="user.ativo ? 'mdi-close-circle-outline' : 'mdi-check-circle-outline'" @click="enableDisable(user)">
            </v-btn>
          </td>
        </tr>
      </tbody>
      <tfoot>
        <tr>
          <td :colspan="authStore.user.filial == 1 ? 5 : 4">
            <v-pagination v-model="page" :length="totalPages" :total-visible="4"></v-pagination>
          </td>
        </tr>
      </tfoot>
    </v-table>
    <v-dialog v-model="dialog" max-width="700" :absolute="false" persistent>
      <v-card>
        <v-card-title class="text-primary text-h5">
          {{ dialogTitle }}
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="salvar()" ref="userForm">
            <v-row>
              <v-col>
                <v-text-field label="Nome" v-model="usuario.nome" type="text" required variant="outlined"
                  color="primary" :rules="[(v) => !!v || 'Nome é obrigatório']" density="compact"></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-text-field label="Email" v-model="usuario.email" type="email" required variant="outlined"
                  color="primary" :rules="emailRules" density="compact"></v-text-field>
              </v-col>
              <v-col cols="2" v-show="usuario.id > 0">
                <v-checkbox v-model="usuario.ativo" label="Ativo" color="primary"></v-checkbox>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-select label="Perfil" v-model="usuario.perfilid" :items="listPerfil" item-title="text"
                  item-value="value" variant="outlined" color="primary" :rules="[(v) => !!v || 'Perfil é obrigatório']"
                  density="compact"></v-select>
              </v-col>
              <v-col>
                <v-select label="Filial" :items="filiais" variant="outlined" color="primary" item-title="text"
                  item-value="value" v-model="usuario.filialid" :disabled="(authStore.user.filial != 1)"
                  :rules="[(v) => !!v || 'Filial é obrigatório']" density="compact"></v-select>
              </v-col>
            </v-row>
            <!-- <v-row>
              <v-col cols="6">
                
              </v-col>
            </v-row> -->
            <v-row>
              <v-col cols="6">
                <v-card :elevation="2" subtitle="Selecione os clientes">
                  <v-card-text>
                    <v-list density="compact" select-strategy="multiple" color="primary">
                      <v-list-item v-for="item in clientes" :key="item.text" @click="item.selected = !item.selected"
                        :active="item.selected">
                        <v-list-item-title>{{ item.text }}</v-list-item-title>
                      </v-list-item>
                    </v-list>
                  </v-card-text>
                </v-card>
              </v-col>
              <v-col cols="1">
                <v-icon icon="mdi-chevron-double-right" color="success" size="x-large"
                  @click="usuarioClienteAdicionarTodos()"></v-icon><br />
                <v-icon icon="mdi-chevron-right" color="success" size="x-large"
                  @click="usuarioClienteAdicionar()"></v-icon><br />
                <v-icon icon="mdi-chevron-double-left" color="success" size="x-large"
                  @click="usuarioClienteRemoverTodos()"></v-icon><br />
                <v-icon icon="mdi-chevron-left" color="success" size="x-large"
                  @click="usuarioClienteRemover()"></v-icon>
              </v-col>
              <v-col cols="5">
                <v-card :elevation="2" subtitle="Clientes do usúario">
                  <v-card-text>
                    <v-list density="compact" select-strategy="multiple" color="primary">
                      <v-list-item v-for="item in usuario.cliente" :key="item.value" :active="item.selected"
                        @click="item.selected = !item.selected">
                        <v-list-item-title>{{ item.text }}</v-list-item-title>
                      </v-list-item>
                    </v-list>
                  </v-card-text>
                </v-card>
              </v-col>
            </v-row>
            <v-row>
              <v-col class="text-right">
                <v-btn variant="outlined" color="primary" @click="closeDialog()">Cancelar</v-btn>
                <v-btn color="primary" type="submit" class="ml-3">Salvar</v-btn>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>

      </v-card>
    </v-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, inject } from "vue";
import userService from "@/services/user.service";
import perfilService from "@/services/perfil.service";
import filialService from "@/services/filial.service";
import clienteService from "@/services/cliente.service";
import handleErrors from "@/helpers/HandleErrors"
import { useAuthStore } from "@/store/auth.store";
import Breadcrumbs from "@/components/breadcrumbs.vue";
import { compararValor } from "@/helpers/functions";

//DATA
const page = ref(1);
const pageSize = 5;
const isBusy = ref(false);
const totalPages = ref(1);
const users = ref([]);
const listPerfil = ref([]);
const filiais = ref([]);
const clientes = ref([]);
const dialogTitle = ref("Novo Usuário");
const dialog = ref(false);
const swal = inject("$swal");
const usuario = ref({
  id: 0,
  nome: "",
  email: "",
  ativo: true,
  filialid: 1,
  perfilid: null,
  cliente: []
});
let filialUsuario = 0;
const authStore = useAuthStore();
const emailRules = ref([
  (v) => !!v || "E-mail é obrigatório",
  (v) => /.+@.+\..+/.test(v) || "E-mail deve ser válido",
]);
const userForm = ref(null)
const filter = ref("");


//VUE METHODS
onMounted(async () =>
{
  clearData();
  await getFilialToList();
  await getPerfilToList();
  await getItems();
});

watch(page, async () => await getItems());
watch(filter, async () => await getItems());

watch(() => usuario.value.filialid, async (filialid, oldValue) =>
{
  clientes.value = [];
  await getClienteToList(filialid)
  clientesListRemove();
  if (filialid != filialUsuario)
  {
    usuario.value.cliente = []
    filialUsuario = filialid
  }
})

//METHODS
function clientesListRemove(removerTodos = false)
{
  if (removerTodos == true)
    clientes.value.splice(0, clientes.value.length)
  else
  {

    usuario.value.cliente.forEach(cliente =>
    {
      let index = clientes.value.findIndex(c => c.value == cliente.value)
      if (index > -1) clientes.value.splice(index, 1)

    })
  }
}
function closeDialog()
{
  dialog.value = false;
  userForm.value.reset()
  clearData();
}

async function enableDisable(item)
{
  try
  {
    let text = item.ativo ? "Desativar" : "Ativar"

    let options = {
      title: text,
      text: `Deseja realmente ${text.toLowerCase()} o usuário: ${item.nome}?`,
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Sim",
      cancelButtonText: "Não",
    }

    let response = await swal.fire(options);
    if (response.isConfirmed)
    {
      let data = { ...item }
      data.ativo = !data.ativo
      await userService.update(data);
      await this.getItems()

      swal.fire({
        toast: true,
        icon: "success",
        position: "top-end",
        title: "Sucesso!",
        text: "Alteração realizada!",
        showConfirmButton: false,
        timer: 2000,
      })
    }
  } catch (error)
  {
    console.log("usuarios.enableDisable.error:", error);
    handleErrors(error)
  }
}

async function editar(item)
{
  try
  {
    isBusy.value = true
    let response = await userService.getById(item.id)
    usuario.value = response.data;
    console.log(usuario.value)
    filialUsuario = usuario.value.filialid;
    await getClienteToList(filialUsuario)
    clientesListRemove();
    dialogTitle.value = "Edição de Usuário";
    dialog.value = true;
  } catch (error)
  {
    console.log("editar.error:", error);
    handleErrors(error)
  } finally
  {
    isBusy.value = false
  }

}


async function salvar()
{
  try
  {
    let { valid } = await userForm.value.validate();
    if (valid)
    {
      let data = usuario.value;
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
    console.log("usuários.error:", error);
    handleErrors(error)
  }
}

async function clearData()
{
  dialogTitle.value = "Novo Usuário";
  usuario.value = {
    id: 0,
    nome: "",
    email: "",
    ativo: true,
    filialid: authStore.user.filial,
    perfilid: null,
    cliente: []
  };
  filialUsuario = authStore.user.filial;
  await getClienteToList(filialUsuario)
}

function getPerfilName(perfilId)
{
  let perfil = listPerfil.value.filter((p) => p.value == perfilId)[0];
  return perfil.text;
}

async function getClienteToList(filial)
{
  try
  {
    let response = await clienteService.toList(filial);
    clientes.value = response.data;
  } catch (error)
  {
    console.log("getClienteToList.error:", error);
    handleErrors(error)
  }
}

async function getPerfilToList()
{
  try
  {
    let response = await perfilService.toList();
    listPerfil.value = response.data;
  } catch (error)
  {
    console.log("getPerfilToList.error:", error);
    handleErrors(error)
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

async function getItems()
{
  try
  {
    isBusy.value = true;
    let response = await userService.paginate(pageSize, page.value, filter.value)
    users.value = response.data.items;
    totalPages.value = response.data.totalPages;
  } catch (error)
  {
    console.log("usuários.error:", error.response);
    handleErrors(error)
  } finally
  {
    isBusy.value = false;
  }
}

function usuarioClienteAdicionar()
{
  let list = clientes.value.slice()
  list.forEach((cliente) =>
  {
    if (cliente.selected != undefined && cliente.selected == true)
    {
      cliente.selected = false
      usuario.value.cliente.push(cliente)
    }
  })
  clientesListRemove();
}

function usuarioClienteAdicionarTodos()
{
  usuario.value.cliente.push(...clientes.value)
  clientes.value.splice(0, clientes.value.length)
  ordernarLista(usuario.value.cliente, "text")
}

function usuarioClienteRemover()
{
  let list = usuario.value.cliente.slice()
  list.forEach((cliente) =>
  {
    if (cliente.selected != undefined && cliente.selected == true)
    {
      cliente.selected = false
      clientes.value.push(cliente)
      let index = usuario.value.cliente.findIndex(c => c.value == cliente.value)
      if (index > -1) usuario.value.cliente.splice(index, 1)
    }
  })
}

function usuarioClienteRemoverTodos()
{
  clientes.value.push(...usuario.value.cliente)
  usuario.value.cliente.splice(0, usuario.value.cliente.length)
  ordernarLista(clientes.value, "text")
}

function ordernarLista(lista, campo)
{
  lista.sort(compararValor(campo))
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