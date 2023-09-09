import { defineStore } from "pinia";
import api from "@/services/reembolso/api"

const rotas = {
    Create: "Filial",
    Update: "Filial",
    Paginar: "Filial/Paginar",
    GetById: "Filial?Id={Id}",
    toComboList: "Filial/ToComboList"
}

export class Filial {
    constructor(data = null)
    {
        this.id     = data? data.id : 0;
        this.nome   = data? data.nome : "";
        this.ativo  = data? data.ativo: true
    }
}

export const useFilialStore = defineStore("filial", {
  actions: {
    
    async add (data) {
        try {
            
            let response = await api.post(rotas.Create, data);
            return new Filial(response.data)

        } catch (error) {
            throw error
        }  
    },
    
    async getById (id) {
        let data = await api.get(rotas.getById.replace("{id}", id));
        return new Filial(data);
    },

    async getPaginate(pageNumber = 1, pageSize = 10, termo ="") {
        let parametros = {
            page: pageNumber,
            pageSize: pageSize,
            termo: termo
        }

        let response = await api.get(rotas.Paginar, {params: parametros});
        return response.data
    },

    async update (data) {
        try {
            
            let response = await api.put(rotas.Update, data);
            return new Filial(response.data)
        } catch (error) {
            throw error
        }  
    },
    async toComboList() {
        let response = await api.get(rotas.toComboList);
        return response.data;
    }
  },
});
