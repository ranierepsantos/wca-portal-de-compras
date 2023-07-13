<template>
  <div>
    <Breadcrumbs title="Tipos de Despesa" @novoClick="dialog = true" />
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
          <v-form @submit.prevent="salvar()" ref="form">
            <v-row>
              <v-col>
                <v-text-field label="Nome" v-model="model.nome" type="text" required variant="outlined" color="primary"
                  :rules="[(v) => !!v || 'Nome é obrigatório']" density="compact"></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col cols="6">
                <v-checkbox v-show="model.id > 0" v-model="model.ativo" label="Ativo" color="primary"></v-checkbox>
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
        <tr v-for="item in tableData" :key="item.id">
          <td class="text-left">
            <v-icon icon="mdi-shape-outline"></v-icon>
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
import handleErrors from "@/helpers/HandleErrors"
import Breadcrumbs from "@/components/breadcrumbs.vue";
import { useDespesaTipoStore, despesaTipoModel } from "@/store/reembolso/despesaTipo.store";

//DATA
const page = ref(1);
const pageSize = 10;
const isBusy = ref(false);
const totalPages = ref(1);
const tableData = ref([]);
const filter = ref("");
const dialogTitle = ref("Nova data");
const dialog = ref(false);
const swal = inject("$swal");
const model = ref({...despesaTipoModel});
const despesaTipoStore = useDespesaTipoStore();

const form = ref(null)

//VUE METHODS
onMounted(async () =>
{
  clearModel();
  await getItems();
});

watch(page, () => getItems());
watch(filter, () => getItems());


//METHODS

function clearModel()
{
  dialogTitle.value = "Novo Tipo";
  model.value = {...despesaTipoModel};
}

function closeDialog()
{
  dialog.value = false;
  form.value.reset()
  clearModel();
}

function editar(item)
{
  model.value = { ...item };
  dialogTitle.value = "Editar Tipo";
  dialog.value = true;
}

async function enableDisable(item)
{
  try
  {
    let text = item.ativo ? "Desativar" : "Ativar"

    let options = {
      title: text,
      text: `Deseja realmente ${text.toLowerCase()} o tipo: ${item.nome}?`,
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
      despesaTipoStore.update(data)

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
    console.log("datas.enableDisable.error:", error);
    handleErrors(error)
  }
}

async function getItems()
{
  try
  {
    isBusy.value = true;
    let response = despesaTipoStore.getPaginate(page.value, pageSize)
    tableData.value = response.items;
    totalPages.value = response.totalPages;

  } catch (error)
  {
    console.log("tipoDespesa.error:", error.response);
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
    let { valid } = await form.value.validate();
    if (valid)
    {
      let data = {...model.value};
      if (data.id == 0)
      {
        await despesaTipoStore.add(data)
      } else
      {
        await despesaTipoStore.update(data);
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
    console.log("tipoDespesa.error:", error);
    handleErrors(error)
  }
}

</script>
  
  