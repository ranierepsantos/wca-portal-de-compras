using System.Linq.Expressions;

namespace wca.compras.domain.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T> CreateAsync(T entity);
        Task<T> GetAsync(string id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task RemoveAsync(string id);
        Task UpdateAsync(T entity);
    }
}
