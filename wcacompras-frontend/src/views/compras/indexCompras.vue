<template>
  <v-app>
    <v-navigation-drawer v-model="drawer" app color="primary"
      style="padding-top: 30px; padding-left: 2px; padding-right: 2px">
      <img src="@/assets/images/logoWCA.png" alt="" class="side-bar-logo" />
      <br />
      <br />
      <v-btn block color="orange" rounded="lg" class="text-capitalize"
        @click="router.push({ name: 'requisicaoCadastro' })"
        v-show="checkPermissao('requisicao|requisicao_all_users')">
        <b>Requisição de compra</b>
      </v-btn>
      <br />
      <v-list class="text-left" density="compact">
        <v-list-item v-for="item in menuItems" :key="item.title" :value="item.value" active-color="info"
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
import { useRouter } from "vue-router";
import headerUserMenu from "@/components/headerUserMenu.vue";
//VARIABLES
const drawer = ref(true);
const menuItems = ref([
  {
    title: "Home",
    value: 1,
    route: "/compras",
    permissao: "livre"
  },
  {
    title: "Categorias",
    value: 2,
    route: "/compras/categorias",
    permissao: "categoria"
  },
  {
    title: "Clientes",
    value: 3,
    route: "/compras/clientes",
    permissao: "cliente"
  },
  {
    title: "Configurações",
    value: 10,
    route: "/compras/configuracoes",
    permissao: "configuracao"
  },
  {
    title: "Filiais",
    value: 4,
    route: "/compras/filiais",
    permissao: "filial"
  },
  {
    title: "Fornecedores",
    value: 5,
    route: "/compras/fornecedores",
    permissao: "fornecedor"
  },
  {
    title: "Perfil",
    value: 6,
    route: "/compras/perfil",
    permissao: "perfil"
  },
  {
    title: "Recorrências",
    value: 7,
    route: "/compras/recorrencias",
    permissao: "recorrencia|recorrencias_view_others_users"
  },
  {
    title: "Requisições",
    value: 8,
    route: "/compras/requisicoes",
    permissao: "aprova_requisicao|aprova_requisicao_cliente|requisicao|requisicao_all_users"
  },
  {
    title: "Usuários",
    value: 9,
    route: "/compras/usuarios",
    permissao: "usuario"
  },
]);
const authStore = useAuthStore();
const router = useRouter();
const usuario = computed(() =>
{
  return authStore.user;
})


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