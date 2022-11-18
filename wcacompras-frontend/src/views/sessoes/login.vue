<template>
  <v-row>
    <v-col cols="6" id="background">
      <img :src="logoWCA" alt="" />
    </v-col>
    <v-col cols="6">
      <v-col cols="10">
        <v-form @submit.prevent="authenticate()">
          <div class="text-h4 mb-10 mt-16 pt-16" style="color: #000066">
            Login
          </div>

          <v-text-field
            label="Email"
            type="email"
            v-model="formData.email"
          ></v-text-field>

          <v-text-field
            label="password"
            type="password"
            v-model="formData.password"
          ></v-text-field>

          <v-btn type="submit" block color="primary" size="large" class="mt-10">
            Login
          </v-btn>
        </v-form>
      </v-col>
    </v-col>
  </v-row>

  <!-- <div>
    <div id="background" class="d-flex flex-row">
      <v-sheet>
        <img :src="logoWCA" alt="" />
      </v-sheet>
    </div>
    <div class="d-flex flex-row mt-10 ml-5">
      <v-sheet class="ma-2 pa-2">
        <v-form ref="form">
          <v-text-field
            label="Email address"
            placeholder="johndoe@gmail.com"
            type="email"
          ></v-text-field>

          <v-text-field
            label="password"
            placeholder="password"
            type="password"
          ></v-text-field>
        </v-form>
      </v-sheet>
    </div>

    <div id="background">
      <img :src="logoWCA" alt="" />
    </div> 
    <div></div>
  </div> -->
</template>
<script setup>
import { ref } from "vue";
import authService from "../../services/auth.service";
const logoWCA = ref(require("../../assets/images/logoWCA.png"));
const formData = ref({
  email: "",
  password: "",
});

async function authenticate() {
  console.log(
    `Email: ${formData.value.email}, password: ${formData.value.password}`
  );
  let response = await authService.login(formData.value);
  console.log(response);
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