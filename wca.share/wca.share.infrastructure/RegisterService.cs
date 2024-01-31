using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using wca.share.application.Contracts.Persistence;
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


        }
    }
}
