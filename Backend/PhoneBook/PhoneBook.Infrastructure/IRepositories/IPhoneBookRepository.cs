using PhoneBook.Domain;
using System.Linq.Expressions;

namespace PhoneBook.Infrastructure.IRepositories
{
    public interface IPhoneBookRepository<T> : IAsyncRepository<T> where T : EntityBase
    {
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, string includString = null);
        Task<T1> GetFirstOrDefaultAsync<T1>(Expression<Func<T1, bool>> predicate, string includString = null) where T1 : EntityBase;
        Task<T> AddAsync<T>(T entity) where T : EntityBase;
        Task<int> UpdateAsync<T>(T entity) where T : EntityBase;
        Task<IReadOnlyList<T>> GetAllAsync<T>() where T : EntityBase;
        Task<T> FindAsync(int id);
    }
}
