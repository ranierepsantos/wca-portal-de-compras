import { defineStore } from "pinia";
import api from "@/services/reembolso/tipodespesa.service"

export class TipoDespesa {
    constructor(data = null)
    {
        this.id     = data? data.id : 0;
        this.nome   = data? data.nome : "";
        this.ativo  = data? data.ativo: true
        this.tipo   = data? data.tipo: 1,
        this.valor  = data? data.valor: 0.0
    }
}

export const useDespesaTipoStore = defineStore("despesaTipo", {
  state: () => ({
    tipoDespesaTipo: [
        {value: 1, text: "Consumo" },
        {value: 2, text: "Distância" },
    ]
  }),
  actions: {
    
    async add (data) {
        try {
            
            let response = await api.create(data);
            console.log(response);

        } catch (error) {
            throw error
        }  
    },
    
    async getById (id) {
        let data = await api.getById(id);
        return new TipoDespesa(data);
    },

    async getPaginate(pageNumber = 1, pageSize = 10, termo ="") {
        let response = await api.paginate(pageSize, pageNumber, termo);
        return response.data
    },

    async update (data) {
        try {
            
            let response = await api.update(data);
            console.log(response);

        } catch (error) {
            throw error
        }  
    },
    async toComboList() {
        let response = await api.toList();
        return response.data;
    },
    getTipoDespesa(tipo) {
        return this.tipoDespesaTipo.find(p => p.value == tipo)
    }
  },
});
