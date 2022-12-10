<template>
  <div>
    <div class="text-center mt-4">
      <img src="../../assets/images/logoWCA.png" alt="" width="300" />
    </div>
    <v-row class="d-flex justify-center">
      <v-col cols="4">
        <v-form ref="form" @submit.prevent="recuperarSenha()">
          <div class="text-h5 mb-10 mt-5" style="color: #000066">
            Alterar Senha
          </div>
          <v-row>
            <v-col><v-text-field label="Senha" type="password" v-model="formData.password"
                required></v-text-field></v-col>
          </v-row>

          <v-row>
            <v-col><v-text-field label="Confirmar Senha" type="password" v-model="formData.confirmPassword"
                :rules="confirmarSenhaRules"></v-text-field></v-col>
          </v-row>

          <v-btn type="submit" block color="primary" size="large" class="text-capitalize mt-5" v-if="!isBusy">
            Alterar
          </v-btn>
          <div class="text-center" v-else>
            <v-progress-circular indeterminate :size="40"></v-progress-circular>
          </div>
        </v-form>
      </v-col>
    </v-row>

  </div>

</template>

<script setup>
import { ref, inject, onMounted } from "vue";
import authService from "@/services/auth.service";
import handleErrors from "@/helpers/HandleErrors";
import router from "@/router";
import { useRoute } from "vue-router";

// VARIABLES
let isBusy = ref(false);
const formData = ref({
  password: "",
  confirmPassword: "",
  token: ""
});
let token = ""
const form = ref(null);
const confirmarSenhaRules = ref([
  (v) => !!v || "Campo é obrigatório",
  (v) => v == formData.value.password || "Valor não é igual ao campo Senha",
]);
const swal = inject("$swal");
const route = useRoute();
//VUE FUNCTIONS
onMounted(() =>
{
  formData.value.token = route.params.token
})

//FUNCTIONS
async function recuperarSenha()
{
  try
  {
    isBusy.value = true;
    let { valid } = await form.value.validate();
    if (valid)
    {
      await authService.changePassword(formData.value)
      swal({
        toast: true,
        icon: "success",
        position: "top-end",
        text: "Senha alterada com sucesso!",
        showConfirmButton: false,
        timer: 2000,
      });
      router.push({ name: "login" })
    }
  } catch (error)
  {
    console.log("recuperarSenha.error:", error);
    handleErrors(error)
  } finally
  {
    isBusy.value = false;
  }
}
</script>