<template>
  <v-container class="justify-center">
    <v-card>
      <v-card-text>
        <v-row>
          <v-col>
            <v-form>
              <v-row>
                <v-col>
                  <v-text-field
                    label="Data Evento"
                    type="date"
                    variant="outlined"
                    color="primary"
                    density="compact"
                    v-model = "despesa.dataEvento"
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                    <v-select
                  label="Tipo Despesa"
                  :items="[{id: 1, nome: 'Hospedagem'}, {id: 2, nome: 'Transporte'},{id: 3, nome: 'Alimentação'}]"
                  density="compact"
                  item-title="nome"
                  item-value="id"
                  variant="outlined"
                  color="primary"
                  v-model = "despesa.tipoDespesaId"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                ></v-select>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-text-field
                    label="Nrº Nota/Cupom Fiscal"
                    type="text"
                    variant="outlined"
                    color="primary"
                    density="compact"
                    v-model = "despesa.nroFiscal"
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-text-field
                    label="Fornecedor"
                    type="text"
                    variant="outlined"
                    color="primary"
                    density="compact"
                    v-model = "despesa.fornecedor"
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-text-field-money
                    label="Valor"
                    :number-decimal="2"
                    density="compact"
                    v-model="despesa.valor"
                  ></v-text-field-money>
                </v-col>
              </v-row>
              
            </v-form>
          </v-col>
          <v-col >
            <dropzone @drop.prevent="drop" @change="selectedFile" style="margin-left: auto; margin-right: auto;" v-show="despesa.comprovanteImage ==''"/>
            <v-btn density="compact" icon="mdi-close" style="margin-bottom:3px; margin-right: 15px;" @click="clearImage()" v-show="despesa.comprovanteImage !=''"></v-btn>        
            <div id="img-preview" style="margin-left: auto; margin-right: auto;" v-show="despesa.comprovanteImage !='' ">
              <img :src="despesa.comprovanteImage" style="max-width: 320px; height:auto;"/>
            </div>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import dropzone from "@/components/dropzone.vue"
import { ref } from "vue";

defineProps({
  despesa: {
    type: Object,
    default: function () {
      return {
        id: 0,
        solicitacaoId: 0,
        tipoDespesaId: null,
        dataEvento: "",
        fornecedor: "",
        nroFiscal: "",
        valor: 0.00,
        comprovanteImage: ""
      };
    }
  },
});
const emit = defineEmits(['changeImage'])

const dropzoneFile = ref("");


function clearImage() {
  dropzoneFile.value =""
  emit("changeImage", dropzoneFile.value)
}

function drop (e) {
    dropzoneFile.value = e.dataTransfer.files[0];
    getImgData()
};

function selectedFile () {
    dropzoneFile.value = document.querySelector(".dropzoneFile").files[0];
    getImgData()
};

function getImgData() {
  const files = dropzoneFile.value;
  if (files) {
    const fileReader = new FileReader();
    fileReader.readAsDataURL(files);
    fileReader.addEventListener("load", function () {
      //imgPreview.style.display = "block"
      emit("changeImage", this.result)
    });    
  }
}

</script>
