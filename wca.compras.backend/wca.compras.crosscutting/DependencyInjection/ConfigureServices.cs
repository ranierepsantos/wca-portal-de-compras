using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using wca.compras.crosscutting.Mapping;
using wca.compras.domain.Interfaces.Services;
using wca.compras.services;

namespace wca.compras.crosscutting.DependencyInjection
{
    public static class ConfigureServices
    {
        public static void ConfigureDependencyService(this IServiceCollection services)
        {
            services.AddTransient<IPerfilService, PerfilService>();
            services.AddTransient<IPermissaoService, PermissaoService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IUsuarioService, UsuarioService>();


            var autoMapperConfig = new MapperConfiguration(
                config =>
                {
                    config.AddProfile(new PerfilProfile());
                    config.AddProfile(new PermissaoProfile());
                    config.AddProfile(new UsuarioProfile());
                }
             );
            IMapper mapper = autoMapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
