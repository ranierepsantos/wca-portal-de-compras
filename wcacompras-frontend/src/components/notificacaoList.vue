<template>
  <v-menu activator="parent">
    <v-list :lines="false" density="compact" nav>
      <v-list-item class="text-primary"
        v-for="nota in notificacoes"
      >
        <template v-slot:prepend>
          <v-icon :icon="nota.lido? 'mdi-checkbox-marked-circle-outline':'mdi-circle-outline'" size="small" @click="marcarLido(nota)"></v-icon>
        </template>
        <v-list-item-title>{{nota.nota}}</v-list-item-title>
      </v-list-item>
      
    </v-list>
  </v-menu>
</template>

<script setup>
import { useAuthStore } from "@/store/auth.store";
  const props = defineProps({
    notificacoes: {
      type: Array,
      default: () => { return []}
    }
  });
  

  function marcarLido(nota) {
    nota.lido = true
    useAuthStore().marcarNotificaoReembolso(nota.id)
  }
</script>
