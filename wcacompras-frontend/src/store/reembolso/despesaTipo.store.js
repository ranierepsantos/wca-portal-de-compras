import { defineStore } from "pinia";
import { paginate } from "@/helpers/functions";

export const despesaTipoModel = {
    id: 0,
    nome: "",
    ativo: true
}

export const useDespesaTipoStore = defineStore("despesaTipo", {
  state: () => ({
    tipoDespesaID: localStorage.getItem("reembolso-tipodespesa-id") || 10000,
    repository: JSON.parse(localStorage.getItem("reembolso-tiposdespesa")) || []
  }),
  actions: {
    
    add (data) {
        this.tipoDespesaID++;
        data.id = this.tipoDespesaID;
        this.repository.push(data)
        localStorage.setItem("reembolso-tiposdespesa", JSON.stringify(this.repository))
        localStorage.setItem("reembolso-tipodespesa-id", this.tipoDespesaID)
    },
    
    getById (id) {
        let data = this.repository.find(c => c.id == id)
        return data;
    },

    getPaginate(pageNumber = 1, pageSize = 10) {
        return paginate(this.repository, pageNumber, pageSize)
    },

    update (data) {
        let index = this.repository.findIndex(q => q.id == data.id)
        if (index == -1) {
            return false;
        }
        this.repository[index] = {...data};
        localStorage.setItem("reembolso-tiposdespesa", JSON.stringify(this.repository))
        return true;
    },
    toComboList() {
        let list = []
        this.repository.forEach(item => {
            list.push ({text: item.nome, value: item.id})
        })
        return list;
    }
  },
});
