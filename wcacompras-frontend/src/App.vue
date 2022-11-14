<template>
  <v-app>
    <v-navigation-drawer
      app
      color="primary"
      style="padding-top: 30px; padding-left: 2px; padding-right: 2px"
    >
      <img src="./assets/images/logoWCA.png" alt="" class="side-bar-logo" />
      <br />
      <br />
      <v-btn block color="orange" rounded="lg" class="text-capitalize">
        <b>Requisição de compra</b>
      </v-btn>
      <br />
      <v-list class="text-left" density="compact">
        <v-list-item
          v-for="item in items"
          :key="item.title"
          :value="item"
          active-color="info"
        >
          <router-link :to="item.route" class="text-decoration-none">
            <v-list-item-content>
              <v-list-item-title>
                {{ item.title }}
              </v-list-item-title>
            </v-list-item-content>
          </router-link>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar elevation="3" rounded>
      <v-app-bar-nav-icon></v-app-bar-nav-icon>
      <v-spacer></v-spacer>

      <v-btn icon>
        <v-icon>mdi-export</v-icon>
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
import { reactive } from "vue";

const items = reactive([
  {
    title: "Home",
    value: 1,
    route: "/",
  },
  {
    title: "About",
    value: 2,
    route: "/about",
  },
]);

const listItems = reactive([
  { text: "Real-Time", icon: "mdi-clock" },
  { text: "Audience", icon: "mdi-account" },
  { text: "Conversions", icon: "mdi-flag" },
]);
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
