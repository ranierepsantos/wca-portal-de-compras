<template>
  <v-row>
    <v-col cols="6">
      <v-card elevation="3" :subtitle="listOrigemTitulo">
        <v-card-title>
          <v-text-field
            label="Pesquisar"
            v-model="boxOrigemTermo"
            type="text"
            required
            variant="outlined"
            color="primary"
            density="compact"
            v-show="showSearchText"
          >
          </v-text-field>
        </v-card-title>

        <v-card-text>
          <v-progress-circular
            color="primary"
            indeterminate
            :size="140"
            v-if="isOrigemLoading"
          ></v-progress-circular>
          <v-list
            density="compact"
            select-strategy="multiple"
            color="primary"
            :bg-color="isReadOnly ? '#f2f2f2' : ''"
            v-else
          >
            <v-list-item
              v-for="item in getFilteredList(listOrigem, boxOrigemTermo)"
              :key="item.value"
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
      <v-icon
        icon="mdi-plus"
        color="success"
        size="x-large"
        @click="$emit('plusClick')"
        v-show="plusButtonShow"
        :title="plusButtonTitle"
      ></v-icon>
      <br />
      <v-progress-circular
        color="primary"
        indeterminate
        :size="40"
        style="margin-top: 20px"
        v-show="isTransferBetween"
      ></v-progress-circular>
    </v-col>
    <v-col cols="5">
      <v-card elevation="3" :subtitle="listDestinoTitulo" readonly>
        <v-card-title>
          <v-text-field
            label="Pesquisar"
            v-model="boxDestinoTermo"
            type="text"
            required
            variant="outlined"
            color="primary"
            density="compact"
            v-show="showSearchText"
          >
          </v-text-field>
        </v-card-title>
        <v-card-text>
          <v-list
            density="compact"
            select-strategy="multiple"
            color="primary"
            :bg-color="isReadOnly ? '#f2f2f2' : ''"
          >
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
    type: Object,
  },
  listDestinoTitulo: "",
  listDestino: {
    type: Object,
  },
  showSearchText: {
    type: Boolean,
    default: true,
  },
  isReadOnly: {
    type: Boolean,
    default: false,
  },
  plusButtonShow: {
    type: Boolean,
    default: false,
  },
  plusButtonTitle: {
    type: String,
    default: "Adicionar",
  },
  isOrigemLoading: { type: Boolean, default: false },
});
const boxOrigemTermo = ref("");
const boxDestinoTermo = ref("");
const isTransferBetween = ref(false);
//FUNCTIONS

const emit = defineEmits(["destinyChange", "plusClick"]);

function adicionar() {
  if (props.isReadOnly) return;
  let list = props.listOrigem.filter((e) => e.selected == true);
  if (list.length > 0) {
    props.listDestino.push(...list.map(p => {
      let {selected, ...novoitem } = p
      return novoitem;
    }))
    removerItems(props.listOrigem, list);
    ordernarLista(props.listDestino, "text");
    emit("destinyChange");
  }
}

function removerItems(list, items = []) {
  if (props.isReadOnly) return;
  if (items.length == 0) list.splice(0, list.length);
  else {
    items.forEach((element) => {
      let index = list.findIndex((c) => c.value == element.value);
      if (index > -1) list.splice(index, 1);
    });
  }
}

async function adicionarTodos() {
  if (props.isReadOnly) return;
  
  let _timeOut = 0
  
  let list = props.listOrigem.map(p => ({value: p.value, text: p.text}));
  if (boxOrigemTermo.value.trim() != "")
    list = getFilteredList(props.listOrigem, boxOrigemTermo.value).map(p => ({value: p.value, text: p.text}));
  
  if (list.length > 1000) _timeOut = 1500;

  isTransferBetween.value = true;
  setTimeout(() => {
    if (list.length > 0 && list.length != props.listOrigem.length) {
      let list = getFilteredList(props.listOrigem, boxOrigemTermo.value);
      if (list.length > 0) {
        list.forEach((element) => {
          element.selected = false;
          props.listDestino.push(element);
        });
        removerItems(props.listOrigem, list);
        ordernarLista(props.listDestino, "text");
        emit("destinyChange");
      }
    } else {
      let _ordenar = true
      if (props.listDestino.length == 0) _ordenar = false
      props.listDestino.push(...props.listOrigem);
      removerItems(props.listOrigem, []); //props.listOrigem.splice(0, props.listOrigem.length);
      if (_ordenar) ordernarLista(props.listDestino, "text");
      emit("destinyChange");
    }
    isTransferBetween.value = false;
  }, _timeOut);
}

function remover() {
  if (props.isReadOnly) return;

  let list = props.listDestino.filter((q) => q.selected == true);
  if (list.length > 0) {
    
    props.listOrigem.push(...list.map(p => {
      let {selected, ...novoitem } = p
      return novoitem;
    }))
    
    //props.listOrigem.push(...list.map(p => ({value: p.value, text: p.text})));
    removerItems(props.listDestino, list);
    ordernarLista(props.listOrigem, "text");
    emit("destinyChange");
  }
}

async function removerTodos() {
  if (props.isReadOnly) return;

  let _timeOut = 0
  
  let list = props.listDestino.map(p => ({value: p.value, text: p.text}));
  if (boxDestinoTermo.value.trim() != "")
    list = getFilteredList(props.listDestino, boxDestinoTermo.value).map(p => ({value: p.value, text: p.text}));
  
  if (list.length > 1000) _timeOut = 1500;

  isTransferBetween.value = true;
  setTimeout(() => {
    if (list.length > 0 && list.length != props.listDestino.length) {
      if (list.length > 0) {
        props.listOrigem.push(...list);
        removerItems(props.listDestino, list);
        ordernarLista(props.listOrigem, "text");
        emit("destinyChange");
      }
    } else {
      let _ordenar = true
      if (props.listOrigem.length == 0) _ordenar = false
      props.listOrigem.push(...props.listDestino);
      removerItems(props.listDestino, []); //props.listDestino.splice(0, props.listDestino.length);
      if (_ordenar) ordernarLista(props.listOrigem, "text");
      emit("destinyChange");
    }
    isTransferBetween.value = false;
  }, _timeOut);
}

function ordernarLista(lista, campo) {
  lista.sort(compararValor(campo));
}

function getFilteredList(lista, termo) {
  let listReturn = lista;

  if (termo && termo.trim() != "")
    listReturn = listReturn.filter((q) =>
      q.text.toLowerCase().includes(termo.toLowerCase())
    );

  return listReturn;
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
