<template>
  <div>
    <Breadcrumbs title="Perfil" @novoClick="editar('novo')"/>
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
            <v-icon icon="mdi-account-circle-outline"></v-icon>
            &nbsp;{{ item.nome }}
          </td>

          <td class="text-center">
            <v-icon :icon="item.ativo ? 'mdi-check' : 'mdi-close'" variant="plain"
              :color="item.ativo ? 'success' : 'error'"></v-icon>
          </td>
          <td class="text-right">
            <v-btn icon="mdi-lead-pencil" variant="plain" color="primary" @click="editar(item.id)"></v-btn>
            <!-- <v-btn
                icon="mdi-delete"
                variant="plain"
                color="error"
                @click="remove(item)"
              ></v-btn> -->
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
//DATA
const router = useRouter()
const page = ref(1);
const pageSize = 5;
const isBusy = ref(false);
const totalPages = ref(1);
const perfis = ref([]);
const dialog = ref(false);
const swal = inject("$swal");

//VUE METHODS
onMounted(async () =>
{
  await getItems();
});

watch(page, () =>
{
  getItems();
});

//METHODS
async function remove(item)
{
  try
  {
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
        page.value--;
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
  } catch (error)
  {
    console.log("usuários.error:", error);
    handleErrors(error)
  }

}

function editar(id)
{
  router.push({ name: "perfilCadastro", query: { id: id } })
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
    }
  } catch (error)
  {
    console.log("usuários.error:", error);
    handleErrors(error)
  }
}


async function getItems()
{
  try
  {
    isBusy.value = true;
    let response = await perfilService.paginate(pageSize, page.value, "");
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
  
  