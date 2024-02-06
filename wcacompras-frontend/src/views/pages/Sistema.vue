<template>
  <v-row>
    <v-col cols="6" id="background">
      <img :src="logoWCA" alt="" />
    </v-col>
    <v-col cols="6">
      <v-col cols="10">
        <div class="text-h4 mb-10 mt-16 pt-16" style="color: #000066">
          APLICATIVOS
        </div>
        <v-row justify="center">
          <v-col cols="auto" v-for="sistema in authStore.user.sistemas">
            <v-btn :height="buttonHeight" min-width="164" @click="router.push({name: sistema.nome.toLowerCase()})">
              <v-icon
                :icon="sistema.icon"
                size="x-large"
                color="purple-darken-2"
                style="margin-right: 5px"
              ></v-icon>
              {{sistema.descricao}}
            </v-btn>
          </v-col>

          <!-- <v-col cols="auto">
            <v-btn :height="buttonHeight" min-width="164" @click="router.push({name: 'reembolso'})">
              <v-icon
                icon="mdi-account-cash-outline"
                size="x-large"
                color="purple-darken-2"
                style="margin-right: 5px"
              ></v-icon>
              Reembolso
            </v-btn>
          </v-col> -->
        </v-row>
      </v-col>
    </v-col>
  </v-row>
</template>
<script setup>
import { ref, onBeforeMount } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/store/auth.store";

// VARIABLES
const logoWCA = ref(require("@/assets/images/logoWCA.png"));

const authStore = useAuthStore();
const router = useRouter();

let buttonHeight = 90;
//VUE FUNCTIONS

onBeforeMount (() => {
  if (authStore.user.sistemas.length == 1) 
    router.push({name: authStore.user.sistemas[0].nome.toLowerCase()})
  
}) 

//FUNCTIONS

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
