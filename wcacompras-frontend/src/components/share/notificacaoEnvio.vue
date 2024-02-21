<template>
    <v-form ref="oForm">
      <v-card>
        <v-row>
          <v-col>
            <v-card-title class="text-primary text-h5 text-left mb-2 mt-2">
              Enviar Notificação
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
                <v-autocomplete
                clearable
                chips
                label="Para"
                :items="usuarioList"
                item-value="value"
                item-title="text"
                multiple
                variant="outlined"
                :rules="[(v) => !!v || 'Campo obrigatório']"
                v-model="usuarioNotificar"
                ></v-autocomplete>
            </v-col>
          </v-row>
            <v-row>
            <v-col>
              <v-textarea
                counter
                variant="outlined"
                label="Nota"
                class="text-primary"
                v-model="evento.nota"
                :rules="[(v) => !!v || 'Campo obrigatório', v => v.length <= 500 || 'Max 500 caracteres']"
              >
              </v-textarea>
            </v-col>
          </v-row>
          
          <v-row>
            <v-col class="text-right">
              <v-progress-circular
                color="primary"
                indeterminate
                v-show="isRunningEvent"
              ></v-progress-circular>
              <v-btn
                color="success"
                :disabled="isRunningEvent"
                >Enviar</v-btn
              >
              &nbsp;
              <v-btn
                class="mr-3"
                style="font-weight: bold"
                @click="$emit('closeForm')"
                :disabled="isRunningEvent"
                >Cancelar</v-btn
              >
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
    </v-form>
  </template>
  
  <script setup>
  
import { Evento } from '@/helpers/share/classes';
import { ref } from 'vue';
  
  // data
  const props =  defineProps({
      usuarioList: { Type: Array, default: function() { return []} },
      entidade: {Type: String, default: "" },
      entidadeId: {Type: Number, default: 0 }

  });
  const emit = defineEmits(['closeForm', 'sendClick'])
  
  const oForm = ref(null)
  const usuarioNotificar = ref([])
  const evento = ref(new Evento())
  const isRunningEvent = ref(false)
  
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