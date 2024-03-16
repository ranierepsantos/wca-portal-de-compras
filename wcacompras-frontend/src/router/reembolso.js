import { protectRoute } from "./functions";

export const reembolso = {
    path: "/reembolso",
    component: () => import("../views/reembolso/indexReembolso"), //webpackChunkName app
    children: [
      {
        path: '',
        name: 'reembolso',
        component: () => import(/* webpackChunkName: "reembolso" */ '@/views/pages/HomeView.vue'),
        meta: {sistema: 2},
        beforeEnter: protectRoute,
      },
      {
        path: 'clientes',
        name: 'reembolsoClientes',
        beforeEnter: protectRoute,
        meta: {permissao: "cliente", sistema: 2},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/reembolso/clientes'),
        
      },
      {
        path: 'clientes/cadastro',
        name: 'reembolsoCadastroCliente',
        beforeEnter: protectRoute,
        meta: {permissao: "cliente", sistema: 2},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/reembolso/clientes/cadastro'),
        
      },
      {
        path: 'contas',
        name: 'reembolsoContas',
        beforeEnter: protectRoute,
        meta: {permissao: "contas", sistema: 2},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/reembolso/contacorrente'),
      },
      {
        path: 'faturamento',
        name: 'reembolsoFaturamento',
        meta: {permissao: "faturamento|cliente_faturamento", sistema: 2},
        beforeEnter: protectRoute,
        //meta: {permissao: "categorias"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/reembolso/faturamento/index.vue'),
        
      },
      {
        path: 'faturamento/cadastro',
        name: 'reembolsoFaturamentoCadastro',
        meta: {permissao: "faturamento|cliente_faturamento", sistema: 2},
        beforeEnter: protectRoute,
        //meta: {permissao: "categorias"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/reembolso/faturamento/cadastro.vue'),
        
      },
      {
        path: 'filiais',
        name: 'reembolsoFiliais',
        beforeEnter: protectRoute,
        meta: {permissao: "filial"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/filiais'),
        
      },
      {
        path: 'movimentacao-financeira',
        name: 'reembolsoContaCorrente',
        beforeEnter: protectRoute,
        meta: {permissao: "contacorrente", sistema: 2},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/reembolso/usuarios/contacorrente'),
        
      },
      {
        path: 'perfil',
        name: 'reembolsoPerfil',
        beforeEnter: protectRoute,
        meta: {permissao: "perfil", sistema: 2},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/perfil'),
        
      },
      {
        path: 'perfil/cadastro',
        name: 'reembolsoPerfilCadastro',
        meta: {permissao: "perfil", sistema: 2},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/perfil/cadastro.vue'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'solicitacoes',
        name: 'reembolsoSolicitacoes',
        meta: {permissao: "solicitacao|cliente_aprovacao|wca_aprovacao", sistema: 2},
        beforeEnter: protectRoute,
        //meta: {permissao: "categorias"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/reembolso/solicitacoes'),
        
      },
      {
        path: 'solicitacao',
        name: 'reembolsoSolicitacaoCadastro',
        meta: {permissao: "solicitacao|cliente_aprovacao|wca_aprovacao|faturamento|cliente_faturamento", sistema: 2},
        beforeEnter: protectRoute,
        //meta: {permissao: "categorias"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/reembolso/solicitacoes/cadastro.vue'),
        
      },
      {
        path: 'tipo-despesa',
        name: 'tipoDespesa',
        meta: {permissao: "tipodespesa", sistema: 2},
        beforeEnter: protectRoute,
        //meta: {permissao: "categorias"},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/reembolso/tipos_despesa'),
      },
      {
        path: 'usuarios',
        name: 'reembolsoUsuarios',
        meta: {permissao: "usuario"},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/reembolso/usuarios')
      },
      {
        path: 'usuarios/cadastro',
        name: 'reembolsoUsuarioCadastro',
        meta: {permissao: "usuario"},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "reembolso" */ '../views/reembolso/usuarios/cadastro')
      },
    ]
  }