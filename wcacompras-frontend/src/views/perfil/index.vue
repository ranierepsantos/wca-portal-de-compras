<template>
  <div>
    <Breadcrumbs title="Perfil" @novoClick="editar('novo')" />
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
          <th class="text-left text-grey">PERFIL</th>
          <th class="text-center text-grey">ATIVO</th>
          <th></th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="item in perfis" :key="item.id">
          <td class="text-left">
            <v-icon icon="mdi-account-details-outline"></v-icon>
            &nbsp;{{ item.nome }}
          </td>

          <td class="text-center">
            <v-icon :icon="item.ativo ? 'mdi-check' : 'mdi-close'" variant="plain"
              :color="item.ativo ? 'success' : 'error'"></v-icon>
          </td>
          <td class="text-right">
            <v-btn icon="mdi-lead-pencil" variant="plain" color="primary" @click="editar(item.id)"></v-btn>
            <v-btn variant="plain" :color="item.ativo ? 'error' : 'success'"
              :title="item.ativo ? 'Desativar' : 'Ativar'"
              :icon="item.ativo ? 'mdi-close-circle-outline' : 'mdi-check-circle-outline'" @click="enableDisable(item)">
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
import perfilService from "../../services/perfil.service";
import handleErrors from "../../helpers/HandleErrors"
import { useRouter } from "vue-router";
import Breadcrumbs from "@/components/breadcrumbs.vue";
import { useAuthStore } from "@/store/auth.store";
//DATA
const router = useRouter()
const page = ref(1);
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const isBusy = ref(false);
const totalPages = ref(1);
const perfis = ref([]);
const swal = inject("$swal");
const filter = ref("");

//VUE METHODS
onMounted(async () =>
{
  await getItems();
});

watch(page, () => getItems());
watch(filter, () => getItems());

//METHODS
function editar(id)
{
  let routeName = "perfilCadastro"

  if (useAuthStore().sistema.id == 2) 
    routeName= "reembolsoPerfilCadastro"
  else if (useAuthStore().sistema.id == 3) 
    routeName= "sharePerfilCadastro"

  router.push({ name: routeName, query: { id: id } })
}

async function enableDisable(item)
{
  try
  {
    let text = item.ativo ? "Desativar" : "Ativar"

    let options = {
      title: text,
      text: `Deseja realmente ${text.toLowerCase()} o perfil: ${item.nome}?`,
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Sim",
      cancelButtonText: "Não",
    }

    let response = await swal.fire(options);
    if (response.isConfirmed)
    {
      let response = await perfilService.getWithPermissions(item.id)
      let data = response.data
      data.ativo = !data.ativo
      await perfilService.update(data);
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
    console.log("perfil.enableDisable.error:", error);
    handleErrors(error)
  }
}

async function getItems()
{
  try
  {
    isBusy.value = true;
    let response = await perfilService.paginate(pageSize, page.value, filter.value);
    perfis.value = response.data.items;
    totalPages.value = response.data.totalPages;
  } catch (error)
  {
    console.log("perfil.error:", error.response);
    handleErrors(error)
  } finally
  {
    isBusy.value = false;
  }
}
</script>
  
  