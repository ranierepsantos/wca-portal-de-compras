import { defineStore } from "pinia";
import { paginate } from "@/helpers/functions";
import userService from "@/services/user.service";

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

export const useUsuarioStore = defineStore("usuario", {
  state: () => ({
    idControl: 11000,
    repository: JSON.parse(localStorage.getItem("reembolso-usuarios")) || []
  }),
  actions: {
    
    add (data) {
        this.idControl++;
        data.id = this.idControl;
        this.repository.push(data)
        localStorage.setItem("reembolso-usuarios", JSON.stringify(this.repository))
    },
    
    async getById (id) {
        let data = this.repository.find(c => c.id == id)
        console.log(`usuario.id ${id} ->`, data )
        return  new Usuario(data);
    },

    async getPaginate(pageNumber = 1, pageSize = 10, filter = "") {
        console.log("repository.lenght", this.repository.length)
        if (this.repository.length == 0) {
            let response = await userService.paginate(pageSize, pageNumber, filter)
            localStorage.setItem("reembolso-usuarios", JSON.stringify(response.data.items))
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

  },
});
