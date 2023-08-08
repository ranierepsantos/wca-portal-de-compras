import api from "./api"
import { route } from "./routeDictionary";
import { useAuthStore } from "@/store/auth.store";

const authStore = useAuthStore();

export default {
    
    create(data)
    {
        return api.post(route.usuarioCreate, data);
    },

    getById(id)
    {
        return api.get(route.usuarioGetById.replace('{id}', id).replace("{sistemaId}", authStore.sistema.id));
    },
    remove(id)
    {
        return api.get(route.usuarioGetById.replace("{id}", id));
    },

    paginate(pageSize, page, termo ="")
    {
        let url = route.usuarioPaginate
                       .replace("{sistemaId}", authStore.sistema.id)               
                       .replace("{pageSize}", pageSize)
                       .replace("{page}", page)
                       
        return api.get(url, {params: { termo: termo}} );
    },

    update(data)
    {
        return api.put(route.usuarioUpdate.replace("{sistemaId}", authStore.sistema.id), data);
    },

    toList()
    {
        return api.get(route.usuarioToList.replace("{sistemaId}", authStore.sistema.id));
    },


    toListByPerfil(perfilId) {
        return api.get(route.usuarioToListByPerfil.replace("{perfilId}", perfilId));
    }
    
    
}