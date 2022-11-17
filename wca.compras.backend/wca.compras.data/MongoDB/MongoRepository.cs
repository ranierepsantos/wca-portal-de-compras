using MongoDB.Driver;
using System.Linq.Expressions;
using wca.compras.domain.Interfaces;


namespace wca.compras.data.MongoDB
{

    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        
        protected readonly IMongoCollection<T> dbCollection;
        protected readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            dbCollection = database.GetCollection<T>(collectionName);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }
        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity));
            }
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, entity.Id);

            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(string id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }

        public async Task<(int totalPages, IReadOnlyList<T> data)> Paginate(int page, int pageSize, FilterDefinition<T> filter, SortDefinition<T> sort)
        {
            return await dbCollection.AggregateByPage(filter, sort, page, pageSize);
        }
            
    }
}