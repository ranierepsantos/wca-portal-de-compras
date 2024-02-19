<template>
  <div>
    <v-select
      :label="labelText"
      :items="comboItems"
      density="compact"
      :item-title="comboItemTitle"
      :item-value="comboItemValue"
      variant="outlined"
      color="primary"
      :model-value="modelValue"
      @update:model-value="val => $emit('update:modelValue',val)"
      :rules="[(v) => !!v || 'Campo obrigatório']"
      v-if="selectMode"
    ></v-select>
    <v-text-field
      :label="labelText"
      type="text"
      variant="outlined"
      color="primary"
      density="compact"
      :rules="[(v) => !!v || 'Campo obrigatório']"
      :model-value="textFieldValue"
      :readonly="true"
      bg-color="#f2f2f2"
      v-else
    ></v-text-field>
  </div>
</template>

<script setup>
defineProps({
    selectMode: {type: Boolean, default: true},
    comboItems: {
        type: Array,
        default: function() { return []; }
    },
    comboItemTitle: {type: String, default: "text"},
    comboItemValue: {type: String, default: "value"},
    modelValue: [String, Number],
    textFieldValue: {type: String, default: ''},
    labelText: {type: String, default: ''}
})
</script>
