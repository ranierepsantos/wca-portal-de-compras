import { defineStore } from "pinia";
import { paginate } from "@/helpers/functions";
import userService from "@/services/user.service";
import clienteService from "@/services/reembolso/cliente.service";
import moment from "moment";

export const IDPERFILGESTOR = 5001
export const IDPERFILCOLABORADOR = 5002

export class Usuario {
    
    constructor(data = undefined) {
        this.id = data ? data.id : 0
        this.nome = data ? data.nome : ""
        this.email = data ? data.email: ""
        this.ativo = data ? data.ativo: true
        this.filialid = data ? data.filialid: null
        this.cliente = data? data.cliente: []
        this.usuarioSistemaPerfil = data? data.usuarioSistemaPerfil: []
        this.usuarioReembolsoComplemento = data? data.usuarioReembolsoComplemento ?? new UsuarioReembolsoComplemento() : new UsuarioReembolsoComplemento()
    }
}

class UsuarioReembolsoComplemento {
    constructor(data = undefined) {
        this.usuarioId = data? data.usuarioId: null;
        this.gestorId = data? data.gestorId: null;
        this.cargo = data? data.cargo: "";
    }
}

export class ContaCorrente {
    constructor (data = null) 
    {
        this.usuarioId = data? data.usuarioId : 0;
        this.saldo = data? data.saldo : 0;
        this.transacoes = data? data.transacoes: [];
    }

    adicionarTransacao(transacao) {
        this.transacoes.push(transacao)
        if (transacao.operador == "+")
            this.saldo += parseFloat(transacao.valor)
        else
            this.saldo -= parseFloat(transacao.valor)
    }

    getSaldo() {
        return this.saldo;
    }
    getTransacoes(){
        return this.transacoes;
    }
}

export class Transacao {
    constructor(descricao, operador = "+", valor){
        this.dataHora = moment().format("YYYY-MM-DDTHH:mm:ss")
        this.descricao = descricao
        this.operador = operador
        this.valor = valor 
    }
}

export const useUsuarioStore = defineStore("usuario", {
  actions: {
    
    async add (data) {

        try {
            let clientes = data.cliente.map(function (el) { return el.value; });

            data.cliente = [];
    
            let response = await userService.create(data);

            let userClientes = {
                usuarioId: response.data.id,
                clienteIds: clientes
            }
            
            await clienteService.RelacionarClienteUsuario(userClientes);


        } catch (error) {
            throw error
        }
    },
    
    async getById (id) {
        try {
            let response = await userService.getById(id);
            let data = response.data;
            
            response = await clienteService.getListByUser(data.id)
            
            data.cliente = response.data.map( item => {return { text: item.nome, value: item.id}})

            let usuario = new Usuario(data);
            return usuario;

        } catch (error) {
            throw error
        }
    },

    async getPaginate(pageNumber = 1, pageSize = 10, filter = "") {
        let response = await userService.paginate(pageSize, pageNumber, filter)
        return response.data
    },

    async update (data) {
            
            let clientes = data.cliente.map(function (el) { return el.value; });

            data.cliente = [];

            data.usuarioReembolsoComplemento.usuarioId = data.id;

            await userService.update(data);


            let userClientes = {
                usuarioId: data.id,
                clienteIds: clientes
            }
            
        await clienteService.RelacionarClienteUsuario(userClientes);

    },

    toComboList() {
        let list = []
        this.repository.forEach(item => {
            list.push ({text: item.nome, value: item.id})
        })
        return list;
    },

    toComboListGestorByCliente(clienteId) {
        let list = []
        let users = this.repository.filter(item => 
            item.cliente.filter(cli => cli.value == clienteId).length > 0 && 
            item.usuarioSistemaPerfil.filter(usp => usp.perfilId == IDPERFILGESTOR).length > 0);
        
            users.forEach(item => {
            list.push ({text: item.nome, value: item.id})
        })
        return list;
    },

    toComboListColaboradorByCliente(clienteId) {
        let list = []
        let users = this.repository.filter(item => 
            item.cliente.filter(cli => cli.value == clienteId).length > 0 && 
            item.usuarioSistemaPerfil.filter(usp => usp.perfilId == IDPERFILCOLABORADOR).length > 0);
        
            users.forEach(item => {
            list.push ({text: item.nome, value: item.id})
        })
        return list;
    },

  },
});
