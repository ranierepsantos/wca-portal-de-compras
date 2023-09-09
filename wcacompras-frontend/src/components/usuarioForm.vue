<template>
  <div>
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
          @blur="$emit('emailChange', $event.target.value)"
        ></v-text-field>
      </v-col>
      <v-col cols="2" v-show="user.id > 0">
        <v-checkbox
          v-model="user.ativo"
          label="Ativo"
          color="primary"
        ></v-checkbox>
      </v-col>
    </v-row>
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
    
  </div>
</template>

<script setup>
import { ref } from "vue";

defineProps({
  user: {
    type: Object,
    default: function () {
      return new Usuario();
    },
  },
  listFilial: {
    type:Array,
    default: () =>{ return []; }
  }
});

const emailRules = ref([
  (v) => !!v || "E-mail é obrigatório",
  (v) => /.+@.+\..+/.test(v) || "E-mail deve ser válido",
]);

</script>
