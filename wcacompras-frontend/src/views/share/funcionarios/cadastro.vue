<template>
  <div>
    <Breadcrumbs
      title="Funcionário"
      :show-button="false"
      :buttons="[
        {
          text: 'Cancelar',
          icon: '',
          event: 'cancelar-click',
          disabled: isBusy,
        },
        { text: 'Salvar', icon: '', event: 'salvar-click', disabled: isBusy },
      ]"
      @salvar-click="salvar()"
      @cancelar-click="router.go(-1)"
    />
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isBusy"
    ></v-progress-linear>
    <v-container class="justify-center">
      <v-card>
        <v-card-text>
          <v-form ref="oForm">
            <v-row>
              <v-col cols="3">
                <v-text-field
                  label="Código Funcionário"
                  v-model="funcionario.codigoFuncionario"
                  type="number"
                  required
                  variant="outlined"
                  color="primary"
                  density="compact"
                  :rules="[(v) => !!v || 'Campo é obrigatório']"
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Nome"
                  v-model="funcionario.nome"
                  type="text"
                  required
                  variant="outlined"
                  color="primary"
                  density="compact"
                  :rules="[(v) => !!v || 'Campo é obrigatório']"
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-autocomplete
                  label="Cliente"
                  :items="clientes"
                  density="compact"
                  item-title="text"
                  item-value="value"
                  variant="outlined"
                  color="primary"
                  v-model="funcionario.clienteId"
                  autocomplete
                ></v-autocomplete>
              </v-col>
              <v-col>
                <v-autocomplete
                  label="Centro de Custo"
                  :items="centrosCusto"
                  density="compact"
                  item-title="text"
                  item-value="value"
                  variant="outlined"
                  color="primary"
                  v-model="funcionario.centroCustoId"
                ></v-autocomplete>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-row>
                  <v-col cols="3">
                    <v-text-field
                      label="Número PIS"
                      v-model="funcionario.numeroPis"
                      type="number"
                      required
                      variant="outlined"
                      color="primary"
                      density="compact"
                      :rules="[(v) => !!v || 'Campo é obrigatório']"
                    ></v-text-field>
                  </v-col>
                  <v-col>
                    <v-text-field
                      label="E-mail"
                      v-model="funcionario.email"
                      type="email"
                      variant="outlined"
                      color="primary"
                      density="compact"
                    />
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="2">
                    <v-text-field
                      label="DDD Celular"
                      v-model="funcionario.dddCelular"
                      type="text"
                      variant="outlined"
                      color="primary"
                      density="compact"
                      v-maska="'(##)'"
                    />
                  </v-col>
                  <v-col cols="2">
                    <v-text-field
                      label="Celular"
                      v-model="funcionario.numeroCelular"
                      type="text"
                      variant="outlined"
                      color="primary"
                      density="compact"
                      v-maska="'#####-####'"
                      
                    />
                  </v-col>
                  <v-col>
                    <v-text-field
                      label="Data Admissão"
                      type="date"
                      variant="outlined"
                      color="primary"
                      density="compact"
                      v-model="funcionario.dataAdmissao"
                    ></v-text-field>
                  </v-col>
                  <v-col>
                    <v-text-field
                      label="Data Demissão"
                      type="date"
                      variant="outlined"
                      color="primary"
                      density="compact"
                      v-model="funcionario.dataDemissao"
                    ></v-text-field>
                  </v-col>
                </v-row>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>
      </v-card>
    </v-container>
  </div>
</template>

<script setup>
import Breadcrumbs from "@/components/breadcrumbs.vue";
import router from "@/router";
import { useAuthStore } from "@/store/auth.store";
import { useShareClienteStore } from "@/store/share/cliente.store";
import { useShareFuncionarioStore } from "@/store/share/funcionario.store";
import { Funcionario } from "@/store/share/funcionario.store";
import { watch } from "vue";
import { handleError } from "vue";
import { inject } from "vue";
import { onBeforeMount } from "vue";
import { ref } from "vue";
import { useRoute } from "vue-router";

const funcionario = ref(new Funcionario());
const clientes = ref([]);
const centrosCusto = ref([]);
const oForm = ref(null);
const isBusy = ref(false);
const route = useRoute();
const swal = inject("$swal");

//VUE FUNCTIONS
onBeforeMount(async () => {
  try {
    isBusy.value = true;
    clientes.value = await useShareClienteStore().toComboList(
      useAuthStore().sistema.filial.value,
      useAuthStore().user.id
    );
    await getItem(parseInt(route.query.id));
  } catch (error) {
    console.debug("onBeforeMount.error => ", error);
    handleError(error);
  } finally {
    isBusy.value = false;
  }
});

watch(
  () => funcionario.value.clienteId,
  async () => {
    centrosCusto.value = await useShareClienteStore().ListCentrosDeCusto([
      funcionario.value.clienteId,
    ]);
  }
);

//FUNCTIONS
async function getItem(id = 0) {
  try {
    if (id > 0)
      funcionario.value = await useShareFuncionarioStore().getById(id);
  } catch (error) {
    console.debug("getItem.error => ", error);
    handleError(error);
  }
}

async function salvar() {
  try {
    isBusy.value = true;
    if (funcionario.value.id > 0)
      await useShareFuncionarioStore().update(funcionario.value);
    else await useShareFuncionarioStore().add(funcionario.value);

    await swal.fire({
      toast: true,
      icon: "success",
      index: "top-end",
      title: "Sucesso!",
      text: "Dados salvos com sucesso!",
      showConfirmButton: false,
      timer: 2000,
    });

    router.push({ name: "shareFuncionarios" });
  } catch (error) {
    console.debug("getItem.error => ", error);
    handleError(error);
  } finally {
    isBusy.value = false;
  }
}
</script>
