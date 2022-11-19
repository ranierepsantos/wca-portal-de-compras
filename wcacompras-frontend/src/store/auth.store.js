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
    actions: {
        async authenticate(credenciais)
        {
            let response = await authService.login(credenciais);
            console.log('response.data', response.data);
            if (response.data.authenticated == true)
                this.startSession(response.data);
            
            return { authenticated: response.data.authenticated, message: response.data.message }
        },
        
        finishSession()
        {
            window.localStorage.clear();
            this.token = ""
            this.expireIn =""
            this.user = { ...modelUser };
            
        },

        isAuthenticated()
        {
            if (this.token == "" || this.expireIn == "" || Date.now() > this.expireIn)
            {
                this.finishSession();
                return false;
            }
            
            return true;
        },

        startSession(auth)
        {
            this.user.id = auth.usuarioId;
            this.user.nome = auth.usuarioNome;
            this.user.perfil.id = auth.perfil.id
            this.user.perfil.nome = auth.perfil.nome
            this.user.perfil.permissoes = auth.perfil.permissoes
            let expiration = (new Date(auth.expiration)).getTime()
            this.token = auth.accessToken
            this.expireIn = expiration

            localStorage.setItem("user", JSON.stringify(this.user))
            localStorage.setItem("token", this.token)
            localStorage.setItem("expireIn", this.expireIn)
        },
        
    },
})