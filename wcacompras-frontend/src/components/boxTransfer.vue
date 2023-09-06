<template>
  <v-row>
    <v-col cols="6">
      <v-card elevation="3" :subtitle="listOrigemTitulo">
        <v-card-text>
          <v-list density="compact" select-strategy="multiple" color="primary">
            <v-list-item
              v-for="item in listOrigem"
              :key="item.text"
              @click="item.selected = !item.selected"
              :active="item.selected"
            >
              <v-list-item-title>{{ item.text }}</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-card-text>
      </v-card>
    </v-col>
    <v-col cols="1">
      <v-icon
        icon="mdi-chevron-double-right"
        color="success"
        size="x-large"
        @click="adicionarTodos()"
      ></v-icon
      ><br />
      <v-icon
        icon="mdi-chevron-right"
        color="success"
        size="x-large"
        @click="adicionar()"
      ></v-icon
      ><br />
      <v-icon
        icon="mdi-chevron-double-left"
        color="success"
        size="x-large"
        @click="removerTodos()"
      ></v-icon
      ><br />
      <v-icon
        icon="mdi-chevron-left"
        color="success"
        size="x-large"
        @click="remover()"
      ></v-icon>
    </v-col>
    <v-col cols="5">
      <v-card elevation="3" :subtitle="listDestinoTitulo">
        <v-card-text>
          <v-list density="compact" select-strategy="multiple" color="primary">
            <v-list-item
              v-for="item in listDestino"
              :key="item.value"
              :active="item.selected"
              @click="item.selected = !item.selected"
            >
              <v-list-item-title>{{ item.text }}</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script setup>

import { compararValor } from "@/helpers/functions";
const props = defineProps({
  listOrigemTitulo: "",
  listOrigem: {
    type: Object
  },
  listDestinoTitulo: "",
  listDestino: {
    type: Object
    
  },
});

//FUNCTIONS

function adicionar() {
  let list = props.listOrigem.slice();
  list.forEach((element) => {
    if (element.selected != undefined && element.selected == true) {
      element.selected = false;
      props.listDestino.push(element);
    }
  });
  removerItemOrigem();
}

function removerItemOrigem(removerTodos = false) {
  if (removerTodos == true) props.listOrigem.splice(0, props.listOrigem.length);
  else {
    props.listDestino.forEach((element) => {
      let index = props.listOrigem.findIndex((c) => c.value == element.value);
      if (index > -1) props.listOrigem.splice(index, 1);
    });
  }
}

function adicionarTodos() {
  props.listDestino.push(...props.listOrigem);
  props.listOrigem.splice(0, props.listOrigem.length);
  ordernarLista(props.listDestino, "text");
}

function remover() {
  let list = props.listDestino.slice();
  list.forEach((element) => {
    if (element.selected != undefined && element.selected == true) {
      element.selected = false;
      props.listOrigem.push(element);
      let index = props.listDestino.findIndex((c) => c.value == element.value);
      if (index > -1) props.listDestino.splice(index, 1);
      ordernarLista(props.listOrigem, "text");
    }
  });
}

function removerTodos() {
    props.listOrigem.push(...props.listDestino);
    props.listDestino.splice(0, props.listDestino.length);
  ordernarLista(props.listOrigem, "text");
}

function ordernarLista(lista, campo) {
  lista.sort(compararValor(campo));
}
</script>

<style scoped>
.v-list {
  height: 145px;
  /* or any height you want */
  overflow-y: auto;
}

.v-icon:hover {
  cursor: pointer;
}
</style>
