<template>
  <div>
    <Breadcrumbs title="Usuário" :show-button="false" />
    <v-progress-linear
      color="primary"
      indeterminate
      :height="5"
      v-show="isBusy"
    ></v-progress-linear>
  </div>
</template>

<script setup>
import Breadcrumbs from "@/components/breadcrumbs.vue";
import { ref } from "vue";

const isBusy = ref(false)


</script>
