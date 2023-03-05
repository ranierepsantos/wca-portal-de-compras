import api from "./api"
import { route } from "./routeDictionary";

export default {
    
    update(data)
    {
        return api.put(route.configuracaoUpdate, data);
    },

    getAll()
    {
        return api.get(route.configuracaoGetAll);
    },
}