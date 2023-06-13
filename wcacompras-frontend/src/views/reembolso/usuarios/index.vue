<template>
  <div>
    <Breadcrumbs title="Usuários" @novoClick="router.push({name: 'reembolsoUsuarioCadastro'})" />
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
          <td class="text-left">{{ getPerfilName(user.usuarioSistemaPerfil[0].perfilId) }}</td>
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
  </div>
</template>

<script setup>
import { ref, onMounted, watch, inject } from "vue";
import userService from "@/services/user.service";
import perfilService from "@/services/perfil.service";
import filialService from "@/services/filial.service";
import handleErrors from "@/helpers/HandleErrors"
import { useAuthStore } from "@/store/auth.store";
import Breadcrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";

//DATA
const page = ref(1);
const pageSize = 5;
const isBusy = ref(false);
const totalPages = ref(1);
const users = ref([]);
const listPerfil = ref([]);
const filiais = ref([]);
const swal = inject("$swal");
const authStore = useAuthStore();
const filter = ref("");


//VUE METHODS
onMounted(async () =>
{
  await getFilialToList();
  await getPerfilToList();
  await getItems();
});

watch(page, async () => await getItems());
watch(filter, async () => await getItems());

//METHODS
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
      await getItems()

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

    let routeName = "reembolsoUsuarioCadastro"
    router.push({ name: routeName, query: { id: item.id } })
  } catch (error)
  {
    console.log("editar.error:", error);
    handleErrors(error)
  } finally
  {
    isBusy.value = false
  }

}

function getPerfilName(perfilId)
{
  let perfil = listPerfil.value.filter((p) => p.value == perfilId)[0];
  
  return perfil ==undefined ? "" : perfil.text;
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