using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using wca.compras.data.DataAccess;
using wca.compras.data.Repositories;
using wca.compras.domain.Interfaces;

namespace wca.compras.crosscutting.DependencyInjection
{
    public static class ConfigureRepository
    {
        public static void ConfigureDependencyRepository(this IServiceCollection services, IConfiguration configuration)
        {
            int timeOut = configuration.GetValue<int?>("ConnectionTimeout") ?? 180;

            services.AddDbContext<WcaContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.CommandTimeout(timeOut) // Define um timeout
                )
            );

            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
    }
}
