using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using wca.compras.data.DataAccess;
using wca.compras.domain.Interfaces;

namespace wca.compras.data.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {

        protected WcaContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(WcaContext context)
        {
           _context= context;
            _dbSet= context.Set<T>();
        }

        public void Create(T entity) => _context.Set<T>().Add(entity);

        public IQueryable<T> SelectAll()=> _context.Set<T>().AsNoTracking();

        public IQueryable<T> SelectByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) => 
            trackChanges? 
                _context.Set<T>().Where(expression): 
                _context.Set<T>().Where(expression).AsNoTracking();
        
        public void Delete(T entity) => _context.Set<T>().Remove(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void Attach(T entity) => _context.Set<T>().Attach(entity);
    }

}
