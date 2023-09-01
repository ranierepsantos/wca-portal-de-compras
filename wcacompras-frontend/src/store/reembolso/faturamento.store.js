import { defineStore } from "pinia";
import { paginate } from "@/helpers/functions";
import moment from "moment";
import api from "@/services/reembolso/api";

const rotas = {
    Paginar: "Faturamento/Paginar",
    GetById: "Faturamento?Id={Id}"
}


export class Faturamento {
    constructor(data = undefined) {
        this.id = data ? data.id : 0;
        this.dataCriacao = data ? data.dataCriacao: moment().format("YYYY-MM-DD")
        this.usuarioId = data ? data.usuarioId: null
        this.clienteId = data ? data.clienteId: null
        this.status = data? data.status: 1
        this.valor = data ? data.valor: 0.00
        this.numeroPO = data? data.numeroPO: null
        this.documentoPO = data? data.documentoPO: null
        this.faturamentoItem = data? data.faturamentoItem: []
        //this.eventos = data? data.eventos: []
    }

    adicionarAlterarItem(faturamentoItem) {
        let index = -1        
        index = this.faturamentoItem.findIndex(q =>  q.id == faturamentoItem.id) 
        if (index == -1)
            this.faturamentoItem.push(faturamentoItem);
        else 
            this.faturamentoItem[index] = faturamentoItem;
    }

    removerItem(faturamentoItem) {
        let index = this.faturamentoItem.findIndex(c => c.id == faturamentoItem.id)
        if (index > -1) {
            this.faturamentoItem.splice(index, 1);
        }
    }

    addEvento (evento) {
        this.eventos.push(evento)
    }

}

export class FaturamentoItem {
    constructor(data = undefined) {
        this.id = data ? data.id : 0;
        this.faturamentoId = data ? data.faturamentoId: null;
        this.solicitacaoId = data ? data.solicitacaoId: null;
        this.solicitacao = data ? data.solicitacao: null;
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
    faturamentoStatus: [
        { value: -1, text: "Todos" },
        { value: 0, text: "Novo", color: "info", notifica: "" },
        { value: 1, text: "Aguardando Aprovação", color: "warning", notifica: "cliente" },
        { value: 2, text: "Aprovado", color: "success", notifica: "usuario" },
        { value: 3, text: "Rejeitado", color: "error", notifica: "usuario" },
        
    ]
  }),
  actions: {
    async getById (id) {
        let response = await api.get(rotas.GetById.replace("{Id}",id));
        return  new Faturamento(response.data);
    },

    async getPaginate(page = 1, pageSize = 10, filters = "") {

        let parametros = {
            page: page,
            pageSize: pageSize,
            filialId: filters.filialId,
            clienteId: filters.clienteId,
            dataIni: filters.dataIni,
            dataFim: filters.dataFim,
            status: filters.status
        }
        let response = await api.get(rotas.Paginar, {params: parametros} );

        return response.data;
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
