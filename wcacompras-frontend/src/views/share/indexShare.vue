<template>
  <v-app>
    <v-navigation-drawer v-model="drawer" app color="#4A148C"
      style="padding-top: 30px; padding-left: 2px; padding-right: 2px;">
      <img src="@/assets/images/logoWCA.png" alt="" class="side-bar-logo" />
      <br />
      <!-- <br />
      <v-btn block color="orange" rounded="lg" class="text-capitalize"
        @click="router.push({ name: 'shareSolicitacaoCreate' })">
        <b>Nova solicitação</b>
      </v-btn> -->
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
        <v-list-item v-show="checkPermissao('configuracao')" active-color="info" key="configuracoes" value="9999">
          <router-link to="/share/configuracoes" class="text-decoration-none">
            <v-list-item-title >
              Configurações
            </v-list-item-title>
          </router-link>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar elevation="3" rounded>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-spacer></v-spacer>

    <!--NOTIFICAÇÕES-->
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
import { useRoute } from "vue-router";
import { onUnmounted } from "vue";
import notificacaoList from "@/components/notificacaoList.vue";

//VARIABLES
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
    permissao: "cliente"
  },
  {
    title: "Desligamento",
    value: 3,
    route: "/share/desligamento",
    permissao: "desligamento-criar|desligamento-executar|desligamento-aprovar|desligamento-finalizar"
  },
  {
    title: "Mudança de base",
    value: 4,
    route: "/share/mudancabase",
    permissao: "mudancabase-criar|mudancabase-executar|mudancabase-finalizar"
  },
  {
    title: "Comunicados",
    value: 5,
    route: "/share/comunicado",
    permissao: "comunicado-criar|comunicado-executar|comunicado-finalizar"
  },
  {
    title: "Perfil",
    value: 6,
    route: "/share/perfil",
    permissao: "perfil"
  },
  {
    title: "Usuários",
    value: 7,
    route: "/share/usuarios",
    permissao: "usuario"
  },
  {
    title: "Férias",
    value: 8,
    route: "/share/ferias",
    permissao: "ferias-criar|ferias-executar|ferias-finalizar"
  },
  // {
  //   title: "Filiais",
  //   value: 9,
  //   route: "/share/filial",
  //   permissao: "Filial"
  // },
  {
    title: "Funcionários",
    value: 10,
    route: "/share/funcionarios",
    permissao: "funcionario"
  },
  {
    title: "Notificações",
    value: 10,
    route: "/share/notificacoes",
    permissao: "livre"
  },
  {
    title: "Assuntos",
    value: 10,
    route: "/share/assuntos",
    permissao: "assunto"
  },
  {
    title: "Docs. Complementares",
    value: 11,
    route: "/share/documentoscomplementares",
    permissao: "livre"
  },
  {
    title: "Escalas",
    value: 12,
    route: "/share/escalas",
    permissao: "livre"
  },
  {
    title: "Gestores",
    value: 13,
    route: "/share/gestores",
    permissao: "livre"
  },
  {
    title: "Horários",
    value: 14,
    route: "/share/horarios",
    permissao: "livre"
  },
  {
    title: "Tipos de Contrato",
    value: 15,
    route: "/share/tiposcontrato",
    permissao: "livre"
  },
  
]);

const authStore = useAuthStore();
const usuario = computed(() =>
{
  return authStore.user;
})
const checkNotificacoes = ref(null);
const notificacoes = ref ([])

//VUE - FUNCTIONS
onMounted(async () => {
  await useShareSolicitacaoStore().listarStatusSolicitacao();
  await useShareSolicitacaoStore().getListaAssuntos();
  await useShareSolicitacaoStore().listarMotivosDemissao();
  await useShareSolicitacaoStore().getTipoFerias();
  await useShareSolicitacaoStore().getListaItensMudanca();

  clearInterval(checkNotificacoes.value)
  startCheckNotificacoes()
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
  },10000)
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