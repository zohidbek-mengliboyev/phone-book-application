using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PhoneBook.Domain;

namespace PhoneBook.Infrastructure.IRepositories
{
    public interface IAsyncRepository<T> where T : EntityBase
    {
        IQueryable<T> GetQueryableAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        string includeString = null,
                                        bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                       List<Expression<Func<T, object>>> includes = null,
                                       bool disableTracking = true);
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
        void Detach(T entity);
        EntityEntry Attach(T entity);
    }
}
