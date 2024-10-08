<template>
  <v-breadcrumbs>
    <v-spacer></v-spacer>
    <v-btn
      color="primary"
      variant="outlined"
      class="text-capitalize"
      :disabled="isDownload"
      title="Gerar PDF"
      @click="gerarPDF()"
    >
      <v-icon icon="mdi-download" />
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
  <v-container
    id="pdfArea"
    class="justify-center"
    style="max-width: 1024px"
    ref="mytest"
    v-else
  >
    <div id="div-solicitacao">
        <v-row>
        <v-col class="text-left">
            <h3>VAGA - SOLICITAÇÃO # {{ solicitacao.id }}</h3>
        </v-col>
        </v-row>
      <v-row>
        <v-col>
          <v-text-field
            variant="outlined"
            label="Cliente"
            
            v-model="solicitacao.clienteNome"
            density="compact"
            :readonly="true"
          >
          </v-text-field>
        </v-col>
      </v-row>
        <v-row class="mb-8">
        <v-col class="text-left mr-3 ml-3 rounded" style="border: 1px solid #BDBDBD">
            <label for="" style="position: absolute; margin-top:-21px; background-color: white; font-size: small; color:gray ">&nbsp; Documentos Complementares &nbsp;</label>
            <p  v-html="enderecoCliente"></p>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
            <v-text-field
            variant="outlined"
            label="Tipo Contrato"
            
            v-model="solicitacao.vaga.tipoContratoNome"
            density="compact"
            :readonly="true"
          >
          </v-text-field>
        </v-col>
        <v-col>
            
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.tipoFaturamentoId"
            :combo-items="[]"
            :select-mode="false"
            label-text="Tipo Faturamento"
            :text-field-value="solicitacao.vaga.tipoFaturamentoNome"
            
            button-title="Adicionar Tipo Faturamento"
            @button-click="$emit('selectButtonClick', 'TipoFaturamento')"
            
          ></select-text>
        </v-col>
      </v-row>
      
      <v-row>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.gestorId"
            :combo-items="[]"
            :select-mode="false"
            label-text="Gestor"
            :text-field-value="solicitacao.vaga.gestorNome"
            
            button-title="Adicionar Gestor"
            @button-click="$emit('selectButtonClick', 'Gestor')"
          ></select-text>
        </v-col>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.funcaoId"
            :combo-items="[]"
            :select-mode="false"
            label-text="Função"
            :text-field-value="solicitacao.vaga.funcaoNome"
            
            button-title="Adicionar Função"
            @button-click="$emit('selectButtonClick', 'Funcao')"
          ></select-text>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-text-field
            variant="outlined"
            label="Quantidade Vagas"
            
            v-model="solicitacao.vaga.quantidadeVagas"
            density="compact"
            
            type="number"
            :readonly="true"
          >
          </v-text-field>
        </v-col>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.sexoId"
            :combo-items="[]"
            :select-mode="false"
            label-text="Sexo"
            :text-field-value="solicitacao.vaga.sexoNome"
            
          ></select-text>
        </v-col>
        <v-col>
          <v-text-field
            variant="outlined"
            label="Idade Miníma"
            
            v-model="solicitacao.vaga.idadeMinima"
            density="compact"
            
            type="number"
            :readonly="true"
          >
          </v-text-field>
        </v-col>
        <v-col>
          <v-text-field
            variant="outlined"
            label="Idade máxima"
            
            v-model="solicitacao.vaga.idadeMaxima"
            density="compact"
            
            type="number"
          >
          </v-text-field>
        </v-col>
      </v-row>
      <v-row class="mb-6">
        <v-col class="text-left mr-3 ml-3 rounded" style="border: 1px solid #BDBDBD">
            <label for="" style="position: absolute; margin-top:-21px; background-color: white; font-size: small; color:gray ">&nbsp; Indicação &nbsp;</label>
            <p  v-html="indicacaoProfissional"></p>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.motivoContratacaoId"
            :combo-items="[]"
            :select-mode="false"
            label-text="Motivo Contratação"
            :text-field-value="solicitacao.vaga.motivoContratacaoNome"
            
          ></select-text>
        </v-col>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.permiteFumante"
            :combo-items="[]"
            :select-mode="false"
            label-text="Permite Fumante"
            :text-field-value="
              solicitacao.vaga.permiteFumante == 1 ? 'Sim' : 'Não'
            "
          ></select-text>
        </v-col>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.escolaridadeId"
            :combo-items="[]"
            :select-mode="false"
            label-text="Escolaridade"
            :text-field-value="solicitacao.vaga.escolaridadeNome"
            
            button-title="Adicionar Escolaridade"
            @button-click="$emit('selectButtonClick', 'Escolaridade')"
          ></select-text>
        </v-col>
      </v-row>
      <v-row class="mb-6">
        <v-col class="text-left mr-3 ml-3 rounded" style="border: 1px solid #BDBDBD">
            <label for="" style="position: absolute; margin-top:-21px; background-color: white; font-size: small; color:gray ">&nbsp; Justificativa &nbsp;</label>
            <p  v-html="justificativa"></p>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-text-field
            variant="outlined"
            label="Local residência"
            
            v-model="solicitacao.vaga.localResidencia"
            density="compact"
            
            :readonly="true"
          >
          </v-text-field>
        </v-col>
        <v-col>
          <v-text-field
            variant="outlined"
            label="Experiência profissional"
            
            v-model="solicitacao.vaga.experienciaProfissinal"
            density="compact"
            
            :readonly="true"
          >
          </v-text-field>
        </v-col>
      </v-row>
      <v-row class="mb-6">
        <v-col class="text-left mr-3 ml-3 rounded" style="border: 1px solid #BDBDBD">
            <label for="" style="position: absolute; margin-top:-21px; background-color: white; font-size: small; color:gray ">&nbsp; Descrição Atividades &nbsp;</label>
            <p  v-html="descricaoAtividade"></p>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.temCNH"
            :combo-items="[]"
            :select-mode="false"
            label-text="Tem CNH"
            :text-field-value="solicitacao.vaga.temCNH == 1 ? 'Sim' : 'Não'"
          ></select-text>
        </v-col>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.categoriaCNH"
            :combo-items="[]"
            :select-mode="false"
            label-text="Categoria CNH"
            :text-field-value="solicitacao.vaga.categoriaCNH"
          ></select-text>
        </v-col>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.temValeTransporte"
            :combo-items="[]"
            :select-mode="false"
            label-text="Vale Transporte"
            :text-field-value="
              solicitacao.vaga.temValeTransporte == 1 ? 'Sim' : 'Não'
            "
          ></select-text>
        </v-col>
        <v-col>
          <v-text-field-money
            label-text="Valor Transporte"
            v-model="solicitacao.vaga.valorValeTransporte"
            color="primary"
            :number-decimal="2"
            prefix="R$"
            :readonly="true"
          ></v-text-field-money>
        </v-col>
        <v-col>
          <v-text-field
            variant="outlined"
            label="Quant. dias Vale Transporte"
            
            v-model="solicitacao.vaga.diasValeTransporte"
            density="compact"
            
            type="number"
            :readonly="true"
          >
          </v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-text-field
            variant="outlined"
            label="Refeição"
            
            v-model="solicitacao.vaga.refeicao"
            density="compact"
            
            :readonly="true"
          >
          </v-text-field>
        </v-col>
        <v-col>
          <v-text-field
            variant="outlined"
            label="Outros benefícios"
            
            v-model="solicitacao.vaga.outrosBeneficios"
            density="compact"
            
            :readonly="true"
          >
          </v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-text-field-money
            label-text="Salário base"
            v-model="solicitacao.vaga.salarioBase"
            color="primary"
            :number-decimal="2"
            prefix="R$"
            :readonly="true"
          ></v-text-field-money>
        </v-col>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.temInsalubridade"
            :combo-items="[]"
            :select-mode="false"
            label-text="Insalubridade"
            :text-field-value="
              solicitacao.vaga.temInsalubridade == 1 ? 'Sim' : 'Não'
            "
          ></select-text>
        </v-col>
        <v-col>
          <v-text-field-money
            label-text="Percentual Insalubridade"
            v-model="solicitacao.vaga.percentualInsalubridade"
            color="primary"
            :number-decimal="2"
            sufix="%"
            :readonly="true"
          ></v-text-field-money>
        </v-col>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.temPericulosidade"
            :combo-items="[]"
            :select-mode="false"
            label-text="Periculosidade"
            :text-field-value="
              solicitacao.vaga.temPericulosidade == 1 ? 'Sim' : 'Não'
            "
          ></select-text>
        </v-col>
        <v-col>
          <v-text-field-money
            label-text="Percentual Periculosidade"
            v-model="solicitacao.vaga.percentualPericulosidade"
            color="primary"
            :number-decimal="2"
            sufix="%"
            :readonly="true"
          ></v-text-field-money>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="4">
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.escalaId"
            :combo-items="[]"
            :select-mode="false"
            label-text="Escala"
            :text-field-value="solicitacao.vaga.escalaNome"
            
            button-title="Adicionar Escala"
            @button-click="$emit('selectButtonClick', 'Escala')"
          ></select-text>
        </v-col>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.horarioId"
            :combo-items="[]"
            :select-mode="false"
            label-text="Horário"
            
            :text-field-value="solicitacao.vaga.horarioNome"
            button-title="Adicionar Horário"
            @button-click="$emit('selectButtonClick', 'Horario')"
          ></select-text>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-text-field
            label="Data Início Prevista"
            v-model="solicitacao.vaga.dataInicioPrevista"
            type="Date"
            variant="outlined"
            
            density="compact"
            :readOnly="true"
          ></v-text-field>
        </v-col>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.temCopiaAdmissaoCliente"
            :combo-items="[]"
            :select-mode="false"
            label-text="Cópia Admissao Cliente"
            :text-field-value="
              solicitacao.vaga.temCopiaAdmissaoCliente == 1 ? 'Sim' : 'Não'
            "
          ></select-text>
        </v-col>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.temIntegracaoCliente"
            :combo-items="[]"
            :select-mode="false"
            label-text="Integração Cliente"
            :text-field-value="
              solicitacao.vaga.temIntegracaoCliente == 1 ? 'Sim' : 'Não'
            "
          ></select-text>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-text-field
            variant="outlined"
            label="Horário Integração"
            
            v-model="solicitacao.vaga.horarioIntegracao"
            density="compact"
            
            :readonly="true"
          >
          </v-text-field>
        </v-col>
        <v-col>
          <select-text :print-mode="true"
            v-model="solicitacao.vaga.integracaoDiasSemanas"
            :combo-items="[]"
            :select-mode="false"
            label-text="Dias da Semana"
            :text-field-value="integracaoDiasSemanaText"
            :is-multiple="true"
          ></select-text>
        </v-col>
      </v-row>
      
    </div>
    <div id="div-text">
        <v-row class="mb-8"></v-row>
        <v-row class="mb-8">
        <v-col class="text-left mr-3 ml-3 rounded" style="border: 1px solid #BDBDBD">
            <label for="" style="position: absolute; margin-top:-21px; background-color: white; font-size: small; color:gray ">&nbsp; Documentos Complementares &nbsp;</label>
            <p  v-html="documentosComplementaresText"></p>
        </v-col>
      </v-row>
      <v-row>
        <v-col class="text-left mr-3 ml-3 rounded" style="border: 1px solid #BDBDBD">
            <label for="" style="position: absolute; margin-top:-21px; background-color: white; font-size: small; color:gray ">&nbsp; Andamentos &nbsp;</label>
            <p  v-html="solicitacaoDescricao"></p>
        </v-col>
      </v-row>
    </div>
  </v-container>
