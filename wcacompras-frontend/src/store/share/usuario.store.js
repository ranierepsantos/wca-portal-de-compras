import { defineStore } from "pinia";
import userService from "@/services/user.service";
import usuarioService from "@/services/share/usuario.service"
import clienteService from "@/services/share/cliente.service";
import filialService from "@/services/filial.service";

export class Usuario {
    
    constructor(data = undefined) {
        this.id = data ? data.id : 0
        this.nome = data ? data.nome : ""
        this.email = data ? data.email: ""
        this.ativo = data ? data.ativo: true
        this.filial = data ? data.filial: []
        this.cliente = data? data.cliente: []
        this.tipoFornecimento = data? data.tipoFornecimento?? []: []
        this.usuarioSistemaPerfil = data? data.usuarioSistemaPerfil: []
    }
}


export const useShareUsuarioStore = defineStore("shareUsuario", {
  
    actions: {

        async add (data) {

            try {
                let clientes = data.cliente.map(function (el) { return el.value; });
                data.cliente = [];
                
                let response = await userService.create(data);
                

                data.id = response.data.id;
                
                await usuarioService.createUpdate(data)
            
                // relacionar clientes x usuario
                if (clientes.length > 0){
                    let userClientes = {
                        usuarioId: response.data.id,
                        clienteIds: clientes
                    }
                    await clienteService.RelacionarClienteUsuario(userClientes);
                }
                
            } catch (error) {
                throw error
            }
        },

        async getById (id) {
            try {
                let response = await userService.getById(id);
                let data = response.data;
                
                response = await clienteService.getListByUser(data.id)
                
                if (response.data.length > 0)
                    data.cliente = response.data.map( item => {return { text: item.nome, value: item.id}})
                else
                    data.cliente = []
                
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
        
        async enabledDisabled (data) {
            await userService.update(data);
        },
        async update (data) {
                debugger
                let clientes = data.cliente.map(function (el) { return el.value; });
                data.cliente = [];
                
                await userService.update(data);

                await usuarioService.createUpdate(data)
            
                // relacionar clientes x usuario
                let userClientes = {
                    usuarioId: data.id,
                    clienteIds: clientes
                }
                await clienteService.RelacionarClienteUsuario(userClientes);
        },

        async toComboList(filial =[]) {
            let response = await userService.toList(filial)
            return response.data;
        },

        /**
         * Retorna uma lista de filiais do usuário
         * objeto: {text: string, value: number }
        */
        async getFiliais () {
            
           let response = await filialService.getListByAuthenticatedUser()
           return response.data;
           
        },

        async getListByCliente(clienteId) {
            let response = await userService.toList()
            let usuarios = response.data;
            
            response = await clienteService.getListUsersByCliente(clienteId);

            let usersCliente = response.data;


            let list = usuarios.filter(user => 
                {return usersCliente.includes(user.value) }
            )

            return list;
        },

        async reembolsoToListByClientePerfil(clienteId, perfilId) {
            let response = await userService.toListByPerfil(perfilId)
            let usuarios = response.data;
            
            response = await clienteService.getListUsersByCliente(clienteId);

            let usersCliente = response.data;


            let list = usuarios.filter(user => 
                {return usersCliente.includes(user.value) }
            )

            return list;
        },

        async getUsuarioToNotificacaoByCliente(clienteId, permissao) {
            if (!permissao || permissao.trim() =="")
            throw new TypeError("A permissão deve ser informada!")
        
            let response = await userService.toListByPermissao(permissao)
            let usuarios = response.data;
            
            response = await clienteService.getListUsersByCliente(clienteId);

            let usersCliente = response.data;


            let list = usuarios.filter(user => 
                {return usersCliente.includes(user.value) }
            )

            return list;
        }

    },
});
