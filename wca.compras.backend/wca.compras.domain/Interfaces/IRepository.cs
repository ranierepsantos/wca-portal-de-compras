using System.Linq.Expressions;

namespace wca.compras.domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> SelectAll();
        IQueryable<T> SelectByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);
        public void Attach(T entity);
    }
}
