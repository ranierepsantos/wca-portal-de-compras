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
      
      <v-list class="text-left" density="compact">
        <v-list-item v-for="item in menuItems.sort(compararValor('title'))" :key="item.title" :value="item.value" active-color="info"
          v-show="checkPermissao(item.permissao)">
          <router-link :to="item.route" class="text-decoration-none">
            <v-list-item-title>
              {{ item.title }}
            </v-list-item-title>
          </router-link>
        </v-list-item>
        <br/>
        <v-divider color="white"></v-divider>
        <br/>
        <v-list-group value="auxmenu" v-show="checkPermissao(auxPermissao)"> 
          <template v-slot:activator="{ props }">
            <v-list-item
              v-bind="props"
              active-color="info"
              style="color: white;"
            >
            <v-list-item-title >
              Cadastros Auxiliares
            </v-list-item-title>
            </v-list-item>
          </template>

          <v-list-item v-for="item in auxItems.sort(compararValor('title'))" :key="item.title" :value="item.value" active-color="info"
          v-show="checkPermissao(item.permissao)">
          <router-link :to="item.route" class="text-decoration-none">
            <v-list-item-title>
              {{ item.title }}
            </v-list-item-title>
          </router-link>
        </v-list-item>
        </v-list-group>

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
          <v-col><card-list :list-data="listPendente" color="orange-lighten-2" card-title="Aguardando"></card-list></v-col>
          <v-col><card-list :list-data="listAndamento" color="lime-darken-1" card-title="Em Andamento"></card-list></v-col>
          <v-col><card-list :list-data="listConcluido" color="deep-purple-lighten-1" card-title="Concluídos"></card-list></v-col>
        </v-row>
         -->
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
import cardList from "@/components/share/cardlist.vue";

//VARIABLES
const listPendente = ref([
  {id: 1, nome:'Pendente 1', list: 1},
  {id: 2, nome:'Pendente 2', list: 1},
  {id: 3, nome:'Pendente 3', list: 1}
])

const listAndamento = ref([
  {id: 1, nome:'Andamento 1', list: 2},
  {id: 2, nome:'Andamento 2', list: 2},
  {id: 3, nome:'Andamento 3', list: 2}
])

const listConcluido = ref([
  {id: 1, nome:'Concluido 1', list: 3},
  {id: 2, nome:'Concluido 2', list: 3},
  {id: 3, nome:'Concluido 3', list: 3}
])


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
  {
    title: "Funcionários",
    value: 9,
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
    title: "Vagas",
    value: 19,
    route: "/share/vagas",
    permissao: "vaga-criar|vaga-executar|vaga-finalizar"
  },
  
]);

const auxItems = ref([
  {
    title: "Assuntos",
    value: 11,
    route: "/share/assuntos",
    permissao: "assunto"
  },
  {
    title: "Docs. Complementares",
    value: 12,
    route: "/share/documentoscomplementares",
    permissao: "documentocomplementar"
  },
  {
    title: "Escalas",
    value: 13,
    route: "/share/escalas",
    permissao: "escala"
  },
  {
    title: "Escolaridade",
    value: 14,
    route: "/share/escolaridades",
    permissao: "escolaridade"
  },
  {
    title: "Funções",
    value: 15,
    route: "/share/funcoes",
    permissao: "funcao"
  },
  {
    title: "Gestores",
    value: 16,
    route: "/share/gestores",
    permissao: "gestor"
  },
  {
    title: "Horários",
    value: 17,
    route: "/share/horarios",
    permissao: "horario"
  },
  {
    title: "Tipos de Contrato",
    value: 18,
    route: "/share/tiposcontrato",
    permissao: "tipocontrato"
  },
  {
    title: "Tipos de Faturamento",
    value: 20,
    route: "/share/tiposfaturamento",
    permissao: "tipofaturamento"
  },
]);

const authStore = useAuthStore();
const usuario = computed(() =>
{
  return authStore.user;
})

const auxPermissao = computed(() => {
  let _perms = auxItems.value.map(p => p.permissao)
  return _perms.join("|")
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