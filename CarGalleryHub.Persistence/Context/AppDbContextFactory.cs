using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Path.Join(Directory.GetCurrentDirectory(), "../CarGalleryHub.Api"))
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsbuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsbuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new AppDbContext(optionsbuilder.Options);
        }
    }
}
