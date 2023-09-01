<template>
  <div>
    <bread-crumbs
      title="Movimentações Financeiras"
      :show-button="false"
    />
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-if="isLoading"
    ></v-progress-linear>
    <v-container class="justify-center" v-else>
      <v-card>
        <v-card-title class="text-right">
          <h4>Saldo: <span :class="'text-' + (conta.saldo < 0 ? 'red': 'green')">{{ formatToCurrencyBRL(conta.saldo) }}</span></h4>
        </v-card-title>
        <v-card-text> 
            <v-table class="elevation-2">
            <thead>
              <tr>
                <th class="text-center text-grey">DATA</th>
                <th class="text-center text-grey">DESCRIÇÃO</th>
                <th class="text-center text-grey">ENTRADA</th>
                <th class="text-center text-grey">SAÍDA</th>
              </tr>
            </thead>
            <tbody>
                <template v-for="(item,index) in conta.transacoes" :key="index">
                    <tr :class="item.operador == '+' ?'text-success':'text-error'">
                        <td class="text-center">{{ moment(item.data).format("DD/MM/YYYY") }}</td>
                        <td class="text-left">{{ item.descricao }}</td>
                        <td class="text-right">{{ item.operador == "+" ? formatToCurrencyBRL(item.valor): "" }}</td>
                        <td class="text-right">{{ item.operador == "-" ? formatToCurrencyBRL(item.valor): "" }}</td>
                    </tr>
                </template>
            </tbody>
          </v-table>
        </v-card-text>
      </v-card>
    </v-container>
  </div>
</template>

<script setup>
import breadCrumbs from "@/components/breadcrumbs.vue";
import { ref, onMounted } from "vue";
import { formatToCurrencyBRL } from "@/helpers/functions";
import moment from "moment";
import { ContaCorrente, useContaStore } from "@/store/reembolso/conta.store";
import { useAuthStore } from "@/store/auth.store";
import handleErrors from "@/helpers/HandleErrors";
// data
const conta =ref(new ContaCorrente())
const isLoading = ref(false)

// vue functions
onMounted(async () => {
  try {
    isLoading.value = true 
    conta.value = await useContaStore().getByUsuarioId(useAuthStore().user.id);
    
  } catch (error) {
    console.error(error)
    handleErrors(error)
  }finally {
    isLoading.value = false
  }
  
});


</script>
