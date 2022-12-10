<template>
  <div>
    <div class="text-center mt-4">
      <img src="../../assets/images/logoWCA.png" alt="" width="300" />
    </div>
    <v-row class="d-flex justify-center">
      <v-col cols="4">
        <v-form ref="form" @submit.prevent="recuperarSenha()">
          <div class="text-h5 mb-10 mt-5" style="color: #000066">
            Recuperar Senha
          </div>
          <v-text-field label="Email" type="email" v-model="formData.email" :rules="emailRules" required></v-text-field>

          <div class="text-body-2 text-center text-grey mt-3">
            As instruções para recuperação de senha serão enviadas para seu e-mail!
          </div>

          <v-btn type="submit" block color="primary" size="large" class="text-capitalize mt-5" v-if="!isBusy">
            Enviar
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
import { ref, inject } from "vue";
import authService from "@/services/auth.service";
import handleErrors from "@/helpers/HandleErrors";
import router from "@/router";
// VARIABLES
let isBusy = ref(false);
const formData = ref({
  email: "",
});

const form = ref(null);
const emailRules = ref([
  (v) => !!v || "E-mail é obrigatório",
  (v) => /.+@.+\..+/.test(v) || "E-mail deve ser válido",
]);
const swal = inject("$swal");

//VUE FUNCTIONS

//FUNCTIONS
async function recuperarSenha()
{
  try
  {
    isBusy.value = true;
    let { valid } = await form.value.validate();
    if (valid)
    {
      let response = await authService.resetPassword(formData.value)
      swal({
        toast: true,
        icon: "success",
        position: "top-end",
        text: "Solicitação realizada, verifique seu e-mail!",
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