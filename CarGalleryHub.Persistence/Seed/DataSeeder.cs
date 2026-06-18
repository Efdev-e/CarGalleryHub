using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CarGalleryHub.Persistence.Seed
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        private readonly string _secretPass;
        public DataSeeder(AppDbContext context, IConfiguration configuration)
        {
            _secretPass = configuration.GetSection("Secrets").GetSection("SecretPass").Value ?? throw new Exception();
            _context = context;
        }

        public async Task SeedAsync() 
        {
            await _context.Database.MigrateAsync();   
            await SeedUsersAsync(_context);
            await SeedBrandAsync(_context);
            await SeedCarModelAsync(_context);
        }

        public async Task SeedUsersAsync(AppDbContext context) 
        {
            if (await context.Users.AnyAsync()) return;

            var Users = new List<User>()
            {
                new User() { FirstName = "Abdul", LastName="Hamid",Role = UserRoles.Admin, PasswordHash = BCrypt.Net.BCrypt.HashPassword(_secretPass), Email="Kingston@gmail.com" , PhoneNumber = "+905487987202"},
                new User() { FirstName = "Test", LastName = "Test2", Role = UserRoles.Customer, PasswordHash = BCrypt.Net.BCrypt.HashPassword("1"), Email = "e@gm", PhoneNumber = "+905852133265" }
            };

            await context.Users.AddRangeAsync(Users);
            await context.SaveChangesAsync();
        }

        public async Task SeedBrandAsync(AppDbContext context) 
        {
            if (await context.Brands.AnyAsync()) return;

            var Brands = new List<Brand>()
            {
                new Brand() {BrandName = "Honda" },
                new Brand() {BrandName = "Toyota"},
                new Brand() {BrandName = "Renault"},
                new Brand() {BrandName = "BMW"},
                new Brand() {BrandName = "Ford"},
                new Brand() {BrandName = "Nissan"},
                new Brand() {BrandName = "Subaru"},
                new Brand() {BrandName = "Volkswagen"},
                new Brand() { BrandName = "Mercedes-Benz" },
                new Brand() { BrandName = "Audi" },
                new Brand() { BrandName = "Hyundai" },
                new Brand() { BrandName = "Kia" },
                new Brand() { BrandName = "Mazda" },
                new Brand() { BrandName = "Chevrolet" },
                new Brand() { BrandName = "Jeep" },
                new Brand() { BrandName = "Volvo" },
                new Brand() { BrandName = "Porsche" },
                new Brand() { BrandName = "Lexus" },
                new Brand() { BrandName = "Tesla" },
                new Brand() { BrandName = "Ferrari" },
                new Brand() { BrandName = "Lamborghini" },
                new Brand() { BrandName = "Aston Martin" },
                new Brand() { BrandName = "Land Rover" },
                new Brand() { BrandName = "BYD" }
            };

            await context.AddRangeAsync(Brands);
            await context.SaveChangesAsync();
        }

        public async Task SeedCarModelAsync(AppDbContext context)
        {
            if (await context.CarModels.AnyAsync()) return;

            var honda = await context.Brands.FirstAsync(x => x.BrandName == "Honda");
            var toyota = await context.Brands.FirstAsync(x => x.BrandName == "Toyota");
            var renault = await context.Brands.FirstAsync(x => x.BrandName == "Renault");
            var bmw = await context.Brands.FirstAsync(x => x.BrandName == "BMW");
            var ford = await context.Brands.FirstAsync(x => x.BrandName == "Ford");
            var nissan = await context.Brands.FirstAsync(x => x.BrandName == "Nissan");
            var subaru = await context.Brands.FirstAsync(x => x.BrandName == "Subaru");
            var volkswagen = await context.Brands.FirstAsync(x => x.BrandName == "Volkswagen");
            var mercedesBenz = await context.Brands.FirstAsync(x => x.BrandName == "Mercedes-Benz");
            var audi = await context.Brands.FirstAsync(x => x.BrandName == "Audi");
            var hyundai = await context.Brands.FirstAsync(x => x.BrandName == "Hyundai");
            var kia = await context.Brands.FirstAsync(x => x.BrandName == "Kia");
            var mazda = await context.Brands.FirstAsync(x => x.BrandName == "Mazda");
            var chevrolet = await context.Brands.FirstAsync(x => x.BrandName == "Chevrolet");
            var jeep = await context.Brands.FirstAsync(x => x.BrandName == "Jeep");
            var volvo = await context.Brands.FirstAsync(x => x.BrandName == "Volvo");
            var porsche = await context.Brands.FirstAsync(x => x.BrandName == "Porsche");
            var lexus = await context.Brands.FirstAsync(x => x.BrandName == "Lexus");
            var tesla = await context.Brands.FirstAsync(x => x.BrandName == "Tesla");
            var ferrari = await context.Brands.FirstAsync(x => x.BrandName == "Ferrari");
            var lamborghini = await context.Brands.FirstAsync(x => x.BrandName == "Lamborghini");
            var astonMartin = await context.Brands.FirstAsync(x => x.BrandName == "Aston Martin");
            var landRover = await context.Brands.FirstAsync(x => x.BrandName == "Land Rover");
            var byd = await context.Brands.FirstAsync(x => x.BrandName == "BYD");

            var carModels = new List<CarModel>()
            {
                // --- Honda ---
                new CarModel()
                {
                    BrandId = honda.Id,
                    Model = "Civic",
                    Series = "EK4 SiR",
                    Brand = honda,
                    ReleaseDate = new DateTime(1999, 12, 11)
                },
                new CarModel()
                {
                    BrandId = honda.Id,
                    Model = "S2000",
                    Series = "AP1",
                    Brand = honda,
                    ReleaseDate = new DateTime(1999, 4, 15)
                },
                new CarModel()
                {
                    BrandId = honda.Id,
                    Model = "Accord",
                    Series = "Generation 6",
                    Brand = honda,
                    ReleaseDate = new DateTime(1997, 9, 4)
                },

                // --- Toyota ---
                new CarModel()
                {
                    BrandId = toyota.Id,
                    Model = "Corolla",
                    Series = "E110",
                    Brand = toyota,
                    ReleaseDate = new DateTime(1995, 5, 15)
                },
                new CarModel()
                {
                    BrandId = toyota.Id,
                    Model = "Prius",
                    Series = "XW10",
                    Brand = toyota,
                    ReleaseDate = new DateTime(1997, 12, 10)
                },
                new CarModel()
                {
                    BrandId = toyota.Id,
                    Model = "Supra",
                    Series = "A80",
                    Brand = toyota,
                    ReleaseDate = new DateTime(1993, 5, 24)
                },

                // --- BMW ---
                new CarModel()
                {
                    BrandId = bmw.Id,
                    Model = "3 Series",
                    Series = "E46",
                    Brand = bmw,
                    ReleaseDate = new DateTime(1998, 4, 3)
                },
                new CarModel()
                {
                    BrandId = bmw.Id,
                    Model = "5 Series",
                    Series = "E39",
                    Brand = bmw,
                    ReleaseDate = new DateTime(1995, 9, 1)
                },

                // --- Ford ---
                new CarModel()
                {
                    BrandId = ford.Id,
                    Model = "Mustang",
                    Series = "SN95 New Edge",
                    Brand = ford,
                    ReleaseDate = new DateTime(1998, 12, 26)
                },
                new CarModel()
                {
                    BrandId = ford.Id,
                    Model = "Focus",
                    Series = "Mk1",
                    Brand = ford,
                    ReleaseDate = new DateTime(1998, 10, 1)
                },

                // --- Volkswagen ---
                new CarModel()
                {
                    BrandId = volkswagen.Id,
                    Model = "Golf",
                    Series = "Mk4",
                    Brand = volkswagen,
                    ReleaseDate = new DateTime(1997, 8, 1)
                },
                new CarModel()
                {
                    BrandId = volkswagen.Id,
                    Model = "Passat",
                    Series = "B5",
                    Brand = volkswagen,
                    ReleaseDate = new DateTime(1996, 10, 15)
                },

                // --- Nissan ---
                new CarModel()
                {
                    BrandId = nissan.Id,
                    Model = "Skyline",
                    Series = "R34 GT-R",
                    Brand = nissan,
                    ReleaseDate = new DateTime(1999, 1, 1)
                },

                // --- Subaru ---
                new CarModel()
                {
                    BrandId = subaru.Id,
                    Model = "Impreza",
                    Series = "WRX STi GC8",
                    Brand = subaru,
                    ReleaseDate = new DateTime(1998, 3, 10)
                },

                // --- Renault ---
                new CarModel()
                {
                    BrandId = renault.Id,
                    Model = "Clio",
                    Series = "Clio II",
                    Brand = renault,
                    ReleaseDate = new DateTime(1998, 3, 1)
                },

                // --- Mercedes-Benz ---
                new CarModel()
                {
                    BrandId = mercedesBenz.Id,
                    Model = "C-Class",
                    Series = "W202",
                    Brand = mercedesBenz,
                    ReleaseDate = new DateTime(1993, 5, 1)
                },

                // --- Audi ---
                new CarModel()
                {
                    BrandId = audi.Id,
                    Model = "TT",
                    Series = "8N Mk1",
                    Brand = audi,
                    ReleaseDate = new DateTime(1998, 9, 1)
                },

                // --- Porsche ---
                new CarModel()
                {
                    BrandId = porsche.Id,
                    Model = "911 Carrera",
                    Series = "996",
                    Brand = porsche,
                    ReleaseDate = new DateTime(1997, 9, 1)
                },

                // --- Chevrolet ---
                new CarModel()
                {
                    BrandId = chevrolet.Id,
                    Model = "Corvette",
                    Series = "C5",
                    Brand = chevrolet,
                    ReleaseDate = new DateTime(1997, 3, 1)
                },

                // --- Mazda ---
                new CarModel()
                {
                    BrandId = mazda.Id,
                    Model = "MX-5 Miata",
                    Series = "NB",
                    Brand = mazda,
                    ReleaseDate = new DateTime(1998, 2, 1)
                }
            };

            await context.CarModels.AddRangeAsync(carModels);  
            await context.SaveChangesAsync();
        }
    }
}
