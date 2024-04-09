import api from "./shareApi"


const rotas = {
    CreateUpdate: "Usuario/CreateUpdate",
    ListFilialByUsuario: "Usuario/ListFilialByUsuario/{usuarioId}",
    ListarCentroCusto: "Usuario/ListarCentroCusto/{usuarioId}/{clienteId}",
    RelacionarUsuarioCentroCusto: "Usuario/RelacionarUsuarioCentroCusto",
    ListarPorCentroCusto: "Usuario/ListarPorCentroCusto/{id}"
}


export default {

    createUpdate(data)
    {
        return api.post(rotas.CreateUpdate, data);
    },
    getFiliais (id) 
    {
        return api.get(rotas.ListFilialByUsuario.replace("{usuarioId}", id));
    },
    getCentrosdeCusto (id, clienteId = 0) 
    {
        return api.get(rotas.ListarCentroCusto.replace("{usuarioId}", id).replace("{clienteId}", clienteId));
    },
    relacionarUsuarioCentroCusto (data) {
        return api.post(rotas.RelacionarUsuarioCentroCusto, data)
    },
    getListByCentroCusto(idCentroCusto) {
        return api.get(rotas.ListarPorCentroCusto.replace("{id}", idCentroCusto));
    }
}