<template>
    <div>
        <v-row>
            <v-col>
                <v-text-field label="Endereço" v-model="data.endereco" type="text" variant="outlined" color="primary"
                    density="compact" maxlength="150" :rules="[(v) => !!v || 'Campo é obrigatório']">
                </v-text-field>
            </v-col>
            <v-col cols="2">
                <v-text-field label="Numero" v-model="data.numero" type="text" variant="outlined" color="primary"
                    density="compact" maxlength="10" :rules="[(v) => !!v || 'Campo é obrigatório']">
                </v-text-field>
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="2">
                <v-text-field label="Cep" v-model="data.cep" type="text" variant="outlined" color="primary"
                    density="compact" v-maska="'#####-###'" :rules="[(v) => !!v || 'Campo é obrigatório']">
                </v-text-field>
            </v-col>
            <v-col>
                <v-text-field label="Cidade" v-model="data.cidade" type="text" variant="outlined" color="primary"
                    density="compact" maxlength="100" :rules="[(v) => !!v || 'Campo é obrigatório']">
                </v-text-field>
            </v-col>
            <v-col cols="2">
                <v-text-field label="UF" v-model="data.uf" type="text" variant="outlined" color="primary"
                    density="compact" v-maska="'AA'" :rules="[(v) => !!v || 'Campo é obrigatório']">
                </v-text-field>
            </v-col>
        </v-row>
    </div>
</template>

<script setup>
    //import { mask } from "maska"
    defineProps(['data'])

</script>   