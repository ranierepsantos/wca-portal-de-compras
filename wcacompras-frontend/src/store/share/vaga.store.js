import { defineStore } from "pinia";
import moment from "moment/moment";
import api from "@/services/share/shareApi"
import configuracaoService from "@/services/share/configuracao.service";
import { useShareUsuarioStore } from "./usuario.store";


const rotas = {
    Create: "Vaga",
    Update: "Vaga",
    GetById: "Vaga",
    Paginar: "Vaga/Paginar",
    AlterarStatus: "Vaga/AlterarStatus",
    RegistrarEvento: "Vaga/RegistrarEvento",
    ListarStatusVaga: "Vaga/ListarStatusVaga",
}

export class Vaga {
    constructor(data = {}) {
        this.id = data.id || 0;
        this.clienteId = data.clienteId || null;
        this.clienteNome = data.clienteNome || null;
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
        this.dataSolicitacao = data.dataSolicitacao ? moment(data.dataSolicitacao).format("YYYY-MM-DD") : moment().format("YYYY-MM-DD");
        this.statusSolicitacaoId = data.statusSolicitacaoId || null;
        this.statusSolicitacaoNome = data.statusSolicitacaoNome || null;
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
        this.vagaHistorico = data.vagaHistorico || [];
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


export const useShareVagaStore = defineStore("shareVaga", {
    state: () => ({
        statusSolicitacao: JSON.parse(localStorage.getItem("share-status-solicitacao")) || [],
    }),
    actions: {
        async add (data) {
            try {
                let checarAprovacao = true;
                //traz o status inicial da solicitação
                let notificacaopermissao = 'vaga-executar'
                data.status = this.statusSolicitacao.find(x => x.statusIntermediario.toLowerCase() == 'pendente');
                if (data.integracaoDiasSemana)
                    data.integracaoDiasSemana = data.integracaoDiasSemana.join(',')
                //verifica se requer aprovação
                if (checarAprovacao){
                    let configuracao = (await configuracaoService.getByChave('vaga.requer.aprovacao')).data;
                    if (configuracao && configuracao.valor == "true"){
                        notificacaopermissao = 'vaga-aprovar'
                        data.status = this.statusSolicitacao.find(x => x.statusIntermediario.toLowerCase() == 'aguardando aprovação');
                    }
                }
                
                data.permiteFumante = data.permiteFumante == 1 ? true : false; 
                data.temCNH = data.temCNH == 1 ? true : false; 
                data.temCopiaAdmissaoCliente = data.temCopiaAdmissaoCliente == 1 ? true : false; 
                data.temInsalubridade = data.temInsalubridade == 1 ? true : false; 
                data.temIntegracaoCliente = data.temIntegracaoCliente == 1 ? true : false; 
                data.temPericulosidade = data.temPericulosidade == 1 ? true : false; 
                data.temValeTransporte = data.temValeTransporte == 1 ? true : false; 


                //retorna a lista de usuários que serão notificados
                data.notificarUsuarioIds = []
                if (data.status.notifica == 1)
                {
                    let notificaList = await useShareUsuarioStore().getUsuarioToNotificacaoByCliente(data.clienteId, notificacaopermissao)
                    data.notificarUsuarioIds = notificaList.map(q => {return q.value})
                }
                
                await api.post(rotas.Create, data);
            } catch (error) {
                throw error
            }
        },
        async update (data) {
            try {
                let notificacaopermissao = 'vaga-executar'
                data.status = this.statusSolicitacao.find(x => x.statusIntermediario.toLowerCase() == 'pendente');
                if (data.integracaoDiasSemana)
                    data.integracaoDiasSemana = data.integracaoDiasSemana.join(',')
                data.permiteFumante = data.permiteFumante == 1 ? true : false; 
                data.temCNH = data.temCNH == 1 ? true : false; 
                data.temCopiaAdmissaoCliente = data.temCopiaAdmissaoCliente == 1 ? true : false; 
                data.temInsalubridade = data.temInsalubridade == 1 ? true : false; 
                data.temIntegracaoCliente = data.temIntegracaoCliente == 1 ? true : false; 
                data.temPericulosidade = data.temPericulosidade == 1 ? true : false; 
                data.temValeTransporte = data.temValeTransporte == 1 ? true : false; 

                //retorna a lista de usuários que serão notificados
                data.notificarUsuarioIds = []
                if (data.status.notifica == 1)
                {
                    let notificaList = await useShareUsuarioStore().getUsuarioToNotificacaoByCliente(data.clienteId, notificacaopermissao)
                    data.notificarUsuarioIds = notificaList.map(q => {return q.value})
                }
                await api.put(rotas.Update, data);
            } catch (error) {
                throw error
            }
        },
        async getById(id) {
            try {
                let response = await api.get(rotas.GetById, {params: {id: id}});
                let vaga = new Vaga(response.data)
                if (vaga.integracaoDiasSemana)
                    vaga.integracaoDiasSemana = vaga.integracaoDiasSemana.split(',');
                
                vaga.permiteFumante = vaga.permiteFumante ? 1 : 0; 
                vaga.temCNH = vaga.temCNH ? 1 : 0; 
                vaga.temCopiaAdmissaoCliente = vaga.temCopiaAdmissaoCliente ? 1 : 0;
                vaga.temInsalubridade = vaga.temInsalubridade ? 1 : 0;
                vaga.temIntegracaoCliente = vaga.temIntegracaoCliente ? 1 : 0;
                vaga.temPericulosidade = vaga.temPericulosidade ? 1 : 0; 
                vaga.temValeTransporte = vaga.temValeTransporte ? 1 : 0; 

                return vaga
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
                    dataIni: filters.dataIni,
                    dataFim: filters.dataFim,
                    status: filters.status
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
                    let response = await api.get(rotas.ListarStatusVaga);
                
                    this.statusSolicitacao = response.data 
                    localStorage.setItem('share-status-solicitacao',JSON.stringify(this.statusSolicitacao))
                }
            } catch (error) {
                throw error
            }  
        },
    }
});