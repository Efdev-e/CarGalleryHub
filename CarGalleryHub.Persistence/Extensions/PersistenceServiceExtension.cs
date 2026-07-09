using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CarGalleryHub.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using CarGalleryHub.Persistence.UnitOfWork;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Persistence.Services;

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
                if (!string.IsNullOrWhiteSpace(connectionString) && connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseSqlServer(connectionString);
                }
                else
                {
                    options.UseSqlite(sqliteConnectionString);
                }
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
