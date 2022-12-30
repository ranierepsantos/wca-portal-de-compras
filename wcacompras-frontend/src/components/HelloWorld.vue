<template>
  <div class="hello">
    <img src="../assets/images/logoWCA.png" alt="" class="mt-10" />

    <!-- <h1>{{ props.msg }}</h1>
    <p><v-icon icon="mdi:mdi-home" /> Teste</p>
    <p>Buttons</p>
    <v-btn color="primary">Primary</v-btn>
    <v-btn color="secondary">Secondary</v-btn>
    <v-btn color="info">Info</v-btn>
    <v-btn color="success">Success</v-btn>
    <v-btn color="warning">Warning</v-btn>
    <v-btn color="error">Error</v-btn>
    <p>Buttons Outline</p>
    <v-btn color="primary" variant="outlined">Primary</v-btn>
    <v-btn color="secondary" variant="outlined">Secondary</v-btn>
    <v-btn color="info" variant="outlined">Info</v-btn>
    <v-btn color="success" variant="outlined">Success</v-btn>
    <v-btn color="warning" variant="outlined">Warning</v-btn>
    <v-btn color="error" variant="outlined">Error</v-btn> -->
  </div>
</template>

<script setup>
const props = defineProps(["msg"]);
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}
</style>
