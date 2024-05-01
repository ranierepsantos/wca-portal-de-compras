<template>
  <div>
    <bread-crumbs
      title="Clientes"
      @novoClick="editar('novo')"
      :buttons="buttons"
      @importar-click="importar()"
    />
    <v-row>
      <v-col cols="5">
        <v-select
          label="Filiais"
          v-model="filter.filial"
          :items="filiais"
          density="compact"
          item-title="text"
          item-value="value"
          variant="outlined"
          color="primary"
          :hide-details="true"
          clearable
        ></v-select>
      </v-col>
      <v-col cols="4">
        <v-text-field
          label="Pesquisar"
          placeholder="(Nome)"
          v-model="filter.termo"
          density="compact"
          variant="outlined"
          color="info"
          clearable
        >
        </v-text-field>
      </v-col>
    </v-row>
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
          <th class="text-left text-grey">CNPJ</th>
          <th class="text-center text-grey">ATIVO</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in clientes" :key="item.id">
          <td class="text-left">
            <v-icon icon="mdi-home-outline"></v-icon>
            &nbsp;{{ item.nome }}
          </td>
          <td class="text-left">{{ item.cnpj }}</td>
          <td class="text-center">
            <v-icon
              :icon="item.ativo ? 'mdi-check' : 'mdi-close'"
              variant="plain"
              :color="item.ativo ? 'success' : 'error'"
            ></v-icon>
          </td>
          <td class="text-right">
            <v-btn
              icon="mdi-lead-pencil"
              variant="plain"
              color="primary"
              @click="editar(item.id)"
            ></v-btn>
            <v-btn
              variant="plain"
              :color="item.ativo ? 'error' : 'success'"
              :title="item.ativo ? 'Desativar' : 'Ativar'"
              :icon="
                item.ativo
                  ? 'mdi-close-circle-outline'
                  : 'mdi-check-circle-outline'
              "
              @click="enableDisable(item)"
            >
            </v-btn>
          </td>
        </tr>
      </tbody>
      <tfoot>
        <tr>
          <td colspan="4">
            <v-pagination
              v-model="filter.page"
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
import { ref, onMounted, watch, inject } from "vue";
import clienteService from "@/services/cliente.service";
import filialService from "@/services/filial.service";
import handleErrors from "@/helpers/HandleErrors";
import BreadCrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import { toBase64 } from "@/helpers/functions";
import { useAuthStore } from "@/store/auth.store";

//DATA
//const page = ref(1);
const pageSize = process.env.VUE_APP_PAGE_SIZE;
const isBusy = ref(false);
const totalPages = ref(1);
const clientes = ref([]);
const filiais = ref([]);
const filter = ref({
  filial: [],
  termo: "",
  page: 1,
});
const swal = inject("$swal");
const buttons = ref([]);
let fromMounted = ref(false)
//VUE METHODS
onMounted(async () => {
  if (useAuthStore().hasPermissao("importar_clientes")) {
    buttons.value.push({
      text: "Importar Clientes",
      icon: "mdi-upload",
      event: "importar-click",
    });
  }
  await getFiliaisByUser();
  fromMounted.value = true
  await getItems(false, true);
  fromMounted.value = false
});

watch(() => filter.value.page, () => { if (!fromMounted.value) getItems() });
watch(() => filter.value.filial,  () => { if (!fromMounted.value) getItems(true) });
watch(() => filter.value.termo,  () => { if (!fromMounted.value)getItems(true) });

//METHODS
function editar(id) {
  router.push({ name: "clienteCadastro", query: { id: id } });
}

async function enableDisable(item) {
  try {
    let text = item.ativo ? "Desativar" : "Ativar";

    let options = {
      title: text,
      text: `Deseja realmente ${text.toLowerCase()} o cliente: ${item.nome}?`,
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Sim",
      cancelButtonText: "Não",
    };

    let response = await swal.fire(options);
    if (response.isConfirmed) {
      let data = { ...item };
      data.ativo = !data.ativo;
      await clienteService.update(data);
      await getItems();

      swal.fire({
        toast: true,
        icon: "success",
        position: "top-end",
        title: "Sucesso!",
        text: "Alteração realizada!",
        showConfirmButton: false,
        timer: 2000,
      });
    }
  } catch (error) {
    console.log("clientes.enableDisable.error:", error);
    handleErrors(error);
  }
}
async function getFiliaisByUser() {
  try {
    isBusy.value = true;
    let response = await filialService.getListByAuthenticatedUser();
    filiais.value = response.data;
  } catch (error) {
    console.log("clientes.getFiliaisByUser.error:", error.response);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}
async function getItems(resetPage = false, fromStoredFilters = false) {
  try {
    isBusy.value = true;
    if (fromStoredFilters) {
      let storedFilters =
        JSON.parse(localStorage.getItem("clientes.filters")) || null;
      if (storedFilters) {
        filter.value.page = storedFilters.page;
        filter.value.termo = storedFilters.termo;
        if (!Array.isArray(storedFilters.filial)) {
          filter.value.filial = storedFilters.filial;
        }
      }
    }
    if (resetPage) filter.value.page = 1
    let filtro = { ...filter.value };
    if (!filtro.filial || filtro.filial.length == 0)
      filtro.filial = filiais.value.map((f) => {
        return f.value;
      });
    
    localStorage.setItem("clientes.filters", JSON.stringify(filtro));
    let response = await clienteService.paginate(pageSize, filtro.page, filtro);
    clientes.value = response.data.items;
    totalPages.value = response.data.totalPages;
  } catch (error) {
    console.log("clientes.getItems.error:", error.response);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}

async function importar() {
  try {
    const { value: file } = await swal.fire({
      title: "Selecionar planilha",
      input: "file",
      showCancelButton: true,
      inputAttributes: {
        accept:
          "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel",
        "aria-label": "Selecione a planilha de clientes",
      },
    });

    if (file) {
      isBusy.value = true;
      var arquivo = await toBase64(file);
      arquivo = arquivo.split(";")[1].replace("base64,", "");

      let arquivoImportacao = {
        arquivo: arquivo,
      };

      await clienteService.importar(arquivoImportacao);

      await getItems();
    }
  } catch (error) {
    console.log("importar.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
}
</script>
