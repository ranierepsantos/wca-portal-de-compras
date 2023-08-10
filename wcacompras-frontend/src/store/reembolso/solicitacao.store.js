import { defineStore } from "pinia";
import api from "@/services/reembolso/solicitacao.service";
import moment from "moment/moment";
import { useUsuarioStore } from "./usuario.store";

export class Solicitacao {
    constructor(data = undefined) {
        this.id= data == undefined? 0: data.id
        this.clienteId= data == undefined? null: data.clienteId
        this.dataSolicitacao= data == undefined? moment().format("YYYY-MM-DD"): data.dataSolicitacao
        this.colaboradorId= data == undefined? "": data.colaboradorId
        this.gestorId = data == undefined? "": data.gestorId
        this.colaboradorCargo= data == undefined? "": data.colaboradorCargo
        this.projeto= data == undefined? "": data.projeto
        this.objetivo=data == undefined? "": data.objetivo
        this.periodoInicial= data == undefined? moment().format("YYYY-MM-DD"): data.periodoInicial
        this.periodoFinal= data == undefined? moment().format("YYYY-MM-DD"): data.periodoFinal
        this.valorAdiantamento= data == undefined? 0.00: data.valorAdiantamento
        this.valorDespesa= data == undefined? 0.00: data.valorDespesado
        this.status = data == undefined? 1: data.status
        this.cliente = data == undefined? {}: data.cliente
        this.despesa= data == undefined? []: data.despesa
        this.solicitacaoHistorico = data == undefined? []: data.solicitacaoHistorico
        this.tipoSolicitacao= data == undefined? null: data.tipoSolicitacao
    }

    salvarDespesa(despesa) {
        let index = -1        
        if (despesa.id == 0) {

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
}

export class Despesa {
    constructor() {
        this.id = 0
        this.solicitacaoId = 0
        this.tipoDespesaId = null
        this.dataEvento = moment().format("YYYY-MM-DD")
        this.razaoSocial = ""
        this.cnpj = ""
        this.inscricaoEstadual = ""
        this.numeroFiscal = ""
        this.valor= 0.0
        this.ImagePath = ""
        this.motivo = ""
        this.origem =""
        this.destino = ""
        this.kmPercorrido = 0
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

let ChangeStatusCommand ={
    "solicitacaoId": 0,
    "evento": "string",
    "status": {
        "id": 0,
        "status": "string",
        "color": "string",
        "notifica": 1,
        "templateNotificacao": "string"
    }
}

export const useSolicitacaoStore = defineStore("solicitacao", {
  state: () => ({
    tipoSolicitacao: [
        {text: "Adiantamento", value: 2},
        {text: "Reembolso", value: 1}
    ],
    statusSolicitacao: JSON.parse(localStorage.getItem("reembolso-status-solicitacao")) || [],
    usuarios: []
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
        let response = await api.getById(id);
        console.log("solicitacao.store.getById", response);
        return new Solicitacao(response.data);
    },

    async update (data) {
        try {
            
            let response = await api.update(data);
            console.log(response);
            return true;
        } catch (error) {
            throw error
        }  
    },
    async getPaginate(page = 1, pageSize = 10,filters) {
        let response = await api.paginate(page, pageSize, filters)
        return response.data    
    },

    async loadListStatusSolicitacao() {
        try {
            
            let response = await api.ListarStatusSolicitacao();
            
            this.statusSolicitacao = response.data 
            localStorage.setItem('reembolso-status-solicitacao',JSON.stringify(this.statusSolicitacao))

        } catch (error) {
            throw error
        }  
    },
    
    async loadUsuarios() {
        this.usuarios = await useUsuarioStore().toComboList()
    },

    async getListarPorColaboradorGestor(colaboradorId = 0, gestorId = 0)
    {
        try {
            let response = await api.getListarPorColaborador(colaboradorId, gestorId)
            return response.data

        } catch (error) {
            throw error
        }
    },

    getStatus(statusId) {
        let data = this.statusSolicitacao.find(q => q.id == statusId)
        return data
    },
    
    getTipoSolicitacao(codigo) {
        let data = this.tipoSolicitacao.find(q => q.value == codigo)
        return data
    },

    async getByTipoAndUsuario(tipoSolicitacao, colaboradorId, status) {
        
        let response = await api.ListarPorColaboradorGestor(colaboradorId, 0, status);
        
        let data = response.data.filter(c => c.tipoSolicitacao == tipoSolicitacao)  

        return data;
    },

    getUsuarioSolicitacao(id) {
        return this.usuarios.find(q => q.value == id)
    }

  },
});
