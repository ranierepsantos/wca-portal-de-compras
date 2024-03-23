import api from "./shareApi"

const rotas = {
    ConfiguracaoGetAll: "Configuracao/GetAll",
    ConfiguracaoUpdate: "Configuracao",
    ConfiguracaoByChave: "Configuracao/ByChave",
}

export default {
    
    update(data)
    {
        return api.put(rotas.ConfiguracaoUpdate, data);
    },

    getAll()
    {
        return api.get(rotas.ConfiguracaoGetAll);
    },

    getByChave(chave)
    {
        return api.get(rotas.ConfiguracaoByChave, {params: {chave: chave}});
    },
}