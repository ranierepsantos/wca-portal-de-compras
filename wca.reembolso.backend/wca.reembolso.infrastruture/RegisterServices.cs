using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.infrastruture.Context;
using wca.reembolso.infrastruture.Persistence;

namespace wca.reembolso.infrastruture
{
    public static class RegisterServices
    {
        public static void ConfigureInfraStructure(this IServiceCollection services,
                                                   IConfiguration configuration)
        {
            services.AddDbContext<WcaReembolsoContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IClienteRepository, ClienteRepository>();
            
        }
    }
}
