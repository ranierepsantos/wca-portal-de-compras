import api from "./api"

const rotas = {
    Create: "Solicitacao",
    Update: "Solicitacao",
    GetById: "Solicitacao",
    Paginar: "Solicitacao/Paginar",
    ListarPorColaboradorGestor: "Solicitacao/ListarPorColaboradorGestor",
    ListarStatusSolicitacao: "Solicitacao/ListarStatusSolicitacao",
    AlterarStatus: "Solicitacao/AlterarStatus",
}

export default {
    changeStatus (data) {
        return api.put(rotas.AlterarStatus, data);
    },
    create(data)
    {
        return api.post(rotas.Create, data);
    },
    getById (id) 
    {
        return api.get(rotas.GetById, {params: {id: id}});
    },
    update(data)
    {
        return api.put(rotas.Update, data);
    },

    ListarPorColaboradorGestor(colaboradorId = 0, gestorId = 0, status = [])
    {
        let parametros = {
            colaboradorId: colaboradorId,
            gestorId: gestorId,
            status: status
        }
        return api.get(rotas.ListarPorColaboradorGestor, parametros);
    },

    ListarStatusSolicitacao()
    {
        return api.get(rotas.ListarStatusSolicitacao);
    },

    paginate(page = 1, pageSize = 10, filters)   {
        
        let parametros = {
            page: page,
            pageSize: pageSize,
            filialId: filters.filialId,
            colaboradorId: filters.colaboradorId,
            clienteId: filters.clienteId,
            gestorId: filters.gestorId,
            dataIni: filters.dataIni,
            dataFim: filters.dataFim,
            status: filters.status
        }
        return api.get(rotas.Paginar, {params: parametros} );
    }
    
}