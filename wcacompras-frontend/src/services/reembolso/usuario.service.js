import api from "./api"


const rotas = {
    CreateUpdate: "Usuario/CreateUpdate",
    ListFilialByUsuario: "Usuario/ListFilialByUsuario/{usuarioId}"
}


export default {

    createUpdate(data)
    {
        return api.post(rotas.CreateUpdate, data);
    },
    getFiliais (id) 
    {
        return api.get(rotas.ListFilialByUsuario.replace("{usuarioId}", id));
    }
}