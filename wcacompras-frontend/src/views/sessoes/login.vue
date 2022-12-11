<template>
  <v-row>
    <v-col cols="6" id="background">
      <img :src="logoWCA" alt="" />
    </v-col>
    <v-col cols="6">
      <v-col cols="10">
        <v-form ref="form" @submit.prevent="authenticate()">
          <div class="text-h4 mb-10 mt-16 pt-16" style="color: #000066">
            Login
          </div>

          <v-text-field variant="outlined" density="compact" label="Email" type="email" v-model="formData.email"
            :rules="emailRules" required></v-text-field>

          <v-text-field variant="outlined" density="compact" label="Senha" type="password" v-model="formData.password"
            :rules="[(v) => !!v || 'Senha é obrigatório']" required></v-text-field>

          <v-btn type="submit" block color="primary" size="large" class="mt-5" v-if="!isBusy">
            Entrar
          </v-btn>
          <div class="text-center" v-else>
            <v-progress-circular indeterminate :size="40"></v-progress-circular>
          </div>
          <div class="text-center mt-5">
            <router-link to="/recuperar-senha" class="text-grey text-decoration-none">
              Esqueci minha senha
            </router-link>
          </div>

        </v-form>
      </v-col>
    </v-col>
  </v-row>
</template>
<script setup>
import { ref, inject } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/store/auth.store";
import handleErrors from "@/helpers/HandleErrors";

// VARIABLES
let isBusy = ref(false);
const logoWCA = ref(require("../../assets/images/logoWCA.png"));
const formData = ref({
  email: "",
  password: "",
});
const authStore = useAuthStore();
const router = useRouter();
const form = ref(null);
const emailRules = ref([
  (v) => !!v || "E-mail é obrigatório",
  (v) => /.+@.+\..+/.test(v) || "E-mail deve ser válido",
]);
const swal = inject("$swal");

//VUE FUNCTIONS

//FUNCTIONS
async function authenticate()
{
  try
  {
    isBusy.value = true;
    let { valid } = await form.value.validate();
    if (valid)
    {
      let response = await authStore.authenticate(formData.value);
      if (response.authenticated == true)
      {
        router.push({ name: "home" });
      } else
      {
        swal({
          toast: true,
          icon: "warning",
          position: "top-end",
          text: "E-mail e/ou senha inválidos!",
          showConfirmButton: false,
          timer: 2000,
        });
      }
    }
  } catch (error)
  {
    console.log("login.error:", error);
    handleErrors(error)
  } finally
  {
    isBusy.value = false;
  }
}
</script>

<style scoped>
img {
  position: relative;
  width: 80%;
  left: 30px;
  top: 120px;
}

#background {
  display: flex;
  flex-direction: column;

  left: 0;
  width: 50%;
  min-height: 103vh;
  background-color: #e3f3f4;
}
</style>