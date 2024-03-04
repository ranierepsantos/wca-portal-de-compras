import { protectRoute } from "./functions";

export const share = {
    path: "/share",
    component: () => import("@/views/share/indexShare"), //webpackChunkName app
    children: [
      {
        path: '',
        name: 'share',
        component: () => import(/* webpackChunkName: "share" */ '../views/pages/HomeView.vue'),
        meta: {sistema: 3},
        beforeEnter: protectRoute
      },
      {
        path: 'clientes',
        name: 'shareClientes',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/clientes'),
      },
      {
        path: 'cliente',
        name: 'shareClienteCadastroDisabled',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/clientes/cadastro'),
      },
      {
        path: 'perfil',
        name: 'sharePerfil',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/perfil'),
      },
      {
        path: 'perfil/cadastro',
        name: 'sharePerfilCadastro',
        meta: {permissao: "livre", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/perfil/cadastro.vue'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'usuarios',
        name: 'shareUsuarios',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/usuarios'),
      },
      {
        path: 'usuario/cadastro',
        name: 'shareUsuarioCadastro',
        meta: {permissao: "livre", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/usuarios/cadastro'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'desligamento',
        name: 'shareDesligamento',
        meta: {permissao: "desligamento-criar|desligamento-executar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes'),
      },
      {
        path: 'desligamento/criar',
        name: 'shareDesligamentoCreate',
        meta: {permissao: "desligamento-criar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes/create'),
      },
      {
        path: 'desligamento/editar',
        name: 'shareDesligamentoEdit',
        meta: {permissao: "desligamento-executar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes/edit'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'solicitacao/create',
        name: 'shareSolicitacaoCreate',
        meta: {permissao: "livre", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes/create'),
      },
    ]
}
