import api from "./shareApi"


const rotas = {
    Create: "Cliente",
    Update: "Cliente",
    GetById: "Cliente",
    ToComboList: "Cliente/ToComboList",
    Paginar: "Cliente/Paginar",
    GetByUsuario: "Cliente/ListarClientesPorUsuario",
    RelacionarClienteUsuario: "Cliente/RelacionarClienteUsuario",
    ListarUsuariosPorCliente: "Cliente/ListarUsuariosPorCliente"
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

    RelacionarClienteUsuario(data) {
        return api.post(rotas.RelacionarClienteUsuario, data);
    },
    getListUsersByCliente (clienteId)
    {
        return api.get(rotas.ListarUsuariosPorCliente , {params: {clienteId: clienteId}});
    },
    getListByUser (usuarioId)
    {
        return api.get(rotas.GetByUsuario, {params: {usuarioId: usuarioId}});
    },
}