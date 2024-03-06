import api from "./api"


const rotas = {
    Create: "Cliente",
    Update: "Cliente",
    GetById: "Cliente",
    ToComboList: "Cliente/ToComboList",
    Paginar: "Cliente/Paginar",
    GetByUsuario: "Cliente/ListarClientesPorUsuario",
    RelacionarClienteUsuario: "Cliente/RelacionarClienteUsuario",
    ListarUsuariosPorCliente: "Cliente/ListarUsuariosPorCliente",
    ListarCentroCustoPorCliente: "Cliente/ListarCentroCustoPorCliente"
}


export default {

    create(cliente)
    {
        return api.post(rotas.Create, cliente);
    },
    getById (id) 
    {
        return api.get(rotas.GetById, {params: {id: id}});
    },
    update(cliente)
    {
        return api.put(rotas.Update, cliente);
    },

    toList(filialId =0, usuarioId =0)
    {
        return api.get(rotas.ToComboList, {params: {filialId: filialId, usuarioId: usuarioId}});
    },

    paginate(filialId, pageSize, page, termo = "")
    {
        
        let parametros = {
            filialId: filialId,
            page: page,
            pageSize: pageSize,
            termo: termo
        }
        return api.get(rotas.Paginar, {params: parametros} );
    },

    getListByUser (usuarioId)
    {
        return api.get(rotas.GetByUsuario, {params: {usuarioId: usuarioId}});
    },
   
    RelacionarClienteUsuario(data) {
        return api.post(rotas.RelacionarClienteUsuario, data);
    },
    getListUsersByCliente (clienteId)
    {
        return api.get(rotas.ListarUsuariosPorCliente , {params: {clienteId: clienteId}});
    },
    
    getListCentroCustoByCliente (clienteId)
    {
        return api.get(rotas.ListarCentroCustoPorCliente , {params: {clienteId: clienteId}});
    },
}