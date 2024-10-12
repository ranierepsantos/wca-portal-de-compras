<template>
  <v-app>
    <v-navigation-drawer v-model="drawer" app color="teal-darken-4"
      style="padding-top: 30px; padding-left: 2px; padding-right: 2px">
      <img src="@/assets/images/logoWCA.png" alt="" class="side-bar-logo" />
      <br />
      <br />
      <v-btn block color="orange" rounded="lg" class="text-capitalize"
      @click="router.push({ name: 'reembolsoSolicitacaoCadastro' })">
        <b>Nova Solicitação</b>
      </v-btn>
      <br />
      <v-list class="text-left" density="compact">
        <v-list-item v-for="item in menuItems.sort(compararValor('title'))" :key="item.title" :value="item.value" color="info"
          v-show="checkPermissao(item.permissao)">
          <router-link :to="item.route" class="text-decoration-none">
            <v-list-item-title>
              {{ item.title }}
            </v-list-item-title>
          </router-link>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar elevation="3" rounded>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-spacer></v-spacer>

      

      <v-btn class="text-none" stacked>
      <v-badge :content="notificacoes.length" color="error">
        <v-icon>mdi-bell-outline</v-icon>
      </v-badge>
      <notificacao-list :notificacoes="notificacoes"/>
    </v-btn>

      <v-btn variant="text" class="text-capitalize" color="primary">
        <v-icon icon="mdi-account-circle-outline" size="x-large"></v-icon>
        {{ usuario.nome }}
        <header-user-menu/>
      </v-btn>
    </v-app-bar>

    <!-- Sizes your content based upon application components -->
    <v-main>
      <!-- Provides the application the proper gutter -->
      <v-container fluid>
        <!-- If using vue-router -->
        <router-view :key="route.fullPath"></router-view>
      </v-container>
    </v-main>

    <v-footer app>
      <!-- -->
    </v-footer>
  </v-app>
</template>

<script setup>
import { ref, computed } from "vue";
import { useAuthStore } from "@/store/auth.store";
import { useRoute, useRouter } from "vue-router";
import headerUserMenu from "@/components/headerUserMenu.vue";
import notificacaoList from "@/components/notificacaoList.vue";
import { compararValor } from "@/helpers/functions";
import { onMounted } from "vue";
import { onUnmounted } from "vue";
import { useSolicitacaoStore } from "@/store/reembolso/solicitacao.store";
//VARIABLES
const route = useRoute();
const drawer = ref(true);
const menuItems = ref([
  {
    title: " Home",
    value: 1,
    route: "/reembolso",
    permissao: "livre"
  },
  {
    title: "Clientes",
    value: 2,
    route: "/reembolso/clientes",
    permissao: "cliente"
  },
  {
    title: "Faturamento",
    value: 8,
    route: "/reembolso/faturamento",
    permissao: "faturamento|cliente_faturamento"
  },
  {
    title: "Filiais",
    value: 3,
    route: "/reembolso/filiais",
    permissao: "filial"
  },
  {
    title: "Perfil",
    value: 4,
    route: "/reembolso/perfil",
    permissao: "perfil"
  },
  {
    title: "Solicitações",
    value: 5,
    route: "/reembolso/solicitacoes",
    permissao: "solicitacao|wca_aprovacao|cliente_aprovacao"
  },
  {
    title: "Tipos Despesa",
    value: 6,
    route: "/reembolso/tipo-despesa",
    permissao: "tipodespesa"
  },
  {
    title: "Usuários",
    value: 7,
    route: "/reembolso/usuarios",
    permissao: "usuario"
  },
  {
    title: "Movimentações Financeiras",
    value: 9,
    route: "/reembolso/movimentacao-financeira",
    permissao: "contacorrente"
  },
  {
    title: "Contas",
    value: 10,
    route: "/reembolso/contas",
    permissao: "contas"
  },
  
]);
const notificacoes = ref([])
const authStore = useAuthStore();
const router = useRouter();
const usuario = computed(() =>
{
  return authStore.user;
})
const checkNotificacoes = ref(null);

//VUE - FUNCTIONS
onMounted(async () => {
  await useSolicitacaoStore().loadListStatusSolicitacao();
  clearInterval(checkNotificacoes.value)
  notificacoes.value = []
  startCheckNotificacoes();
});

onUnmounted(() => {  clearInterval(checkNotificacoes.value) }) 


//FUNCTIONS
function checkPermissao(permissao)
{
  return authStore.hasPermissao(permissao)
}

function startCheckNotificacoes () {
  checkNotificacoes.value = setInterval(async () => {
    //notificacoes.value = []
    notificacoes.value = await authStore.getNotificacoes();
  },5000)
}


</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

.v-list-item:hover {
  background-color: #9bb9d9;
}

nav a {
  color: white !important;
  font-size: large;
}

.side-bar-logo {
  height: 68.89952087402344px;
  width: 100px;
  left: 95px;
  top: 84px;
  border-radius: 10px;
}
</style>