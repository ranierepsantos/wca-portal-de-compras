import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import { useAuthStore} from '@/store/auth.store'
const routes = [
  {
    path: "/",
    redirect: "sistemas"
  },
  {
    path: "/compras",
    component: () => import("../views/indexCompras"), //webpackChunkName app
    children: [
      {
        path: '',
        name: 'compras',
        meta: {sistema: 1},
        component: HomeView,
        beforeEnter: protectRoute,
      },
      {
        path: 'categorias',
        name: 'categorias',
        meta: {permissao: "categoria", sistema: 1},
        beforeEnter: protectRoute,
        //meta: {permissao: "categorias"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "categorias" */ '../views/compras/categorias'),
        
      },
      {
        path: 'clientes',
        name: 'clientes',
        beforeEnter: protectRoute,
        meta: {permissao: "cliente", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "cliente" */ '../views/compras/clientes'),
        
      },
      {
        path: 'clientes/cadastro',
        name: 'clienteCadastro',
        beforeEnter: protectRoute,
        meta: {permissao: "cliente", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "cliente" */ '../views/compras/clientes/cadastro.vue'),
      },
      {
        path: 'configuracoes',
        name: 'configuracoes',
        beforeEnter: protectRoute,
        meta: {permissao: "configuracao", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "configuracoes" */ '../views/compras/configuracoes/configuracoes'),
        
      },
      {
        path: 'filiais',
        name: 'comprasFiliais',
        beforeEnter: protectRoute,
        meta: {permissao: "filial", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "filial" */ '../views/filiais'),
        
      },
      {
        path: 'fornecedores',
        name: 'fornecedores',
        beforeEnter: protectRoute,
        meta: {permissao: "fornecedor", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "fornecedor" */ '../views/compras/fornecedores'),
        
      },
      {
        path: 'fornecedores/cadastro',
        name: 'fornecedorCadastro',
        beforeEnter: protectRoute,
        meta: {permissao: "fornecedor", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "fornecedor" */ '../views/compras/fornecedores/cadastro.vue'),
        
      },
      {
        path: 'fornecedores/produtos',
        name: 'fornecedorProdutos',
        beforeEnter: protectRoute,
        meta: {permissao: "fornecedor", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "fornecedor" */ '../views/compras/fornecedores/produtosindex.vue'),
        
      },
      {
        path: 'perfil',
        name: 'perfil',
        beforeEnter: protectRoute,
        meta: {permissao: "perfil", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "perfil" */ '../views/perfil'),
        
      },
      {
        path: 'perfil/cadastro',
        name: 'perfilCadastro',
        meta: {permissao: "perfil", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "perfil" */ '../views/perfil/cadastro.vue'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'recorrencias',
        name: 'recorrencias',
        meta: {permissao:"recorrencia|recorrencias_view_others_users", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "requisicoes" */ '../views/compras/recorrencias')
      },
      {
        path: 'recorrencias/cadastro',
        name: 'recorrenciaCadastro',
        meta: {permissao:"recorrencia|recorrencias_view_others_users", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "requisicoes" */ '../views/compras/recorrencias/cadastro')
      },
      {
        path: 'recorrencias/edicao/:codigo',
        name: 'recorrenciaEdicao',
        meta: {permissao:"recorrencia|recorrencias_view_others_users", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "requisicoes" */ '../views/compras/recorrencias/cadastro')
      },
      {
        path: 'requisicoes',
        name: 'requisicoes',
        meta: {permissao:"aprova_requisicao|aprova_requisicao_cliente|requisicao|requisicao_all_users", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "requisicoes" */ '../views/compras/requisicoes')
      },
      {
        path: 'requisicoes/cadastro',
        name: 'requisicaoCadastro',
        meta: {permissao:"requisicao", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "requisicoes" */ '../views/compras/requisicoes/cadastro')
      },
      {
        path: 'requisicoes/edicao/:requisicao',
        name: 'requisicaoEdicao',
        meta: {permissao:"aprova_requisicao|aprova_requisicao_cliente|requisicao|requisicao_all_users", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "requisicoes" */ '../views/compras/requisicoes/edicao')
      },
      {
        path: 'usuarios',
        name: 'comprasUsuarios',
        meta: {permissao: "usuario", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "usuarios" */ '../views/compras/usuarios')
      },
      {
        path: 'usuarios/cadastro',
        name: 'comprasUsuarioCadastro',
        meta: {permissao: "usuario", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "usuarios" */ '../views/compras/usuarios/cadastro')
      },
      
    ]
  },
  {
    path: "/reembolso",
    component: () => import("../views/indexReembolso"), //webpackChunkName app
    children: [
      {
        path: '',
        name: 'reembolso',
        component: HomeView,
        meta: {sistema: 2},
        beforeEnter: protectRoute,
      },
      {
        path: 'clientes',
        name: 'reembolsoClientes',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 2},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "perfil" */ '../views/reembolso/clientes'),
        
      },
      {
        path: 'clientes/cadastro',
        name: 'reembolsoCadastroCliente',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 2},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "perfil" */ '../views/reembolso/clientes/cadastro'),
        
      },
      {
        path: 'faturamento',
        name: 'reembolsoFaturamento',
        meta: {permissao: "livre", sistema: 2},
        beforeEnter: protectRoute,
        //meta: {permissao: "categorias"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso_solicitacao" */ '../views/reembolso/faturamento/index.vue'),
        
      },
      {
        path: 'faturamento/cadastro',
        name: 'reembolsoFaturamentoCadastro',
        meta: {permissao: "livre", sistema: 2},
        beforeEnter: protectRoute,
        //meta: {permissao: "categorias"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso_solicitacao" */ '../views/reembolso/faturamento/cadastro.vue'),
        
      },
      {
        path: 'filiais',
        name: 'reembolsoFiliais',
        beforeEnter: protectRoute,
        meta: {permissao: "filial", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "filial" */ '../views/filiais'),
        
      },
      {
        path: 'movimentacao-financeira',
        name: 'reembolsoContaCorrente',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 2},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "usuarios" */ '../views/reembolso/usuarios/contacorrente'),
        
      },
      {
        path: 'perfil',
        name: 'reembolsoPerfil',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 2},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "perfil" */ '../views/perfil'),
        
      },
      {
        path: 'perfil/cadastro',
        name: 'reembolsoPerfilCadastro',
        meta: {permissao: "livre", sistema: 2},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "perfil" */ '../views/perfil/cadastro.vue'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'solicitacoes',
        name: 'reembolsoSolicitacoes',
        meta: {permissao: "livre", sistema: 2},
        beforeEnter: protectRoute,
        //meta: {permissao: "categorias"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso_solicitacao" */ '../views/reembolso/solicitacoes'),
        
      },
      {
        path: 'solicitacao',
        name: 'reembolsoSolicitacaoCadastro',
        meta: {permissao: "livre", sistema: 2},
        beforeEnter: protectRoute,
        //meta: {permissao: "categorias"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso_solicitacao" */ '../views/reembolso/solicitacoes/cadastro.vue'),
        
      },
      {
        path: 'tipo-despesa',
        name: 'tipoDespesa',
        meta: {permissao: "livre", sistema: 2},
        beforeEnter: protectRoute,
        //meta: {permissao: "categorias"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso_tipodespesa" */ '../views/reembolso/tipos_despesa'),
      },
      {
        path: 'usuarios',
        name: 'reembolsoUsuarios',
        meta: {permissao: "livre"},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "usuarios" */ '../views/reembolso/usuarios')
      },
      {
        path: 'usuarios/cadastro',
        name: 'reembolsoUsuarioCadastro',
        meta: {permissao: "livre"},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "usuarios" */ '../views/reembolso/usuarios/cadastro')
      },
    ]
  },
  {
    path: '/app/acessonegado',
    name: 'acessonegado',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "pages" */ '../views/pages/acessoNegado.vue')
  },
  {
    path: "/app/requisicoes/aprovar/:token",
    name: "aprovar",
    meta: { sistema: 1},
    component: () => import('../views/compras/requisicoes/aprovar')
  },
  {
    path: "/app/agradecimento",
    name: "agradecimento",
    meta: { sistema: 1},
    component: () => import('../views/compras/requisicoes/agradecimento')
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
    path: "/sistemas",
    name: "sistemas",
    beforeEnter: protectRoute,
    component: () => import("../views/Sistema")
  },
  {
    path: "/:pathMatch(.*)*",
    component: () => import(/* webpackChunkName: "pages" */ '../views/pages/notFound')
  }
]


async function protectRoute(to, from, next)
{
  
  let authStore = useAuthStore();
  if (!authStore.isAuthenticated()) next({ name: 'login' })

  //checa se a rota que esta acesso é do sistema atual que esta
  if (to.meta.sistema != undefined && to.meta.sistema != authStore.sistema.id ) {
    //não é o mesmo sistema, checar se o usuario tem acesso ao sistema
    let sistema = authStore.user.sistemas.filter(c => c.id == to.meta.sistema)[0];
    
    if (sistema != undefined)
      await authStore.setSistema(sistema)
    else 
      next({name: "acessonegado"})
  }

  if (to.meta.permissao != undefined && !authStore.hasPermissao(to.meta.permissao)) 
    next({name: "acessonegado"})
  else 
    next()
}


const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
