<template>
  <div>
    <bread-crumbs
      title="Movimentações Financeiras"
      :show-button="false"
    />
    <v-container class="justify-center">
      <v-card>
        <v-card-title class="text-right">
          <h4>Saldo: <span :class="'text-' + (usuario.contaCorrente.saldo < 0 ? 'red': 'green')">{{ formatToCurrencyBRL(usuario.contaCorrente.saldo) }}</span></h4>
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
                <template v-for="(item,index) in usuario.contaCorrente.transacoes" :key="index">
                    <tr>
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
import handleErrors from "@/helpers/HandleErrors";
import { formatToCurrencyBRL } from "@/helpers/functions";
import { IDPERFILCOLABORADOR, Usuario, useUsuarioStore } from "@/store/reembolso/usuario.store";
import moment from "moment";
import { computed } from "vue";

// data
const usuario = ref(new Usuario());
// vue functions
onMounted(async () => {
    usuario.value = new Usuario(await useUsuarioStore().repository.find(q => q.usuarioSistemaPerfil.filter(qy => qy.perfilId == IDPERFILCOLABORADOR).length > 0))
});

//methods


</script>
