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
        path: 'app/about',
        name: 'about',
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "about" */ '../views/AboutView.vue')
      },
      {
        path: 'app/perfil',
        name: 'perfil',
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "usuarios" */ '../views/perfil')
      },
      {
        path: 'app/perfil/cadastro',
        name: 'perfilCadastro',
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "usuarios" */ '../views/perfil/cadastro.vue'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'app/usuarios',
        name: 'usuarios',
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "usuarios" */ '../views/usuarios')
      },
      
    ]
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
  }
]


function protectRoute(to, from, next)
{
  let authStore = useAuthStore();
  if (!authStore.isAuthenticated()) next({ name: 'login' })
  else next()
}


const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
