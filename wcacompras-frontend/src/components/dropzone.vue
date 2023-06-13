<template>
  <div
    @dragenter.prevent="toggleActive"
    @dragleave.prevent="toggleActive"
    @dragover.prevent
    @drop.prevent="toggleActive"
    :class="{ 'active-dropzone': active }"
    class="dropzone"
  >
    <span>Arraste e solte o arquivo</span>
    <span>Ou</span>
    <label for="dropzoneFile">
        <v-icon icon="mdi-camera"></v-icon>
        Selecione o arquivo
    </label>
    <input type="file" id="dropzoneFile" class="dropzoneFile" accept="image/png, image/jpeg, image/bmp" />
    
  </div>
</template>

<script setup>
import { ref } from "vue";
const active = ref(false);

const toggleActive = () => {
  active.value = !active.value;
};
</script>

<style scoped>
.dropzone {
  width: 400px;
  height: 200px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  row-gap: 16px;
  border: 2px dashed #000066;
  background-color: #fff;
  transition: 0.3s ease all;
}

.dropzone input {
  display: none;
}
.dropzone label {
  padding: 8px 12px;
  color: #fff;
  background-color: #000066;
  transition: 0.3s ease all;
}

.active-dropzone {
  color: #fff;
  border-color: #fff;
  background-color: #41b883;
}

.active-dropzone label {
  background-color: #fff;
  color: #41b883;
}
</style>
