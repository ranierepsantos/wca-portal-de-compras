import api from "./api"
import { route } from "./routeDictionary";

export default {

    create(data)
    {
        return api.post(route.recorrenciaCreate, data);
    },

    enabledDisabled(data) 
    {
        return api.put(route.recorrenciaEnabledDisabled, data);
    },

    getById (id) 
    {
        return api.get(route.recorrenciaGetById.replace("{id}", id));
    },
    
    update(data)
    {
        return api.put(route.recorrenciaUpdate, data);
    },

    paginate(pageSize, page, filters)
    {
        let url = route.recorrenciaPaginate.replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url, {params: { ...filters }} );
    },

    paginateLogs(recorrenciaId, pageSize, page)
    {
        let url = route.recorrenciaPaginateLogs.replace("{id}", recorrenciaId).replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url);
    },
}