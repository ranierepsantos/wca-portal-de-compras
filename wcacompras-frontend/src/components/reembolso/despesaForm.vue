<template>
  <v-container class="justify-center">
    <v-card>
      <v-breadcrumbs>
        <div class="text-h6 text-primary">Detalhamento de Despesa</div>
        <v-spacer></v-spacer>
        <v-btn
          color="primary"
          variant="outlined"
          class="text-capitalize"
          @click="emit('cancelaClick')"
        >
          <b>Cancelar</b>
        </v-btn>

        <v-btn
          color="primary"
          variant="outlined"
          class="text-capitalize"
          @click="submitData()"
          style="margin-left: 5px"
        >
          <b>Salvar</b>
        </v-btn>
      </v-breadcrumbs>
      <v-card-text>
        <v-row>
          <v-col>
            <v-form ref="despesaForm">
              <v-row>
                <v-col>
                  <v-text-field
                    label="Data Evento"
                    type="date"
                    variant="outlined"
                    color="primary"
                    density="compact"
                    v-model="despesa.dataEvento"
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-select
                    label="Tipo Despesa"
                    :items="comboTipoDespea"
                    density="compact"
                    item-title="nome"
                    item-value="id"
                    variant="outlined"
                    color="primary"
                    v-model="despesa.tipoDespesaId"
                    :rules="[(v) => !!v || 'Campo obrigatório']"
                  ></v-select>
                </v-col>
              </v-row>
              <div v-if="despesaTipo.tipo == 1">
                <v-row>
                  <v-col>
                    <v-text-field
                      label="Nrº Nota/Cupom Fiscal"
                      type="text"
                      variant="outlined"
                      color="primary"
                      density="compact"
                      v-model="despesa.numeroFiscal"
                      :rules="
                        despesaTipo.tipo == 1
                          ? [(v) => !!v || 'Campo é obrigatório']
                          : []
                      "
                    ></v-text-field>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-text-field
                      label="CNPJ"
                      type="text"
                      variant="outlined"
                      color="primary"
                      density="compact"
                      v-model="despesa.cnpj"
                      v-maska="'##.###.###/####-##'"
                      :rules="
                        despesaTipo.tipo == 1
                          ? [(v) => !!v || 'Campo é obrigatório']
                          : []
                      "
                    ></v-text-field>
                  </v-col>
                  <v-col>
                    <v-text-field
                      label="Inscrição Estadual"
                      type="text"
                      variant="outlined"
                      color="primary"
                      density="compact"
                      v-model="despesa.inscricaoEstadual"
                      v-maska="'###.###.###.###'"
                      :rules="
                        despesaTipo.tipo == 1
                          ? [(v) => !!v || 'Campo é obrigatório']
                          : []
                      "
                    ></v-text-field>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-text-field
                      label="Razão Social"
                      type="text"
                      variant="outlined"
                      color="primary"
                      density="compact"
                      v-model="despesa.razaoSocial"
                      :rules="
                        despesaTipo.tipo == 1
                          ? [(v) => !!v || 'Campo é obrigatório']
                          : []
                      "
                    ></v-text-field>
                  </v-col>
                </v-row>
              </div>
              <div v-else>
                <v-row>
                  <v-col>
                    <v-text-field
                      label="Origem"
                      type="text"
                      variant="outlined"
                      color="primary"
                      density="compact"
                      v-model="despesa.origem"
                      :rules="
                        despesaTipo.tipo == 2
                          ? [(v) => !!v || 'Campo é obrigatório']
                          : []
                      "
                    ></v-text-field>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-text-field
                      label="Destino"
                      type="text"
                      variant="outlined"
                      color="primary"
                      density="compact"
                      v-model="despesa.destino"
                      :rules="
                        despesaTipo.tipo == 2
                          ? [(v) => !!v || 'Campo é obrigatório']
                          : []
                      "
                    ></v-text-field>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-text-field
                      label="Km percorrido"
                      type="text"
                      variant="outlined"
                      color="primary"
                      density="compact"
                      v-model="despesa.kmPercorrido"
                      :rules="
                        despesaTipo.tipo == 2
                          ? [(v) => !!v || 'Campo é obrigatório']
                          : []
                      "
                      @update:model-value="calcularValor()"
                    ></v-text-field>
                  </v-col>
                </v-row>
              </div>
              <v-row>
                <v-col>
                  <v-text-field-money
                    label="Valor"
                    :number-decimal="2"
                    density="compact"
                    v-model="despesa.valor"
                    :rules="
                      despesaTipo.tipo == 1
                        ? [(v) => !!v || 'Campo é obrigatório']
                        : []
                    "
                    :disabled="despesaTipo.tipo == 2"
                  ></v-text-field-money>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-textarea
                    variant="outlined"
                    label="Motivo"
                    class="text-primary"
                    v-model="despesa.motivo"
                    no-resize
                    rows="3"
                    :rules="[(v) => !!v || 'Campo obrigatório']"
                  >
                  </v-textarea>
                </v-col>
              </v-row>
            </v-form>
          </v-col>
          <v-col v-show="despesaTipo.tipo==1">
            <dropzone
              @drop.prevent="drop"
              @change="selectedFile"
              style="margin-left: auto; margin-right: auto"
              v-show="despesa.imagePath == ''"
            />
            <v-btn
              density="compact"
              icon="mdi-close"
              style="margin-bottom: 3px; margin-right: 15px"
              @click="clearImage()"
              v-show="despesa.imagePath != ''"
            ></v-btn>
            <div
              id="img-preview"
              style="margin-left: auto; margin-right: auto"
              v-show="despesa.imagePath != ''"
            >
              <img
                :src="despesa.imagePath"
                style="max-width: 320px; height: auto"
              />
            </div>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import dropzone from "@/components/dropzone.vue";
