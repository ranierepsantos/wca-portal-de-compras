<template>
  <v-row>
    <v-col cols="6">
      <v-card elevation="3" :subtitle="listOrigemTitulo">
        <v-card-title>
          <v-text-field label="Pesquisar" v-model="boxOrigemTermo" type="text" required variant="outlined" color="primary"
                   density="compact" v-show="showSearchText">
          </v-text-field>
        </v-card-title>
        
        <v-card-text>
          <v-list density="compact" select-strategy="multiple" color="primary" :bg-color="isReadOnly? '#f2f2f2':''">
            <v-list-item
              v-for="item in getFilteredList(listOrigem, boxOrigemTermo)"
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
        color="primary"
        size="x-large"
        @click="adicionarTodos()"
        title="Adicionar todos items na lista"
      ></v-icon
      ><br />
      <v-icon
        icon="mdi-chevron-right"
        color="primary"
        size="x-large"
        @click="adicionar()"
        title="Adicionar item na lista"
      ></v-icon
      ><br />
      <v-icon
        icon="mdi-chevron-double-left"
        color="primary"
        size="x-large"
        @click="removerTodos()"
        title="Remover todos os item da lista"
      ></v-icon>
      <br />
      <v-icon
        icon="mdi-chevron-left"
        color="primary"
        size="x-large"
        @click="remover()"
        title="Remover item da lista"
      ></v-icon>
      <br />
      <br />
      <v-icon
        icon="mdi-plus"
        color="success"
        size="x-large"
        @click="$emit('plusClick')"
        v-show="plusButtonShow"
        :title="plusButtonTitle"
      ></v-icon>
    </v-col>
    <v-col cols="5" >
      <v-card elevation="3" :subtitle="listDestinoTitulo" readonly>
        <v-card-title>
          <v-text-field label="Pesquisar" v-model="boxDestinoTermo" type="text" required variant="outlined" color="primary"
                   density="compact" v-show="showSearchText">
          </v-text-field>
        </v-card-title>
        <v-card-text>
          <v-list density="compact" select-strategy="multiple" color="primary" :bg-color="isReadOnly? '#f2f2f2':''">
            <v-list-item
              v-for="item in getFilteredList(listDestino, boxDestinoTermo)"
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
import { ref } from "vue";
const props = defineProps({
  listOrigemTitulo: "",
  listOrigem: {
    type: Object
  },
  listDestinoTitulo: "",
  listDestino: {
    type: Object
    
  },
  showSearchText: {
    type:Boolean,
    default: true
  },
  isReadOnly: {
    type: Boolean,
    default: false
  }, 
  plusButtonShow: {
    type: Boolean,
    default: false
  },
  plusButtonTitle: {
    type: String,
    default: 'Adicionar'
  }

});
const boxOrigemTermo = ref("")
const boxDestinoTermo = ref("")
//FUNCTIONS

function adicionar() {
  if (props.isReadOnly) return
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
  if (props.isReadOnly) return
  if (removerTodos == true) props.listOrigem.splice(0, props.listOrigem.length);
  else {
    props.listDestino.forEach((element) => {
      let index = props.listOrigem.findIndex((c) => c.value == element.value);
      if (index > -1) props.listOrigem.splice(index, 1);
    });
  }
}

function adicionarTodos() {
  if (props.isReadOnly) return
  props.listDestino.push(...props.listOrigem);
  props.listOrigem.splice(0, props.listOrigem.length);
  ordernarLista(props.listDestino, "text");
}

function remover() {
  if (props.isReadOnly) return
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
  if (props.isReadOnly) return
    
  props.listOrigem.push(...props.listDestino);
  props.listDestino.splice(0, props.listDestino.length);
  ordernarLista(props.listOrigem, "text");
}

function ordernarLista(lista, campo) {
  lista.sort(compararValor(campo));
}

function getFilteredList (lista, termo)
{
  let listReturn = lista

  if (termo && termo.trim() != "")
    listReturn = lista.filter(q => q.text.toLowerCase().includes(termo.toLowerCase()))

  return listReturn
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
