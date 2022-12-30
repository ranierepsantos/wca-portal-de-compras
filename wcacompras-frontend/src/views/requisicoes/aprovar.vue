<template>
    <v-app>
        <v-app-bar elevation="3" rounded>
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

        <v-main>
            <v-container fluid>
                <h1 class="text-primary mb-2">Pedido #{{ requisicao.id }}</h1>
                <p>
                    <span class="text-primary wca-texto">Cliente: </span>
                    <span class="text-grey wca-texto">{{ requisicao.cliente.nome }}</span>
                </p>
                <p>
                    <span class="text-primary wca-texto">CNPJ: </span>
                    <span class="text-grey wca-texto">{{ requisicao.cliente.cnpj }}</span>
                </p>
                <p><span class="text-primary wca-texto">Endereço: </span> <span class="text-grey wca-texto">{{
        requisicao.cliente.endereco + ', ' + requisicao.cliente.numero +
        ', ' + requisicao.cliente.cidade + ', ' + requisicao.cliente.uf + ', ' +
        requisicao.cliente.cep
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
                        <v-btn :color="requisicao.id == null ? 'grey' : '#EDCCCC'" class="mr-4"
                            style="color:#950000; font-weight: bold;" @click="aprovarReprovar(false)"
                            :disabled="requisicao.id == null">
                            Recusar
                        </v-btn>
                        <v-btn :disabled="requisicao.id == null" :color="requisicao.id == null ? 'grey' : 'success'"
                            style="font-weight: bold;" @click="aprovarReprovar(true)">
                            Aprovar
                        </v-btn>
                    </v-col>
                </v-row>
            </v-container>
        </v-main>

    </v-app>
</template>

<script setup>
import { ref, onMounted, inject } from "vue";
import requisicaoService from "@/services/requisicao.service";
import handleErrors from "@/helpers/HandleErrors"
import { useRoute } from "vue-router";

//DATA
const swal = inject("$swal")
const isBusy = ref(false);
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
    await getRequisicaoData(token.value)
});


//METHODS
async function getRequisicaoData(token)
{
    try
    {
        isBusy.value = true;
        let response = await requisicaoService.getByToken(token);
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
        let data = {
            id: requisicao.id,
            aprovado: isAprovado,
            comentario: comentario.value,
            token: token.value
        }
        await requisicaoService.aprovar(data);

        swal.fire({
            toast: true,
            icon: "success",
            index: "top-end",
            title: "Sucesso!",
            text: "Dados salvos com sucesso! <br/> Criar pagina!",
            showConfirmButton: false,
            timer: 2000,
        })

    } catch (error)
    {
        console.log("aprovarReprovar.error:", error);
        handleErrors(error)
    }
}

</script>

<style scoped>
.wca-texto {
    font-size: 18px;
}
</style>