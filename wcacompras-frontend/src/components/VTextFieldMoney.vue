<template>
    <v-text-field density="compact" type="number" :model-value="initialDecimalNumber(modelValue, 2)" :label="labelText"
        variant="outlined" :color="color" class="right-input" :rules="fieldRules" :prefix="prefix" :suffix="sufix"
        @input="$emit('update:modelValue', toDecimalNumber($event.target.value, numberDecimal))">
    </v-text-field>
</template>

<script setup>

const props = defineProps({
    modelValue: [String, Number],
    numberDecimal: Number,
    labelText: String,
    color: String,
    fieldRules: Object,
    prefix: {String,default:""},
    sufix: {String,default:""}
})
// defines what events our component emits
defineEmits(['update:modelValue'])

function initialDecimalNumber(value, numberOfDecimal)
{

    let num = 0
    if (value && value.toString().trim() != '') num = value

    let arr = num.toString().split('.')
    let dec = ""
    if (arr.length == 1)
        dec = "0".repeat(numberOfDecimal);
    else
    {
        dec = arr[1] + (arr[1].length < 2 ? "0".repeat(numberOfDecimal - arr[1].length): "")
    }
    num = arr[0] + '.' + dec
    return num;
}

function toDecimalNumber(value, numberOfDecimal)
{

    let num = 0
    if (value && value.toString().trim() != '') num = value

    num = parseInt(num.toString().replace(".", "")) / 10 ** numberOfDecimal;
    return num.toFixed(numberOfDecimal);
}

</script>

<style scoped>
.right-input :deep(input) {
    text-align: right
}

.right-input :deep(input::-webkit-outer-spin-button),
.right-input :deep(input::-webkit-inner-spin-button) {
    -webkit-appearance: none;
    margin: 0;
}
</style>