</template>

<script setup>
import {
  Solicitacao,
  useShareSolicitacaoStore,
} from "@/store/share/solicitacao.store";
import { computed, onBeforeMount, readonly, ref } from "vue";
import { useRoute } from "vue-router";
import selectText from "@/components/selectText.vue";
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import handleErrors from "@/helpers/HandleErrors";
import jsPDF from "jspdf";
import html2canvas from 'html2canvas';
import { PDFDocument } from "pdf-lib";

//DATA
const isBusy = ref(false);
const isDownload = ref(false);
const route = useRoute();
const solicitacao = ref(new Solicitacao());
const solicitacaoStore = useShareSolicitacaoStore();

//VUE
//FUNCTIONS
onBeforeMount(async () => {
  if (!isNaN(route.query.id) && route.query.id > 0) {
    solicitacao.value = await solicitacaoStore.getById(route.query.id);
  }
});
//COMPUTED
const integracaoDiasSemanaText = computed(() => {
  if (solicitacao.vaga && solicitacao.vaga.integracaoDiasSemana)
    return solicitacao.vaga.integracaoDiasSemana.join(", ");
  else return "Não informado";
});
const documentosComplementaresText = computed(() => {
  if (
    solicitacao.value.vaga &&
    solicitacao.value.vaga.documentoComplementares
  ) {
    let docs = solicitacao.value.vaga.documentoComplementares.map(
      (p) => p.text
    );
    
    return docs.join(", ");
  } else return "Não informado";
});
const solicitacaoDescricao = computed(() => {
    let descricao =solicitacao.value.descricao
    console.debug('desc:', descricao);
    if (descricao)
        return descricao.replace(/\n/g, "<br/>")
    return descricao;
})

