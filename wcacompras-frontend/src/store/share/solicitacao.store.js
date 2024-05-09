import { defineStore } from "pinia";
import moment from "moment/moment";
import api from "@/services/share/shareApi"
import configuracaoService from "@/services/share/configuracao.service";
import { useShareUsuarioStore } from "./usuario.store";

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
    ListarMotivoDemissao: "Solicitacao/ListarMotivoDemissao",
    ListarAssuntos: "Solicitacao/ListarAssuntos",
    ListarTipoFerias: "Solicitacao/ListarTipoFerias",
}

export class Solicitacao {
    constructor(data = null) {
        this.id = data ? data.id : 0
        this.solicitacaoTipoId = data ? data.solicitacaoTipoId: null
        this.clienteId = data ? data.clienteId: null
        this.clienteNome = data ? data.clienteNome: null
        this.funcionarioId = data ? data.funcionarioId: null
        this.funcionarioNome = data ? data.funcionarioNome: null
        this.eSocialMatricula  = data ? data.eSocialMatricula: null
        this.funcionarioDataAdmissao = data && data.funcionarioDataAdmissao ?  moment(data.funcionarioDataAdmissao).format("YYYY-MM-DD"): null
        this.dataSolicitacao = data ?  moment(data.dataSolicitacao).format("YYYY-MM-DD"): moment().format("YYYY-MM-DD")
        this.descricao = data? data.descricao: null
        this.statusSolicitacaoId = data? data.statusSolicitacaoId: null
        this.responsavelId = data ? data.responsavelId: null
        this.centroCustoId = data ? data.centroCustoId: null
        this.centroCustoNome = data ? data.centroCustoNome: null
        this.comunicado = data && data.comunicado ? new Comunicado(data.comunicado) : new Comunicado()
        this.desligamento = data ? new Desligamento(data.desligamento) : new Desligamento()
        this.ferias = data ? new Ferias(data.ferias) : new Ferias()
        this.mudancaBase = data && data.mudancaBase ? data.mudancaBase : null
        this.anexos = data && data.anexos ? data.anexos: []
        this.historico = data && data.historico ? data.historico: []
        this.status = {
            id: 0,
            status: "",
            statusIntermediario: "",
            color: "",
            notifica: 0,
            autorizar: false,
            templateNotificacao: null,
            proximoStatusId: null
        };
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
    constructor(data = null) {
        this.solicitacaoId = data ? data.solicitacaoId : 0
        this.dataAlteracao = data && data.dataAlteracao? moment(data.dataAlteracao).format("YYYY-MM-DD") : null
        this.assuntoId = data ? data.assuntoId: null
        this.observacao = data? data.observacao: ""
        this.assunto = data && data.assunto ? data.assunto: {id: null, nome: "", ativo: false}
    }
}

export class Ferias {
    constructor(data = null) {
        this.solicitacaoId = data ? data.solicitacaoId : 0
        this.dataSaida = data && data.dataSaida? moment(data.dataSaida).format("YYYY-MM-DD") : null
        this.dataRetorno = data && data.dataRetorno? moment(data.dataRetorno).format("YYYY-MM-DD") : null
        this.tipoFeriasId = data ? data.tipoFeriasId : null
        this.tipoFerias = data && data.tipoFerias ? data.tipoFerias: {id: null, descricao: "", quantidadeDias: 0}
    }
}



export class MudancaBase {
    constructor(data = null) {
        this.solicitacaoId = data ? data.solicitacaoId : 0
        this.clienteDestinoId = data ? data.clienteDestinoId : 0
        this.clienteDestinoNome = data ? data.clienteDestinoNome : null
        this.dataAlteracao = data && data.dataAlteracao? moment(data.dataAlteracao).format("YYYY-MM-DD") : null
        this.itensMudanca = data && data.itensMudanca ? data.itensMudanca: []
    }
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
    motivosDemissao: JSON.parse(localStorage.getItem("share-motivo-demissao")) || [],
    assuntos: JSON.parse(localStorage.getItem("share-assuntos")) || [],
    tipoFerias: JSON.parse(localStorage.getItem("share-tipoFerias")) || [],
  }),
  actions: {
    async add (data) {
        try {
            let checarAprovacao = true
            if (data.solicitacaoTipoId == 1) //Desligamento
            {
                data.comunicado  = null
                data.mudancaBase = null 
                data.ferias = null

                //Não checar aprovação se motivo demissão for:
                //  3 - PEDIDO DE DEMISSÃO ou 
                //  6 - TERMINO CONTRATO DETERMINADO
                if (data.desligamento.motivoDemissaoId == 3 || data.desligamento.motivoDemissaoId == 6)
                    checarAprovacao = false;

            }else if (data.solicitacaoTipoId == 2)  //Comunicado
            {
                delete data.comunicado.assunto
                data.desligamento  = null
                data.mudancaBase = null 
                data.ferias = null
            }else if (data.solicitacaoTipoId == 3) //Férias
            {
                delete data.ferias.tipoFerias
                data.comunicado  = null
                data.desligamento  = null
                data.mudancaBase = null 
            }else if (data.solicitacaoTipoId == 4) //Mudança de Base
            {
                data.comunicado  = null
                data.desligamento = null 
                data.ferias = null
            }
            //traz o status inicial da solicitação
            let notificacaopermissao = data.regra + '-executar'
            data.status = this.statusSolicitacao.find(x => x.statusIntermediario.toLowerCase() == 'pendente');

            //verifica se requer aprovação
            if (checarAprovacao){
                let configuracao = (await configuracaoService.getByChave(data.regra.toLowerCase()+'.requer.aprovacao')).data;
                if (configuracao && configuracao.valor == "true"){
                    notificacaopermissao = data.regra + '-aprovar'
                    data.status = this.statusSolicitacao.find(x => x.statusIntermediario.toLowerCase() == 'aguardando aprovação');
                }
            }
            
            //retorna a lista de usuários que serão notificados
            data.notificarUsuarioIds = await this.retornaUsuariosParaNotificar(data.status, data.clienteId, notificacaopermissao)

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
                delete data.comunicado.assunto
                data.desligamento  = null
                data.mudancaBase = null 
            }else if (data.solicitacaoTipoId == 3) //Férias
            {
                delete data.ferias.tipoFerias
                data.comunicado  = null
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
                clienteIds: filters.clienteIds,
                centroCustoIds: filters.centroCustoIds,
                dataIni: filters.dataIni,
                dataFim: filters.dataFim,
                status: filters.status,
                tipoSolicitacao: filters.tipoSolicitacao
            }
            

            let response = await api.post(rotas.Paginar, parametros );
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
            return this.motivosDemissao;
        } catch (error) {
            throw error
        }  
    },

    async getListaAssuntos() {
        try {
            if (this.assuntos.length == 0)
            {
                let response = await api.get(rotas.ListarAssuntos);
            
                this.assuntos = response.data 
                localStorage.setItem('share-assuntos',JSON.stringify(this.assuntos))
            }
            return this.assuntos;
        } catch (error) {
            throw error
        }  
    },

    async retornaUsuariosParaNotificar(status, clienteId, permissao) {
        let list = []
        if (status.notifica == 1)
        {
          let notificaList = await useShareUsuarioStore().getUsuarioToNotificacaoByCliente(clienteId, permissao)
          list = notificaList.map(q => {return q.value})
        }
        return list;
    },
    async changeStatus (data) {
        try {
            await api.put(rotas.AlterarStatus, data);
        } catch (error) {
            throw error
        }
    },
    getStatus(statusId) {
        let data = this.statusSolicitacao.find(q => q.id == statusId)
        return data
    },
    async getTipoFerias() {
        try {
            if (this.tipoFerias.length == 0)
            {
                let response = await api.get(rotas.ListarTipoFerias);
            
                this.tipoFerias = response.data 
                localStorage.setItem('share-tipoFerias',JSON.stringify(this.tipoFerias))
            }
            return this.tipoFerias;
        } catch (error) {
            throw error
        }  
    },
  }
})