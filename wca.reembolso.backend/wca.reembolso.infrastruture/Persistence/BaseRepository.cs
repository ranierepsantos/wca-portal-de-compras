using Microsoft.EntityFrameworkCore;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.infrastruture.Context;

namespace wca.reembolso.infrastruture.Persistence
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {

        protected readonly WcaReembolsoContext _context;

        public BaseRepository(WcaReembolsoContext context)
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
