using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using wca.share.application.Contracts.Integration.Email;
using wca.share.application.Contracts.Integration.GI;
using wca.share.application.Contracts.Persistence;
using wca.share.infrastructure.Integration.Email;
using wca.share.infrastructure.Integration.GI;
using wca.share.infrastruture.Context;
using wca.share.infrastruture.Persistence;

namespace wca.share.infrastructure
{
    public static class RegisterServices
    {
        public static void ConfigureInfraStructure(this IServiceCollection services,
                                                        IConfiguration configuration)
        {
            services.AddDbContext<WcaContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IIntegrationGI, IntegrationGI>();
            services.AddSingleton<IEmailService, EmailService>();
            //Configuração do e-mail
            services.AddSingleton(serviceProvider =>
            {   
                EmailConfiguration emailConfiguration = new EmailConfiguration()
                {
                    From = configuration["EmailConfiguration:From"],
                    Password = configuration["EmailConfiguration:Password"],
                    SmtpServer = configuration["EmailConfiguration:SmtpServer"],
                    Port = int.Parse(configuration["EmailConfiguration:Port"]),
                    UserName = configuration["EmailConfiguration:UserName"]
                };
                return emailConfiguration;
            });
        }
    }
}
