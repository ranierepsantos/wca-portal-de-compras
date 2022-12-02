import api from "./api"
import { route } from "./routeDictionary";

export default {

    create(perfil)
    {
        return api.post(route.perfilCreate, perfil);
    },

    update(perfil)
    {
        return api.put(route.perfilUpdate, perfil);
    },

    toList()
    {
        return api.get(route.perfilToList);
    },

    getWithPermissions(id)
    {
        return api.get(route.perfilGetWithPermissions.replace("{id}", id));
    },

    paginate(pageSize, page, termo = "")
    {
        let url = route.perfilPaginate.replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url, {params: { termo: termo}} );
    },

    permissaoToList () {
        return api.get(route.permissaoToList);
    },

    permissaoAll () {
        return api.get(route.permissaoAll);
    }
}