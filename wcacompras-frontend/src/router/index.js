import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import { useAuthStore} from '@/store/auth.store'
const routes = [
  {
    path: "/",
    redirect: "/app",
    component: () => import("../views"), //webpackChunkName app
    children: [
      {
        path: 'app',
        name: 'home',
        component: HomeView,
        beforeEnter: protectRoute,
      },
      {
        path: 'app/clientes',
        name: 'clientes',
        beforeEnter: protectRoute,
        meta: {permissao: "cliente"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "cliente" */ '../views/clientes'),
        
      },
      {
        path: 'app/clientes/cadastro',
        name: 'clienteCadastro',
        beforeEnter: protectRoute,
        meta: {permissao: "cliente"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "cliente" */ '../views/clientes/cadastro.vue'),
        
      },
      {
        path: 'app/filiais',
        name: 'filiais',
        beforeEnter: protectRoute,
        meta: {permissao: "filial"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "filial" */ '../views/filiais'),
        
      },
      {
        path: 'app/fornecedores',
        name: 'fornecedores',
        beforeEnter: protectRoute,
        meta: {permissao: "fornecedor"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "fornecedor" */ '../views/fornecedores'),
        
      },
      {
        path: 'app/fornecedores/cadastro',
        name: 'fornecedorCadastro',
        beforeEnter: protectRoute,
        meta: {permissao: "fornecedor"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "fornecedor" */ '../views/fornecedores/cadastro.vue'),
        
      },
      {
        path: 'app/fornecedores/produtos',
        name: 'fornecedorProdutos',
        beforeEnter: protectRoute,
        meta: {permissao: "fornecedor"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "fornecedor" */ '../views/fornecedores/produtosindex.vue'),
        
      },
      {
        path: 'app/perfil',
        name: 'perfil',
        beforeEnter: protectRoute,
        meta: {permissao: "perfil"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "perfil" */ '../views/perfil'),
        
      },
      {
        path: 'app/perfil/cadastro',
        name: 'perfilCadastro',
        meta: {permissao: "perfil"},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "perfil" */ '../views/perfil/cadastro.vue'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'app/requisicoes',
        name: 'requisicoes',
        meta: {permissao:"requisicao|requisicao_all_users"},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "requisicoes" */ '../views/requisicoes')
      },
      {
        path: 'app/requisicoes/cadastro',
        name: 'requisicaoCadastro',
        meta: {permissao:"requisicao|requisicao_all_users"},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "requisicoes" */ '../views/requisicoes/cadastro')
      },
      {
        path: 'app/requisicoes/edicao/:requisicao',
        name: 'requisicaoEdicao',
        meta: {permissao:"requisicao|requisicao_all_users"},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "requisicoes" */ '../views/requisicoes/edicao')
      },
      
      {
        path: 'app/usuarios',
        name: 'usuarios',
        meta: {permissao: "usuario"},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "usuarios" */ '../views/usuarios')
      },
      {
        path: 'app/acessonegado',
        name: 'acessonegado',
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "pages" */ '../views/pages/acessoNegado.vue')
      },
    ]
  },
  {
    path: "/app/requisicoes/aprovar/:token",
    name: "aprovar",
    component: () => import('../views/requisicoes/aprovar')
  },
  {
    path: "/login",
    name: "login",
    component: () => import("../views/sessoes/login")
  },
  {
    path: "/recuperar-senha",
    name: "recuperarSenha",
    component: () => import("../views/sessoes/recuperarSenha")
  },
  {
    path: "/recuperar-senha/:token",
    name: "resetSenha",
    component: () => import("../views/sessoes/resetSenha")
  },
  {
    path: "/:pathMatch(.*)*",
    component: () => import(/* webpackChunkName: "pages" */ '../views/pages/notFound')
  }
]


function protectRoute(to, from, next)
{
  let authStore = useAuthStore();
  if (!authStore.isAuthenticated()) next({ name: 'login' })
  if (to.meta.permissao != undefined && !authStore.hasPermissao(to.meta.permissao)) 
    next({name: "acessonegado"})
  else next()
}


const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
