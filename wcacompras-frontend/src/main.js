import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { createPinia } from 'pinia'
import vuetify from "./plugins/vuetify"
import maska from 'maska'
import money from "v-money3"
import VueSweetAlert from 'vue-sweetalert2'
import '@sweetalert2/theme-material-ui/material-ui.css'
import './assets/css/app.css'

const pinia = createPinia()

createApp(App)
    .use(router)
    .use(vuetify)
    .use(pinia)
    .use(maska)
    .use(money)
    .use(VueSweetAlert)
    .mount('#app')
