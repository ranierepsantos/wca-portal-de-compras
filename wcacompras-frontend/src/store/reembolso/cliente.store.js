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
            this.naoUltrapassarLimite= false
            this.valorLimite = 0 
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
            this.naoUltrapassarLimite= data.naoUltrapassarLimite
            this.valorLimite= data.valorLimite 
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
    clientes: JSON.parse(localStorage.getItem("reembolso-clientes")) || [],
  }),
  actions: {
    
    addCliente (cliente) {
        this.idControl++;
        cliente.id = this.idControl;
        this.clientes.push(cliente)
        localStorage.setItem("reembolso-clientes", JSON.stringify(this.clientes))
    },
    
    getClienteById (id) {
        let model = this.clientes.find(c => c.id == id)
        return new Cliente(model);
    },

    getClientesByPaginate(pageNumber = 1, pageSize = 10) {
        return paginate(this.clientes, pageNumber, pageSize)
    },

    updateCliente (cliente) {
        let index = this.clientes.findIndex(q => q.id == cliente.id)
        if (index == -1) {
            return false;
        }
        this.clientes[index] = {...cliente};
        localStorage.setItem("reembolso-clientes", JSON.stringify(this.clientes))
        return true;
    },
    toComboList(filial = 0)  {

        let list = []
        let lista = this.clientes
        if (filial != 0){
            lista = lista.filter(q => q.filialId == filial)
        }
        lista.forEach(item => {
            list.push ({text: item.nome, value: item.id})
        })
        return list;
    }
  },
});
