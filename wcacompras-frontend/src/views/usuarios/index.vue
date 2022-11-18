<template>
  <div class="layout-wrap"></div>
</template>

<script setup>
import { ref } from "vue";
</script>

<style scoped>
.layout-wrap {
  display: flex;
  flex-direction: column;
  /*justify-content: center;*/
  /*align-items: center;*/
  min-height: 100vh;
  background-size: 50% 100%;
  background-repeat: no-repeat;
  background-color: aqua;
}
</style>