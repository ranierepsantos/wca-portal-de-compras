<template>
    <div>
        <bread-crumbs title="Configurações" :show-button="false" 
        :buttons="[{text: 'Salvar', icon:'', event:'save-click'}]" 
        @save-click = "salvar()"
        />
        <v-progress-linear color="primary" indeterminate :height="5" v-if="isBusy" v-show="isSaving"></v-progress-linear>
        <v-container class="justify-center" v-else>
            <v-card class="mx-auto">
                <v-card-text>
                    <v-row v-for="item in configuracoes" :key="item.id">
                        <v-col class="text-left">
                            <div class="mt-3" style="font-size: 16px;">{{ item.descricao }}</div>
                        </v-col>
                        <v-col cols="3" v-if="item.tipoCampo == '1'" class="text-left">
                            <v-switch :color="color" v-model="item.valor" true-value="true" false-value="false"
                                :hide-details="true" density="compact"></v-switch>
                        </v-col>
                        <v-col cols="3" v-else-if="item.tipoCampo == '2' || item.tipoCampo == '3'">
                            <v-text-field :color="color" v-model="item.valor" :type='getTipoCampoNome(item.tipoCampo)' :hide-details="true"
                                density="compact" variant="outlined"></v-text-field>
                        </v-col>
                        <v-col cols="3" v-else-if="item.tipoCampo == '4'">
                            <v-text-field-money :color="color" v-model="item.valor" :number-decimal="2" :hide-details="true"
                                density="compact"></v-text-field-money>
                        </v-col>
                        <v-col v-else="item.tipoCampo=='3'" cols="3">
                            <v-select :color="color" v-model="item.valor" :items="item.comboValores" item-value="value"
                                item-title="text" :hide-details="true" density="compact" variant="outlined"></v-select>
                        </v-col>

                    </v-row>
                </v-card-text>
            </v-card>
        </v-container>
    </div>
</template>

<script setup>
import { ref, onMounted, inject } from 'vue';
import VTextFieldMoney from '@/components/VTextFieldMoney.vue';
import { TipoCampo } from '@/helpers/functions';
import BreadCrumbs from '@/components/breadcrumbs.vue';
import handleErrors from '@/helpers/HandleErrors';
import configuracaoService from '@/services/configuracao.service'


//VARIÁVEIS
const color = 'primary';

const configuracoes = ref([])

const isBusy = ref(false)
const isSaving = ref(false)
const swal = inject("$swal")

// VUE FUNCTIONS - LYFE CYCLE
onMounted (async () => {
    await getConfiguracoes();
});

//FUNCTIONS
function getTipoCampoNome(codigo)
{
    return TipoCampo.filter(c => c.value == codigo)[0].text
}

async function getConfiguracoes () {
    try {
        isBusy.value = true;
        let response = await configuracaoService.getAll();
        let configs = response.data;
        configs.forEach(cf => {
            if (cf.tipoCampo == 5 ) {
                cf.comboValores = JSON.parse(cf.comboValores.replaceAll('\\',''))
            }
        })
        configuracoes.value = configs;
    } catch (error) {
        console.log('getConfiguracoes.error', error)
        handleErrors(error)
    }finally
    {
        isBusy.value = false;
    }
}
async function salvar () {
    try {
        isSaving.value = true;

        configuracoes.value.forEach(async (config) => {
            let data = {
                id: config.id,
                valor: config.valor.toString()
            }
            
            await configuracaoService.update(data);
            
            swal.fire({
                toast: true,
                icon: "success",
                index: "top-end",
                title: "Sucesso!",
                text: "Dados salvos com sucesso!",
                showConfirmButton: false,
                timer: 2000,
            })
        })
        
    } catch (error) {
        console.log('salvar.error', error)
        handleErrors(error)
    }finally
    {
        isSaving.value = false;
    }


}
</script>