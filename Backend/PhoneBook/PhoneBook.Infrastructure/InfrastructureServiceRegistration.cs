using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhoneBook.Infrastructure.Context;
using PhoneBook.Infrastructure.IRepositories;
using PhoneBook.Infrastructure.Repositories;

namespace PhoneBook.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PhoneBookDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString"),
                    builder =>
                    {
                        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);

                        builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                    })
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging();
            });

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped(typeof(IPhoneBookRepository<>), typeof(PhoneBookRepository<>));


            return services;
        }
    }
}
