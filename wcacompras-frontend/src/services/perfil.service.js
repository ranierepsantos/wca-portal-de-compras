import api from "./api"
import { route } from "./routeDictionary";

export default {

    create(perfil)
    {
        return api.post(route.perfilCreate, perfil);
    },

    update(perfil)
    {
        return api.put(route.perfilCreate, perfil);
    },

    toList()
    {
        return api.get(route.perfilToList);
    },

    getWithPermissions(id)
    {
        return api.get(route.getWithPermissions.replace("{id}", id));
    },

    paginate(pageSize, page, termo = "")
    {
        let url = route.UsuarioPaginate.replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url, {params: { termo: termo}} );
    }
}