<template>
  <div>
    <v-row>
      <v-col>
        <v-text-field
          label="Nome"
          v-model="user.nome"
          type="text"
          required
          variant="outlined"
          color="primary"
          :rules="[(v) => !!v || 'Nome é obrigatório']"
          density="compact"
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-text-field
          label="Email"
          v-model="user.email"
          type="email"
          required
          variant="outlined"
          color="primary"
          :rules="emailRules"
          density="compact"
        ></v-text-field>
      </v-col>
      <v-col>
        <v-select
          label="Filial"
          :items="listFilial"
          variant="outlined"
          color="primary"
          item-title="text"
          item-value="value"
          v-model="user.filialid"
          :disabled="authStore.user.filial != 1"
          :rules="[(v) => !!v || 'Filial é obrigatório']"
          density="compact"
        ></v-select>
      </v-col>
      <v-col cols="2" v-show="user.id > 0">
        <v-checkbox
          v-model="user.ativo"
          label="Ativo"
          color="primary"
        ></v-checkbox>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useAuthStore } from "@/store/auth.store";

defineProps({
  user: {
    type: Object,
    default: function () {
      return {
        id: 0,
        nome: "",
        email: "",
        ativo: true,
        filialid: null
      };
    },
  },
  listFilial: {
    type:Array,
    default: () =>{ return []; }
  }
});
const authStore = useAuthStore();
const emailRules = ref([
  (v) => !!v || "E-mail é obrigatório",
  (v) => /.+@.+\..+/.test(v) || "E-mail deve ser válido",
]);

</script>
