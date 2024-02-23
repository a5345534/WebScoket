import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import { FontAwesomeIcon } from './plugs/fontawesome';

var app = createApp(App);
app.component('font-awesome-icon', FontAwesomeIcon);
app.mount('#app')
