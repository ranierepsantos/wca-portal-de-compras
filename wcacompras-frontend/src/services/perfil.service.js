import api from "./api"
import { route } from "./routeDictionary";
import { useAuthStore } from "@/store/auth.store";

const authStore = useAuthStore();

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
        return api.get(route.perfilToList.replace("{sistemaId}", authStore.sistema.id));
    },

    getWithPermissions(id)
    {
        return api.get(route.perfilGetWithPermissions.replace("{id}", id));
    },

    paginate(pageSize, page, termo = "")
    {
        let url = route.perfilPaginate
                    .replace("{sistemaId}", authStore.sistema.id)
                    .replace("{pageSize}", pageSize)
                    .replace("{page}", page)
        return api.get(url, {params: { termo: termo}} );
    },

    permissaoToList () {
        return api.get(route.permissaoToList.replace("{sistemaId}", authStore.sistema.id));
    },

    permissaoAll () {
        return api.get(route.permissaoAll.replace("{sistemaId}", authStore.sistema.id));
    }
}