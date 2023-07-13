using application.Contracts.Persistence;

namespace Data.Persistence.Common
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private IList<T> _data = new List<T>();

        public Task<T> Add(T data)
        {
            _data.(data);
            return Task.FromResult(data);
        }

        public Task<T> Attach(T data)
        {
            throw new NotImplementedException();
        }

        public Task<T> Delete(T data)
        {
            var remove = _data.FirstOrDefault(d => d.Equals(data));

            if (remove != null)
            {
                _data.Remove(remove);
            }
            return Task.FromResult(data);
        }

        public IQueryable<T> ToQuery()
        {
            return _data.AsQueryable();
        }

        public Task<T> Update(T data)
        {
            var dado = _data.IndexOf(data);
            if (dado != -1)
            {
                _data[dado] = data;
            }
            return Task.FromResult(data);
        }
    }
}
