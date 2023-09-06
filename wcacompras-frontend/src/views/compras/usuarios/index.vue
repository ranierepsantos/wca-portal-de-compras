<template>
  <div>
    <Breadcrumbs title="Usuários" @novoClick="router.push({name: 'comprasUsuarioCadastro'})" />
    <v-row>
        <v-col cols="5">
            <v-select label="Filiais" v-model="filter.filial" :items="filiais" density="compact"
                item-title="text" item-value="value" variant="outlined" color="primary"
                :hide-details="true" clearable></v-select>
        </v-col>
        <v-col cols="4">
            <v-text-field label="Pesquisar" placeholder="(Nome)" v-model="filter.termo" density="compact"
                variant="outlined" color="info" clearable>
            </v-text-field>
        </v-col>
    </v-row>
    <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
    <v-table class="elevation-2">
      <thead>
        <tr>
          <th class="text-left text-grey">NOME</th>
          <th class="text-left text-grey">PERFIL</th>
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
          <td colspan="4">
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
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const isBusy = ref(false);
const totalPages = ref(1);
const users = ref([]);
const listPerfil = ref([]);
const filiais = ref([]);
const swal = inject("$swal");
const filter = ref({
    filial: [],
    termo: ""
});


//VUE METHODS
onMounted(async () =>
{
  await getFilialToList();
  await getPerfilToList();
  await getItems();
});

watch(page, async () => await getItems());
watch(() =>filter, () => getItems(), {deep: true});

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
    let routeName = "comprasUsuarioCadastro"
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
    let response = await filialService.getListByAuthenticatedUser();
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
    let filtro = {...filter.value}
    if (!filtro.filial || filtro.filial.length ==0 ) filtro.filial = filiais.value.map(f => {return f.value})

    let response = await userService.paginate(pageSize, page.value, filtro)
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