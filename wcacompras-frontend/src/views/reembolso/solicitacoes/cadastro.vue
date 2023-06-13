<template>
  <div>
    <bread-crumbs title="Solicitação" :show-button="false" :buttons="formButtons"/>
    <v-container class="justify-center">
      <v-card>
        <v-card-text>
          <v-form>
            <v-row>
              <v-col cols="7">
                <v-select
                  label="Cliente"
                  :items="[]"
                  density="compact"
                  item-title="nome"
                  item-value="id"
                  variant="outlined"
                  color="primary"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                ></v-select>
              </v-col>
              <v-col cols="2">
                <v-text-field
                  label="Data Solicitação"
                  type="date"
                  variant="outlined"
                  color="primary"
                  density="compact"
                ></v-text-field>
              </v-col>
              <v-col cols="3">
                <v-select
                  label="Tipo Solicitação"
                  :items="[
                    { id: 1, nome: 'Reembolso' },
                    { id: 2, nome: 'Adiantamento' },
                  ]"
                  v-model="tipoSolicitacao"
                  density="compact"
                  item-title="nome"
                  item-value="id"
                  variant="outlined"
                  color="primary"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                ></v-select>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-select
                  label="Colaborador"
                  :items="[]"
                  density="compact"
                  item-title="nome"
                  item-value="id"
                  variant="outlined"
                  color="primary"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                ></v-select>
              </v-col>
              <v-col>
                <v-select
                  label="Gestor Responsável"
                  :items="[]"
                  density="compact"
                  item-title="nome"
                  item-value="id"
                  variant="outlined"
                  color="primary"
                  :rules="[(v) => !!v || 'Campo obrigatório']"
                ></v-select>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-text-field
                  label="Cargo"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Local Projeto"
                  type="text"
                  variant="outlined"
                  color="primary"
                  density="compact"
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
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Período Inicial"
                  type="date"
                  variant="outlined"
                  color="primary"
                  density="compact"
                ></v-text-field>
              </v-col>
              <v-col>
                <v-text-field
                  label="Período Final"
                  type="date"
                  variant="outlined"
                  color="primary"
                  density="compact"
                ></v-text-field>
              </v-col>
              <v-col v-show="tipoSolicitacao == 2">
                <v-text-field-money
                  label-text="Valor Solicitação"
                  v-model="valorSolicitacao"
                  color="primary"
                  :number-decimal="2"
                  prefix="R$"
                ></v-text-field-money>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>
      </v-card>

      <v-card style="margin-top: 20px">
        <v-card-title>
            <v-breadcrumbs>
          <div class="text-h6 text-primary">Detalhamento de Despesas</div>
          <v-spacer></v-spacer>
          <v-btn
            color="primary"
            variant="outlined"
            class="text-capitalize"
            @click="openDespesaForm = true"
          >
            <b>Nova Despesa</b>
          </v-btn>
        </v-breadcrumbs>
        </v-card-title>
        
        <v-card-text>
          <v-table class="elevation-2">
            <thead>
              <tr>
                <th class="text-center text-grey">DATA</th>
                <th class="text-center text-grey">NOTA/CUPOM FISCAL</th>
                <th class="text-left text-grey">FORNECEDOR</th>
                <th class="text-center text-grey">VALOR</th>
                <th class="text-center text-grey"></th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in despesas" :key="item.id">
                <td class="text-center">
                  {{ new Date(item.dataEvento).toLocaleDateString() }}
                </td>
                <td class="text-left">{{ item.nroFiscal}}</td>
                <td class="text-left">{{ item.fornecedor}}</td>

                <td class="text-right">
                  {{
                    parseFloat(item.valor)
                  }}
                </td>
                <td class="text-right">
                  <v-btn
                    icon="mdi-lead-pencil"
                    size="smaller"
                    variant="plain"
                    color="primary"
                    @click="editar(item)"
                    title="Editar"
                    :disabled="isBusy"
                  ></v-btn>
                  
                </td>
              </tr>
            </tbody>
            <tfoot>
              <tr>
                <td colspan="8">
                  
                </td>
              </tr>
            </tfoot>
          </v-table>
        </v-card-text>
      </v-card>
      <v-dialog
        v-model="openDespesaForm"
        max-width="900"
        :absolute="false"
        persistent
      >
        <v-card>
          <v-breadcrumbs>
            <div class="text-h6 text-primary">Detalhamento de Despesa</div>
            <v-spacer></v-spacer>
            <v-btn
              color="primary"
              variant="outlined"
              class="text-capitalize"
              @click="openDespesaForm = false"
            >
              <b>Cancelar</b>
            </v-btn>
            
            <v-btn
              color="primary"
              variant="outlined"
              class="text-capitalize"
              @click="adicionarDespesa()"
              style="margin-left: 5px;"
            >
              <b>Salvar</b>
            </v-btn>
          </v-breadcrumbs>
          <div class="text-center">
            <despesa-form :despesa="despesa" @change-image="(image) => despesa.comprovanteImage = image"></despesa-form>
          </div>
          
        </v-card>
      </v-dialog>
    </v-container>
  </div>
</template>

<script setup>
import breadCrumbs from "@/components/breadcrumbs.vue";
import { ref } from "vue";
import vTextFieldMoney from "@/components/VTextFieldMoney.vue";
import DespesaForm from "@/components/reembolso/despesaForm.vue";

const tipoSolicitacao = ref(2);
const valorSolicitacao = ref(0.0);
const openDespesaForm = ref(false);
const despesas = ref([]);
const despesa = ref({
        id: 0,
        solicitacaoId: 0,
        tipoDespesaId: null,
        dataEvento: "",
        fornecedor: "",
        nroFiscal: "",
        valor: 0.00,
        comprovanteImage: ""
      })
let formButtons = [{text: 'Aprovar / Reprovar', icon:'', event:'aprovar-click'},
                   {text: 'Salvar', icon:'', event:'salvar-click'}
                  ];



//FUNCTIONS
const adicionarDespesa = () => {
  despesas.value.push({...despesa.value});
  limparDadosDespesa();
  openDespesaForm.value = false
}

const limparDadosDespesa = () => {
  despesa.value = {
        id: 0,
        solicitacaoId: 0,
        tipoDespesaId: null,
        dataEvento: "",
        fornecedor: "",
        nroFiscal: "",
        valor: 0.00,
        comprovanteImage: ""
      }
}
const editar = (item) => {
  console.log("despesa-item", item)
  despesa.value = {...item}
  openDespesaForm.value = true
}
</script>
