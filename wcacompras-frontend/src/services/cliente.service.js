import api from "./api"
import { route } from "./routeDictionary";

export default {

    create(cliente)
    {
        return api.post(route.clienteCreate, cliente);
    },
    remove (id) 
    {
        return api.delete(route.clienteRemove, { params: {"id": id } });
    },
    getById (id) 
    {
        return api.get(route.clienteGetById.replace("{id}", id));
    },
    update(cliente)
    {
        return api.put(route.clienteUpdate, cliente);
    },

    toList(filial)
    {
        return api.get(route.clienteToList.replace("{filial}",filial));
    },

    paginate(pageSize, page, termo = "")
    {
        let url = route.clientePaginate.replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url, {params: { termo: termo}} );
    },

    getListByAuthenticatedUser () 
    {
        return api.get(route.clienteListByAuthenticatedUser);
    },

    importar(data)
    {
        return api.post(route.clienteImportar, data);
    },
   
}