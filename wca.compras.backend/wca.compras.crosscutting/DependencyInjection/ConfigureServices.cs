using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using wca.compras.crosscutting.Mapping;
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
            services.AddTransient<ITipoFornecimentoService, TipoFornecimentoService>();
            services.AddTransient<IFornecedorService, FornecedorService>();
            services.AddTransient<IRequisicaoService, RequisicaoService>();
            services.AddTransient<IRecorrenciaService, RecorrenciaService>();
            services.AddTransient<IConfiguracaoService, ConfiguracaoService>();

            var autoMapperConfig = new MapperConfiguration(
                config =>
                {
                    config.AddProfile(new PerfilProfile());
                    config.AddProfile(new PermissaoProfile());
                    config.AddProfile(new UsuarioProfile());
                    config.AddProfile(new ClienteProfile());
                    config.AddProfile(new FilialProfile());
                    config.AddProfile(new TipoFornecimentoProfile());
                    config.AddProfile(new FornecedorProfile());
                    config.AddProfile(new RequisicaoProfile());
                    config.AddProfile(new RecorrenciaProfile());

                }
             );
            IMapper mapper = autoMapperConfig.CreateMapper();
            services.AddSingleton(mapper);

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
