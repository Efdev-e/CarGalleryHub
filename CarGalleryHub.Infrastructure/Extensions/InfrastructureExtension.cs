using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Application.Interfaces.Payment;
using CarGalleryHub.Infrastructure.Options;
using CarGalleryHub.Infrastructure.Services;
using CarGalleryHub.Infrastructure.Services.Payment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CarGalleryHub.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.Configure<IyzicoOptions>(configuration.GetSection(IyzicoOptions.SectionName));

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IPaymentProviderFactory, PaymentProviderFactory>();

            return services;
        }
    }
}
