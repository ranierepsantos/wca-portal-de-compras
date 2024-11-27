import { defineStore } from "pinia";
import api from "@/services/share/shareApi";

const rotas = {
  Create: "Assunto",
  Update: "Assunto",
  GetById: "Assunto",
  Paginar: "Assunto/Paginar",
};

export class Assunto {
  constructor(data = null) {
    this.id = data ? data.id : 0;
    this.nome = data ? data.nome : "";
    this.ativo = data ? data.ativo : true;
  }
}

export const useShareAssuntoStore = defineStore("shareAssunto", {
  actions: {
    async add(data) {
      try {
        let response = await api.post(rotas.Create, data);
        return new Assunto(response.data);
      } catch (error) {
        throw error;
      }
    },
    async update(data) {
      try {
        let response = await api.put(rotas.Update, data);
        return new Assunto(response.data);
      } catch (error) {
        throw error;
      }
    },
    async getById(id) {
      try {
        let response = await api.get(rotas.GetById, { params: { id: id } });
        return new Assunto(response.data);
      } catch (error) {
        throw error;
      }
    },
    async getPaginate(page = 1, pageSize = 10, filters) {
      try {
        let parametros = {
          params: {
            page: page,
            pageSize: pageSize,
            termo: filters,
          },
        };
        let response = await api.get(rotas.Paginar, parametros);
        return response.data;
      } catch (error) {
        throw error;
      }
    },
  },
});
