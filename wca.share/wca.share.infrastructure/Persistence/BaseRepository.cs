using Microsoft.EntityFrameworkCore;
using wca.share.application.Contracts.Persistence;
using wca.share.infrastruture.Context;

namespace wca.share.infrastruture.Persistence
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {

        protected readonly WcaContext _context;

        public BaseRepository(WcaContext context)
        {
            _context = context;
        }

        public void Attach(T entity) => _context.Set<T>().Attach(entity);

        public void Create(T entity) => _context.Set<T>().Add(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public IQueryable<T> ToQuery()
        {
           return _context.Set<T>().AsNoTracking();
        }

        public async Task<int> ExecuteCommandAsync(string command)
        {
            int result = await _context.Database.ExecuteSqlRawAsync(command);
            return result;
        }

    }
}
