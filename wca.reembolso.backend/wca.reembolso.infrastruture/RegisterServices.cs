using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using wca.compras.domain.Security;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;
using wca.reembolso.infrastruture.Context;
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

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IRepository<ContaCorrente>, BaseRepository<ContaCorrente>>();
            services.AddScoped<IRepository<Despesa>, BaseRepository<Despesa>>();
            services.AddScoped<IRepository<Faturamento>, BaseRepository<Faturamento>>();
            services.AddScoped<IRepository<FaturamentoItem>, BaseRepository<FaturamentoItem>>();
            services.AddScoped<IRepository<Notificacao>, BaseRepository<Notificacao>>();
            services.AddScoped<IRepository<Solicitacao>, BaseRepository<Solicitacao>>();
            services.AddScoped<IRepository<SolicitacaoHistorico>, BaseRepository<SolicitacaoHistorico>>();
            services.AddScoped<IRepository<StatusSolicitacao>, BaseRepository<StatusSolicitacao>>();
            services.AddScoped<IRepository<TipoDespesa>, BaseRepository<TipoDespesa>>();
            services.AddScoped<IRepository<Transacao>, BaseRepository<Transacao>>();
            services.AddScoped<IRepository<UsuarioClientes>, BaseRepository<UsuarioClientes>>();



        }
    }
}
