import { defineStore } from "pinia";
import { paginate } from "@/helpers/functions";
import userService from "@/services/user.service";
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
        this.cargo = data ? data.cargo: ""
        this.usuarioGestor = data? data.usuarioGestor: null
        this.usuarioSistemaPerfil = data? data.usuarioSistemaPerfil: []
        this.contaCorrente = data ? new ContaCorrente(data.contaCorrente) : new ContaCorrente()
    }

    alterarGestor (gestorId) {
        let usuarioGestor = new UsuarioGestor(this.id, gestorId)
        this.usuarioGestor = []
        this.usuarioGestor.push(usuarioGestor)
    }
}

class UsuarioGestor {
    constructor(usuarioId, gestorId) {
        this.usuarioId = usuarioId
        this.gestorId = gestorId
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
  state: () => ({
    idUsuario: localStorage.getItem("reembolso-usuario-id") || 11000,
    repository: JSON.parse(localStorage.getItem("reembolso-usuarios")) || []
  }),
  actions: {
    
    add (data) {
        this.idUsuario++;
        data.id = this.idUsuario;
        this.repository.push(data)
        localStorage.setItem("reembolso-usuarios", JSON.stringify(this.repository))
        localStorage.setItem("reembolso-usuarios-id", this.idUsuario)
    },
    
    async getById (id) {
        let data = this.repository.find(c => c.id == id)
        return  new Usuario(data);
    },

    async getPaginate(pageNumber = 1, pageSize = 10, filter = "") {
        if (this.repository.length == 0) {
            let response = await userService.paginate(pageSize, pageNumber, filter)
            this.repository = response.data.items
            localStorage.setItem("reembolso-usuarios", JSON.stringify(this.repository))
            return response.data
        }
        return paginate(this.repository, pageNumber, pageSize)
    },

    update (data) {
        let index = this.repository.findIndex(q => q.id == data.id)
        if (index == -1) {
            return false;
        }
        this.repository[index] = {...data};
        localStorage.setItem("reembolso-usuarios", JSON.stringify(this.repository))
        return true;
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
