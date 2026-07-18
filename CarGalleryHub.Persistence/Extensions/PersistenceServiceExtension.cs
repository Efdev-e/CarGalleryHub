using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Persistence.Context;
using CarGalleryHub.Persistence.Services;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CarGalleryHub.Persistence.Extensions
{
    public static class PersistenceServiceExtension
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration) 
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var sqliteConnectionString = configuration.GetConnectionString("SqliteConnection") ?? "Data Source=cargalleryhub.db";

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            return services;
        }
    }
}
