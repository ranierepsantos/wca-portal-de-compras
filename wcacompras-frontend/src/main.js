import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { createPinia } from 'pinia'
import vuetify from "./plugins/vuetify"

import VueSweetAlert from 'vue-sweetalert2'
import '@sweetalert2/theme-material-ui/material-ui.css'


const pinia = createPinia()

createApp(App)
    .use(router)
    .use(vuetify)
    .use(pinia)
    .use(VueSweetAlert)
    .mount('#app')
