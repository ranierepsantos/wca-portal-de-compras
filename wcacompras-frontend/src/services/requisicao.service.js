import api from "./api"
import { route } from "./routeDictionary";

export default {

    create(requisicao)
    {
        return api.post(route.requisicaoCreate, requisicao);
    },
    remove (id) 
    {
        return api.delete(route.requisicaoRemove, { params: {"id": id } });
    },
    getById (id) 
    {
        return api.get(route.requisicaoGetById.replace("{id}", id));
    },
    update(requisicao)
    {
        return api.put(route.requisicaoUpdate, requisicao);
    },

    paginate(pageSize, page, filters)
    {
        let url = route.requisicaoPaginate.replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url, {params: { ...filters }} );
    },
}