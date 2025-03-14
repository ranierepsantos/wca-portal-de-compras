<template>
  <div>
    <v-row>
      <v-col>
        <h3 class="text-left text-grey mt-2">ANEXOS</h3>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-row>
          <v-col cols="3">
            <v-select
              label="Tipo"
              :items="comboItems"
              density="compact"
              item-title="text"
              item-value="text"
              variant="outlined"
              color="primary"
              v-model="tipo"
              v-show="!isReadOnly"
            ></v-select>
          </v-col>
          <v-col>
            <v-file-input
              accept=".pdf,.doc,.docx"
              label="Anexar arquivo"
              variant="outlined"
              density="compact"
              @change="handleFile()"
              :clearable="true"
              v-model="fileInput"
              v-show="!isReadOnly"
              :disabled="tipo == null"
            ></v-file-input>
          </v-col>
        </v-row>
        

        <v-table>
          <thead>
            <tr>
              <th class="text-left text-grey">Arquivo</th>
              <th class="text-left text-grey">Tipo</th>
              <th class="text-center text-grey">Ações</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in anexos" :key="item.id">
              <td class="text-left">
                <v-icon icon="mdi-file-document-check-outline"></v-icon>
                &nbsp;{{ item.descricao }}
              </td>
              <td class="text-left">{{ item.tipo }}</td>
              <td class="text-center">
                <v-btn
                  icon="mdi-file-download-outline"
                  variant="plain"
                  color="primary"
                  @click="downloadFile(item.caminhoArquivo, item.descricao)"
                ></v-btn>
                <v-btn
                  variant="plain"
                  color="error"
                  icon="mdi-trash-can-outline"
                  title="Exluir"
                  @click="removeFile(item)"
                  v-show="!isReadOnly"
                >
                </v-btn>
              </td>
            </tr>
          </tbody>
        </v-table>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import { ref, inject } from "vue";
import axios from "axios";
import { realizarDownload, toBase64 } from "@/helpers/functions";
import { Anexo } from "@/store/share/solicitacao.store";

const props = defineProps({
  anexos: {
    type: Array,
    default: function () {
      return [];
    },
  },
  comboItems: {
    type: Array,
    default: function () {
      return [];
    },
  },
  isReadOnly: { type: Boolean, default: false },
});

const swal = inject("$swal");
const fileInput = ref([]);
const tipo = ref(null);
const emit = defineEmits(['changeStatus'])

async function handleFile() {
  console.log("handle.file", fileInput.value)
  if (fileInput.value.size > 0) {
    let file = fileInput.value;
    var arquivo = await toBase64(file);
    let anexo = new Anexo();
    anexo.caminhoArquivo = arquivo;
    anexo.descricao = file.name;
    anexo.tipo = tipo.value 
    let _check = props.anexos.find(
      (q) => q.descricao.toLowerCase() == anexo.descricao.toLowerCase()
    );

    if (_check) {
      swal.fire({
        toast: true,
        icon: "info",
        position: "top-end",
        title: "Info",
        text: "Arquivo já existe na lista",
        showConfirmButton: false,
        timer: 3000,
      });
    } else {
      props.anexos.push(anexo);
      emit('changeStatus', anexo.tipo);
    }
    _check = null;
    fileInput.value = [];
  }
}

function downloadFile(url, nomeArquivo) {
  axios({
    url: url, // Download File URL Goes Here
    method: "GET",
    responseType: "blob",
    headers: {
      "Access-Control-Allow-Origin": "*",
      "Access-Control-Allow-Methods": " GET, PUT, POST, DELETE, OPTIONS",
      "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token",
      "Access-Control-Allow-Credentials": "false",
    },
  }).then(async (res) => {
    await realizarDownload(res, nomeArquivo);
  });
}

async function removeFile(item) {
  let options = {
    title: "Confirmação",
    html: `Confirma a exclusão do arquivo?`,
    icon: "question",
    showCancelButton: true,
    confirmButtonText: "Sim",
    cancelButtonText: "Não",
  };

  let response = await swal.fire(options);
  if (response.isConfirmed) {
    let index = props.anexos.findIndex((q) => q.descricao == item.descricao);
    if (index > -1) {
      props.anexos.splice(index, 1);
    }
  }
}
</script>
