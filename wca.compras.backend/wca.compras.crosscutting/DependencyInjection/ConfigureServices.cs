using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using wca.compras.domain.Email;
using wca.compras.domain.Interfaces.Services;
using wca.compras.services;

namespace wca.compras.crosscutting.DependencyInjection
{
    public static class ConfigureServices
    {
        public static void ConfigureDependencyService(this IServiceCollection services)
        {
            services.AddTransient<IPermissaoService, PermissaoService>();
            services.AddTransient<IPerfilService, PerfilService>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IClienteService, ClienteService>();
            services.AddTransient<IFilialService, FilialService>();
            services.AddTransient<ITipoFornecimentoervice, TipoFornecimentoervice>();
            services.AddTransient<IFornecedorService, FornecedorService>();
            services.AddTransient<IRequisicaoService, RequisicaoService>();
            services.AddTransient<IRecorrenciaService, RecorrenciaService>();
            services.AddTransient<IConfiguracaoService, ConfiguracaoService>();
            
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //Configuração do e-mail
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
                return emailConfig;
            });
        }
    }
}
