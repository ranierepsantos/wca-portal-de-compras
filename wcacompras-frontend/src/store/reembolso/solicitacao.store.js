import { defineStore } from "pinia";
import api from "@/services/reembolso/solicitacao.service";
import moment from "moment/moment";

export class Solicitacao {
    controlIdDespesa = sessionStorage.getItem("reembolso-despesa-fakeid")||0
    constructor(data = {}) {
        this.id= data.id || 0
        this.clienteId= data.clienteId || null
        this.dataSolicitacao=data.dataSolicitacao || moment().format("YYYY-MM-DD")
        this.colaboradorId= data.colaboradorId || null
        this.colaboradorNome = data.colaboradorNome || ""
        this.centroCustoId = data.centroCustoId || null
        this.centroCustoNome = data.centroCustoNome || ""
        this.colaboradorCargo= data.colaboradorCargo || ""
        this.projeto= data == data.projeto || ""
        this.objetivo= data.objetivo || ""
        this.periodoInicial= data.periodoInicial ||moment().format("YYYY-MM-DD")
        this.periodoFinal=  data.periodoFinal || moment().format("YYYY-MM-DD")
        this.valorAdiantamento = data.valorAdiantamento || 0.00
        this.valorDespesa =  data.valorDespesa || 0.00
        this.status = data.status || 1
        this.statusAnterior =  data.statusAnterior || 1
        this.cliente = data.cliente || {}
        this.despesa= data.despesa || []
        this.solicitacaoHistorico = data.solicitacaoHistorico ||[]
        this.tipoSolicitacao= data.tipoSolicitacao || null
        this.marca = data.marca || null
        this.valorUnitario = data.valorUnitario || 0.00
        this.quantidade = data.quantidade || 1
        this.valorFrete = data.valorFrete || 0.00
        this.descricao = data.descricao || null
        this.dataPrevistaEntrega = data.dataPrevistaEntrega || null
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

    calcularTotalDespesas() {
        let valorTotalDespesa = 0;

        this.despesa.forEach((item) => {
            valorTotalDespesa += parseFloat(item.valor);
        });
        valorTotalDespesa = valorTotalDespesa.toFixed(2);
        this.valorDespesa = valorTotalDespesa;
        return valorTotalDespesa;
    }

    calcularColaboradorReembolso() {
        let valor = 0;
        this.despesa.forEach((item) => {
            if (item.reembolsarColaborador)
                valor += parseFloat(item.valor);
        });
        valor = valor.toFixed(2);
        return valor;
    }

    calcularValorSolicitacaoEspecial() {
        let valor = 0

        valor = (parseFloat(this.valorUnitario) * parseInt(this.quantidade)) + parseFloat(this.valorFrete)
        valor = valor.toFixed(2);
        this.valorAdiantamento = valor;
        return valor;
    }
}

export class Despesa {
    constructor(data = null) {
        this.id = data ?  data.id: 0
        this.solicitacaoId = data ?  data.solicitacaoId : 0
        this.tipoDespesaId = data ?  data.tipoDespesaId : null
        this.tipoDespesaNome = data && data.tipoDespesaNome ? data.tipoDespesaNome: null
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
        {text: "EPI", value: 3},
        {text: "Reembolso", value: 1},
        {text: "Voucher", value: 4},
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
        let solicitacao = new Solicitacao(response.data);
        return solicitacao
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
