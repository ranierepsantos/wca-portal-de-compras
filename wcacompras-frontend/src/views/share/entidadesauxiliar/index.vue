<template>
    <div>
      <Breadcrumbs :title="pageInfo.title" @novoClick="dialog = true" />
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
            <v-form @submit.prevent="salvar()" ref="entidadeForm">
              <v-row>
                <v-col>
                  <v-text-field label="Nome" v-model="entidade.nome" type="text" required variant="outlined" color="primary"
                    :rules="[(v) => !!v || 'Nome é obrigatório']" density="compact"></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col cols="6">
                  <v-checkbox v-show="entidade.id > 0" v-model="entidade.ativo" label="Ativo" color="primary"></v-checkbox>
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
          <tr v-for="item in entidades" :key="item.id">
            <td class="text-left">{{ item.nome }}</td>
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
  import { ref, onMounted, watch, inject, onBeforeMount } from "vue";
  import {EntidadeAuxiliar, useShareEntidadeAuxiliarStore} from "@/store/share/entidadesauxiliares.store"
  import handleErrors from "@/helpers/HandleErrors"
  import Breadcrumbs from "@/components/breadcrumbs.vue";
import { useRoute } from "vue-router";
  
  //DATA
  const page = ref(1);
  const pageSize = process.env.VUE_APP_PAGE_SIZE;
  const isBusy = ref(false);
  const totalPages = ref(1);
  const entidades = ref([]);
  const filter = ref("");
  const dialogTitle = ref("Novo");
  const dialog = ref(false);
  const swal = inject("$swal");
  const entidade = ref(new EntidadeAuxiliar());
  const entidadeForm = ref(null)
  const pageInfo = ref({
    title:  "",
    singleTitle: "",
    entidade: ""
  })
  const route = useRoute();
  
  //VUE METHODS
  onBeforeMount(async () => {
  try {
    isBusy.value = true;

    if (route.path.includes("horarios")) {
      pageInfo.value.title = "Horários";
      pageInfo.value.singleTitle = "Horário";
      pageInfo.value.entidade = "Horario";
    } else if (route.path.includes("documentoscomplementares")) {
      pageInfo.value.title = "Documentos Complementares";
      pageInfo.value.singleTitle = "Documento Complementar";
      pageInfo.value.entidade = "DocumentoComplementar";
    } else if (route.path.includes("escalas")) {
      pageInfo.value.title = "Escalas";
      pageInfo.value.singleTitle = "Escala";
      pageInfo.value.entidade = "Escala";
    } else if (route.path.includes("funcoes")) {
      pageInfo.value.title = "Funções";
      pageInfo.value.singleTitle = "Função";
      pageInfo.value.entidade = "Funcao";
    } else if (route.path.includes("gestores")) {
      pageInfo.value.title = "Gestores";
      pageInfo.value.singleTitle = "Gestor";
      pageInfo.value.entidade = "Gestor";
    } else if (route.path.includes("tiposcontrato")) {
      pageInfo.value.title = "Tipos de Contrato";
      pageInfo.value.singleTitle = "Tipo de Contrato";
      pageInfo.value.entidade = "TipoContrato";
    }
  console.debug('PageInfo', pageInfo.value)
    
  } catch (error) {
  } finally {
    isBusy.value = true;
  }
});

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
    dialogTitle.value = `Novo(a) ${pageInfo.value.singleTitle}`;
    entidade.value = new EntidadeAuxiliar();
  }
  
  function closeDialog()
  {
    dialog.value = false;
    entidadeForm.value.reset()
    clearData();
  }
  
  function editar(item)
  {
    entidade.value = { ...item };
    dialogTitle.value = `Editar ${pageInfo.value.singleTitle}`;
    dialog.value = true;
  }
  
  async function enableDisable(item)
  {
    try
    {
      let text = item.ativo ? "Desativar" : "Ativar"
  
      let options = {
        title: text,
        text: `Deseja realmente ${text.toLowerCase()} o item: ${item.nome}?`,
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
        await useShareEntidadeAuxiliarStore().update(pageInfo.value.entidade, data);
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
      console.error("enableDisable.error:", error);
      handleErrors(error)
    }
  }
  
  async function getItems()
  {
    try
    {
      isBusy.value = true;
      let response = await useShareEntidadeAuxiliarStore().getPaginate(pageInfo.value.entidade, page.value, pageSize, filter.value);
      entidades.value = response.items;
      totalPages.value = response.totalPages;
    } catch (error)
    {
      console.error("getItems.error:", error.response);
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
      let { valid } = await entidadeForm.value.validate();
      if (valid)
      {
        let data = entidade.value;
        let response = null
        if (data.id == 0)
        {
          response = await useShareEntidadeAuxiliarStore().add(pageInfo.value.entidade, data);
        } else
        {
          response = await useShareEntidadeAuxiliarStore().update(pageInfo.value.entidade, data);
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
      console.error("salvar.error:", error);
      handleErrors(error)
    }
  }
  
  </script>
    
    