import api from "./api"
import { route } from "./routeDictionary";
import { useAuthStore } from "@/store/auth.store";

const authStore = useAuthStore();

export default {
    
    create(data)
    {
        return api.post(route.usuarioCreate.replace("{sistemaId}", authStore.sistema.id), data);
    },

    getById(id)
    {
        return api.get(route.usuarioGetById.replace('{id}', id).replace("{sistemaId}", authStore.sistema.id));
    },
    remove(id)
    {
        return api.get(route.usuarioGetById.replace("{id}", id));
    },

    paginate(pageSize, page, filters)
    {
        let url = route.usuarioPaginate
                       .replace("{sistemaId}", authStore.sistema.id)               
                       .replace("{pageSize}", pageSize)
                       .replace("{page}", page)
                       
        return api.get(url, {params: filters} );
    },

    update(data)
    {
        return api.put(route.usuarioUpdate.replace("{sistemaId}", authStore.sistema.id), data);
    },

    toList(filial = [])
    {
        return api.get(route.usuarioToList.replace("{sistemaId}", authStore.sistema.id), {params: {filial: filial}});
    },


    toListByPerfil(perfilId) {
        return api.get(route.usuarioToListByPerfil.replace("{perfilId}", perfilId));
    }
    
    
}