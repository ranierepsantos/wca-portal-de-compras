import { defineStore } from "pinia";
import { paginate } from "@/helpers/functions";

export const despesaTipoModel = {
    id: 0,
    nome: "",
    ativo: true
}

export const useDespesaTipoStore = defineStore("despesaTipo", {
  state: () => ({
    idControl: 0,
    repository: JSON.parse(localStorage.getItem("reembolso-tiposdespesa")) || []
  }),
  actions: {
    
    add (data) {
        this.idControl++;
        data.id = this.idControl;
        this.repository.push(data)
        localStorage.setItem("reembolso-tiposdespesa", JSON.stringify(this.repository))
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
