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
          @click="openAprovacaoForm=true"
          v-show="canAprove && (despesa.aprovada == null || despesa.aprovada == 0)"
        >
          <b>Aprovar/Rejeitar</b>
        </v-btn>

        <v-btn
          color="primary"
          variant="outlined"
          class="text-capitalize"
          @click="emit('cancelaClick')"
          style="margin-left: 5px"
        >
          <b>{{ readOnly? 'Voltar': 'Cancelar' }}</b>
        </v-btn>

        <v-btn
          color="primary"
          variant="outlined"
          class="text-capitalize"
          @click="submitData()"
          style="margin-left: 5px"
          v-show="!readOnly && (despesa.aprovada == 0 || despesa.aprovada == 2 || despesa.aprovada == null)"
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
                    :readonly="readOnly"
                    :bg-color = 'readOnly ? "#f2f2f2":"" '
                  ></v-text-field>
                </v-col>
                <v-col class="text-right" v-show="despesa.aprovada !=null && despesa.aprovada > 0 ">
                  <v-btn
                  :color="despesa.aprovada ==1 ? 'success': 'error'"
                  variant="tonal"
                  class="text-center"
                >
                  {{
                    despesa.aprovada ==1 ? 'Aprovada': 'Reprovada'
                  }}</v-btn
                >
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
                    :readonly="readOnly"
                    :bg-color = 'readOnly ? "#f2f2f2":"" '
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
                      :readonly="readOnly"
                      :bg-color = 'readOnly ? "#f2f2f2":"" '
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
                      :readonly="readOnly"
                      :bg-color = 'readOnly ? "#f2f2f2":"" '
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
                      :readonly="readOnly"
                      :bg-color = 'readOnly ? "#f2f2f2":"" '
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
                      :readonly="readOnly"
                      :bg-color = 'readOnly ? "#f2f2f2":"" '
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
                      :readonly="readOnly"
                      :bg-color = 'readOnly ? "#f2f2f2":"" '
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
                      :readonly="readOnly"
                      :bg-color = 'readOnly ? "#f2f2f2":"" '
                    ></v-text-field>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <!-- <v-text-field
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
                      :readonly="readOnly"
                      :bg-color = 'readOnly ? "#f2f2f2":"" '
                    ></v-text-field> -->
                    <v-text-field-money
                    label="KM percorrido"
                    :number-decimal="3"
                    density="compact"
                    v-model="despesa.kmPercorrido"
                    :rules="
                      despesaTipo.tipo == 2
                        ? [(v) => !!v || 'Campo é obrigatório']
                        : []
                    "
                    :readonly="readOnly"
                    :bg-color = 'readOnly ? "#f2f2f2":"" '
                    @update:model-value="calcularValor()"
                  ></v-text-field-money>
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
                    :readonly="readOnly || despesaTipo.tipo == 2"
                    :bg-color = 'readOnly ||despesaTipo.tipo == 2 ? "#f2f2f2":"" '
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
                    rows="1"
                    :rules="[(v) => !!v || 'Campo obrigatório']"
                    :readonly="readOnly"
                    :bg-color = 'readOnly ? "#f2f2f2":"" '
                  >
                  </v-textarea>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-textarea
                    variant="outlined"
                    label="Observação"
                    class="text-primary"
                    v-model="despesa.observacao"
                    no-resize
                    rows="1"
                    :readonly="readOnly"
                    :bg-color = 'readOnly ? "#f2f2f2":"" '
                    v-show="despesa.observacao!= null && despesa.observacao!=''"
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
            <br/>
            <v-alert
              density="compact"
              type="warning"
              title="Arquivo inválido"
              text="O comprovante deve ser uma imagem ou um pdf!"
              v-show="isFileInvalid"
            ></v-alert>
            <v-btn
              density="compact"
              icon="mdi-close"
              style="margin-bottom: 3px; margin-right: 15px"
              @click="clearImage()"
              v-show="despesa.imagePath != '' &&  !readOnly "
            ></v-btn>
            <div
              id="img-preview"
              style="margin-left: auto; margin-right: auto;"
              v-show="despesa.imagePath != ''"
            >
              <img
                :src="despesa.imagePath"
                style="max-width: 100%; height: 520px"
                v-if="despesa.imagePath.indexOf('pdf') ==-1"
              />
              <object v-else :data="despesa.imagePath" type="application/pdf" width="100%" height="500px">
                <p>Não foi possível exibir o PDF. <a :href="despesa.imagePath">Clique aqui</a> para download.</p>
              </object>  
            </div>
            
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
    <v-dialog
        v-model="openAprovacaoForm"
        max-width="700"
        :absolute="false"
        persistent
      >
        <aprovar-rejeitar-form
          title="DESPESA - Aprovar / Rejeitar"
          @aprovar-click="aprovarReprovar(true, $event)"
          @reprovar-click="aprovarReprovar(false, $event)"
          @close-form="openAprovacaoForm = false"
          
        />
      </v-dialog>
  </v-container>
</template>

<script setup>
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import dropzone from "@/components/dropzone.vue";
import { Despesa } from "@/store/reembolso/solicitacao.store";
import aprovarRejeitarForm from "@/components/aprovarRejeitarForm.vue";
import { watch } from "vue";
import { computed } from "vue";
import { ref,inject } from "vue";
import handleErrors from "@/helpers/HandleErrors";

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
  readOnly: {
    type: Boolean,
    default: true
  },
  canAprove: {type: Boolean, default: false}
});
const openAprovacaoForm = ref(false)
const despesaForm = ref(null);
const emit = defineEmits(["cancelaClick", "changeImage", "changeValor", "saveClick"]);

const dropzoneFile = ref("");
const tipoEscolhido = ref({
  tipo: 0,
  valor: 0
})
const isFileInvalid = ref(false);
watch( () => tipoEscolhido.value.tipo, (novoTipo, oldTipo) => {
  if (!props.readOnly && oldTipo !=0) {
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
  }
  
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


async function aprovarReprovar(isAprovado, comentario) {
  try {

    if (!isAprovado) 
      props.despesa.aprovada = 2; //rejeitado
    else
      props.despesa.aprovada = 1; //aprovado
    
    props.despesa.observacao = comentario;

    submitData(false)
    
  } catch (error) {
    console.log("aprovarReprovar.error:", error);
    handleErrors(error);
  }
}
function calcularValor() {
    let valor = parseFloat(props.despesa.kmPercorrido) * parseFloat(tipoEscolhido.value.valor)
    props.despesa.valor = parseFloat(valor.toFixed(2));
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
  isFileInvalid.value = false 
  const files = dropzoneFile.value;
  if (files) {
    const fileReader = new FileReader();
    fileReader.readAsDataURL(files);
    fileReader.addEventListener("load", function () {
      if(this.result.indexOf("data:image") ==-1 && this.result.indexOf("data:application/pdf") ==-1 )
        isFileInvalid.value = true  
      else
        emit("changeImage", this.result);
    });
  }
}

async function submitData(fromSaveButton = true) {
  let { valid } = await despesaForm.value.validate();
  if (valid) {
    if (fromSaveButton)
      props.despesa.aprovada = null

    emit('saveClick')
  }
}
</script>
