<template>
    <v-app>
        <v-progress-linear color="primary" indeterminate :height="5" v-show="isBusy"></v-progress-linear>
        <v-app-bar elevation="3" rounded v-show="!isBusy">
            <div style="padding-top: 2px; margin-left: 10px;">
                <img src="@/assets/images/logoWCA.png" alt="" class="side-bar-logo" width="45" height="45"
                    style="border-radius: 20%;" />
            </div>
            <v-spacer></v-spacer>
            <v-btn variant="text" class="text-capitalize" color="primary">
                <v-icon icon="mdi-account-circle-outline" size="x-large"></v-icon>
                {{ requisicao.nomeAprovador }}

            </v-btn>
        </v-app-bar>

        <v-main v-show="!isBusy">
            <v-container fluid>
                <v-row class="ma-1">
                    <h1 class="text-primary">Pedido #{{ requisicao.id || '' }}</h1>
                    <v-spacer></v-spacer>
                    <v-btn variant="outlined" class="text-capitalize" color="primary" @click="download()"
                        v-if="!isDownloading" :disabled="requisicao.id == null">
                        <v-icon icon="mdi-download" size="x-large"></v-icon>
                        Download
                    </v-btn>
                    <v-progress-circular color="primary" indeterminate v-else></v-progress-circular>
                </v-row>
                <p>
                    <span class="text-primary wca-texto">Cliente: </span>
                    <span class="text-grey wca-texto">{{ requisicao.cliente.nome }}</span>
                </p>
                <p>
                    <span class="text-primary wca-texto">CNPJ: </span>
                    <span class="text-grey wca-texto">{{ requisicao.cliente.cnpj }}</span>
                </p>
                <p><span class="text-primary wca-texto">Endereço: </span> <span class="text-grey wca-texto">{{
                    Endereco
                }}</span></p>
                <p>
                    <span class="text-primary wca-texto">Supervisor: </span>
                    <span class="text-grey wca-texto">{{ requisicao.usuario.text }}</span>
                </p>

                <!--TABELA DE EXIBIÇÃO DOS PRODUTOS -->
                <v-row class="mt-2">
                    <v-col cols="10" offset="1">
                        <v-table class="elevation-2">
                            <thead>
                                <tr>
                                    <th class="text-center text-grey">CÓDIGO</th>
                                    <th class="text-center text-grey">PRODUTO</th>
                                    <th class="text-center text-grey">VALOR</th>
                                    <th class="text-center text-grey">QUANTIDADE</th>
                                    <th class="text-center text-grey">U. M.</th>
                                    <th class="text-center text-grey">TOTAL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="item in requisicao.requisicaoItens" :key="item.id">
                                    <td class="text-left">{{ item.codigo }}</td>
                                    <td class="text-left">{{ item.nome }}</td>
                                    <td class="text-right">{{ item.valor.toFixed(2) }}</td>
                                    <td class="text-center">{{ item.quantidade }}</td>
                                    <td class="text-center">{{ item.unidadeMedida }}</td>
                                    <td class="text-right">{{ (item.valorTotal).toFixed(2) }}
                                    </td>
                                </tr>
                            </tbody>
                        </v-table>
                    </v-col>
                </v-row>
                <!-- COMENTÁRIO -->
                <v-row class="mt-2">
                    <v-col cols="10" offset="1">
                        <v-textarea variant="outlined" label="Comentário" class="text-primary" v-model="comentario">

                        </v-textarea>
                    </v-col>
                </v-row>
                <!-- BOTÕES DE APROVAÇÃO / RECUSA -->
                <v-row>
                    <v-col cols="10" offset="1" class="text-right">
                        <v-progress-circular color="primary" indeterminate v-show="isSaving"></v-progress-circular>
                        <v-btn :color="requisicao.id == null ? 'grey' : '#EDCCCC'" class="mr-4"
                            style="color:#950000; font-weight: bold;" @click="aprovarReprovar(false)"
                            :disabled="requisicao.id == null" v-show="!isSaving">
                            Recusar
                        </v-btn>
                        <v-btn :disabled="requisicao.id == null" :color="requisicao.id == null ? 'grey' : 'success'"
                            style="font-weight: bold;" @click="aprovarReprovar(true)" v-show="!isSaving">
                            Aprovar
                        </v-btn>
                    </v-col>
                </v-row>
            </v-container>
        </v-main>

    </v-app>
</template>

<script setup>
import { ref, onMounted, inject, computed } from "vue";
import requisicaoService from "@/services/requisicao.service";
import handleErrors from "@/helpers/HandleErrors"
import { useRoute } from "vue-router";
import { realizarDownload } from "@/helpers/functions";
import router from "@/router";
//DATA
const swal = inject("$swal")
const isBusy = ref(true);
const isDownloading = ref(false);
const isSaving = ref(false)
const route = useRoute();
const requisicao = ref({
    id: null,
    cliente: {
        nome: "",
        cnpj: "",
        endereco: "",
        numero: "",
        cidade: "",
        uf: "",
        cep: ""
    },
    valorTotal: 0,
    taxaGestao: 0,
    destino: 0,
    usuario: {
        text: "",
        value: ""
    },
    nomeAprovador: "",
    requisicaoItens: []
});
const comentario = ref("")
const token = ref("");
//VUE METHODS
onMounted(async () =>
{
    token.value = route.params.token;
    await getRequisicaoData()
});
const Endereco = computed(() =>
{
    return requisicao.value.id == null ? '' : requisicao.value.cliente.endereco + ', ' + requisicao.value.cliente.numero +
        ', ' + requisicao.value.cliente.cidade + ', ' + requisicao.value.cliente.uf + ', '
        + requisicao.value.cliente.cep;
})

//METHODS
async function download()
{
    try
    {
        isDownloading.value = true;
        let response = await requisicaoService.downloadByToken(token.value);
        let nomeArquivo = response.headers['content-disposition'].split(';')[1].replace('filename=', '').trim()
        realizarDownload(response, nomeArquivo, response.headers.getContentType());

    } catch (error)
    {
        console.log("download.error:", error);
        handleErrors(error)
    } finally
    {
        isDownloading.value = false
    }
}
async function getRequisicaoData()
{
    try
    {
        isBusy.value = true;
        let response = await requisicaoService.getByToken(token.value);
        requisicao.value = response.data;
    } catch (error)
    {
        console.log("getRequisicaoData.error:", error);
        handleErrors(error)
    } finally
    {
        isBusy.value = false
    }
}

async function aprovarReprovar(isAprovado)
{
    try
    {
        isSaving.value = true;
        let data = {
            id: requisicao.id,
            aprovado: isAprovado,
            comentario: comentario.value,
            token: token.value
        }
        await requisicaoService.aprovar(data);

        router.push({ name: 'agradecimento' })

    } catch (error)
    {
        console.log("aprovarReprovar.error:", error);
        handleErrors(error)
    } finally { isSaving.value = false }
}

</script>

<style scoped>
.wca-texto {
    font-size: 18px;
}
</style>