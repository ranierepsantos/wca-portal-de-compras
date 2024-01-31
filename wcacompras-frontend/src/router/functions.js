import { useAuthStore } from "@/store/auth.store";

export const protectRoute = async function (to, from, next)
{
  
  let authStore = useAuthStore();
  if (!authStore.isAuthenticated()) next({ name: 'login' })

  //checa se a rota que esta acesso é do sistema atual que esta
  if (to.meta.sistema != undefined && to.meta.sistema != authStore.sistema.id ) {
    //não é o mesmo sistema, checar se o usuario tem acesso ao sistema
    let sistema = authStore.user.sistemas.filter(c => c.id == to.meta.sistema)[0];
    
    if (sistema != undefined)
      await authStore.setSistema(sistema)
    else 
      next({name: "acessonegado"})
  }

  if (to.meta.permissao != undefined && !authStore.hasPermissao(to.meta.permissao)) 
    next({name: "acessonegado"})
  else 
    next()
}

