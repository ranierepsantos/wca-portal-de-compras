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
    ListarItensMudanca: "Solicitacao/ListarItensMudanca",
}

export class Solicitacao {
    constructor(data = null) {
        this.id = data ? data.id : 0
        this.solicitacaoTipoId = data ? data.solicitacaoTipoId: null
        this.clienteId = data ? data.clienteId: null
        this.clienteNome = data ? data.clienteNome: null
        this.dataSolicitacao = data ?  moment(data.dataSolicitacao).format("YYYY-MM-DD"): moment().format("YYYY-MM-DD")
        this.descricao = data? data.descricao: null
        this.statusSolicitacaoId = data? data.statusSolicitacaoId: null
        this.responsavelId = data ? data.responsavelId: null
        this.comunicado = data && data.comunicado ? new Comunicado(data.comunicado) : new Comunicado()
        this.desligamento = data ? new Desligamento(data.desligamento) : new Desligamento()
        this.ferias = data ? new Ferias(data.ferias) : new Ferias()
        this.mudancaBase = data && data.mudancaBase ? new MudancaBase(data.mudancaBase) : new MudancaBase()
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
        this.vaga = data && data.vaga ? new Vaga(data.vaga) : new Vaga() 
    }
}
export class Anexo {
    constructor(data = {}) {
        if (!data) data = {}
        this.id = data.id || 0
        this.solicitacaoId = data.solicitacaoId || 0
        this.descricao = data.descricao || ''
        this.tipo = data.tipo || null
        this.caminhoArquivo = data.caminhoArquivo || ''
    }
}

export class Desligamento {
    constructor(data = {}){
        if (!data) data = {}
        this.solicitacaoId =  data.solicitacaoId || 0
        this.funcionarioId = data.funcionarioId || null
        this.funcionarioNome =  data.funcionarioNome || null
        this.eSocialMatricula  = data.eSocialMatricula || null
        this.funcionarioDataAdmissao = data.funcionarioDataAdmissao ?  moment(data.funcionarioDataAdmissao).format("YYYY-MM-DD"): null
        this.centroCustoId = data.centroCustoId || null
        this.centroCustoNome = data.centroCustoNome || null
        this.dataDemissao = data && data.dataDemissao? moment(data.dataDemissao).format("YYYY-MM-DD") : null
        this.motivoDemissaoId =  data.motivoDemissaoId || null
        this.temContratoExperiencia = data.temContratoExperiencia || false
        this.statusApontamento = data.statusApontamento || 1
        this.statusAvisoPrevio = data.statusAvisoPrevio || null
        this.statusFichaEpi = data.statusFichaEpi || 1
        this.statusExameDemissional = data.statusExameDemissional || 1
        this.dataCredito = data && data.dataCredito ? moment(data.dataCredito).format("YYYY-MM-DD") : null
        this.statusBeneficio = data.statusBeneficio || 1
        this.statusReembolso = data.statusReembolso || 1
    }
}

export class Comunicado {
    constructor(data = {}) {
        if (!data) data = {}
        this.solicitacaoId = data.solicitacaoId || 0
        this.funcionarioId = data.funcionarioId || null
        this.funcionarioNome =  data.funcionarioNome || null
        this.eSocialMatricula  = data.eSocialMatricula || null
        this.funcionarioDataAdmissao = data.funcionarioDataAdmissao ?  moment(data.funcionarioDataAdmissao).format("YYYY-MM-DD"): null
        this.centroCustoId = data.centroCustoId || null
        this.centroCustoNome = data.centroCustoNome || null
        this.dataAlteracao = data && data.dataAlteracao? moment(data.dataAlteracao).format("YYYY-MM-DD") : null
        this.assuntoId = data.assuntoId || null
        this.assuntoNome = data.assuntoNome || ""
        this.observacao = data.observacao || ""
    }
}

export class Ferias {
    constructor(data = {}) {
        if (!data) data = {}
        this.solicitacaoId = data.solicitacaoId || 0
        this.funcionarioId = data.funcionarioId || null
        this.funcionarioNome =  data.funcionarioNome || null
        this.eSocialMatricula  = data.eSocialMatricula || null
        this.funcionarioDataAdmissao = data.funcionarioDataAdmissao ?  moment(data.funcionarioDataAdmissao).format("YYYY-MM-DD"): null
        this.centroCustoId = data.centroCustoId || null
        this.centroCustoNome = data.centroCustoNome || null
        this.dataSaida = data.dataSaida? moment(data.dataSaida).format("YYYY-MM-DD") : null
        this.dataRetorno = data.dataRetorno? moment(data.dataRetorno).format("YYYY-MM-DD") : null
        this.tipoFeriasId = data.tipoFeriasId || null
        //this.tipoFerias = data.tipoFerias || {id: null, descricao: "", quantidadeDias: 0}
        this.tipoFeriasNome = data.tipoFeriasNome || null;
    }
}


