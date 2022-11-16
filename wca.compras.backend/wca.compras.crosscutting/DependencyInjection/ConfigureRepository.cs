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
            services.AddMongoPerfilRepository("perfil");
            services.AddMongoRepository<Permissao>("permissoes");
            services.AddMongoRepository<PerfilRelPermissoes>("perfil_rel_permissoes");
            services.AddMongoRepository<Usuario>("usuarios");
        }
    }
}
