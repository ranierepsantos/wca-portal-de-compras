import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const routes = [
  {
    path: "/",
    redirect: "/app",
    component: () => import("../views"), //webpackChunkName app
    children: [
      {
        path: 'app',
        name: 'home',
        component: HomeView
      },
      {
        path: 'app/about',
        name: 'about',
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "about" */ '../views/AboutView.vue')
      },
      {
        path: 'app/usuarios',
        name: 'usuarios',
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "usuarios" */ '../views/usuarios/index.vue')
      }
    ]
  },
  {
    path: "/app/sessions",
    component: () => import("../views/sessoes"),
    redirect: "/app/sessions/login",
    children: [
      {
        path: "login",
        name: "login",
        component: () => import("../views/sessoes/login")
      },
      {
        path: "recuperarSenha",
        name: "recuperarSenha",
        component: () => import("../views/sessoes/recuperarSenha")
      }
    ]
  },
  
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
