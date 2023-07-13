import { defineStore } from "pinia";
import { paginate } from "@/helpers/functions";
import moment from "moment/moment";

export class Solicitacao {
    idDespesaControl = 0

    constructor(data = undefined) {
        this.id= data == undefined? 0: data.id
        this.clienteId= data == undefined? null: data.clienteId
        this.tipoSolicitacao= data == undefined? null: data.tipoSolicitacao
        this.dataSolicitacao= data == undefined? moment().format("YYYY-MM-DD"): data.dataSolicitacao
        this.colaborador= data == undefined? "": data.colaborador
        this.gestor= data == undefined? "": data.gestor
        this.cargo= data == undefined? "": data.cargo
        this.localProjeto= data == undefined? "": data.localProjeto
        this.objetivo=data == undefined? "": data.objetivo
        this.periodoInicial= data == undefined? moment().format("YYYY-MM-DD"): data.periodoInicial
        this.periodoFinal= data == undefined? moment().format("YYYY-MM-DD"): data.periodoFinal
        this.valor= data == undefined? 0.00: data.valor
        this.status = data == undefined? 0: data.status
        this.despesas= data == undefined? []: data.despesas
        this.eventos = data == undefined? []: data.eventos
    }

    salvarDespesa(despesa) {
        let index = -1        
        if (despesa.id == 0) {
            this.idDespesaControl +=-1
            despesa.id = this.idDespesaControl
            despesa.solicitacaoId = this.id
        }else {
            index = this.despesas.findIndex(q =>  q.id == despesa.id) 
        }
        if (index == -1)
            this.despesas.push(despesa);
        else 
            this.despesas[index] = despesa;
    }

    removerDespesa(despesa) {
        let index = this.despesas.findIndex(c => c.id == despesa.id)
        if (index > -1) {
            this.despesas.splice(index, 1);
        }
    }

    addEvento (evento) {
        this.eventos.push(evento)
    }
}

export class Despesa {
    constructor() {
        this.id = 0
        this.solicitacaoId = 0
        this.tipoDespesaId = null
        this.dataEvento = moment().format("YYYY-MM-DD")
        this.fornecedor = ""
        this.nroFiscal = ""
        this.valor= 0.0
        this.comprovanteImage = ""
    }
}

export class Evento {
    constructor(solicitacaoId =0, usuario = "sistema", descricao ="") {
        this.id = 0,
        this.solicitacaoId = solicitacaoId,
        this.dataEvento = moment().format("YYYY-MM-DDTHH:mm:ss"),
        this.usuario = usuario
        this.descricao = descricao
    }
}


export const useSolicitacaoStore = defineStore("solicitacao", {
  state: () => ({
    idControl: 0,
    idDespesaControl:0,
    repository: JSON.parse(localStorage.getItem("reembolso-solicitacoes")) || [],
    tipoSolicitacao: [
        {text: "Adiantamento", value: 2},
        {text: "Reembolso", value: 1}
    ],
    statusSolicitacao: [
        { value: -1, text: "Todos" },
        { value: 0, text: "Solicitado", color: "warning" },
        { value: 1, text: "Aguardando Despesas", color: "warning" },
        { value: 2, text: "Aprovado", color: "success" },
        { value: 3, text: "Rejeitado", color: "error" },
        { value: 4, text: "Aguardando aprovação", color: "info" }
    ]
  }),
  actions: {
    
    add (data) {
        this.idControl++;
        data.id = this.idControl;
        data = new Solicitacao(data)
        data.addEvento(new Evento(data.id, data.colaborador, `Solicitação de ${this.getTipoSolicitacao(data.tipoSolicitacao).text} criada!`))
        this.repository.push(data)
        localStorage.setItem("reembolso-solicitacoes",JSON.stringify(this.repository))
    },
    
    getById (id) {
        let data = this.repository.find(c => c.id == id)
        return new Solicitacao(data);
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
        localStorage.setItem("reembolso-solicitacoes",JSON.stringify(this.repository))
        return true;
    },
    getStatus(statusId) {
        let data = this.statusSolicitacao.find(q => q.value == statusId)
        return data
    },
    getTipoSolicitacao(codigo) {
        let data = this.tipoSolicitacao.find(q => q.value == codigo)
        return data
    }
  },
});
