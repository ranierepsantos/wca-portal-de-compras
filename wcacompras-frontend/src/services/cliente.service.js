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
        return api.get(route.clienteToList, { params: {filial: filial} });
    },

    paginate(pageSize, page, filter)
    {
        let url = route.clientePaginate.replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url, {params: filter} );
    },

    getListByAuthenticatedUser (filial = null) 
    {
        let parametros = null 
        if (filial) {
            parametros = {
                params: {filial: filial}
            }
        }
        return api.get(route.clienteListByAuthenticatedUser,parametros);
    },

    importar(data)
    {
        return api.post(route.clienteImportar, data);
    },
   
}