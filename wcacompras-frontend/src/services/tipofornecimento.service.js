import api from "./api"
import { route } from "./routeDictionary";

export default {
    create(data)
    {
        return api.post(route.tipoFornecimentoCreate, data);
    },

    update(data)
    {
        return api.put(route.tipoFornecimentoUpdate, data);
    },

    toList()
    {
        return api.get(route.tipoFornecimentoToList);
    },

    paginate(pageSize, page, termo = "")
    {
        let url = route.tipoFornecimentoPaginate.replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url, {params: { termo: termo}} );
    },
}