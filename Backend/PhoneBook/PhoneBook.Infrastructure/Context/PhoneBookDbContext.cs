using Microsoft.EntityFrameworkCore;

namespace PhoneBook.Infrastructure.Context
{
    public class PhoneBookDbContext : DbContext
    {
        public PhoneBookDbContext(DbContextOptions<PhoneBookDbContext> options) : base(options) { }
    }
}
