import { defineStore } from "pinia";
import api from "@/services/reembolso/tipodespesa.service"

export class TipoDespesa {
    constructor(data = {})
    {
        this.id     = data.id    ?? 0;
        this.nome   = data.nome  ?? "";
        this.ativo  = data.ativo ?? true
        this.tipo   = data.tipo  ?? 1,
        this.valor  = data.valor ?? 0.0
        this.reembolsarColaborador = data.reembolsarColaborador ?? true;
        this.faturarCliente = data.faturarCliente ?? true;
        this.exibirParaColaborador = data.exibirParaColaborador ?? true;

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
    async toComboList(exibirParaColaborador = true) {
        let listarTipoDespesa = 1; //todos
        if (exibirParaColaborador)
            listarTipoDespesa = 2 // somente visivel para colaborador

        let response = await api.toList(listarTipoDespesa);
        return response.data;
    },
    getTipoDespesa(tipo) {
        return this.tipoDespesaTipo.find(p => p.value == tipo)
    }
  },
});
