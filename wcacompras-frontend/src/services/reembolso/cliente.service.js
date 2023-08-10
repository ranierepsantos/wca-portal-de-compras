import api from "./api"


const rotas = {
    Create: "Cliente",
    Update: "Cliente",
    GetById: "Cliente",
    ToComboList: "Cliente/ToComboList",
    Paginar: "Cliente/Paginar",
    GetByUsuario: "Cliente/ListarClientesPorUsuario",
    RelacionarClienteUsuario: "Cliente/RelacionarClienteUsuario"
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

    toList(filial)
    {
        return api.get(rotas.ToComboList, {params: {filialId: filial}});
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
    }
}