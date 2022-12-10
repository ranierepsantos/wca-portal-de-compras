<template>
    <v-form>
        <v-card>
            <v-card-title class="text-primary text-h5 text-left">
                {{ title }}
            </v-card-title>
            <v-card-text>
                <v-row>
                    <v-col cols="8">
                        <v-text-field label="Nome" v-model="produto.nome" type="text" required variant="outlined"
                            color="primary" :rules="[(v) => !!v || 'Nome é obrigatório']" density="compact">
                        </v-text-field>
                    </v-col>
                    <v-col>
                        <v-select label="Categoria" v-model="produto.tipoFornecimentoId" :items="categorias"
                            density="compact" item-title="text" item-value="value" variant="outlined"
                            color="primary"></v-select>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col>
                        <v-text-field-money label-text="Valor" v-model="produto.valor" color="primary"
                            :number-decimal="2"></v-text-field-money>
                    </v-col>
                    <v-col>
                        <v-text-field label="Unidade Medida" v-model="produto.unidadeMedida" type="text" required
                            variant="outlined" color="primary" :rules="[(v) => !!v || 'U.M. é obrigatório']"
                            density="compact">
                        </v-text-field>
                    </v-col>
                    <v-col>
                        <v-text-field-money label-text="Taxa Gestão" v-model="produto.taxaGestao" color="primary"
                            :number-decimal="2"></v-text-field-money>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col class="text-right">
                        <v-btn variant="outlined" color="primary">Cancelar</v-btn>
                        <v-btn color="primary" type="submit" class="ml-3">Salvar</v-btn>
                    </v-col>
                </v-row>
            </v-card-text>
        </v-card>
    </v-form>
</template>
<script>
const modelProduto = {
    id: null,
    fornecedorId: null,
    nome: null,
    valor: 0,
    taxaGestao: 0,
    tipoFornecimentoId: null,
    unidadeMedida: ""
}
function toFloat(num)
{
    return num.parseFloat();
}
</script>
<script setup>
import { ref, onMounted, watch } from "vue";
import tipoFornecimentoService from "@/services/tipofornecimento.service";
import handleErrors from "@/helpers/HandleErrors"
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";


defineProps({
    title: { Type: String, default: "Novo Produto" },
    fornecedorId: { type: Number },
})

const categorias = ref([]);
const produto = ref({
    id: null,
    fornecedorId: null,
    nome: null,
    valor: 0,
    taxaGestao: 0,
    tipoFornecimentoId: null,
    unidadeMedida: ""
})

//VUE FUNCTIONS
onMounted(async () => await getTipoFornecimentoToList())



//FUNCTIONS

async function getTipoFornecimentoToList() 
{
    try
    {
        let response = await tipoFornecimentoService.toList();
        categorias.value = response.data;
    } catch (error)
    {
        console.log("getTipoFornecimentoToList.error:", error);
        handleErrors(error)
    }
}

</script>


