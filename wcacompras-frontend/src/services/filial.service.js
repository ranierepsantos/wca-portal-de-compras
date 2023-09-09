import api from "./api"
import { route } from "./routeDictionary";
import { useAuthStore } from "@/store/auth.store";

export default {

    create(filial)
    {
        return api.post(route.filialCreate, filial);
    },

    update(filial)
    {
        return api.put(route.filialUpdate, filial);
    },

    toList()
    {
        let _url = route.filialToList.replace("{sistemaId}", useAuthStore().sistema.id)
        return api.get(_url);
    },

    paginate(pageSize, page, termo = "")
    {
        let url = route.filialPaginate
        .replace("{pageSize}", pageSize)
        .replace("{page}", page)
        .replace("{sistemaId}", useAuthStore().sistema.id)

        return api.get(url, {params: { termo: termo}} );
    },
    
    getListByAuthenticatedUser () 
    {
        return api.get(route.filialListByAuthenticatedUser.replace("{sistemaId}", useAuthStore().sistema.id));
    },

   
}