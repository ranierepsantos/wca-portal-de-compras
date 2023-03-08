<template>
    <v-row v-for="dia in diasDaSemana" :key="dia.value" >
        <v-col cols="4">
            <v-checkbox :label="dia.text" color="primary" density="compact" :hide-details="true" v-model="diaSelecionado[dia.value].selected"
            v-on:vnode-updated="onCheckBoxChange(diaSelecionado[dia.value])"
            ></v-checkbox>
        </v-col>
        <v-col>
            <v-combobox v-model="diaSelecionado[dia.value].periodo" :items="periodosDia" label="Período(s)" multiple density="comfortable"
                :hide-details="true" :disabled="!diaSelecionado[dia.value].selected">
                <template v-slot:selection="data">
                    <v-chip :key="JSON.stringify(data.item)" v-bind="data.attrs" :model-value="data.selected"
                        :disabled="data.disabled" size="small">
                        <template v-slot:prepend>
                            <v-avatar class="bg-info text-uppercase" start>{{ data.item.title.slice(0, 1) }}</v-avatar>
                        </template>
                        {{ data.item.title }}
                    </v-chip>
                </template>
            </v-combobox>
        </v-col>
    </v-row>
</template>

<script setup>
import { diasDaSemana, periodosDia } from '@/helpers/functions';
// import { ref } from 'vue';
defineProps(['diaSelecionado'])

function onCheckBoxChange(e) {
    if (!e.selected) {
        e.periodo =[]
    }
}

</script>