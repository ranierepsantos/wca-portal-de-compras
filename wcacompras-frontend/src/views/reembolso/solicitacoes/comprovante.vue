<template>
  <div>
    <v-breadcrumbs>
            <v-spacer></v-spacer>
            <v-btn
              color="primary"
              variant="outlined"
              class="text-capitalize"
              @click="baixarComprovantes()"
              v-show="solicitacao.despesa.length > 0"
              :disabled="isDownload"
              title="Gerar PDF"
            >
            <v-icon icon="mdi-download"/>
              <b> Gerar PDF</b>
            </v-btn>
    </v-breadcrumbs>
    <v-progress-linear
      color="orange"
      indeterminate
      :height="5"
      v-show="isDownload"
    ></v-progress-linear>
    
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-if="isBusy"
    ></v-progress-linear>
    <v-container id = "pdfArea" class="justify-center" ref="mytest" v-else>
      
          <v-form>
            <v-row>
              <v-col>
                <h3>SOLICITAÇÃO # {{ solicitacao.id }}</h3>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-text-field
                  label="Cliente"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.cliente.nome"
                  :readonly="true"
                ></v-text-field>
                
              </v-col>
              <v-col cols="4">
                <select-text
                  
                  v-model="solicitacao.tipoSolicitacao"
                  :combo-items="solicitacaoStore.tipoSolicitacao"
                  :select-mode="false"
                  :text-field-value="solicitacao.id == 0 ? '' : solicitacaoStore.tipoSolicitacao.find(p => p.value == solicitacao.tipoSolicitacao ).text"
                  label-text="Tipo Solicitação"
                ></select-text>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-text-field
                  label="Centro de Custo"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.centroCustoNome"
                  :readonly="true"
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-text-field
                  label="Colaborador"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                  v-model="solicitacao.colaboradorNome"
                  :readonly="true"
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Cargo"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.colaboradorCargo"
                  :readonly="true"
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row>
              <v-col cols="6">
                <v-text-field
                  label="Objetivo Solicitação"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.objetivo"
                  :readonly="true"
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Período Inicial"
                  type="date"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.periodoInicial"
                  :readonly="true"
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Período Final"
                  type="date"
                  variant="outlined"
                  color="primary"
                  density="compact"
                  v-model="solicitacao.periodoFinal"
                  :readonly="true"
                  
                ></v-text-field>
              </v-col>
              <v-col v-show="solicitacao.tipoSolicitacao == 2">
                <v-text-field-money
                  label-text="Valor Solicitação"
                  v-model="solicitacao.valorAdiantamento"
                  color="primary"
                  :number-decimal="2"
                  prefix="R$"
                  :readonly="true"
                ></v-text-field-money>
              </v-col>
            </v-row>
          </v-form>
          <v-table>
            <thead>
              <tr>
                <th class="text-center text-grey">DATA</th>
                <th class="text-center text-grey">TIPO</th>
                <th class="text-center text-grey" colspan="2">DESCRICAO</th>
                <th class="text-center text-grey">VALOR</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="(item, index) in solicitacao.despesa.sort(
                  compararValor('dataEvento', 'asc')
                )"
                :key="index"
              >
                <td class="text-center">
                  {{ moment(item.dataEvento).format("DD/MM/YYYY") }}
                </td>
                <td class="text-center">
                  {{ getDespesaTipo(item.tipoDespesaId).nome }}
                </td>
                <td class="text-center" colspan="2">
                  {{
                    getDespesaTipo(item.tipoDespesaId).tipo == 1
                      ? item.razaoSocial + " : " + item.numeroFiscal
                      : item.origem + " > " + item.destino
                  }}
                </td>
                <td class="text-right">
                  {{ formatToCurrencyBRL(parseFloat(item.valor)) }}
                </td>
                
              </tr>
            </tbody>
            <tfoot>
              <tr>
                <td class="text-right" colspan="4"><b>TOTAL:</b></td>
                <td class="text-right">
                  {{ formatToCurrencyBRL(calcularTotalDespesa()) }}
                </td>
              </tr>
            </tfoot>
          </v-table>
    </v-container>
  </div>
</template>

<script setup>
import selectText from "@/components/selectText.vue";
import { ref, onMounted} from "vue";
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import { useAuthStore } from "@/store/auth.store";
import handleErrors from "@/helpers/HandleErrors";
import { formatToCurrencyBRL } from "@/helpers/functions";
import { useClienteStore } from "@/store/reembolso/cliente.store";
import {
  Usuario,
  useUsuarioStore,
} from "@/store/reembolso/usuario.store";
import {
  Solicitacao,
  useSolicitacaoStore,
} from "@/store/reembolso/solicitacao.store";
import moment from "moment";
import { useRoute } from "vue-router";
import { useDespesaTipoStore } from "@/store/reembolso/despesaTipo.store";
import { compararValor } from "@/helpers/functions";
import jsPDF from "jspdf";
import html2canvas from 'html2canvas';
import { PDFDocument } from "pdf-lib";


