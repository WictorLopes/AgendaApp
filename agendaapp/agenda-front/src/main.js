import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import PrimeVue from 'primevue/config';
import PrimeButton from 'primevue/button';
import PrimeInputText from 'primevue/inputtext';
import PrimeDataTable from 'primevue/datatable';
import PrimeColumn from 'primevue/column';
import VueTheMask from 'vue-the-mask';


import 'primevue/resources/themes/saga-blue/theme.css';
import 'primevue/resources/primevue.min.css';
import 'primeicons/primeicons.css';

const app = createApp(App);
app.use(router);
app.use(PrimeVue);
app.use(VueTheMask);

app.component('PrimeButton', PrimeButton);
app.component('PrimeInputText', PrimeInputText);
app.component('PrimeDataTable', PrimeDataTable);
app.component('PrimeColumn', PrimeColumn);

app.mount('#app');
