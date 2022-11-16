using MongoDB.Bson;
using MongoDB.Driver;
using wca.compras.data.MongoDB;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces.Repositories;

namespace wca.compras.data.Repositories
{
    public class PerfilRepository: MongoRepository<Perfil>, IPerfilRepository
    {

        public PerfilRepository(IMongoDatabase database, string collectionName) : base(database, collectionName)
        {
        }

        public async Task<PerfilPermissoes> GetWithPermissoesByIdAsync(string id)
        {
            var data = await dbCollection.Aggregate()
                    .Match(r => r.Id.Equals(id))
                     .Lookup("perfil_rel_permissoes", "_id", "perfil_id", "perfil_rel_permissoes")
                     .Project(new BsonDocument{
                        {"_id", 1},
                        {"nome", 1},
                        {"descricao", 1 },
                        {"ativo",1 },
                        {"perfil_rel_permissoes.permissao_id", 1}
                     })
                     .Lookup("permissoes", "perfil_rel_permissoes.permissao_id", "_id", "Permissoes")
                     .Project<PerfilPermissoes>(new BsonDocument{
                        {"_id", 1},
                        {"nome", 1},
                        {"descricao", 1 },
                        {"ativo",1 },
                        {"Permissoes", 1}
                     }).FirstOrDefaultAsync();

            return data;
        }
    }
}