const authStore = useAuthStore();
const clienteStore = useClienteStore();
const despesaTipoStore = useDespesaTipoStore();
const route = useRoute();
const solicitacaoStore = useSolicitacaoStore();
const isDownload = ref(false);
const isBusy = ref(false);
const clientes = ref([]);
const solicitacao = ref(new Solicitacao());
const despesaTipos = ref([]);
const usuario = ref(new Usuario());
const mytest = ref(null)
//COMPUTED

//VUE FUNCTIONS
onMounted(async () => {
  try {
    isBusy.value = true;
    usuario.value = await useUsuarioStore().getById(authStore.user.id);
    clientes.value = await clienteStore.ListByUsuario(usuario.value.id);
    despesaTipos.value = await despesaTipoStore.toComboList();

    if (parseInt(route.query.id) > 0) {
      await getSolicitacao(route.query.id);
    }
  } catch (error) {
    console.log("onMounted.error:", error);
    handleErrors(error);
  } finally {
    isBusy.value = false;
  }
});


//FUNCTIONS
function calcularTotalDespesa() {
  let valorTotalDespesa = 0;

  solicitacao.value.despesa.forEach((item) => {
    valorTotalDespesa += parseFloat(item.valor);
  });
  valorTotalDespesa = valorTotalDespesa.toFixed(2);
  solicitacao.value.valorDespesa = valorTotalDespesa;
  return valorTotalDespesa;
}

function getDespesaTipo(id) {
  return despesaTipos.value.find((q) => q.id == id);
}

async function getSolicitacao(solicitacaoId) {
  try {
    let data = await solicitacaoStore.getById(solicitacaoId);
    data.periodoInicial = data.periodoInicial.split("T")[0];
    data.periodoFinal = data.periodoFinal.split("T")[0];
    data.despesa.forEach((element) => {
      element.dataEvento = element.dataEvento.split("T")[0];
    });
    solicitacao.value = data;
  } catch (error) {
    console.log("getSolicitacao.error:", error);
    handleErrors(error);
  }
}

async function baixarComprovantes() {
  try {
    isDownload.value = true;

    let doc = new jsPDF({
        orientation: 'p',
        unit: 'px',
        format: 'a4',
        hotfixes: ['px_scaling'],
      });
      let arrCapa = null;
      let width = doc.internal.pageSize.getWidth();
      let height = doc.internal.pageSize.getHeight();
		//1.8,1.6
      let widthProporcao = 1.8
      let heightProporcao = 1.6

      if (solicitacao.value.despesa.length > 25 && solicitacao.value.despesa.length< 50){
        widthProporcao = 1.8
        heightProporcao = 2.5
      }
      else if (solicitacao.value.despesa.length > 50)
      {
        widthProporcao = 1.8
        heightProporcao = 3.3
      }
        
	    await html2canvas(mytest.value.$el, {
        width: width * widthProporcao,
        height: height * heightProporcao
      }).then((canvas) => {
        const img = canvas.toDataURL("image/png");
        //arrCanvas = base64ToArrayBuffer(img)
        doc.addImage(img, "PNG",40, 10, width, height);
        //doc.save("jspdf_html2canvas.pdf");
        
      })
      //doc.save("jspdf_html2canvas.pdf");
      arrCapa = doc.output('arraybuffer');

      let pdfDoc = await PDFDocument.create();
      const firstDoc = await PDFDocument.load(arrCapa);
      const firstPage = firstDoc.getPages();
      //await pdfDoc.copyPages(firstDoc, [0])
      let arrPages = [];
      for (const key of firstPage.keys()) {
        arrPages.push(key);
      }
      let copiedPages = await pdfDoc.copyPages(firstDoc, arrPages);
      copiedPages.forEach((page) => pdfDoc.addPage(page));

      for (let ii = 0; ii < solicitacao.value.despesa.length; ii++)
      {
          let _file = solicitacao.value.despesa[ii].imagePath;
          let secondPdfBytes = await fetch(_file).then(res => res.arrayBuffer())
          console.log(_file,secondPdfBytes)
          if (_file.indexOf(".pdf") != -1){
            let secondDoc = await PDFDocument.load(secondPdfBytes);
            let secondPage = await pdfDoc.copyPages(secondDoc,secondDoc.getPageIndices());
            secondPage.forEach((page) => pdfDoc.addPage(page));
          }else {
            
            let _image = null;
            if (_file.indexOf(".jpg") != -1 || _file.indexOf(".jpeg") != -1)
              _image = await pdfDoc.embedJpg(secondPdfBytes)
            else if (_file.indexOf(".png") != -1 )
              _image = await pdfDoc.embedPng(secondPdfBytes)

            if (_image) {
              const dims = _image.scale(0.25)
              const page = pdfDoc.addPage()
              // Draw the JPG image in the center of the page
              page.drawImage(_image, {
                x: page.getWidth() / 2 - dims.width / 2,
                y: page.getHeight() / 2 - dims.height / 2,
                width: dims.width,
                height: dims.height,
              })
            }
          }
      }

      const pdfBytes = await pdfDoc.save();
      let file = new Blob([pdfBytes], { type: 'application/pdf' });
      var fileURL = URL.createObjectURL(file);
      window.open(fileURL);
    
  } catch (error) {
    console.error(error);
    handleErrors(error, error.response.status == 404 ? "Não há comprovantes anexos!": null);
  } finally {
    isDownload.value = false;
  }
}
</script>()
