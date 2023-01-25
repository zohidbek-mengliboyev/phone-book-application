using Microsoft.EntityFrameworkCore;
using PhoneBook.Domain;
using PhoneBook.Infrastructure.Context;
using PhoneBook.Infrastructure.IRepositories;
using System.Linq.Expressions;

namespace PhoneBook.Infrastructure.Repositories
{
    public class PhoneBookRepository<T> : RepositoryBase<T>, IPhoneBookRepository<T> where T : EntityBase
    {
        public PhoneBookRepository(PhoneBookDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<T> AddAsync<T>(T entity) where T : EntityBase
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync<T>() where T : EntityBase
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, string includString = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includString))
            {
                foreach (var item in includString.Split(";"))
                {
                    query = query.Include(item);
                }
            };

            if (predicate != null) query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T1> GetFirstOrDefaultAsync<T1>(Expression<Func<T1, bool>> predicate, string includString = null) where T1 : EntityBase
        {
            IQueryable<T1> query = _dbContext.Set<T1>().AsNoTracking().Where(predicate);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAsync<T>(T entity) where T : EntityBase
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<T> FindAsync(Guid id)
         => await _dbContext.Set<T>().FindAsync(id);
    }
}
