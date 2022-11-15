using Microsoft.Extensions.DependencyInjection;
using wca.compras.data.MongoDB;
using wca.compras.domain.Entities;

namespace wca.compras.crosscutting.DependencyInjection
{
    public static class ConfigureRepository
    {
        public static void ConfigureDependencyRepository(this IServiceCollection services)
        {
            services.AddMongo();
            services.AddMongoProfileRepository("profiles");
            services.AddMongoRepository<Permissao>("permissions");
            services.AddMongoRepository<PerfilRelPermissoes>("profile_has_permissions");
        }
    }
}
