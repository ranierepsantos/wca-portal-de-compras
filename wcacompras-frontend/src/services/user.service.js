import api from "./api"
import { route } from "./routeDictionary";

export default {
    create(data)
    {
        return api.post(route.usuarioCreate, data);
    },

    getById(id)
    {
        return api.get(route.usuarioGetById.replace('{id}', id));
    },
    remove(id)
    {
        return api.get(route.usuarioGetById.replace("{id}", id));
    },

    paginate(pageSize, page, termo ="")
    {
        let url = route.usuarioPaginate.replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url, {params: { termo: termo}} );
    },

    update(data)
    {
        return api.put(route.usuarioUpdate, data);
    },

    toList()
    {
        return api.get(route.usuarioToList);
    },
    
}