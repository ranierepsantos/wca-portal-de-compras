<template>
  <v-form ref="oForm">
    <v-card>
      <v-row>
        <v-col>
          <v-card-title class="text-primary text-h5 text-left mb-2 mt-2">
            {{title}}
          </v-card-title>
        </v-col>
        <v-col class="text-right">
          <v-btn
            class="mb-2 mt-3 mr-2"
            variant="plain"
            color="grey"
            @click="$emit('closeForm')"
            icon="mdi-close-circle-outline"
          ></v-btn>
        </v-col>
      </v-row>
      <v-card-text>
        <v-row>
          <v-col>
            <v-textarea
              variant="outlined"
              label="Comentário"
              class="text-primary"
              v-model="comentario"
              :rules="[(v) => !!v || 'Campo obrigatório']"
            >
            </v-textarea>
          </v-col>
        </v-row>
        <v-row>
          <v-col class="text-right">
            <v-progress-circular v-show="isRunningEvent"
              color="primary"
              indeterminate
            ></v-progress-circular>
          </v-col>
          <v-col class="text-right">
            <v-btn
              color="success"
              @click="aprovarReprovar(true)"
              :disabled="isRunningEvent"
              >Aprovar</v-btn
            >
            &nbsp;
            <v-btn
              color="#EDCCCC"
              class="mr-3"
              style="color: #950000; font-weight: bold"
              @click="aprovarReprovar(false)"
              :disabled="isRunningEvent"
              >{{reprovarTitle}}</v-btn
            >
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-form>
</template>

<script setup>
import { ref } from 'vue';

// data
const props =  defineProps({
    title: "",
    isRunningEvent: false,
    reprovarTitle: {Type: String, default: "Rejeitar"}
});
const emit = defineEmits(['closeForm', 'aprovarClick', 'reprovarClick'])

const comentario = ref("")
const oForm = ref(null)


//functions

const aprovarReprovar = async (isAproved) => {
  let valido = true;
  if (!isAproved) {
    let { valid } = await oForm.value.validate();
    valido = valid;
  }
  if (valido)
  {
      let evento = isAproved? 'aprovarClick': 'reprovarClick';
      emit(evento, comentario.value) 
  }
}

</script>