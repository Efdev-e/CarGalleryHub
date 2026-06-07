using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CarGalleryHub.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T item);
        void Remove(T item);
        void Update(T item);
        IQueryable<T> Query();
    }
}
