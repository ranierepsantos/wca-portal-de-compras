<template>
  <v-app>
    <v-navigation-drawer v-model="drawer" app color="primary"
      style="padding-top: 30px; padding-left: 2px; padding-right: 2px">
      <img src="../assets/images/logoWCA.png" alt="" class="side-bar-logo" />
      <br />
      <br />
      <v-btn block color="orange" rounded="lg" class="text-capitalize" disabled>
        <b>Requisição de compra</b>
      </v-btn>
      <br />
      <v-list class="text-left" density="compact">
        <v-list-item v-for="item in menuItems" :key="item.title" :value="item" active-color="info"
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
        <v-menu activator="parent">
          <v-list :lines="false" density="compact" nav>
            <v-list-item class="text-primary" @click="logout()">
              <template v-slot:prepend>
                <v-icon icon="mdi-export" size="small"></v-icon>
              </template>

              <v-list-item-title v-text="'Sair'"></v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </v-btn>
    </v-app-bar>

    <!-- Sizes your content based upon application components -->
    <v-main>
      <!-- Provides the application the proper gutter -->
      <v-container fluid>
        <!-- If using vue-router -->
        <router-view></router-view>
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
//VARIABLES
const drawer = ref(true);
const menuItems = ref([
  {
    title: "Home",
    value: 1,
    route: "/app",
    permissao: "livre"
  },
  {
    title: "Clientes",
    value: 2,
    route: "/app/clientes",
    permissao: "cliente"
  },
  {
    title: "Filiais",
    value: 2,
    route: "/app/filiais",
    permissao: "filial"
  },
  {
    title: "Perfil",
    value: 2,
    route: "/app/perfil",
    permissao: "perfil"
  },
  {
    title: "Usuários",
    value: 3,
    route: "/app/usuarios",
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
function logout()
{
  authStore.finishSession()
  router.push({ name: "login" })
}

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
  color: white;
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