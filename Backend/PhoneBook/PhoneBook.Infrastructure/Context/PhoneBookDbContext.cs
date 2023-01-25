    using Microsoft.EntityFrameworkCore;
using PhoneBook.Domain.Entities;

namespace PhoneBook.Infrastructure.Context
{
    /// <summary>
    /// 
    /// </summary>
    public class PhoneBookDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public PhoneBookDbContext(DbContextOptions<PhoneBookDbContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Communication> Communications { get; set; }
        public DbSet<ImageContact> ImageContacts { get; set; }
    }
}
