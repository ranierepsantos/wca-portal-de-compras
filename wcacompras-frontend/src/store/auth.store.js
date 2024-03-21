import { defineStore } from "pinia";
import authService from "@/services/auth.service";
import apiReembolso from "@/services/reembolso/api"
import filialService from "@/services/filial.service";

const modelUser = {
  id: 0,
  nome: "",
  sistemas: [],
};

const modelPerfil = {
  id: 0,
  nome: "",
  permissao: [],
};

const modelSistema = {
    id: 0,
    nome: "",
    descricao: "",
    perfil: {...modelPerfil},
    filial: null,
    isMatriz: false
}

export const useAuthStore = defineStore("auth", {
  state: () => ({
    user: JSON.parse(localStorage.getItem("user")) || { ...modelUser },
    sistema: JSON.parse(localStorage.getItem("sistema")) || { ...modelSistema },
    token: localStorage.getItem("token") || "",
    expireIn: localStorage.getItem("expireIn") || "",
  }),
  actions: {
    async authenticate(credenciais) {
      let response = await authService.login(credenciais);
      if (response.data.authenticated == true) this.startSession(response.data);

      return {
        authenticated: response.data.authenticated,
        message: response.data.message,
      };
    },

    async getPerfilSistema () {
        let response = await authService.getPerfilSistema(this.user.id, this.sistema.id);
        this.sistema.perfil = response.data;
    },

    async getFilialSistema () {
      let response = await filialService.getListByAuthenticatedUser()
      this.sistema.filial = response.data[0];
      this.sistema.isMatriz = this.sistema.filial.text.toLowerCase() =="matriz"
    },

    async setSistema (sistema) {
        this.sistema.id = sistema.id;
        this.sistema.nome = sistema.nome 
        this.sistema.descricao = sistema.descricao
        await this.getPerfilSistema()
        this.sistema.filial = null
        if (this.sistema.id == 2 || this.sistema.id == 3) {
          await this.getFilialSistema()
        }
        localStorage.setItem("sistema", JSON.stringify(this.sistema));
    },

    finishSession() {
      window.localStorage.clear();
      this.token = "";
      this.expireIn = "";
      this.user = { ...modelUser };
      this.sistema = { ...modelSistema };
    },

    hasPermissao(permissao) {
      if (permissao == "livre") return true;

      let perm = undefined;
      if (permissao.indexOf("|") > -1) {
        let permissoes = permissao.split("|");

        for (let index = 0; index < permissoes.length; index++) {
          perm = this.sistema.perfil.permissao.filter(
            (p) => p.regra.toLowerCase() == permissoes[index].toLowerCase()
          )[0];
          if (perm != undefined) break;
        }
      } else {
        perm = this.sistema.perfil.permissao.filter(
          (p) => p.regra.toLowerCase() == permissao.toLowerCase()
        )[0];
      }

      if (permissao == "filial") {
        return perm != undefined && this.sistema.isMatriz;
      }
      // if (permissao == "perfil") {
      //   return perm != undefined && this.sistema.isMatriz;
      // }
      return perm != undefined;
    },

    isAuthenticated() {
      if (
        this.token == "" ||
        this.expireIn == "" ||
        Date.now() > this.expireIn
      ) {
        this.finishSession();
        return false;
      }

      return true;
    },

    startSession(auth) {
      this.user.id = auth.usuarioId;
      this.user.nome = auth.usuarioNome;
      this.user.sistemas = auth.sistemas;
      let expiration = new Date(auth.expiration).getTime();
      this.token = auth.accessToken;
      this.expireIn = expiration;

      localStorage.setItem("user", JSON.stringify(this.user));
      localStorage.setItem("token", this.token);
      localStorage.setItem("expireIn", this.expireIn);
    },

    async getNotificacoesReembolso () {

      let response = await apiReembolso.get("Notificacao/ListarPorUsuario", {params: {usuarioId: this.user.id}})
      return response.data

    },

    async marcarNotificaoReembolso(notificacaoId) {
      await apiReembolso.put("Notificacao/MarcarComoLido", {id: notificacaoId})
    }


  },
});
