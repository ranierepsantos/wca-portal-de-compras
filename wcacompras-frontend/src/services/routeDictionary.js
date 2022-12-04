export const route = {
    login: "/Authentication/Autenticar",
    recuperarSenha: "Authentication/RecuperarSenha",
    alterarSenha: "Authentication/AlterarSenha",
    
    clienteCreate: "Cliente",
    clienteUpdate: "Cliente",
    clienteRemove: "Cliente",
    clienteGetById: "Cliente/{id}",
    clienteToList: "Cliente/ToList/{filial}",
    clientePaginate: "Cliente/Paginate/{pageSize}/{page}",

    filialCreate: "Filial",
    filialUpdate: "Filial",
    filialToList: "Filial/ToList",
    filialPaginate: "Filial/Paginate/{pageSize}/{page}",

    perfilCreate: "Perfil",
    perfilUpdate: "Perfil",
    perfilPaginate: "Perfil/Paginate/{pageSize}/{page}",
    perfilToList: "Perfil/ToList",
    perfilGetWithPermissions: "Perfil/{id}",
    permissaoToList: "Permissao/ToList",
    permissaoAll: "Permissao/all",

    tipoFornecimentoToList: "TipoFornecimento/ToList",

    usuarioCreate: "Usuario",
    usuarioRemove: "Usuario",
    usuarioUpdate: "Usuario",
    usuarioPaginate: "Usuario/Paginate/{pageSize}/{page}",

    
}