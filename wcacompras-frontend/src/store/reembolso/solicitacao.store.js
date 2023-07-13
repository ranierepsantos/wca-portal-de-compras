import { defineStore } from "pinia";
import { paginate } from "@/helpers/functions";
import moment from "moment/moment";

export class Solicitacao {
    constructor() {
        this.id= 0,
        this.clienteId= null, 
        this.tipoSolicitacao= null,
        this.dataSolicitacao= moment().format("YYYY-MM-DD"),
        this.colaborador= "",
        this.gestor= "",
        this.cargo= "",
        this.localProjeto= "",
        this.objetivo="",
        this.periodoInicial= moment().format("YYYY-MM-DD"),
        this.periodoFinal= moment().format("YYYY-MM-DD"),
        this.valor= 0.00,
        this.status = 0,
        this.despesas= []
    }
}

export class Despesa {
    constructor() {
        this.id = 0,
        this.solicitacaoId = 0,
        this.tipoDespesaId = null,
        this.dataEvento = moment().format("YYYY-MM-DD"),
        this.fornecedor = "",
        this.nroFiscal = "",
        this.valor= 0.0,
        this.comprovanteImage = ""
    }
}

export const useSolicitacaoStore = defineStore("solicitacao", {
  state: () => ({
    idControl: 0,
    idDespesaControl:0,
    repository: [],
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
        this.repository.push(data)
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
        return true;
    },
    getStatus(statusId) {
        let data = this.statusSolicitacao.find(q => q.value == statusId)
        return data
    },
    getTipoSolicitacao(codigo) {
        let data = this.tipoSolicitacao.find(q => q.value == codigo)
        console.log(`tipoId: ${codigo}, tipo: ${data}`)
        return data
    }
  },
});
