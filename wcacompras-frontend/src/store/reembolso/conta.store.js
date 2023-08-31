import { defineStore } from "pinia";
import moment from "moment";
import api from "@/services/reembolso/api";

const rotas = {
    AddTransacao: "ContaCorrente",
    GetById: "ContaCorrente?UsuarioId={id}"
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
}

export class Transacao {
    constructor(descricao, operador = "+", valor){
        this.dataHora = moment().format("YYYY-MM-DDTHH:mm:ss")
        this.descricao = descricao
        this.operador = operador
        this.valor = valor 
    }
}


export const useContaStore = defineStore("contaCorrente", {
    actions: {
      
        async addTransacao (usuarioId, transacao) {
            try {
                let conta = new ContaCorrente();
                conta.usuarioId = usuarioId;
                conta.adicionarTransacao(transacao);

                let response = await api.post(rotas.AddTransacao, conta);

                console.log(response);

            } catch (error) {
                throw error
            }  
        },
      
        async getByUsuarioId (usuarioId) {
            let response = await api.get (rotas.GetById.replace("{id}", usuarioId));
            return new ContaCorrente(response.data);
        }
    },
  });
  