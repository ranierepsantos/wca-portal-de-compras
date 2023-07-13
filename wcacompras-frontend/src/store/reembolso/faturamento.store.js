import { defineStore } from "pinia";
import { paginate } from "@/helpers/functions";
import moment from "moment";


export class Faturamento {
    constructor(data = undefined) {
        this.id = data ? data.id : useFaturamentoStore().idFaturamento++;
        this.dataCriacao = data ? data.dataCriacao: moment().format("YYYY-MM-DD");
        this.usuario = data ? data.usuario: "";
        this.clienteId = data ? data.clienteId: null
        this.valor = data ? data.valor: 0.00
        this.status = data? data.status: 0
        this.faturamentoItems = data? data.faturamentoItems: []
        this.eventos = data? data.eventos: []
        localStorage.setItem("reembolso-faturamento-id",useFaturamentoStore().idFaturamento)
    }

    adicionarAlterarItem(faturamentoItem) {
        let index = -1        
        index = this.faturamentoItems.findIndex(q =>  q.id == faturamentoItem.id) 
        if (index == -1)
            this.faturamentoItems.push(faturamentoItem);
        else 
            this.faturamentoItems[index] = faturamentoItem;
    }

    removerItem(faturamentoItem) {
        let index = this.faturamentoItems.findIndex(c => c.id == faturamentoItem.id)
        if (index > -1) {
            this.faturamentoItems.splice(index, 1);
        }
    }

    addEvento (evento) {
        this.eventos.push(evento)
    }

}

export class FaturamentoItem {
    constructor(data = undefined) {
        this.id = data ? data.id : useFaturamentoStore().faturamentoItemId++;
        this.faturamentoId = data ? data.faturamentoId: null;
        this.solicitacaoId = data ? data.solicitacaoId: null;
        this.solicitacao = data ? data.solicitacao: null;
        localStorage.setItem("reembolso-faturamentoItemid", useFaturamentoStore().faturamentoItemId)
    }
}

export class FaturamentoEvento {
    constructor(faturamentoId =0, usuario = "sistema", descricao ="") {
        this.id = 0,
        this.faturamentoId = faturamentoId,
        this.dataEvento = moment().format("YYYY-MM-DDTHH:mm:ss"),
        this.usuario = usuario
        this.descricao = descricao
    }
}

export const useFaturamentoStore = defineStore("faturamento", {
  state: () => ({
    faturamentoItemId: localStorage.getItem("reembolso-faturamentoItemid") || 11000, 
    idFaturamento: localStorage.getItem("reembolso-faturamento-id") || 11000,
    repository: JSON.parse(localStorage.getItem("reembolso-faturamento")) || [],
    faturamentoStatus: [
        { value: -1, text: "Todos" },
        { value: 0, text: "Novo", color: "info", notifica: "" },
        { value: 1, text: "Aguardando Aprovação", color: "warning", notifica: "cliente" },
        { value: 2, text: "Aprovado", color: "success", notifica: "usuario" },
        { value: 3, text: "Rejeitado", color: "error", notifica: "usuario" },
        
    ]
  }),
  actions: {
    
    add (data) {
        data.status = 1;
        this.repository.push(data)
        localStorage.setItem("reembolso-faturamento", JSON.stringify(this.repository))
        localStorage.setItem("reembolso-faturamento-id", this.idFaturamento)
    },
    
    async getById (id) {
        let data = this.repository.find(c => c.id == id)
        return  new Faturamento(data);
    },

    async getPaginate(pageNumber = 1, pageSize = 10, filter = "") {
        return paginate(this.repository, pageNumber, pageSize)
    },

    update (data) {
        let index = this.repository.findIndex(q => q.id == data.id)
        if (index == -1) {
            return false;
        }
        this.repository[index] = {...data};
        localStorage.setItem("reembolso-faturamento", JSON.stringify(this.repository))
        return true;
    },

    toComboList() {
        let list = []
        this.repository.forEach(item => {
            list.push ({text: item.nome, value: item.id})
        })
        return list;
    },
    getStatus(statusId) {
        let data = this.faturamentoStatus.find(q => q.value == statusId)
        return data
    },
    
  },
});
