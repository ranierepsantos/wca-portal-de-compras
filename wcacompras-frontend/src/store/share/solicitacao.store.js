import { defineStore } from "pinia";
import moment from "moment/moment";
import api from "@/services/share/shareApi"

const rotas = {
    Create: "Solicitacao",
    Update: "Solicitacao",
    GetById: "Solicitacao",
    Paginar: "Solicitacao/Paginar",
    AlterarStatus: "Solicitacao/AlterarStatus",
    RegistrarEvento: "Solicitacao/RegistrarEvento",
    ExportarParaExcel: "Solicitacao/ExportarParaExcel",
    ListarPorColaboradorGestor: "Solicitacao/ListarPorColaboradorGestor",
    ListarStatusSolicitacao: "Solicitacao/ListarStatusSolicitacao",
    ListarMotivoDemissao: "Solicitacao/ListarMotivoDemissao"
}

export class Solicitacao {
    constructor(data = null) {
        this.id = data ? data.id : 0
        this.solicitacaoTipoId = data ? data.solicitacaoTipoId: null
        this.clienteId = data ? data.clienteId: null
        this.clienteNome = data ? data.clienteNome: null
        this.funcionarioId = data ? data.funcionarioId: null
        this.funcionarioNome = data ? data.funcionarioNome: null
        this.funcionarioDataAdmissao = data && data.funcionarioDataAdmissao ?  moment(data.funcionarioDataAdmissao).format("YYYY-MM-DD"): null
        this.dataSolicitacao = data ?  moment(data.dataSolicitacao).format("YYYY-MM-DD"): moment().format("YYYY-MM-DD")
        this.descricao = data? data.descricao: null
        this.statusSolicitacaoId = data? data.statusSolicitacaoId: null
        this.responsavelId = data ? data.responsavelId: null
        this.centroCustoId = data ? data.centroCustoId: null
        this.centroCustoNome = data ? data.centroCustoNome: null
        this.comunicado = data && data.comunicado ? data.comunicado :null
        this.desligamento = data ? new Desligamento(data.desligamento) : new Desligamento()
        this.mudancaBase = data && data.mudancaBase ? data.mudancaBase : null
        this.anexos = data && data.anexos ? data.anexos: []
        this.historico = data && data.historico ? data.historico: []
    }
}
export class Anexo {
    constructor(data = null) {
        this.id = data? data.id: 0
        this.solicitacaoId = data ?data.solicitacaoId : 0
        this.descricao = data? data.descricao: ''
        this.tipo = data? data.tipo: null
        this.caminhoArquivo = data? data.caminhoArquivo: ''
    }
}

export class Desligamento {
    constructor(data = null){
        this.solicitacaoId = data ? data.solicitacaoId : 0
        this.dataDemissao = data && data.dataDemissao? moment(data.dataDemissao).format("YYYY-MM-DD") : null
        this.motivoDemissaoId = data ? data.motivoDemissaoId: null
        this.temContratoExperiencia = data? data.temContratoExperiencia: false
        this.statusApontamento = data? data.statusApontamento: 1
        this.statusAvisoPrevio = data? data.statusAvisoPrevio: null
        this.statusFichaEpi = data? data.statusFichaEpi: 1
        this.statusExameDemissional = data? data.statusExameDemissional: 1
        this.dataCredito = data && data.dataCredito ? moment(data.dataCredito).format("YYYY-MM-DD") : null
        this.statusBeneficio = data? data.statusBeneficio: 1
        this.statusReembolso = data? data.statusReembolso: 1
    }
}

export class Comunicado {
    
}

export class MudancaBase {
    
}


/**NaoSeAplica = 0,
Indenizado = 1, 
Trabalho =2  */


export const useShareSolicitacaoStore = defineStore("shareSolicitacao", {
  state : () => ({
    fieldStatus : [
        {text: "Pendente", value: 1},
        {text: "Concluído", value: 2}
    ],
    exameAdmissionalStatus : [
        {text: "Não se aplica", value: 3},
        {text: "Pendente", value: 1},
        {text: "Concluído", value: 2}
    ],
    avisoPrevioStatus: [
        {text: "Não se aplica", value: 1},
        {text: "Indenizado", value: 2},
        {text: "Trabalho", value: 3},
    ],
    statusSolicitacao: JSON.parse(localStorage.getItem("share-status-solicitacao")) || [],
    motivosDemissao: JSON.parse(localStorage.getItem("share-motivo-demissao")) || []
  }),
  actions: {
    async add (data) {
        try {
            console.debug("solicitacao.store.add",data);
            if (data.solicitacaoTipoId == 1) {
                data.comunicado  = null
                data.mudancaBase = null 
            }else if (data.solicitacaoTipoId == 2) {
                data.desligamento  = null
                data.mudancaBase = null 
            }else if (data.solicitacaoTipoId == 4) {
                data.comunicado  = null
                data.desligamento = null 
            }
            data.status = this.statusSolicitacao.find(x => x.id == 1);
            console.debug("add=>data.status",data.status);
            await api.post(rotas.Create, data);
        } catch (error) {
            throw error
        }
    },
    async update (data) {
        try {
            
            if (data.solicitacaoTipoId == 1) {
                data.comunicado  = null
                data.mudancaBase = null 
            }else if (data.solicitacaoTipoId == 2) {
                data.desligamento  = null
                data.mudancaBase = null 
            }else if (data.solicitacaoTipoId == 4) {
                data.comunicado  = null
                data.desligamento = null 
            }
            await api.put(rotas.Update, data);
        } catch (error) {
            throw error
        }
    },
    async getById(id) {
        try {
            let response = await api.get(rotas.GetById, {params: {id: id}});
            return new Solicitacao(response.data)
        } catch (error) {
            throw error
        }
    },
    async getPaginate(page = 1, pageSize = 10, filters) {
        try {
            let parametros = {
                page: page,
                pageSize: pageSize,
                filialId: filters.filialId,
                responsavelId: filters.responsavelId,
                clienteId: filters.clienteId,
                dataIni: filters.dataIni,
                dataFim: filters.dataFim,
                status: filters.status,
                tipoSolicitacao: filters.tipoSolicitacao
            }
            

            let response = await api.get(rotas.Paginar, {params: parametros} );
            return response.data        
        } catch (error) {
            throw error
        }
    },
    async listarStatusSolicitacao() {
        try {
            if (this.statusSolicitacao.length == 0)
            {
                let response = await api.get(rotas.ListarStatusSolicitacao);
            
                this.statusSolicitacao = response.data 
                localStorage.setItem('share-status-solicitacao',JSON.stringify(this.statusSolicitacao))
            }
        } catch (error) {
            throw error
        }  
    },
    async listarMotivosDemissao() {
        try {
            if (this.motivosDemissao.length == 0)
            {
                let response = await api.get(rotas.ListarMotivoDemissao);
            
                this.motivosDemissao = response.data 
                localStorage.setItem('share-motivo-demissao',JSON.stringify(this.motivosDemissao))
            }
        } catch (error) {
            throw error
        }  
    },
  }
})