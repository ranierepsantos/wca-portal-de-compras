import { defineStore } from "pinia";
import moment from "moment";
import api from "@/services/reembolso/api";

const rotas = {
    Create: "Faturamento",
    Paginar: "Faturamento/Paginar",
    GetById: "Faturamento?Id={Id}",
    AddPO: "Faturamento/AdicionarPO",
    Finalizar: "Faturamento/Finalizar",
    ExportarParaExcel: "Faturamento/ExportarParaExcel"
}


export class Faturamento {
    constructor(data = undefined) {
        this.id = data ? data.id : 0;
        this.dataCriacao = data ? data.dataCriacao: moment().format("YYYY-MM-DD")
        this.usuarioId = data ? data.usuarioId: null
        this.clienteId = data ? data.clienteId: null
        this.clienteNome = data ? data.clienteNome: null
        this.status = data? data.status: 1
        this.valor = data ? data.valor: 0.00
        this.numeroPO = data? data.numeroPO: null
        this.documentoPO = data? data.documentoPO: null
        this.faturamentoItem = data? data.faturamentoItem: []
        this.dataFinalizacao = data? data.dataFinalizacao: null
        this.notaFiscal = data? data.notaFiscal: null
        this.faturamentoHistorico = data? data.faturamentoHistorico: []
    }

    adicionarAlterarItem(faturamentoItem) {
        let index = -1        
        index = this.faturamentoItem.findIndex(q =>  q.solicitacaoIdid == faturamentoItem.solicitacaoId) 
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
        { id: -1, status: "Todos" },
        { id: 1, status: "Aguardando P.O", color: "warning", notifica: 2, autorizar: false, templateNotificacao:"Faturamento #{id}, aguardando P.O!" },
        { id: 2, status: "P.O Emitido", color: "success", notifica: 1, autorizar: false, templateNotificacao:"Faturamento #{id}, P.O emitido!" },
        { id: 3, status: "Finalizado", color: "success", notifica: "", autorizar: false, templateNotificacao:"" },
    ]
  }),
  actions: {
    async add (data) {
        try {
            await api.post(rotas.Create, data);
        } catch (error) {
            throw error
        }  
    },
    
    async addPO (data) {
        try {
            await api.post(rotas.AddPO, data);
        } catch (error) {
            throw error
        }  
    },
    async finalizar (data) {
        try {
            await api.post(rotas.Finalizar, data);
        } catch (error) {
            throw error
        }  
    },
    
    

    async getById (id) {
        let response = await api.get(rotas.GetById.replace("{Id}",id));
        return  new Faturamento(response.data);
    },

    async getPaginate(page = 1, pageSize = 10, filters) {

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
        let data = this.faturamentoStatus.find(q => q.id == statusId)
        return data
    },
    gerarRelatorio(filters)   {
        return api.get(rotas.ExportarParaExcel, {params: filters, responseType: 'blob'} );
    },
  },
});
