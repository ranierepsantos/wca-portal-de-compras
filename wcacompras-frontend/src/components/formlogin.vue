<template>
    <v-form ref="form" @submit.prevent="submit()">
        <div class="text-right">
            <span v-once style="font-size: 10px;">{{`v${packageJsonInfo.version} ${packageJsonInfo.enviroment}`}}</span>
        </div>
        <v-text-field variant="outlined" density="compact" label="Email" type="email" v-model="formData.email"
            :rules="emailRules" autocomplete="email" required></v-text-field>

        <v-text-field variant="outlined" density="compact" label="Senha" type="password" v-model="formData.password"
            :rules="[(v) => !!v || 'Senha é obrigatório']" autocomplete="password" required></v-text-field>

        <v-btn type="submit" block color="primary" size="large" class="mt-5" v-if="!isBusy">
            Entrar
        </v-btn>
        <div class="text-center" v-else>
            <v-progress-circular indeterminate :size="40"></v-progress-circular>
        </div>
    </v-form>
</template>

<script setup>
import {ref} from 'vue'

defineProps({
    isBusy: {type: Boolean, default: false}
})
const emit = defineEmits(['autenticar'])

const form = ref(null);

const formData = ref({
  email: "",
  password: "",
});

const emailRules = ref([
  (v) => !!v || "E-mail é obrigatório",
  (v) => /.+@.+\..+/.test(v) || "E-mail deve ser válido",
]);

async function submit() {
    let { valid } = await form.value.validate();
    if (valid) {
        emit('autenticar',formData.value)
    }
}

var packageJsonInfo = require('../../package.json');

</script>