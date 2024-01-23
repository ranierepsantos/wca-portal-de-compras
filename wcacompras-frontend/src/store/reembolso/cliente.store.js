import { defineStore } from "pinia";
import clienteService from "@/services/reembolso/cliente.service";

export class Cliente {
    clienteID = 0

    constructor (data = undefined) {
        if (data ==undefined) {
            this.id= 0
            this.filialId= null
            this.nome= ""
            this.cnpj= ""
            this.inscricaoEstadual= ""
            this.endereco= ""
            this.numero= ""
            this.cep= ""
            this.cidade= ""
            this.uf= ""
            this.ativo= true
            this.valorLimite = 0,
            this.codigoCliente = ""
            this.centroCusto = []
        }else {
            this.id= data.id
            this.filialId= data.filialId
            this.nome= data.nome
            this.cnpj= data.cnpj
            this.inscricaoEstadual= data.inscricaoEstadual
            this.endereco= data.endereco
            this.numero= data.numero
            this.cep= data.cep
            this.cidade= data.cidade
            this.uf= data.uf
            this.ativo= data.ativo
            this.valorLimite= data.valorLimite,
            this.codigoCliente = data.codigoCliente,
            this.centroCusto = data.centroCusto
        }
    }
}

export const useClienteStore = defineStore("cliente", {
  actions: {
    async addCliente (cliente) {
        let response = await clienteService.create(cliente);
        return response.data;
    },
    
    async getClienteById (id) {
        let model = (await clienteService.getById(id)).data;
        return new Cliente(model);
    },

    async getClientesByPaginate(filial, pageNumber = 1, pageSize = 10, termo ="") {
        let response  = await clienteService.paginate(filial, pageSize, pageNumber, termo)
        return response.data;
    },

    async updateCliente (cliente) {
        let response = await clienteService.update(cliente);
        return response.data;
    },
    
    async toComboList(filial = 0, usuarioId = 0)  {
        let response = await clienteService.toList(filial, usuarioId);
        return response.data;
    },

    async ListByUsuario(usuarioId) {
        let response = await clienteService.getListByUser(usuarioId)
        return response.data;
    }
  },
});
