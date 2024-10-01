<template>
  <v-card variant="flat">
    <v-card-title class="no-hidden-mobile">
      <v-icon
        style="margin-left: -20px"
        title="Voltar"
        size="large"
        icon="mdi-arrow-left"
        color="primary"
        @click="router.go(-1)"
        v-show="backButtonShow"
      ></v-icon>
      <span :class="`text-${titleSize} text-primary`">{{ title }}</span>
      <v-spacer></v-spacer>
      <v-menu>
        <template v-slot:activator="{ props }">
          <v-btn icon="mdi-dots-vertical" variant="text" v-bind="props"></v-btn>
        </template>

        <v-list>
          <v-list-item
            v-for="(button, index) in buttons"
            :key="index"
            @click="$emit(button.event)"
            v-show="button.visible ?? true"
          >
            <v-btn
              :prepend-icon="button.icon"
              variant="plain"
              color="primary"
              @click="$emit(button.event)"
              size="small"
              >{{ button.text }}
            </v-btn>
          </v-list-item>
          <v-list-item v-show="customButtonShow">
            <v-btn
              color="primary"
              variant="plain"
              size="small"
              class="text-capitalize"
              @click="$emit('customClick')"
              :disabled="customButtonDisabled"
            >
              <v-icon
                :icon="customButtonIcon"
                v-if="customButtonIcon != ''"
              ></v-icon>
              {{ customButtonText }}
            </v-btn>
          </v-list-item>
          <v-list-item v-show="showButton">
            <v-btn
              color="primary"
              variant="plain"
              size="small"
              class="text-capitalize"
              @click="$emit('novoClick')"
            >
              Novo
            </v-btn>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-card-title>
    <v-card-title class="hidden-mobile">
      <v-row style="margin-left: -15px">
          <v-col cols="12" sm="4" class="text-left">
            <v-icon style="margin-top: -13px"  size="large" icon="mdi-arrow-left" color="primary" @click="router.go(-1)" v-show="backButtonShow"></v-icon>
            <span :class="`text-${titleSize} text-primary`">{{ title }}</span>
          </v-col>
          <v-col cols="12" sm="8" class="text-right">
            <v-btn color="primary" variant="outlined" class="text-capitalize mr-1" v-for="(button, index) in buttons"
              @click="$emit(button.event)" :key="index" :disabled="button.disabled??false" v-show="button.visible ?? true">
              <v-icon :icon="button.icon" v-if="button.icon != ''"></v-icon>
              <b>{{ button.text }}</b>
            </v-btn>
            
            <v-btn color="primary" variant="outlined" class="text-capitalize mr-1" @click="$emit('customClick')"
              v-show="customButtonShow" :disabled="customButtonDisabled">
              <v-icon :icon="customButtonIcon" v-if="customButtonIcon != ''"></v-icon>
              <b>{{ customButtonText }}</b>
            </v-btn>
            
            <v-btn color="primary" variant="outlined" class="text-capitalize mr-1" @click="$emit('novoClick')" v-show="showButton">
              <b>Novo</b>
            </v-btn>
          </v-col>
        </v-row> 
    </v-card-title>
  </v-card>
</template>

<script setup>
import router from "@/router";
defineProps({
  title: String,
  titleSize: { type: String, default: "h4" },
  backButtonShow: { type: Boolean, default: true },
  showButton: { type: Boolean, default: true },
  customButtonShow: { type: Boolean, default: false },
  customButtonText: { type: String, default: "customButtonText" },
  customButtonIcon: { type: String, default: "" },
  customButtonDisabled: { type: Boolean, default: false },
  buttons: {
    type: Object,
    default: function () {
      return [];
    },
  },
});
</script>

<style scoped>
.no-hidden-mobile {
  display: flex;
}
@media screen and (max-width: 600px) {
  .hidden-mobile {
    display: none;
  }
}
@media screen and (min-width: 600px) {
  .no-hidden-mobile {
    display: none;
  }
}
</style>
