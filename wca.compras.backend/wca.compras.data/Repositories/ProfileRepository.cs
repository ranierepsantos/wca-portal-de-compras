using MongoDB.Bson;
using MongoDB.Driver;
using wca.compras.data.MongoDB;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces.Repositories;

namespace wca.compras.data.Repositories
{
    public class ProfileRepository: MongoRepository<Perfil>, IProfileRepository
    {

        public ProfileRepository(IMongoDatabase database, string collectionName) : base(database, collectionName)
        {
        }

        public async Task<ProfilePermissions> GetWithPermissionsById(string id)
        {
            var profiles = await dbCollection.Aggregate()
                    .Match(r => r.Id.Equals(id))
                     .Lookup("profile_has_permissions", "id", "profile_id", "profile_has_permissions")
                     .Project(new BsonDocument{
                        {"id", 1},
                        {"name", 1},
                        {"description", 1 },
                        {"active",1 },
                        {"profile_has_permissions.permission_id", 1}
                     })
                     .Lookup("permissions", "profile_has_permissions.permission_id", "id", "Permissions")
                     .Project<ProfilePermissions>(new BsonDocument{
                        {"id", 1},
                        {"name", 1},
                        {"description", 1 },
                        {"active",1 },
                        {"Permissions", 1}
                     }).FirstOrDefaultAsync();

            return profiles;
        }
    }
}