export class MudancaBase {
    constructor(data = {}) {
        if (!data) data = {}
        this.solicitacaoId = data.solicitacaoId || 0
        this.funcionarioId = data.funcionarioId || null
        this.funcionarioNome =  data.funcionarioNome || null
        this.eSocialMatricula  = data.eSocialMatricula || null
        this.funcionarioDataAdmissao = data.funcionarioDataAdmissao ?  moment(data.funcionarioDataAdmissao).format("YYYY-MM-DD"): null
        this.centroCustoId = data.centroCustoId || null
        this.centroCustoNome = data.centroCustoNome || null
        this.clienteDestinoId = data.clienteDestinoId || null
        this.clienteDestinoNome =  data.clienteDestinoNome || null
        this.observacao = data.observacao || null
        this.dataAlteracao = data.dataAlteracao? moment(data.dataAlteracao).format("YYYY-MM-DD") : null
        this.itensMudanca = data.itensMudanca || []
    }
}

export class Vaga {
    constructor(data = {}) {
        if (!data) data = {}
        this.solicitacaoId = data.solicitacaoId || 0;
        this.tipoContratoId = data.tipoContratoId || null;
        this.tipoContratoNome = data.tipoContratoNome || null;
        this.tipoFaturamentoId = data.tipoFaturamentoId || null;
        this.tipoFaturamentoNome = data.tipoFaturamentoNome || null;
        this.gestorId = data.gestorId || null;
        this.gestorNome = data.gestorNome || null;
        this.funcaoId = data.funcaoId || null;
        this.funcaoNome = data.funcaoNome || null;
        this.escalaId = data.escalaId || null;
        this.escalaNome = data.escalaNome || null;
        this.horarioId = data.horarioId || null;
        this.horarioNome = data.horarioNome || null;
        this.quantidadeVagas = data.quantidadeVagas || null;
        this.sexoId = data.sexoId || null;
        this.sexoNome = data.sexoNome || null;
        this.idadeMinima = data.idadeMinima || null;
        this.idadeMaxima = data.idadeMaxima || null;
        this.caracteristica = data.caracteristica || null;
        this.indicacao = data.indicacao || null;
        this.enderecoCliente = data.enderecoCliente || null;
        this.anotacoes = data.anotacoes || null;
        this.motivoContratacaoId = data.motivoContratacaoId || null;
        this.motivoContratacaoNome = data.motivoContratacaoNome || null;
        this.justificativaContratacao = data.justificativaContratacao || null;
        this.permiteFumante = data.permiteFumante || 0;
        this.escolaridadeId = data.escolaridadeId || null;
        this.escolaridadeNome = data.escolaridadeNome || null;
        this.localResidencia = data.localResidencia || null;
        this.experienciaProfissinal = data.experienciaProfissinal || null;
        this.descricaoAtividades = data.descricaoAtividades || null;
        this.temCNH = data.temCNH || 0;
        this.categoriaCNH = data.categoriaCNH || null;
        this.temValeTransporte = data.temValeTransporte || 0;
        this.valorValeTransporte = data.valorValeTransporte || 0.0;
        this.diasValeTransporte = data.diasValeTransporte || null;
        this.refeicao = data.refeicao || null;
        this.outrosBeneficios = data.outrosBeneficios || null;
        this.salarioBase = data.salarioBase || 0;
        this.temInsalubridade = data.temInsalubridade || 0;
        this.percentualInsalubridade = data.percentualInsalubridade || 0.0;
        this.temPericulosidade = data.temPericulosidade || 0;
        this.percentualPericulosidade = data.percentualPericulosidade || 0.0;
        this.dataInicioPrevista = data.dataInicioPrevista ? moment(data.dataInicioPrevista).format("YYYY-MM-DD"): null;
        this.temCopiaAdmissaoCliente = data.temCopiaAdmissaoCliente || 0;
        this.temIntegracaoCliente = data.temIntegracaoCliente || 0;
        this.horarioIntegracao = data.horarioIntegracao || null;
        this.integracaoDiasSemana = data.integracaoDiasSemana || null;
        this.documentoComplementares = data.documentoComplementares || [];
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
    itensMudanca: JSON.parse(localStorage.getItem("share-itens-mudanca")) || [],
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
                data.vaga = null
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
                data.vaga = null
            }else if (data.solicitacaoTipoId == 3) //Férias
            {
                delete data.ferias.tipoFerias
                data.comunicado  = null
                data.desligamento  = null
                data.mudancaBase = null 
                data.vaga = null
            }else if (data.solicitacaoTipoId == 4) //Mudança de Base
            {
                data.comunicado  = null
                data.desligamento = null 
                data.ferias = null
                data.vaga = null
            }else if (data.solicitacaoTipoId == 5) //Vaga
            {
                data.comunicado  = null
                data.desligamento = null 
                data.ferias = null
                data.mudancaBase = null

                data.vaga.permiteFumante = data.vaga.permiteFumante == 1 ? true : false; 
                data.vaga.temCNH = data.vaga.temCNH == 1 ? true : false; 
                data.vaga.temCopiaAdmissaoCliente = data.vaga.temCopiaAdmissaoCliente == 1 ? true : false; 
                data.vaga.temInsalubridade = data.vaga.temInsalubridade == 1 ? true : false; 
                data.vaga.temIntegracaoCliente = data.vaga.temIntegracaoCliente == 1 ? true : false; 
                data.vaga.temPericulosidade = data.vaga.temPericulosidade == 1 ? true : false; 
                data.vaga.temValeTransporte = data.vaga.temValeTransporte == 1 ? true : false; 

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
                data.ferias = null 
                data.vaga = null
            }else if (data.solicitacaoTipoId == 2) {
                delete data.comunicado.assunto
                data.desligamento  = null
                data.mudancaBase = null 
                data.ferias = null
                data.vaga = null
            }else if (data.solicitacaoTipoId == 3) //Férias
            {
                delete data.ferias.tipoFerias
                data.comunicado  = null
                data.desligamento  = null
                data.mudancaBase = null 
                data.vaga = null
            }else if (data.solicitacaoTipoId == 4) {
                data.comunicado  = null
                data.desligamento = null 
                data.ferias = null
                data.vaga = null
            }else if (data.solicitacaoTipoId == 5) //Vaga
            {
                data.comunicado  = null
                data.desligamento = null 
                data.ferias = null
                data.mudancaBase = null

                data.vaga.permiteFumante = data.vaga.permiteFumante == 1 ? true : false; 
                data.vaga.temCNH = data.vaga.temCNH == 1 ? true : false; 
                data.vaga.temCopiaAdmissaoCliente = data.vaga.temCopiaAdmissaoCliente == 1 ? true : false; 
                data.vaga.temInsalubridade = data.vaga.temInsalubridade == 1 ? true : false; 
                data.vaga.temIntegracaoCliente = data.vaga.temIntegracaoCliente == 1 ? true : false; 
                data.vaga.temPericulosidade = data.vaga.temPericulosidade == 1 ? true : false; 
                data.vaga.temValeTransporte = data.vaga.temValeTransporte == 1 ? true : false; 
            }
            await api.put(rotas.Update, data);
        } catch (error) {
            throw error
        }
    },
    async getById(id) {
        try {
            let response = await api.get(rotas.GetById, {params: {id: id}});
            let solicitacao = new Solicitacao(response.data)

            if (solicitacao.solicitacaoTipoId == 5) //vaga
            {
                if (solicitacao.vaga.integracaoDiasSemana)
                    solicitacao.vaga.integracaoDiasSemana = solicitacao.vaga.integracaoDiasSemana.split(',');
                
                solicitacao.vaga.permiteFumante = solicitacao.vaga.permiteFumante ? 1 : 0; 
                solicitacao.vaga.temCNH = solicitacao.vaga.temCNH ? 1 : 0; 
                solicitacao.vaga.temCopiaAdmissaoCliente = solicitacao.vaga.temCopiaAdmissaoCliente ? 1 : 0;
                solicitacao.vaga.temInsalubridade = solicitacao.vaga.temInsalubridade ? 1 : 0;
                solicitacao.vaga.temIntegracaoCliente = solicitacao.vaga.temIntegracaoCliente ? 1 : 0;
                solicitacao.vaga.temPericulosidade = solicitacao.vaga.temPericulosidade ? 1 : 0; 
                solicitacao.vaga.temValeTransporte = solicitacao.vaga.temValeTransporte ? 1 : 0; 

            }
            return solicitacao;
        } catch (error) {
            console.log("solicitacao.store.error", error)
            throw error
        }
    },
    async getPaginate(page = 1, pageSize = 10, filters) {
        try {
            let parametros =  {...filters}
            parametros.page = page;
            parametros.pageSize = pageSize;
            

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
                let response = await api.get(rotas.ListarAssuntos);
                this.assuntos = response.data;
                localStorage.setItem('share-assuntos',JSON.stringify(this.assuntos))
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
    async getListaItensMudanca() {
        try {
            if (this.itensMudanca.length == 0)
            {

                let response = await api.get(rotas.ListarItensMudanca);
                this.itensMudanca = response.data 
            
                localStorage.setItem('share-itens-mudanca',JSON.stringify(this.itensMudanca))
            }
            return this.itensMudanca;
        } catch (error) {
            throw error
        }  
    },
  }
})