import { Despesa } from "@/store/reembolso/solicitacao.store";
import { watch } from "vue";
import { computed } from "vue";
import { ref } from "vue";

const props = defineProps({
  despesa: {
    type: Despesa,
    default: function () {
      return new Despesa();
    },
  },
  comboTipoDespea: {
    type: Array,
    default: function () {
      return [];
    },
  },
});

const despesaForm = ref(null);
const emit = defineEmits(["cancelaClick", "changeImage", "changeValor", "saveClick"]);

const dropzoneFile = ref("");
const tipoEscolhido = ref({
  tipo: 1,
  valor: 0
})

watch( () => tipoEscolhido.value.tipo, (novoTipo) => {
  if (novoTipo == 1) {
    props.despesa.kmPercorrido = 0
    props.despesa.destino = "";
    props.despesa.origem = "";
    
  } else {
    props.despesa.cnpj = ""
    props.despesa.inscricaoEstadual = ""
    props.despesa.razaoSocial= ""
    props.despesa.nroFiscal = ""
  }
  props.despesa.valor = 0
})

const despesaTipo = computed(() => {
  let _tipo = { tipo: 1,  valor: 0 }

  if (props.despesa.tipoDespesaId > 0) {
    let tipo = props.comboTipoDespea.find(
      (q) => q.id == props.despesa.tipoDespesaId
    );
    _tipo  = tipo ? { tipo: tipo.tipo, valor: tipo.valor} : _tipo;
  }
  tipoEscolhido.value.tipo = _tipo.tipo
  tipoEscolhido.value.valor = _tipo.valor
  return _tipo;
});



function calcularValor() {
    let valor = parseFloat(props.despesa.kmPercorrido) * parseFloat(tipoEscolhido.value.valor)
    props.despesa.valor = valor;
}

function clearImage() {
  dropzoneFile.value = "";
  emit("changeImage", dropzoneFile.value);
}

function drop(e) {
  dropzoneFile.value = e.dataTransfer.files[0];
  getImgData();
}

function selectedFile() {
  dropzoneFile.value = document.querySelector(".dropzoneFile").files[0];
  getImgData();
}

function getImgData() {
  const files = dropzoneFile.value;
  if (files) {
    const fileReader = new FileReader();
    fileReader.readAsDataURL(files);
    fileReader.addEventListener("load", function () {
      //imgPreview.style.display = "block"
      emit("changeImage", this.result);
    });
  }
}

async function submitData() {
  let { valid } = await despesaForm.value.validate();
  console.log("Form valido: ", valid);
  if (valid) {
    emit('saveClick')
  }
}
</script>
