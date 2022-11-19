import api from "./api"
import { route } from "./routeDictionary";

export default {
    create(data)
    {
        return api.post(route.usuarioCreate, data);
    },

    update(data)
    {
        return api.put(route.usuarioUpdate, data);
    },
    paginate(pageSize, page, termo ="")
    {
        let url = route.usuarioPaginate.replace("{pageSize}", pageSize).replace("{page}", page)
        return api.get(url, {params: { termo: termo}} );
    }
}