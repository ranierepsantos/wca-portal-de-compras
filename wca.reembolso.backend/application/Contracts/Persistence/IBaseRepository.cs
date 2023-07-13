using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Contracts.Persistence
{
    public interface IBaseRepository <T> where T : class
    {
        IQueryable<T> ToQuery ();
        Task<T> Add (T data);
        Task<T> Update (T data);
        Task<T> Delete (T data);
        Task<T> Attach(T data);
    }
}
