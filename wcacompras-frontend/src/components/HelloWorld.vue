<template>
  <div class="hello">
    <img src="../assets/images/logoWCA.png" alt="" class="mt-10" />
    <p class="text-primary text-center"><b>{{ authStore.sistema.descricao.toUpperCase() }}</b>
      <br/>
      <small>{{ ` ver. ${packageJsonInfo.version} ${packageJsonInfo.enviroment}` }}</small>
    </p>
    
  </div>
</template>

<script setup>
  import { useAuthStore } from '@/store/auth.store';
  const authStore = useAuthStore();
  var packageJsonInfo = require('../../package.json');
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
