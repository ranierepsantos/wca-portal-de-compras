import { defineStore } from "pinia";
import moment from "moment/moment";
import api from "@/services/share/shareApi"

const rotas = {
    Create: "Funcionario",
    Update: "Funcionario",
    GetById: "Funcionario",
    Paginar: "Funcionario/Paginar",
    ListByClienteToCombo: "Funcionario/ListByClienteToCombo"
}

export class Funcionario {
    constructor(data = null) {
        this.id = data ? data.id : 0
        this.nome = data ? data.nome: null
        this.clienteId = data ? data.clienteId: null
        this.centroCustoId= data ? data.centroCustoId: null
        this.dataAdmissao = data ?  moment(data.dataAdmissao).format("YYYY-MM-DD"): null
        this.dataDemissao = data && data.dataDemissao ?  moment(data.dataDemissao).format("YYYY-MM-DD"): null
        this.codigoFuncionario = data ? data.codigoFuncionario: null
        this.email = data ? data.email: null
        this.dddCelular = data ? data.dddCelular: null
        this.numeroCelular = data ? data.numeroCelular: null
        this.numeroPis = data ? data.numeroPis: null
    }
}


export const useShareFuncionarioStore = defineStore("shareFuncionario", {
  actions: {
    async add (data) {
        try 
        {
            data.dddCelular = data.dddCelular.replace("(","").replace(")","").replace("-","")
            data.numeroCelular = data.numeroCelular.replace("(","").replace(")","").replace("-","")
            let response = await api.post(rotas.Create, data);
            return new Funcionario(response.data);
        } catch (error) {
            throw error
        }
    },
    async update (data) {
        try {
            data.dddCelular = data.dddCelular.replace("(","").replace(")","").replace("-","")
            data.numeroCelular = data.numeroCelular.replace("(","").replace(")","").replace("-","")
            let response = await api.put(rotas.Update, data);
            return new Funcionario(response.data);
        } catch (error) {
            throw error
        }
    },
    async getById(id) {
        try {
            let response = await api.get(rotas.GetById, {params: {id: id}});
            return new Funcionario(response.data)
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
                termo: filters.termo,
                clienteIds: filters.clienteIds,
                centroCustoIds: filters.centroCustoIds,
            }
            let response = await api.post(rotas.Paginar, parametros);
            return response.data        
        } catch (error) {
            throw error
        }
    },
    async getToComboByCliente(clienteId, usuarioId){
        try {
            let response = await api.get(rotas.ListByClienteToCombo, {params: {clienteId: clienteId, usuarioId: usuarioId}} );
            return response.data;    
        } catch (error) {
            throw error
        }
    }


  }
})