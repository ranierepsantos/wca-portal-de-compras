<template>
  <div>
    <v-autocomplete
      :label="labelText"
      :items="comboItems"
      density="compact"
      :item-title="comboItemTitle"
      :item-value="comboItemValue"
      variant="outlined"
      color="primary"
      :model-value="modelValue"
      @update:model-value="val => $emit('update:modelValue',val)"
      :rules="fieldRules"
      v-if="selectMode"
      :disabled="disabled"
      :multiple="isMultiple"
      :key="comboItems[comboItemValue]"
    >
      <template v-slot:append v-if="buttonShow">
      <v-icon
        :icon="buttonIcon"
        color="success"
        size="small"
        @click="$emit('buttonClick')"
        :title="buttonTitle"
        
      ></v-icon>      
    </template>

    <template v-slot:append-inner>
      <v-fade-transition leave-absolute>
        <v-progress-circular
          v-show="buttonLoading"
          color="primary"
          size="24"
          :width="2"
          indeterminate
        ></v-progress-circular>
      </v-fade-transition>
    </template>
    
  </v-autocomplete>
    <v-text-field
      :label="labelText"
      type="text"
      variant="outlined"
      color="primary"
      density="compact"
      :model-value="textFieldValue"
      :readonly="true"
      :bg-color="!printMode? '#f2f2f2': ''"
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
    labelText: {type: String, default: ''},
    fieldRules: {
        type: Array,
        default: function() { return []; }
    },
    disabled: { type: Boolean, default: false },
    buttonTitle: {type: String, default: 'Adicionar'},
    buttonShow: {type: Boolean, default: false},
    buttonIcon: {type: String, default: 'mdi-plus'},
    buttonLoading: {type: Boolean, default: false},
    isMultiple: {type: Boolean, default: false},
    printMode:{type: Boolean, default: false},
})
//const emit = defineEmits(['buttonClick'])

// function sendCommand() {
//   console.log('buttonClick')
//   emit('buttonClick')
// }

</script>
