﻿namespace wca.share.application.Contracts.Persistence
{
    public interface IRepository<T> where T : class
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Attach(T entity);
        IQueryable<T> ToQuery();
        IQueryable<T> FromQuery(string query);
        Task<int> ExecuteCommandAsync(string command);

    }
}
