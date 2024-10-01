<template>
  <v-row>
    <v-col cols="12" sm="6" id="background">
      <img :src="logoWCA" alt="" />
    </v-col>
    <v-col cols="12" sm="6">
      <v-col cols="10" class="ml-10">
        <div class="text-h4 mb-10 mt-16 pt-16" style="color: #000066">
          Login
        </div>
        <formlogin :is-busy="isBusy" @autenticar="(data) => authenticate(data)" ref="form" />

        <div class="text-center mt-5">
          <router-link to="/recuperar-senha" class="text-grey text-decoration-none" v-show="!isBusy">
            Esqueci minha senha
          </router-link>
        </div>
      </v-col>
    </v-col>
  </v-row>
</template>
<script setup>
import { ref, inject } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/store/auth.store";
import handleErrors from "@/helpers/HandleErrors";
import formlogin from "@/components/formlogin.vue";

// VARIABLES
let isBusy = ref(false);
const logoWCA = ref(require("../../assets/images/logoWCA.png"));

const authStore = useAuthStore();
const router = useRouter();
const swal = inject("$swal");

//VUE FUNCTIONS

//FUNCTIONS
async function authenticate(loginData) {
  try {
    isBusy.value = true;
    let response = await authStore.authenticate(loginData);
    if (response.authenticated == true) {
      if (authStore.user.sistemas.length == 1) 
        router.push({name: authStore.user.sistemas[0].nome.toLowerCase()})
      else
        router.push({ name: "sistemas" });
    } else {
      swal({
        toast: true,
        icon: "warning",
        position: "top-end",
        text: "E-mail e/ou senha inválidos!",
        showConfirmButton: false,
        timer: 2000,
      });
    }
  } catch (error) {
    console.log("login.error:", error);
    handleErrors(error)
  } finally {
    isBusy.value = false;
  }
}
</script>

<style scoped>
img {
  position: relative;
  width: 80%;
  left: 50px;
  top: 120px;
}

#background {
  display: flex;
  flex-direction: column;

  left: 0;
  width: 50%;
  /* background-color: #e3f3f4; */
}
</style>