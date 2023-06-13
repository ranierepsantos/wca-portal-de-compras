<template>
  <v-menu activator="parent">
    <v-list :lines="false" density="compact" nav>
      <v-list-item
        class="text-primary"
        v-for="sistema in authStore.user.sistemas.filter(
          (x) => x.id != authStore.sistema.id
        )"
        @click="router.push({ name: sistema.nome.toLowerCase() })"
      >
        <template v-slot:prepend>
          <v-icon :icon="sistema.icon" size="small"></v-icon>
        </template>
        <v-list-item-title>{{ sistema.descricao }}</v-list-item-title>
      </v-list-item>
      <v-list-item class="text-primary" @click="logout()">
        <template v-slot:prepend>
          <v-icon icon="mdi-export" size="small"></v-icon>
        </template>
        <v-list-item-title v-text="'Sair'"></v-list-item-title>
      </v-list-item>
    </v-list>
  </v-menu>
</template>

<script setup>
import { useAuthStore } from "@/store/auth.store";
import { useRouter } from "vue-router";

const authStore = useAuthStore();
const router = useRouter();

function logout() {
  authStore.finishSession();
  router.push({ name: "login" });
}
</script>
