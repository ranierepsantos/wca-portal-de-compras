import { protectRoute } from "./functions";

export const compras = {
    path: "/compras",
    component: () => import(/* webpackChunkName: "compras" */ '@/views/indexCompras'),
    children: [
      {
        path: '',
        name: 'compras',
        meta: {sistema: 1},
        component: () => import(/* webpackChunkName: "compras" */ '@/views/HomeView.vue'),
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
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/categorias'),
        
      },
      {
        path: 'clientes',
        name: 'clientes',
        beforeEnter: protectRoute,
        meta: {permissao: "cliente", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/clientes'),
        
      },
      {
        path: 'clientes/cadastro',
        name: 'clienteCadastro',
        beforeEnter: protectRoute,
        meta: {permissao: "cliente", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/clientes/cadastro.vue'),
      },
      {
        path: 'configuracoes',
        name: 'configuracoes',
        beforeEnter: protectRoute,
        meta: {permissao: "configuracao", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/configuracoes/configuracoes'),
        
      },
      {
        path: 'filiais',
        name: 'comprasFiliais',
        beforeEnter: protectRoute,
        meta: {permissao: "filial", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/filiais'),
        
      },
      {
        path: 'fornecedores',
        name: 'fornecedores',
        beforeEnter: protectRoute,
        meta: {permissao: "fornecedor", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/fornecedores'),
        
      },
      {
        path: 'fornecedores/cadastro',
        name: 'fornecedorCadastro',
        beforeEnter: protectRoute,
        meta: {permissao: "fornecedor", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/fornecedores/cadastro.vue'),
        
      },
      {
        path: 'fornecedores/produtos',
        name: 'fornecedorProdutos',
        beforeEnter: protectRoute,
        meta: {permissao: "fornecedor", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/fornecedores/produtosindex.vue'),
        
      },
      {
        path: 'perfil',
        name: 'perfil',
        beforeEnter: protectRoute,
        meta: {permissao: "perfil", sistema: 1},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/perfil'),
        
      },
      {
        path: 'perfil/cadastro',
        name: 'perfilCadastro',
        meta: {permissao: "perfil", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/perfil/cadastro.vue'),
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
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/recorrencias')
      },
      {
        path: 'recorrencias/cadastro',
        name: 'recorrenciaCadastro',
        meta: {permissao:"recorrencia|recorrencias_view_others_users", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/recorrencias/cadastro')
      },
      {
        path: 'recorrencias/edicao/:codigo',
        name: 'recorrenciaEdicao',
        meta: {permissao:"recorrencia|recorrencias_view_others_users", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/recorrencias/cadastro')
      },
      {
        path: 'requisicoes',
        name: 'requisicoes',
        meta: {permissao:"aprova_requisicao|aprova_requisicao_cliente|requisicao|requisicao_all_users", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/requisicoes')
      },
      {
        path: 'requisicoes/cadastro',
        name: 'requisicaoCadastro',
        meta: {permissao:"requisicao", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/requisicoes/cadastro')
      },
      {
        path: 'requisicoes/edicao/:requisicao',
        name: 'requisicaoEdicao',
        meta: {permissao:"aprova_requisicao|aprova_requisicao_cliente|requisicao|requisicao_all_users", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/requisicoes/edicao')
      },
      {
        path: 'usuarios',
        name: 'comprasUsuarios',
        meta: {permissao: "usuario", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/usuarios')
      },
      {
        path: 'usuarios/cadastro',
        name: 'comprasUsuarioCadastro',
        meta: {permissao: "usuario", sistema: 1},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "compras" */ '../views/compras/usuarios/cadastro')
      },
      
    ]
  }