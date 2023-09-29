import { defineStore } from "pinia";
import userService from "@/services/user.service";
import reembolsoUsuarioService from "@/services/reembolso/usuario.service"
import clienteService from "@/services/reembolso/cliente.service";
import filialService from "@/services/filial.service";

export const IDPERFILCOLABORADOR = process.env.VUE_APP_REEM_PERFILCOLABORADOR
export const IDPERFILGESTOR = process.env.VUE_APP_REEM_PERFILGESTOR

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
        this.usuarioReembolsoComplemento = data? data.usuarioReembolsoComplemento ?? new UsuarioReembolsoComplemento() : new UsuarioReembolsoComplemento()
    }
}

class UsuarioReembolsoComplemento {
    constructor(data = undefined) {
        this.usuarioId = data? data.usuarioId: 0;
        this.gestorId = data? data.gestorId: null;
        this.cargo = data? data.cargo: "";
    }
}

export const useUsuarioStore = defineStore("usuario", {
  
    actions: {

        async add (data) {

            try {
                let clientes = data.cliente.map(function (el) { return el.value; });
                data.cliente = [];
                
                let response = await userService.create(data);
                

                data.id = response.data.id;
                
                await reembolsoUsuarioService.createUpdate(data)
            
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

                await reembolsoUsuarioService.createUpdate(data)
            
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
