import api from "./api"
import { route } from "./routeDictionary";

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
        return api.get(route.filialToList);
    },

    paginate(pageSize, page, termo = "")
    {
        let url = route.filialPaginate.replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url, {params: { termo: termo}} );
    },
    
    getListByAuthenticatedUser () 
    {
        return api.get(route.filialListByAuthenticatedUser);
    },

   
}