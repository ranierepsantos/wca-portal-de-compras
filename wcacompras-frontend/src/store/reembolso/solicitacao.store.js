import { defineStore } from "pinia";
import api from "@/services/reembolso/solicitacao.service";
import moment from "moment/moment";

export class Solicitacao {
    controlIdDespesa = sessionStorage.getItem("reembolso-despesa-fakeid")||0
    constructor(data = undefined) {
        this.id= data == undefined? 0: data.id
        this.clienteId= data == undefined? null: data.clienteId
        this.dataSolicitacao= data == undefined? moment().format("YYYY-MM-DD"): data.dataSolicitacao
        this.colaboradorId= data == undefined? "": data.colaboradorId
        this.colaboradorNome = data ==undefined? "": data.colaboradorNome
        this.centroCustoId = data == undefined? "": data.centroCustoId
        this.centroCustoNome = data ==undefined? "": data.centroCustoNome
        this.colaboradorCargo= data == undefined? "": data.colaboradorCargo
        this.projeto= data == undefined? "": data.projeto
        this.objetivo=data == undefined? "": data.objetivo
        this.periodoInicial= data == undefined? moment().format("YYYY-MM-DD"): data.periodoInicial
        this.periodoFinal= data == undefined? moment().format("YYYY-MM-DD"): data.periodoFinal
        this.valorAdiantamento= data == undefined? 0.00: data.valorAdiantamento
        this.valorDespesa= data == undefined? 0.00: data.valorDespesa
        this.status = data == undefined? 1: data.status
        this.statusAnterior = data == undefined? 1: data.statusAnterior
        this.cliente = data == undefined? {}: data.cliente
        this.despesa= data == undefined? []: data.despesa
        this.solicitacaoHistorico = data == undefined? []: data.solicitacaoHistorico
        this.tipoSolicitacao= data == undefined? null: data.tipoSolicitacao
    }

    salvarDespesa(despesa) {
        console.log("salvarDespesa->despesa: ", despesa)
        let index = -1        
        if (despesa.id == 0) {
            this.controlIdDespesa--
            despesa.id = this.controlIdDespesa
            despesa.solicitacaoId = this.id
            sessionStorage.setItem("reembolso-despesa-fakeid", this.controlIdDespesa);
        }else {
            index = this.despesa.findIndex(q =>  q.id == despesa.id)
        }
        if (index == -1)
            this.despesa.push(despesa);
        else 
            this.despesa[index] = despesa;
    }

    removerDespesa(despesa) {
        let index = this.despesa.findIndex(c => c.id == despesa.id)
        if (index > -1) {
            this.despesa.splice(index, 1);
        }
    }
}

export class Despesa {
    constructor(data = null) {
        this.id = data ?  data.id: 0
        this.solicitacaoId = data ?  data.solicitacaoId : 0
        this.tipoDespesaId = data ?  data.tipoDespesaId : null
        this.dataEvento = data ?  moment(data.dataEvento).format("YYYY-MM-DD"): moment().format("YYYY-MM-DD")
        this.razaoSocial = data ?  data.razaoSocial :""
        this.cnpj = data ?  data.cnpj :""
        this.inscricaoEstadual = data ?  data.inscricaoEstadual :""
        this.numeroFiscal = data ?  data.numeroFiscal :""
        this.valor= data ?  data.valor :0.0
        this.imagePath = data ?  data.imagePath :""
        this.motivo = data ?  data.motivo :""
        this.origem =data ?  data.origem :""
        this.destino = data ?  data.destino :""
        this.kmPercorrido = data ?  data.kmPercorrido :0
        this.aprovada = data? data.aprovada : 0
        this.observacao = data? data.observacao: ""
    }
}

export class Evento {
    constructor(solicitacaoId =0, evento ="") {
        this.solicitacaoId = solicitacaoId,
        this.evento = evento
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
            return await api.create(data);
        } catch (error) {
            throw error
        }  
    },
    
    async changeStatus (data) {
        try {
            
            await api.changeStatus(data);
        } catch (error) {
            throw error
        }  
    },
    
    async getById (id) {
        let response = await api.getById(id);
        return new Solicitacao(response.data);
    },

    async update (data) {
        try {
            
            await api.update(data);
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
            if (this.statusSolicitacao.length ==0)
            {
                let response = await api.ListarStatusSolicitacao();
            
                this.statusSolicitacao = response.data 
                localStorage.setItem('reembolso-status-solicitacao',JSON.stringify(this.statusSolicitacao))
            }
        } catch (error) {
            throw error
        }  
    },
    
    async getListarPorColaboradorGestor(colaboradorId = 0)
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
        
        let response = await api.ListarPorColaboradorGestor(colaboradorId, status);
        let data = response.data.filter(c => c.tipoSolicitacao == tipoSolicitacao)  

        return data;
    },

    getUsuarioSolicitacao(id) {
        let usuario =  this.usuarios.find(q => q.value == id)
        return usuario || { value: 0, text: "" }
    },
    
    async registrarEvento(evento) {
        return await api.registrarEvento(evento);
    },
    async checarSeDespesaExiste(despesa) {
        let response = await api.despesaChecaSeExiste(despesa.cnpj, despesa.numeroFiscal, despesa.id);
        return response.data
    },
    async gerarRelatorio(filters) {
        let response = await api.exportarParaExcel (filters)
        return response
    },

    async criarDespesa(despesa) {
        let response = await api.despesaCreate(despesa)
        return response.data
    },

    async atualizarDespesa(despesa) {
        let response = await api.despesaUpdate(despesa)
        return response.data
    },

    async excluirDespesa(despesa) {
        let response = await api.despesaDelete(despesa.id)
        return response.data
    },
    async getDespesasToZipFile(solicitacaoId) {
        let response = await api.despesasToZipFile(solicitacaoId)
        return response
    },

  },
});
