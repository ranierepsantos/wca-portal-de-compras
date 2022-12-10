import api from "./api"
import { route } from "./routeDictionary";

export default {

    create(fornecedor)
    {
        return api.post(route.fornecedorCreate, fornecedor);
    },
    remove (id) 
    {
        return api.delete(route.fornecedorRemove, { params: {"id": id } });
    },
    getById (id) 
    {
        return api.get(route.fornecedorGetById.replace("{id}", id));
    },
    update(fornecedor)
    {
        return api.put(route.fornecedorUpdate, fornecedor);
    },

    toList(filial)
    {
        return api.get(route.fornecedorToList.replace("{filial}",filial));
    },

    paginate(pageSize, page, termo = "")
    {
        let url = route.fornecedorPaginate.replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url, {params: { termo: termo}} );
    },


    produtoCreate(produto)
    {
        return api.post(route.produtoCreate, produto);
    },

    produtoGetById (fornecedorId, id) 
    {
        return api.get(route.produtoGetById.replace("{fornecedorId}", fornecedorId).replace("{id}", id));
    },
    produtoPaginate(fornecedorId, pageSize, page, termo = "")
    {
        let url = route.produtoPaginate.replace("{fornecedorId}", fornecedorId).replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url, {params: { termo: termo}} );
    },
   
    produtoRemove(fornecedorId, id)
    {
        return api.delete(route.produtoRemove, { params: {"fornecedorId": fornecedorId, "id": id } });
    },

    produtoUpdate(produto)
    {
        return api.put(route.produtoUpdate, produto);
    },

}