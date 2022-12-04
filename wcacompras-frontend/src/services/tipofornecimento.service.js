import api from "./api"
import { route } from "./routeDictionary";

export default {
    toList()
    {
        return api.get(route.tipoFornecimentoToList);
    }
}