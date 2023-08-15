<template>
  <div>
    <Breadcrumbs title="Filial" @novoClick="dialog = true" />
    <v-row>
      <v-col cols="6">
        <v-text-field label="Pesquisar" placeholder="(Nome)" v-model="filter" density="compact" variant="outlined"
          color="info">
        </v-text-field>
      </v-col>
    </v-row>
    <v-dialog v-model="dialog" max-width="700" :absolute="false">
      <v-card>
        <v-card-title class="text-primary text-h5">
          {{ dialogTitle }}
        </v-card-title>
        <v-card-text>
          <v-form @submit.prevent="salvar()" ref="filialForm">
            <v-row>
              <v-col>
                <v-text-field label="Nome" v-model="filial.nome" type="text" required variant="outlined" color="primary"
                  :rules="[(v) => !!v || 'Nome é obrigatório']" density="compact"></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col cols="6">
                <v-checkbox v-show="filial.id > 0" v-model="filial.ativo" label="Ativo" color="primary"></v-checkbox>
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
        <v-card-actions> </v-card-actions>
      </v-card>
    </v-dialog>
    <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
    <v-table class="elevation-2">
      <thead>
        <tr>
          <th class="text-left text-grey">NOME</th>
          <th class="text-center text-grey">ATIVO</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in filiais" :key="item.id">
          <td class="text-left">
            <v-icon icon="mdi-domain-plus"></v-icon>
            &nbsp;{{ item.nome }}
          </td>
          <td class="text-center">
            <v-icon :icon="item.ativo ? 'mdi-check' : 'mdi-close'" variant="plain"
              :color="item.ativo ? 'success' : 'error'"></v-icon>
          </td>
          <td class="text-right">
            <v-btn icon="mdi-lead-pencil" variant="plain" color="primary" @click="editar(item)"></v-btn>
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
import filialService from "@/services/filial.service";
import handleErrors from "@/helpers/HandleErrors"
import { useAuthStore } from "@/store/auth.store";
import Breadcrumbs from "@/components/breadcrumbs.vue";

//DATA
const page = ref(1);
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const isBusy = ref(false);
const totalPages = ref(1);
const filiais = ref([]);
const filter = ref("");
const dialogTitle = ref("Nova Filial");
const dialog = ref(false);
const swal = inject("$swal");
const filial = ref({
  id: 0,
  nome: "",
  ativo: true
});
const authStore = useAuthStore();

const filialForm = ref(null)

//VUE METHODS
onMounted(async () =>
{
  clearData();
  await getItems();
});

watch(page, () => getItems());
watch(filter, () => getItems());


//METHODS

function clearData()
{
  dialogTitle.value = "Nova Filial";
  filial.value = {
    id: 0,
    nome: "",
    ativo: true
  };
}

function closeDialog()
{
  dialog.value = false;
  filialForm.value.reset()
  clearData();
}

function editar(item)
{
  filial.value = { ...item };
  dialogTitle.value = "Edição de Filial";
  dialog.value = true;
}

async function enableDisable(item)
{
  try
  {
    let text = item.ativo ? "Desativar" : "Ativar"

    let options = {
      title: text,
      text: `Deseja realmente ${text.toLowerCase()} a filial: ${item.nome}?`,
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
      await filialService.update(data);
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
    console.log("filiais.enableDisable.error:", error);
    handleErrors(error)
  }
}

async function getItems()
{
  try
  {
    isBusy.value = true;
    let response = await filialService.paginate(pageSize, page.value, filter.value);
    filiais.value = response.data.items;
    totalPages.value = response.data.totalPages;
  } catch (error)
  {
    console.log("filiais.error:", error.response);
    handleErrors(error)
  } finally
  {
    isBusy.value = false;
  }
}

async function salvar()
{
  try
  {
    let { valid } = await filialForm.value.validate();
    if (valid)
    {
      let data = filial.value;
      if (data.id == 0)
      {
        await filialService.create(data);
      } else
      {
        await filialService.update(data);
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
    console.log("filiais.error:", error);
    handleErrors(error)
  }
}

</script>
  
  