import api from "./api"

const rotas = {
    Create: "TipoDespesa",
    Update: "TipoDespesa",
    GetById: "TipoDespesa",
    ToComboList: "TipoDespesa/ToComboList",
    Paginar: "TipoDespesa/Paginar",
}

export default {

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

    toList(listarTipoDespesa = 1)
    {
        return api.get(`${rotas.ToComboList}?ListarTipoDespesa=${listarTipoDespesa}`);
    },

    paginate(pageSize, page, termo = "")
    {
        
        let parametros = {
            page: page,
            pageSize: pageSize,
            termo: termo
        }
        return api.get(rotas.Paginar, {params: parametros} );
    }
    
}