using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using wca.reembolso.application.Contracts.Integration.NorgeChatBot;
using wca.reembolso.application.Contracts.Integration.WcaCompras;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.infrastruture.Context;
using wca.reembolso.infrastruture.Integration.NorgeChatBot;
using wca.reembolso.infrastruture.Integration.WcaCompras;
using wca.reembolso.infrastruture.Persistence;

namespace wca.reembolso.infrastruture
{
    public static class RegisterServices
    {
        public static void ConfigureInfraStructure(this IServiceCollection services,
                                                   IConfiguration configuration)
        {


            ////Configuração para utilização de token JWT
            //var signingConfiguration = new SigningConfiguration();
            //services.AddSingleton(signingConfiguration);

            //TokenConfiguration tokenConfiguration = new TokenConfiguration();
                
            //new ConfigureFromConfigurationOptions<TokenConfiguration>
            //(
            //    configuration.GetSection("TokenConfigurations")
            //).Configure(tokenConfiguration);
             
            //services.AddSingleton(tokenConfiguration);

            ////Serviços de autenticação
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    var paramsValidation = options.TokenValidationParameters;
            //    paramsValidation.IssuerSigningKey = signingConfiguration.Key;
            //    paramsValidation.ValidAudience = tokenConfiguration.Audience;
            //    paramsValidation.ValidIssuer = tokenConfiguration.Issuer;
            //    paramsValidation.ValidateIssuerSigningKey = true;
            //    paramsValidation.ValidateLifetime = true;
            //    paramsValidation.ClockSkew = TimeSpan.Zero;
            //});

            services.AddDbContext<WcaReembolsoContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IIntegrationNorgeChatBot, IntegrationNorgeChatBot>();
            services.AddScoped<IIntegrationWcaCompras, IntegrationWcaCompras>();
            

        }
    }
}
