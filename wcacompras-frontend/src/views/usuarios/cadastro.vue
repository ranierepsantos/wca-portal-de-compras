<template>
    <div>
        <Breadcrumbs  title="Usuário" :show-button="false"/>
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <v-container class="justify-center">
            <v-card>
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
                      <v-col cols="6">
                          <v-card :elevation="2" subtitle="Selecione as categorias">
                          <v-card-text>
                              <v-list density="compact" select-strategy="multiple" color="primary">
                              <v-list-item v-for="item in tipos" :key="item.text" @click="item.selected = !item.selected"
                                  :active="item.selected">
                                  <v-list-item-title>{{ item.text }}</v-list-item-title>
                              </v-list-item>
                              </v-list>
                          </v-card-text>
                          </v-card>
                      </v-col>
                      <v-col cols="1">
                          <v-icon icon="mdi-chevron-double-right" color="success" size="x-large"
                          @click="usuarioTipoFornecimentoAdicionarTodos()"></v-icon><br />
                          <v-icon icon="mdi-chevron-right" color="success" size="x-large"
                          @click="usuarioTipoFornecimentoAdicionar()"></v-icon><br />
                          <v-icon icon="mdi-chevron-double-left" color="success" size="x-large"
                          @click="usuarioTipoFornecimentoRemoverTodos()"></v-icon><br />
                          <v-icon icon="mdi-chevron-left" color="success" size="x-large"
                          @click="usuarioTipoFornecimentoRemover()"></v-icon>
                      </v-col>
                      <v-col cols="5">
                          <v-card :elevation="2" subtitle="Categorias de Compra do usúario">
                          <v-card-text>
                              <v-list density="compact" select-strategy="multiple" color="primary">
                              <v-list-item v-for="item in usuario.tipoFornecimento" :key="item.value" :active="item.selected"
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
        </v-container>
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
import { useRoute } from "vue-router";
import Breadcrumbs from "@/components/breadcrumbs.vue";
import { compararValor } from "@/helpers/functions";
import tipoFornecimentoService from "@/services/tipofornecimento.service";

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
  tipoFornecimento:[]
});
let filialUsuario = 0;
const authStore = useAuthStore();
const emailRules = ref([
  (v) => !!v || "E-mail é obrigatório",
  (v) => /.+@.+\..+/.test(v) || "E-mail deve ser válido",
]);
const userForm = ref(null)
const tipos = ref([])

//VUE METHODS
onMounted(async () =>
{
  clearData();
  await getFilialToList();
  await getPerfilToList();
  // await getClienteToList(usuario.value.filialid);
  await getTipoFornecimentoToList();
  if (parseInt(route.query.id) > 0)
  {
      await getUsuario(route.query.id)
  }
  isBusy.value = false;

});

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
  usuario.value = {
    id: 0,
    nome: "",
    email: "",
    ativo: true,
    filialid: authStore.user.filial,
    perfilid: null,
    cliente: [],
    tipoFornecimento: []
  };
  filialUsuario = authStore.user.filial;
  await getClienteToList(filialUsuario)
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

async function getTipoFornecimentoToList() 
{
    try
    {
        let response = await tipoFornecimentoService.toList();
        tipos.value = response.data;
    } catch (error)
    {
        console.log("getTipoFornecimentoToList.error:", error);
        handleErrors(error)
    }
}

async function getUsuario(usuarioId)
{
    try
    {
        isBusy.value = true
        let response = await userService.getById(usuarioId);
        filialUsuario = response.data.filialid;
        usuario.value = response.data;
        tiposListRemove()
        clientesListRemove()
    } catch (error)
    {
        console.log("getUsuario.error:", error);
        handleErrors(error)
    } finally
    {
        isBusy.value = false
    }
}

function tiposListRemove(removerTodos = false)
{
  if (removerTodos == true)
    tipos.value.splice(0, clientes.value.length)
  else
  {

    usuario.value.tipoFornecimento.forEach(tipo =>
    {
      let index = tipos.value.findIndex(c => c.value == tipo.value)
      if (index > -1) tipos.value.splice(index, 1)

    })
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

function usuarioTipoFornecimentoAdicionar()
{
  let list = tipos.value.slice()
  list.forEach((tipo) =>
  {
    if (tipo.selected != undefined && tipo.selected == true)
    {
      tipo.selected = false
      usuario.value.tipoFornecimento.push(tipo)
    }
  })
  tiposListRemove();
}

function usuarioTipoFornecimentoAdicionarTodos()
{
  usuario.value.tipoFornecimento.push(...tipos.value)
  tipos.value.splice(0, tipos.value.length)
  ordernarLista(usuario.value.tipoFornecimento, "text")
}

function usuarioTipoFornecimentoRemover()
{
  let list = usuario.value.tipoFornecimento.slice()
  list.forEach((tipo) =>
  {
    if (tipo.selected != undefined && tipo.selected == true)
    {
      tipo.selected = false
      tipos.value.push(tipo)
      let index = usuario.value.tipoFornecimento.findIndex(c => c.value == tipo.value)
      if (index > -1) usuario.value.tipoFornecimento.splice(index, 1)
    }
  })
}

function usuarioTipoFornecimentoRemoverTodos()
{
  tipos.value.push(...usuario.value.tipoFornecimento)
  usuario.value.tipoFornecimento.splice(0, usuario.value.tipoFornecimento.length)
  ordernarLista(tipos.value, "text")
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