const enderecoCliente = computed(() => {
    let descricao = solicitacao.value.vaga.enderecoCliente
    
    if (descricao)
        return descricao.replace(/\n/g, "<br/>")
    return descricao;
})

const justificativa = computed(() => {
    let descricao = solicitacao.value.vaga.justificativaContratacao
    
    if (descricao)
        return descricao.replace(/\n/g, "<br/>")
    return descricao;
})
const descricaoAtividade = computed(() => {
    let descricao = solicitacao.value.vaga.descricaoAtividades
    
    if (descricao)
        return descricao.replace(/\n/g, "<br/>")
    return descricao;
})
const indicacaoProfissional = computed(() => {
    let descricao = solicitacao.value.vaga.indicacao
    
    if (descricao)
        return descricao.replace(/\n/g, "<br/>")
    return descricao;
})


// WATCH

//FUNCTIONS
async function gerarPDF() {
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
      let widthProporcao = 1.6
      let heightProporcao = 1.6
      let arrCanvas = null;
      
	    await html2canvas(document.getElementById("div-solicitacao")).then((canvas) => {
        let img = canvas.toDataURL("image/png");
        const imgProps= doc.getImageProperties(img);
        const pdfWidth = width * 0.85;
        const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

        // doc.addPage();
        doc.addImage(img, "PNG",55, 50, pdfWidth, pdfHeight);
        // doc.addImage(img, "PNG",20, 10, width, height);
      })

      await html2canvas(document.getElementById("div-text"))
      .then((canvas) => {
        let img = canvas.toDataURL("image/png");
        const imgProps= doc.getImageProperties(img);
        const pdfWidth = width * 0.85;
        const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

        doc.addPage();
        doc.addImage(img, "PNG",55, 50, pdfWidth, pdfHeight);
      })

    //   await html2canvas(document.getElementById("historico")).then((canvas) => {
    //     let img = canvas.toDataURL("image/png");
    //     const imgProps= doc.getImageProperties(img);
    //     const pdfWidth = width * 0.85;
    //     const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

    //     doc.addPage();
    //     doc.addImage(img, "PNG",35, 50, pdfWidth, pdfHeight);
    //   })

      //doc.save("jspdf_html2canvas_" +  new Date().toLocaleDateString() + ".pdf");
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

    //   for (let ii = 0; ii < solicitacao.value.despesa.length; ii++)
    //   {
    //       let _file = solicitacao.value.despesa[ii].imagePath;
    //       let secondPdfBytes = await fetch(_file).then(res => res.arrayBuffer())
    //       console.log(_file,secondPdfBytes)
    //       if (_file.indexOf(".pdf") != -1){
    //         let secondDoc = await PDFDocument.load(secondPdfBytes);
    //         let secondPage = await pdfDoc.copyPages(secondDoc,secondDoc.getPageIndices());
    //         secondPage.forEach((page) => pdfDoc.addPage(page));
    //       }else {
            
    //         let _image = null;
    //         if (_file.indexOf(".jpg") != -1 || _file.indexOf(".jpeg") != -1)
    //           _image = await pdfDoc.embedJpg(secondPdfBytes)
    //         else if (_file.indexOf(".png") != -1 )
    //           _image = await pdfDoc.embedPng(secondPdfBytes)

    //         if (_image) {
    //           const dims = _image.scale(0.25)
    //           const page = pdfDoc.addPage()
    //           // Draw the JPG image in the center of the page
    //           page.drawImage(_image, {
    //             x: page.getWidth() / 2 - dims.width / 2,
    //             y: page.getHeight() / 2 - dims.height / 2,
    //             width: dims.width,
    //             height: dims.height,
    //           })
    //         }
    //       }
    //   }

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

</script>
