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

    fornecedorCreate: "Fornecedor",
    fornecedorUpdate: "Fornecedor",
    fornecedorRemove: "Fornecedor",
    fornecedorGetById: "Fornecedor/{id}",
    fornecedorToList: "Fornecedor/ToList/{filial}",
    fornecedorPaginate: "Fornecedor/Paginate/{pageSize}/{page}",


    perfilCreate: "Perfil",
    perfilUpdate: "Perfil",
    perfilPaginate: "Perfil/Paginate/{pageSize}/{page}",
    perfilToList: "Perfil/ToList",
    perfilGetWithPermissions: "Perfil/{id}",
    permissaoToList: "Permissao/ToList",
    permissaoAll: "Permissao/all",

    produtoCreate: "Fornecedor/Produto",
    produtoUpdate: "Fornecedor/Produto",
    produtoRemove: "Fornecedor/Produto",
    produtoImportFromExcel: "Fornecedor/Produtos/ImportFromExcel",
    produtoGetById: "Fornecedor/{fornecedorId}/Fornecedor/{id}",
    produtoPaginate: "Fornecedor/{fornecedorId}/Produtos/Paginate/{pageSize}/{page}",

    requisicaoAprovar: "Requisicao/AprovarReprovar",
    requisicaoCreate: "Requisicao",
    requisicaoGetById: "Requisicao/{id}",
    requisicaoGetByToken: "Requisicao/GetByToken/{token}",
    requisicaoRemove: "Requisicao",
    requisicaoUpdate: "Requisicao",
    requisicaoPaginate: "Requisicao/Paginate/{pageSize}/{page}",

    tipoFornecimentoToList: "TipoFornecimento/ToList",

    usuarioCreate: "Usuario",
    usuarioGetById: "Usuario/{id}",
    usuarioRemove: "Usuario",
    usuarioUpdate: "Usuario",
    usuarioPaginate: "Usuario/Paginate/{pageSize}/{page}",
    usuarioToList: "Usuario/ToList",
    
}