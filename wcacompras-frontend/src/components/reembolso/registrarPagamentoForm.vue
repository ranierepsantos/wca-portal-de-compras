<template>
  <v-form ref="oForm">
    <v-card>
      <v-row>
        <v-col>
          <v-card-title class="text-primary text-h5 text-left mb-2 mt-2">
            Registrar Pagamento
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
                <v-text-field-money
                  label-text="Saldo Colaborador"
                  v-model="data.saldo"
                  color="primary"
                  :number-decimal="2"
                  prefix="R$"
                  :readonly="true"
                  bg-color = "#f2f2f2"
                ></v-text-field-money>
            </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-text-field
              label="Data Pagamento"
              type="date"
              variant="outlined"
              color="primary"
              density="compact"
              v-model="data.dataDeposito"
            ></v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-text-field-money
              label-text="Valor Pagamento"
              v-model="data.valorDeposito"
              color="primary"
              :number-decimal="2"
              prefix="R$"
            ></v-text-field-money>
          </v-col>
        </v-row>
        <v-row>
          <v-col class="text-right">
            <v-btn
              color="primary"
              variant="outlined"
              @click="$emit('closeForm')"
              :disabled="isRunningEvent"
              >Cancelar</v-btn
            >
            &nbsp;
            <v-btn
              color="primary"
              class="mr-3"
              @click="registrar()"
              :disabled="isRunningEvent"
              >Registrar</v-btn
            >
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-form>
</template>

<script setup>
import moment from "moment";
import { ref } from "vue";
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";

// data
const props = defineProps({
  title: "",
  isRunningEvent: false,
  data: {
    type: Object,
    default: function () {
      return {
        saldo: 0,
        dataDeposito: moment().format("YYYY-MM-DD"),
        valorDeposito: 0,
      };
    },
  },
});
const emit = defineEmits(["closeForm", "registrarPagamento"]);

const oForm = ref(null);

//functions

const registrar = async () => {
  let valido = true;
  let { valid } = await oForm.value.validate();
  if (valid) {
    emit("registrarPagamento", props.data);
  }
};
</script>
