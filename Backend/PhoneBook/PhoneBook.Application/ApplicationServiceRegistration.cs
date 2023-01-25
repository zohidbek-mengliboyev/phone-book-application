using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Application.CommunicationService;
using PhoneBook.Application.ContactService;
using PhoneBook.Application.ImageContactService;
using PhoneBook.Application.Mapping;

namespace PhoneBook.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddMemoryCache();
            services.AddLogging();

            services.AddScoped<IContactService, ContactService.ContactService>();
            services.AddScoped<ICommunicationService, CommunicationService.CommunicationService>();
            services.AddScoped<IContactImageService, ImageContactService.ImageContactService>();

            return services;
        }
    }
}
