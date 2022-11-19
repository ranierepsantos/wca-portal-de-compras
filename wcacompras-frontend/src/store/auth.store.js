import { defineStore } from 'pinia'
import authService from '@/services/auth.service'

const modelUser = {
    id: 0,
    nome: '',
    perfil: {
        id: 0,
        nome: '',
        permissoes: []
    }
}


export const useAuthStore = defineStore('auth', {
    state: () => ({
        user: JSON.parse(localStorage.getItem("user"))|| {...modelUser},
        token: localStorage.getItem("token") || "",
        expireIn: localStorage.getItem("expireIn") || "",
    }),
    getters: {
        isAuthenticated: (state) =>
        {
            if (state.token == "" || state.expireIn == "")
            {
                return false;
            }
            if (Date.now() > state.expireIn) return false
            return true;
        }
    },
    actions: {
        async authenticate(credenciais)
        {
            let response = await authService.login(credenciais);
            console.log('response.data', response.data);
            if (response.data.authenticated == true)
            {
                this.user.id = response.data.usuarioId;
                this.user.nome = response.data.usuarioNome;
                this.user.perfil.id = response.data.perfil.id
                this.user.perfil.nome = response.data.perfil.nome
                this.user.perfil.permissoes = response.data.perfil.permissoes
                
                let expiration = (new Date(response.data.expiration)).getTime()
                console.log("expiration", expiration)
                localStorage.setItem("user", JSON.stringify(this.user))
                localStorage.setItem("token", response.data.accessToken)
                localStorage.setItem("expireIn", expiration)

            }
            return { authenticated: response.data.authenticated, message: response.data.message }
        }
    },
})