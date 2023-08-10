using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace wca.reembolso.application.Contracts.Persistence
{
    public interface IRepository<T> where T : class
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Attach(T entity);
        IQueryable<T> ToQuery();
        Task SaveChangesAsync();
        Task<int> ExecuteCommandAsync(string command);
        
    }
}
