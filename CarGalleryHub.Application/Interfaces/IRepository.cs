using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CarGalleryHub.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdIncludedAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T item);
        void Remove(T item);
        void Update(T item);
        IQueryable<T> Query();
    }
}
