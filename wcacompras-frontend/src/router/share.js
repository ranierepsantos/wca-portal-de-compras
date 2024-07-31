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
        meta: {permissao: "cliente", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/clientes'),
      },
      {
        path: 'cliente',
        name: 'shareClienteCadastro',
        beforeEnter: protectRoute,
        meta: {permissao: "cliente", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/clientes/cadastro'),
      },
      {
        path: 'configuracoes',
        name: 'shareConfiguracoes',
        beforeEnter: protectRoute,
        meta: {permissao: "configuracao", sistema: 3},
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/configuracoes/configuracoes'),
        
      },
      {
        path: 'filial',
        name: 'shareFilial',
        beforeEnter: protectRoute,
        meta: {permissao: "filial", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/filiais'),
      },
      {
        path: 'funcionarios',
        name: 'shareFuncionarios',
        beforeEnter: protectRoute,
        meta: {permissao: "funcionario", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/funcionarios'),
      },
      {
        path: 'funcionario',
        name: 'shareFuncionarioCadastro',
        beforeEnter: protectRoute,
        meta: {permissao: "funcionario", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/funcionarios/cadastro'),
      },
      {
        path: 'perfil',
        name: 'sharePerfil',
        beforeEnter: protectRoute,
        meta: {permissao: "perfil", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/perfil'),
      },
      {
        path: 'perfil/cadastro',
        name: 'sharePerfilCadastro',
        meta: {permissao: "perfil", sistema: 3},
        beforeEnter: protectRoute,
        component: () => import(/* webpackChunkName: "share" */ '../views/perfil/cadastro.vue'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'usuarios',
        name: 'shareUsuarios',
        beforeEnter: protectRoute,
        meta: {permissao: "usuario", sistema: 3},
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
        meta: {permissao: "desligamento-criar|desligamento-executar|desligamento-aprovar", sistema: 3},
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
        name: 'shareDesligamentoCadastro',
        meta: {permissao: "desligamento-executar|desligamento-aprovar|desligamento-finalizar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes/edit'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'notificacoes',
        name: 'shareNotificacoes',
        meta: {permissao: "livre", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/notificacoes'),
      },
      {
        path: 'comunicado',
        name: 'shareComunicado',
        meta: {permissao: "comunicado-criar|comunicado-executar|comunicado-finalizar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes'),
      },
      {
        path: 'comunicado/criar',
        name: 'shareComunicadoCreate',
        meta: {permissao: "comunicado-criar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes/create'),
      },
      {
        path: 'comunicado/editar',
        name: 'shareComunicadoCadastro',
        meta: {permissao: "comunicado-executar|comunicado-finalizar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes/edit'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'assuntos',
        name: 'shareAssuntos',
        beforeEnter: protectRoute,
        meta: {permissao: "assunto", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/assuntos'),
      },
      {
        path: 'ferias',
        name: 'shareFerias',
        meta: {permissao: "ferias-criar|ferias-executar|ferias-finalizar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes'),
      },
      {
        path: 'ferias/criar',
        name: 'shareFeriasCreate',
        meta: {permissao: "ferias-criar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes/create'),
      },
      {
        path: 'ferias/editar',
        name: 'shareFeriasCadastro',
        meta: {permissao: "ferias-executar|ferias-finalizar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes/edit'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'mudancabase',
        name: 'shareMudancabase',
        meta: {permissao: "mudancabase-criar|mudancabase-executar|mudancabase-finalizar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes'),
      },
      {
        path: 'mudancabase/criar',
        name: 'shareMudancaBaseCreate',
        meta: {permissao: "mudancabase-criar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes/create'),
      },
      {
        path: 'mudancabase/editar',
        name: 'shareMudancaBaseCadastro',
        meta: {permissao: "mudancabase-executar|mudancabase-finalizar", sistema: 3},
        beforeEnter: protectRoute,
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "share" */ '../views/share/solicitacoes/edit'),
        props: route => ({ query: route.query.id })
      },
      {
        path: 'documentoscomplementares',
        name: 'shareDocumentosComplementares',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/entidadesauxiliar'),
      },
      {
        path: 'escalas',
        name: 'shareEscalas',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/entidadesauxiliar'),
      },
      {
        path: 'funcoes',
        name: 'shareFuncoes',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/entidadesauxiliar'),
      },
      {
        path: 'gestores',
        name: 'shareGestores',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/entidadesauxiliar'),
      },
      {
        path: 'horarios',
        name: 'shareHorarios',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/entidadesauxiliar'),
      },
      {
        path: 'tiposcontrato',
        name: 'shareTiposContrato',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/entidadesauxiliar'),
      },
      {
        path: 'tiposfaturamento',
        name: 'shareTiposFaturamento',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/entidadesauxiliar'),
      },
      {
        path: 'escolaridades',
        name: 'shareEscolaridades',
        beforeEnter: protectRoute,
        meta: {permissao: "livre", sistema: 3},
        component: () => import(/* webpackChunkName: "share" */ '../views/share/entidadesauxiliar'),
      },
    ]
}
