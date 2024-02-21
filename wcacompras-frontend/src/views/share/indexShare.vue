<template>
  <v-app>
    <v-navigation-drawer v-model="drawer" app color="#4A148C"
      style="padding-top: 30px; padding-left: 2px; padding-right: 2px;">
      <img src="@/assets/images/logoWCA.png" alt="" class="side-bar-logo" />
      <br />
      <br />
      <v-btn block color="orange" rounded="lg" class="text-capitalize"
        @click="router.push({ name: 'shareSolicitacaoCreate' })">
        <b>Nova solicitação</b>
      </v-btn>
      <br />
      <v-list class="text-left" density="compact">
        <v-list-item v-for="item in menuItems.sort(compararValor('title'))" :key="item.title" :value="item.value" active-color="info"
          v-show="checkPermissao(item.permissao)">
          <router-link :to="item.route" class="text-decoration-none">
            <v-list-item-title>
              {{ item.title }}
            </v-list-item-title>
          </router-link>
        </v-list-item>
        <v-list-item v-show="checkPermissao('livre')" active-color="info">
          <router-link to="/share/configuracoes" class="text-decoration-none">
            <v-list-item-title>
              Configurações
            </v-list-item-title>
          </router-link>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar elevation="3" rounded>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-spacer></v-spacer>

      

    <!-- <v-btn class="text-none" stacked>
      <v-badge :content="notificacoes.length" color="error">
        <v-icon>mdi-bell-outline</v-icon>
      </v-badge>
      <notificacao-list :notificacoes="notificacoes"/>
    </v-btn> -->

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
        <!-- <v-row>
          <v-col><card-list :list-data="list1" color="orange-lighten-2" card-title="Aguardando"></card-list></v-col>
          <v-col><card-list :list-data="list2" color="lime-darken-1" card-title="Em Andamento"></card-list></v-col>
          <v-col><card-list :list-data="list3" color="deep-purple-lighten-1" card-title="Concluídos"></card-list></v-col>
        </v-row> -->
        
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
import headerUserMenu from "@/components/headerUserMenu.vue";
import { compararValor } from "@/helpers/functions";
import { onMounted } from "vue";
import { useShareSolicitacaoStore } from "@/store/share/solicitacao.store";
import { useRouter,useRoute } from "vue-router";


//VARIABLES
const router = useRouter();
const route = useRoute();
const drawer = ref(true);
const menuItems = ref([
  {
    title: " Home",
    value: 1,
    route: "/share",
    permissao: "livre"
  },
  {
    title: "Clientes",
    value: 2,
    route: "/share/clientes",
    permissao: "livre"
  },
  {
    title: "Desligamento",
    value: 3,
    route: "/share/desligamento",
    permissao: "livre"
  },
  {
    title: "Mudança de base",
    value: 4,
    route: "/share/mudancabase",
    permissao: "livre"
  },
  {
    title: "Comunicados",
    value: 5,
    route: "/share/comunicados",
    permissao: "livre"
  },
  {
    title: "Perfil",
    value: 6,
    route: "/share/perfil",
    permissao: "livre"
  },
  {
    title: "Usuários",
    value: 7,
    route: "/share/usuarios",
    permissao: "livre"
  },
  {
    title: "Férias",
    value: 8,
    route: "/share/ferias",
    permissao: "livre"
  },
]);

const authStore = useAuthStore();
const usuario = computed(() =>
{
  return authStore.user;
})


//VUE - FUNCTIONS
onMounted(async () => {
  useShareSolicitacaoStore().listarStatusSolicitacao();
  useShareSolicitacaoStore().listarMotivosDemissao();
});

//FUNCTIONS
function checkPermissao(permissao)
{
  return authStore.hasPermissao(permissao)
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