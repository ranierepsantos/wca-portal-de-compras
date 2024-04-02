import { defineStore } from "pinia";
import api from "@/services/share/shareApi"

const rotas = {
    EnviarNotificacao: "Notificacao/EnviarNotificacao",
    ListarPorUsuario: "Notificacao/ListarPorUsuario",
    Paginar: "Notificacao/Paginar",
    MarcarComoLido: "Notificacao/MarcarComoLido"
}


export const useShareNotificacaoStore = defineStore("shareNotificacao",{
    actions: {
        async getPaginate(page = 1, pageSize = 10, filters) {
            try {
                let parametros = {
                    page: page,
                    pageSize: pageSize,
                    filialId: 0,
                    usuarioId: filters.usuarioId,
                    notRead: filters.naoLido,
                    dataIni: filters.dataIni,
                    dataFim: filters.dataFim
                }
                let response = await api.get(rotas.Paginar, {params: parametros} );
                return response.data        
            } catch (error) {
                throw error
            }
        },
        async enviarNotificacao(data) {
            try {
                let response = await api.post(rotas.EnviarNotificacao, data );
                return response.data        
            } catch (error) {
                throw error
            }
        }
    }
})