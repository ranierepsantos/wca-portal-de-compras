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
        path: 'desligamento/create',
        name: 'shareDesligamentoCreate',
        meta: {permissao: "livre", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes/create'),
        props: route => ({ query: route.query.id })
      },
    ]
}
