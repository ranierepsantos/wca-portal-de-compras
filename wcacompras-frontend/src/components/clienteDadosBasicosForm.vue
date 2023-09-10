<template>
  <div>
    <v-row>
      <v-col cols="8">
        <v-text-field
          label="Nome"
          v-model="cliente.nome"
          type="text"
          required
          variant="outlined"
          color="primary"
          density="compact"
          :rules="[(v) => !!v || 'Campo é obrigatório']"
        ></v-text-field>
      </v-col>
      <v-col>
        <v-text-field
          label="CNPJ"
          v-model="cliente.cnpj"
          type="text"
          required
          density="compact"
          variant="outlined"
          color="primary"
          :rules="[(v) => !!v || 'Campo é obrigatório']"
          v-maska="'##.###.###/####-##'"
        >
        </v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-text-field
          label="Inscrição Estadual"
          v-model="cliente.inscricaoEstadual"
          type="text"
          required
          density="compact"
          variant="outlined"
          color="primary"
          v-maska="'###.###.###.###'"
          :rules="[(v) => !!v || 'Campo é obrigatório']"
        ></v-text-field>
      </v-col>
      <v-col>
        <v-select
          label="Matriz/Filial"
          :items="filiais"
          variant="outlined"
          color="primary"
          item-title="text"
          item-value="value"
          v-model="cliente.filialId"
          density="compact"
          :rules="[(v) => !!v || 'Filial é obrigatório']"
          :disabled="comboFilialDisabled"
        ></v-select>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <h3 class="text-left text-grey">Endereço</h3>
        <v-divider class="mt-3"></v-divider>
      </v-col>
    </v-row>
    <endereco v-model:data="cliente"/>
  </div>
</template>

<script setup>
import { useAuthStore } from "@/store/auth.store";
import Endereco from '@/components/endereco.vue';

defineProps({
  cliente: {
    type: Object,
    default: function () {
      return {
        id: 0,
        nome: "",
        cnpj: "",
        inscricaoEstadual: "",
        endereco: "",
        numero: "",
        cep: "",
        cidade: "",
        uf: "",
        ativo: true,
        filialId: null,
      };''
    },
  },
  filiais: {
      type: Array,
      default: function() {
          return []
      }
  },
  comboFilialDisabled: {
    type: Boolean,
    default: false
  }
});
const authStore = useAuthStore();

</script>
