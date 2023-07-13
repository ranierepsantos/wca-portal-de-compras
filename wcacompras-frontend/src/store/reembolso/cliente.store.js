import { defineStore } from "pinia";
import { paginate } from "@/helpers/functions";

export class Cliente {
    idControl = 0

    constructor (data = undefined) {
        if (data ==undefined) {
            this.id= 0
            this.nome= ""
            this.cnpj= ""
            this.inscricaoEstadual= ""
            this.endereco= ""
            this.numero= ""
            this.cep= ""
            this.cidade= ""
            this.uf= ""
            this.ativo= true
            this.filialId= null
            this.naoUltrapassarLimitePorRequisicao= false
            this.valorLimiteRequisicao= 0 
            this.clienteContatos= []
        }else {
            this.id= data.id
            this.nome= data.nome
            this.cnpj= data.cnpj
            this.inscricaoEstadual= data.inscricaoEstadual
            this.endereco= data.endereco
            this.numero= data.numero
            this.cep= data.cep
            this.cidade= data.cidade
            this.uf= data.uf
            this.ativo= data.ativo
            this.filialId= data.filialId
            this.naoUltrapassarLimitePorRequisicao= data.naoUltrapassarLimitePorRequisicao
            this.valorLimiteRequisicao= data.valorLimiteRequisicao 
            this.clienteContatos= data.clienteContatos
        }
        
    }

    salvarContato(contato) {
        let index = -1        
        if (contato.id == 0) {
            this.idControl +=-1
            contato.id = this.idControl
        }else {
            index = this.clienteContatos.findIndex(q =>  q.id ==contato.id) 
        }
        if (index == -1)
            this.clienteContatos.push(contato);
        else 
            this.clienteContatos[index] = contato;
    }

    removerContato(contato) {
        let index = this.clienteContatos.findIndex(c => c.id == contato.id)
        if (index > -1) {
            this.clienteContatos.splice(index, 1);
        }
    }
}

export class ClienteContato {
    
    constructor() {
        this.id= 0,
        this.clienteId= 0,
        this.nome= "",
        this.email= "",
        this.telefone= "",
        this.celular= "",
        this.aprovaPedido= false
    }
}

export const useClienteStore = defineStore("cliente", {
  state: () => ({
    idControl: 0,
    clientes: [],
  }),
  actions: {
    
    addCliente (cliente) {
        debugger
        this.idControl++;
        cliente.id = this.idControl;
        this.clientes.push(cliente)
        console.log("store.clientes:", this.clientes)
    },
    
    getClienteById (id) {
        debugger
        let model = this.clientes.find(c => c.id == id)
        console.log(model)
        
        return new Cliente(model);
    },

    getClientesByPaginate(pageNumber = 1, pageSize = 10) {
        console.log("store.clientes:", this.clientes)
        return paginate(this.clientes, pageNumber, pageSize)
    },

    updateCliente (cliente) {
        let index = this.clientes.findIndex(q => q.id == cliente.id)
        if (index == -1) {
            return false;
        }
        this.clientes[index] = {...cliente};
        return true;
    },
    toComboList() {
        let list = []
        this.clientes.forEach(item => {
            list.push ({text: item.nome, value: item.id})
        })
        return list;
    }
  },
});
