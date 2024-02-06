import { createRouter, createWebHistory } from 'vue-router'
import { protectRoute } from './functions';

import { share } from './share';
import { reembolso } from './reembolso';
import { compras } from './compras';

const routes = [
  {
    path: "/",
    redirect: "sistemas"
  },
  compras,
  reembolso,
  share,
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
    component: () => import("../views/pages/Sistema")
  },
  {
    path: "/:pathMatch(.*)*",
    component: () => import(/* webpackChunkName: "pages" */ '../views/pages/notFound')
  }
]





const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
