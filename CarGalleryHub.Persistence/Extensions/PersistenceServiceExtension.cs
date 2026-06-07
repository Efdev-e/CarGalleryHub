using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CarGalleryHub.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using CarGalleryHub.Persistence.UnitOfWork;

namespace CarGalleryHub.Persistence.Extensions
{
    public static class PersistenceServiceExtension
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            
            return services;
        }
    }
}
