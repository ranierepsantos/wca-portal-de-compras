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
        this.celular = data? data.celular: null
        this.filial = data ? data.filial: []
        this.cliente = data? data.cliente: []
        this.usuarioSistemaPerfil = data? data.usuarioSistemaPerfil: []
        this.usuarioConfiguracoes = data? data.usuarioConfiguracoes: []
        this.centroCusto = data? data.centroCusto: []
    }
}


export const useShareUsuarioStore = defineStore("shareUsuario", {
  
    actions: {

        async add (data) {

            try {
                let clientes = data.cliente.map(function (el) { return el.value; });
                data.cliente = [];

                let centrosDeCusto = data.centroCusto.map(function (el) { return el.value; });
                data.centroCusto = []

                let response = await userService.create(data);
                data.id = response.data.id;
                
                let configuracoes = data.usuarioConfiguracoes[0]
                delete data.usuarioConfiguracoes
                data.usuarioConfiguracoes = configuracoes

                await usuarioService.createUpdate(data)
            
                // relacionar clientes x usuario
                if (clientes.length > 0){
                    let userClientes = {
                        usuarioId: response.data.id,
                        clienteIds: clientes
                    }
                    await clienteService.RelacionarClienteUsuario(userClientes);
                }
                
                // relacionar usuario x centros de custos
                if (centrosDeCusto.length > 0) {
                    
                    await usuarioService.relacionarUsuarioCentroCusto({
                        usuarioId: response.data.id,
                        centroCustoIds: centrosDeCusto
                    });
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
                    data.cliente = response.data.map( item => {return { text: (item.codigoCliente ? item.codigoCliente + " - ": "") + item.nome, value: item.id}})
                else
                    data.cliente = []

                response = await usuarioService.getCentrosdeCusto(id)

                data.centroCusto = response.data.map(p => ({value:  p.id, text: p.nome, clienteId: p.clienteId}));
    
                let usuario = new Usuario(data);
                return usuario;

            } catch (error) {
                throw error
            }
        },
        async getCentrosdeCusto (usuarioId, clienteId = 0) {
            let response = await usuarioService.getCentrosdeCusto(usuarioId, clienteId);
            return response.data
        },
        async getPaginate(pageNumber = 1, pageSize = 10, filter = "") {
            let response = await userService.paginate(pageSize, pageNumber, filter)
            return response.data
        },
        
        async enabledDisabled (data) {
            await userService.update(data);
        },
        async update (data) {
                
                let clientes = data.cliente.map(function (el) { return el.value; });
                data.cliente = [];
                
                let centrosDeCusto = data.centroCusto.map(function (el) { return el.value; });
                data.centroCusto = []


                await userService.update(data);

                let configuracoes = data.usuarioConfiguracoes[0]
                delete data.usuarioConfiguracoes
                data.usuarioConfiguracoes = configuracoes

                await usuarioService.createUpdate(data)
            
                // relacionar clientes x usuario
                if (clientes.length > 0){
                    let userClientes = {
                        usuarioId: data.id,
                        clienteIds: clientes
                    }
                    await clienteService.RelacionarClienteUsuario(userClientes);
                }

                // relacionar usuario x centros de custos
                if (centrosDeCusto.length > 0) {
                    
                    await usuarioService.relacionarUsuarioCentroCusto({
                        usuarioId: data.id,
                        centroCustoIds: centrosDeCusto
                    });
                }
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

        async toListByClientePerfil(clienteId, perfilId) {
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
