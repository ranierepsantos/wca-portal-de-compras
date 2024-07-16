import { defineStore } from "pinia";
import api from "@/services/share/shareApi";

const rotas = {
  Create: "{Entidade}",
  Update: "{Entidade}",
  GetById: "{Entidade}",
  Paginar: "{Entidade}/Paginar",
  ToComboList: "{Entidade}/ToComboList",
};

export class EntidadeAuxiliar {
  constructor(data = null) {
    this.id = data ? data.id : 0;
    this.nome = data ? data.nome : "";
    this.ativo = data ? data.ativo : true;
  }
}

export const useShareEntidadeAuxiliarStore = defineStore("shareEntidadeAuxiliar", {
  actions: {
    async add(entidade, data) {
      try {
        let response = await api.post(rotas.Create.replace('{Entidade}', entidade), data);
        return new EntidadeAuxiliar(response.data);
      } catch (error) {
        throw error;
      }
    },
    async update(entidade, data) {
      try {
        let response = await api.put(rotas.Update.replace('{Entidade}', entidade), data);
        return new EntidadeAuxiliar(response.data);
      } catch (error) {
        throw error;
      }
    },
    async getById(entidade, id) {
      try {
        let response = await api.get(rotas.GetById.replace('{Entidade}', entidade), { params: { id: id } });
        return new EntidadeAuxiliar(response.data);
      } catch (error) {
        throw error;
      }
    },
    async getPaginate(entidade, page = 1, pageSize = 10, filters) {
      try {
        let parametros = {
          params: {
            page: page,
            pageSize: pageSize,
            termo: filters,
          },
        };
        let response = await api.get(rotas.Paginar.replace('{Entidade}', entidade), parametros);
        return response.data;
      } catch (error) {
        throw error;
      }
    },
    async getToComboList(entidade) {
      try {
        let response = await api.get(rotas.ToComboList.replace('{Entidade}', entidade), { params: { id: id } });
        return response.data;
      } catch (error) {
        throw error;
      }
    }
  },
});
