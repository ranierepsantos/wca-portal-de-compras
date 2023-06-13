import api from "./api"
import { route } from "./routeDictionary";

export default {
    login(credencials)
    {
        return api.post(route.login, credencials);
    },
    changePassword(data)
    {
        return api.post(route.alterarSenha, data)
    },
    resetPassword(data)
    {
        return api.post(route.recuperarSenha, data)
    },
    getPerfilSistema (usuarioId, sistemaId) {
        let endPoint = route.perfilGetByUserAndSistemaWithPermissions
                       .replace("{usuarioId}", usuarioId)
                       .replace("{sistemaId}", sistemaId);

        return api.get(endPoint)
    }
}