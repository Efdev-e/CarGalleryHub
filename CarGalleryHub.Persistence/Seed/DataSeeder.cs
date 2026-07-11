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
            _secretPass = configuration["Secrets:SecretPass"]?.Trim() ?? string.Empty;
            _context = context;
        }

        public async Task SeedAsync() 
        {
            await _context.Database.MigrateAsync();   
            await SeedUsersAsync(_context);
            await SeedBrandAsync(_context);
            await SeedCarModelAsync(_context);
            await SeedCarsAndAdvertsAsync(_context);
        }

        public async Task SeedUsersAsync(AppDbContext context) 
        {
            if (await context.Users.AnyAsync()) return;

            var Users = new List<User>()
            {
                new User() { FirstName = "Efe", LastName="Ok",Role = UserRoles.Admin, PasswordHash = BCrypt.Net.BCrypt.HashPassword(_secretPass), Email="Kingston@gmail.com" , PhoneNumber = "+905487987202"},
                new User() { FirstName = "Abdul", LastName = "Hamid", Role = UserRoles.Customer, PasswordHash = BCrypt.Net.BCrypt.HashPassword("GAbdul1155"), Email = "AbdulHamid@gmail.com", PhoneNumber = "+905852133265" }
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
                // --- Honda (8) ---
                new CarModel() { BrandId = honda.Id, Model = "Civic", Series = "EK4 SiR", Brand = honda, ReleaseDate = new DateTime(1999, 12, 11) },
                new CarModel() { BrandId = honda.Id, Model = "S2000", Series = "AP1", Brand = honda, ReleaseDate = new DateTime(1999, 4, 15) },
                new CarModel() { BrandId = honda.Id, Model = "Accord", Series = "Gen 6", Brand = honda, ReleaseDate = new DateTime(1997, 9, 4) },
                new CarModel() { BrandId = honda.Id, Model = "Prelude", Series = "Gen 5", Brand = honda, ReleaseDate = new DateTime(1996, 11, 7) },
                new CarModel() { BrandId = honda.Id, Model = "Integra", Series = "DC2 Type R", Brand = honda, ReleaseDate = new DateTime(1995, 8, 10) },
                new CarModel() { BrandId = honda.Id, Model = "CR-V", Series = "RD1", Brand = honda, ReleaseDate = new DateTime(1995, 10, 9) },
                new CarModel() { BrandId = honda.Id, Model = "NSX", Series = "NA1", Brand = honda, ReleaseDate = new DateTime(1990, 9, 13) },
                new CarModel() { BrandId = honda.Id, Model = "Jazz", Series = "GD1", Brand = honda, ReleaseDate = new DateTime(2001, 6, 21) },

                // --- Toyota (9) ---
                new CarModel() { BrandId = toyota.Id, Model = "Corolla", Series = "E110", Brand = toyota, ReleaseDate = new DateTime(1995, 5, 15) },
                new CarModel() { BrandId = toyota.Id, Model = "Prius", Series = "XW10", Brand = toyota, ReleaseDate = new DateTime(1997, 12, 10) },
                new CarModel() { BrandId = toyota.Id, Model = "Supra", Series = "A80", Brand = toyota, ReleaseDate = new DateTime(1993, 5, 24) },
                new CarModel() { BrandId = toyota.Id, Model = "Celica", Series = "T230", Brand = toyota, ReleaseDate = new DateTime(1999, 9, 20) },
                new CarModel() { BrandId = toyota.Id, Model = "GT86", Series = "ZN6", Brand = toyota, ReleaseDate = new DateTime(2012, 4, 6) },
                new CarModel() { BrandId = toyota.Id, Model = "RAV4", Series = "XA10", Brand = toyota, ReleaseDate = new DateTime(1994, 5, 10) },
                new CarModel() { BrandId = toyota.Id, Model = "Land Cruiser", Series = "J100", Brand = toyota, ReleaseDate = new DateTime(1998, 1, 8) },
                new CarModel() { BrandId = toyota.Id, Model = "Yaris", Series = "XP10", Brand = toyota, ReleaseDate = new DateTime(1999, 1, 13) },
                new CarModel() { BrandId = toyota.Id, Model = "Camry", Series = "XV20", Brand = toyota, ReleaseDate = new DateTime(1996, 8, 1) },

                // --- Renault (7) ---
                new CarModel() { BrandId = renault.Id, Model = "Clio", Series = "Clio II", Brand = renault, ReleaseDate = new DateTime(1998, 3, 1) },
                new CarModel() { BrandId = renault.Id, Model = "Megane", Series = "Megane I", Brand = renault, ReleaseDate = new DateTime(1995, 9, 1) },
                new CarModel() { BrandId = renault.Id, Model = "Captur", Series = "Gen 1", Brand = renault, ReleaseDate = new DateTime(2013, 4, 1) },
                new CarModel() { BrandId = renault.Id, Model = "Scenic", Series = "Scenic I", Brand = renault, ReleaseDate = new DateTime(1996, 11, 15) },
                new CarModel() { BrandId = renault.Id, Model = "Laguna", Series = "Laguna I", Brand = renault, ReleaseDate = new DateTime(1994, 1, 1) },
                new CarModel() { BrandId = renault.Id, Model = "Talisman", Series = "Gen 1", Brand = renault, ReleaseDate = new DateTime(2015, 6, 1) },
                new CarModel() { BrandId = renault.Id, Model = "Zoe", Series = "Gen 1", Brand = renault, ReleaseDate = new DateTime(2012, 12, 1) },

                // --- BMW (9) ---
                new CarModel() { BrandId = bmw.Id, Model = "3 Series", Series = "E46", Brand = bmw, ReleaseDate = new DateTime(1998, 4, 3) },
                new CarModel() { BrandId = bmw.Id, Model = "5 Series", Series = "E39", Brand = bmw, ReleaseDate = new DateTime(1995, 9, 1) },
                new CarModel() { BrandId = bmw.Id, Model = "7 Series", Series = "E38", Brand = bmw, ReleaseDate = new DateTime(1994, 6, 1) },
                new CarModel() { BrandId = bmw.Id, Model = "8 Series", Series = "E31", Brand = bmw, ReleaseDate = new DateTime(1990, 5, 1) },
                new CarModel() { BrandId = bmw.Id, Model = "M3", Series = "E36", Brand = bmw, ReleaseDate = new DateTime(1992, 11, 1) },
                new CarModel() { BrandId = bmw.Id, Model = "M5", Series = "E60", Brand = bmw, ReleaseDate = new DateTime(2004, 9, 1) },
                new CarModel() { BrandId = bmw.Id, Model = "X5", Series = "E53", Brand = bmw, ReleaseDate = new DateTime(1999, 9, 1) },
                new CarModel() { BrandId = bmw.Id, Model = "Z4", Series = "E85", Brand = bmw, ReleaseDate = new DateTime(2002, 10, 1) },
                new CarModel() { BrandId = bmw.Id, Model = "i8", Series = "I12", Brand = bmw, ReleaseDate = new DateTime(2014, 6, 1) },

                // --- Ford (8) ---
                new CarModel() { BrandId = ford.Id, Model = "Mustang", Series = "SN95", Brand = ford, ReleaseDate = new DateTime(1998, 12, 26) },
                new CarModel() { BrandId = ford.Id, Model = "Focus", Series = "Mk1", Brand = ford, ReleaseDate = new DateTime(1998, 10, 1) },
                new CarModel() { BrandId = ford.Id, Model = "Fiesta", Series = "Mk4", Brand = ford, ReleaseDate = new DateTime(1995, 8, 1) },
                new CarModel() { BrandId = ford.Id, Model = "Mondeo", Series = "Mk2", Brand = ford, ReleaseDate = new DateTime(1996, 10, 1) },
                new CarModel() { BrandId = ford.Id, Model = "Kuga", Series = "Gen 1", Brand = ford, ReleaseDate = new DateTime(2008, 3, 1) },
                new CarModel() { BrandId = ford.Id, Model = "Ranger", Series = "T6", Brand = ford, ReleaseDate = new DateTime(2011, 6, 1) },
                new CarModel() { BrandId = ford.Id, Model = "Explorer", Series = "Gen 3", Brand = ford, ReleaseDate = new DateTime(2001, 1, 1) },
                new CarModel() { BrandId = ford.Id, Model = "GT", Series = "Gen 1", Brand = ford, ReleaseDate = new DateTime(2004, 8, 1) },

                // --- Volkswagen (7) ---
                new CarModel() { BrandId = volkswagen.Id, Model = "Golf", Series = "Mk4", Brand = volkswagen, ReleaseDate = new DateTime(1997, 8, 1) },
                new CarModel() { BrandId = volkswagen.Id, Model = "Passat", Series = "B5", Brand = volkswagen, ReleaseDate = new DateTime(1996, 10, 15) },
                new CarModel() { BrandId = volkswagen.Id, Model = "Polo", Series = "Polo III", Brand = volkswagen, ReleaseDate = new DateTime(1994, 9, 1) },
                new CarModel() { BrandId = volkswagen.Id, Model = "Tiguan", Series = "Gen 1", Brand = volkswagen, ReleaseDate = new DateTime(2007, 9, 1) },
                new CarModel() { BrandId = volkswagen.Id, Model = "Touareg", Series = "Gen 1", Brand = volkswagen, ReleaseDate = new DateTime(2002, 10, 1) },
                new CarModel() { BrandId = volkswagen.Id, Model = "Arteon", Series = "Gen 1", Brand = volkswagen, ReleaseDate = new DateTime(2017, 3, 1) },
                new CarModel() { BrandId = volkswagen.Id, Model = "Scirocco", Series = "Scirocco III", Brand = volkswagen, ReleaseDate = new DateTime(2008, 8, 1) },

                // --- Nissan (7) ---
                new CarModel() { BrandId = nissan.Id, Model = "Skyline", Series = "R34 GT-R", Brand = nissan, ReleaseDate = new DateTime(1999, 1, 1) },
                new CarModel() { BrandId = nissan.Id, Model = "GT-R", Series = "R35", Brand = nissan, ReleaseDate = new DateTime(2007, 12, 1) },
                new CarModel() { BrandId = nissan.Id, Model = "350Z", Series = "Z33", Brand = nissan, ReleaseDate = new DateTime(2002, 7, 1) },
                new CarModel() { BrandId = nissan.Id, Model = "Qashqai", Series = "J10", Brand = nissan, ReleaseDate = new DateTime(2006, 12, 1) },
                new CarModel() { BrandId = nissan.Id, Model = "Juke", Series = "F15", Brand = nissan, ReleaseDate = new DateTime(2010, 6, 1) },
                new CarModel() { BrandId = nissan.Id, Model = "Leaf", Series = "ZE0", Brand = nissan, ReleaseDate = new DateTime(2010, 12, 1) },
                new CarModel() { BrandId = nissan.Id, Model = "Micra", Series = "K11", Brand = nissan, ReleaseDate = new DateTime(1992, 1, 1) },

                // --- Subaru (5) ---
                new CarModel() { BrandId = subaru.Id, Model = "Impreza", Series = "WRX GC8", Brand = subaru, ReleaseDate = new DateTime(1998, 3, 10) },
                new CarModel() { BrandId = subaru.Id, Model = "Forester", Series = "SF", Brand = subaru, ReleaseDate = new DateTime(1997, 2, 1) },
                new CarModel() { BrandId = subaru.Id, Model = "Outback", Series = "BG", Brand = subaru, ReleaseDate = new DateTime(1995, 8, 1) },
                new CarModel() { BrandId = subaru.Id, Model = "Legacy", Series = "BD", Brand = subaru, ReleaseDate = new DateTime(1993, 10, 1) },
                new CarModel() { BrandId = subaru.Id, Model = "BRZ", Series = "ZC6", Brand = subaru, ReleaseDate = new DateTime(2012, 3, 1) },

                // --- Mercedes-Benz (8) ---
                new CarModel() { BrandId = mercedesBenz.Id, Model = "C-Class", Series = "W202", Brand = mercedesBenz, ReleaseDate = new DateTime(1993, 5, 1) },
                new CarModel() { BrandId = mercedesBenz.Id, Model = "E-Class", Series = "W210", Brand = mercedesBenz, ReleaseDate = new DateTime(1995, 6, 1) },
                new CarModel() { BrandId = mercedesBenz.Id, Model = "S-Class", Series = "W140", Brand = mercedesBenz, ReleaseDate = new DateTime(1991, 3, 1) },
                new CarModel() { BrandId = mercedesBenz.Id, Model = "G-Class", Series = "W463", Brand = mercedesBenz, ReleaseDate = new DateTime(1990, 9, 1) },
                new CarModel() { BrandId = mercedesBenz.Id, Model = "A-Class", Series = "W168", Brand = mercedesBenz, ReleaseDate = new DateTime(1997, 10, 1) },
                new CarModel() { BrandId = mercedesBenz.Id, Model = "AMG GT", Series = "C190", Brand = mercedesBenz, ReleaseDate = new DateTime(2014, 9, 1) },
                new CarModel() { BrandId = mercedesBenz.Id, Model = "CLA", Series = "C117", Brand = mercedesBenz, ReleaseDate = new DateTime(2013, 4, 1) },
                new CarModel() { BrandId = mercedesBenz.Id, Model = "GLC", Series = "X253", Brand = mercedesBenz, ReleaseDate = new DateTime(2015, 6, 1) },

                // --- Audi (9) ---
                new CarModel() { BrandId = audi.Id, Model = "TT", Series = "8N Mk1", Brand = audi, ReleaseDate = new DateTime(1998, 9, 1) },
                new CarModel() { BrandId = audi.Id, Model = "A3", Series = "8L", Brand = audi, ReleaseDate = new DateTime(1996, 9, 1) },
                new CarModel() { BrandId = audi.Id, Model = "A4", Series = "B5", Brand = audi, ReleaseDate = new DateTime(1994, 11, 1) },
                new CarModel() { BrandId = audi.Id, Model = "A6", Series = "C5", Brand = audi, ReleaseDate = new DateTime(1997, 4, 1) },
                new CarModel() { BrandId = audi.Id, Model = "A8", Series = "D2", Brand = audi, ReleaseDate = new DateTime(1994, 3, 1) },
                new CarModel() { BrandId = audi.Id, Model = "Q5", Series = "8R", Brand = audi, ReleaseDate = new DateTime(2008, 8, 1) },
                new CarModel() { BrandId = audi.Id, Model = "Q7", Series = "4L", Brand = audi, ReleaseDate = new DateTime(2005, 9, 1) },
                new CarModel() { BrandId = audi.Id, Model = "R8", Series = "Type 42", Brand = audi, ReleaseDate = new DateTime(2006, 9, 1) },
                new CarModel() { BrandId = audi.Id, Model = "RS6", Series = "C5", Brand = audi, ReleaseDate = new DateTime(2002, 7, 1) },

                // --- Hyundai (7) ---
                new CarModel() { BrandId = hyundai.Id, Model = "i10", Series = "PA", Brand = hyundai, ReleaseDate = new DateTime(2007, 10, 1) },
                new CarModel() { BrandId = hyundai.Id, Model = "i20", Series = "PB", Brand = hyundai, ReleaseDate = new DateTime(2008, 10, 1) },
                new CarModel() { BrandId = hyundai.Id, Model = "i30", Series = "FD", Brand = hyundai, ReleaseDate = new DateTime(2007, 3, 1) },
                new CarModel() { BrandId = hyundai.Id, Model = "Tucson", Series = "JM", Brand = hyundai, ReleaseDate = new DateTime(2004, 8, 1) },
                new CarModel() { BrandId = hyundai.Id, Model = "Santa Fe", Series = "SM", Brand = hyundai, ReleaseDate = new DateTime(2000, 11, 1) },
                new CarModel() { BrandId = hyundai.Id, Model = "Elantra", Series = "XD", Brand = hyundai, ReleaseDate = new DateTime(2000, 6, 1) },
                new CarModel() { BrandId = hyundai.Id, Model = "Kona", Series = "OS", Brand = hyundai, ReleaseDate = new DateTime(2017, 6, 1) },

                // --- Kia (6) ---
                new CarModel() { BrandId = kia.Id, Model = "Picanto", Series = "SA", Brand = kia, ReleaseDate = new DateTime(2004, 3, 1) },
                new CarModel() { BrandId = kia.Id, Model = "Ceed", Series = "ED", Brand = kia, ReleaseDate = new DateTime(2006, 12, 1) },
                new CarModel() { BrandId = kia.Id, Model = "Sportage", Series = "JE", Brand = kia, ReleaseDate = new DateTime(2004, 9, 1) },
                new CarModel() { BrandId = kia.Id, Model = "Sorento", Series = "BL", Brand = kia, ReleaseDate = new DateTime(2002, 2, 1) },
                new CarModel() { BrandId = kia.Id, Model = "Stinger", Series = "CK", Brand = kia, ReleaseDate = new DateTime(2017, 5, 1) },
                new CarModel() { BrandId = kia.Id, Model = "EV6", Series = "CV", Brand = kia, ReleaseDate = new DateTime(2021, 3, 1) },

                // --- Mazda (6) ---
                new CarModel() { BrandId = mazda.Id, Model = "MX-5 Miata", Series = "NB", Brand = mazda, ReleaseDate = new DateTime(1998, 2, 1) },
                new CarModel() { BrandId = mazda.Id, Model = "Mazda3", Series = "BK", Brand = mazda, ReleaseDate = new DateTime(2003, 6, 1) },
                new CarModel() { BrandId = mazda.Id, Model = "Mazda6", Series = "GG", Brand = mazda, ReleaseDate = new DateTime(2002, 5, 1) },
                new CarModel() { BrandId = mazda.Id, Model = "CX-5", Series = "KE", Brand = mazda, ReleaseDate = new DateTime(2012, 2, 1) },
                new CarModel() { BrandId = mazda.Id, Model = "RX-7", Series = "FD", Brand = mazda, ReleaseDate = new DateTime(1992, 12, 1) },
                new CarModel() { BrandId = mazda.Id, Model = "RX-8", Series = "SE3P", Brand = mazda, ReleaseDate = new DateTime(2003, 4, 1) },

                // --- Chevrolet (6) ---
                new CarModel() { BrandId = chevrolet.Id, Model = "Corvette", Series = "C5", Brand = chevrolet, ReleaseDate = new DateTime(1997, 3, 1) },
                new CarModel() { BrandId = chevrolet.Id, Model = "Camaro", Series = "Gen 4", Brand = chevrolet, ReleaseDate = new DateTime(1993, 1, 1) },
                new CarModel() { BrandId = chevrolet.Id, Model = "Tahoe", Series = "GMT800", Brand = chevrolet, ReleaseDate = new DateTime(1999, 12, 1) },
                new CarModel() { BrandId = chevrolet.Id, Model = "Cruze", Series = "J300", Brand = chevrolet, ReleaseDate = new DateTime(2008, 10, 1) },
                new CarModel() { BrandId = chevrolet.Id, Model = "Spark", Series = "M300", Brand = chevrolet, ReleaseDate = new DateTime(2009, 5, 1) },
                new CarModel() { BrandId = chevrolet.Id, Model = "Bolt", Series = "Gen 1", Brand = chevrolet, ReleaseDate = new DateTime(2016, 10, 1) },

                // --- Jeep (4) ---
                new CarModel() { BrandId = jeep.Id, Model = "Wrangler", Series = "TJ", Brand = jeep, ReleaseDate = new DateTime(1996, 1, 1) },
                new CarModel() { BrandId = jeep.Id, Model = "Grand Cherokee", Series = "WJ", Brand = jeep, ReleaseDate = new DateTime(1998, 9, 1) },
                new CarModel() { BrandId = jeep.Id, Model = "Compass", Series = "MK49", Brand = jeep, ReleaseDate = new DateTime(2006, 5, 1) },
                new CarModel() { BrandId = jeep.Id, Model = "Renegade", Series = "BU", Brand = jeep, ReleaseDate = new DateTime(2014, 3, 1) },

                // --- Volvo (6) ---
                new CarModel() { BrandId = volvo.Id, Model = "S60", Series = "Gen 1", Brand = volvo, ReleaseDate = new DateTime(2000, 11, 1) },
                new CarModel() { BrandId = volvo.Id, Model = "S90", Series = "Gen 2", Brand = volvo, ReleaseDate = new DateTime(2016, 3, 1) },
                new CarModel() { BrandId = volvo.Id, Model = "XC40", Series = "Gen 1", Brand = volvo, ReleaseDate = new DateTime(2017, 9, 1) },
                new CarModel() { BrandId = volvo.Id, Model = "XC60", Series = "Gen 1", Brand = volvo, ReleaseDate = new DateTime(2008, 3, 1) },
                new CarModel() { BrandId = volvo.Id, Model = "XC90", Series = "Gen 1", Brand = volvo, ReleaseDate = new DateTime(2002, 10, 1) },
                new CarModel() { BrandId = volvo.Id, Model = "V60", Series = "Gen 1", Brand = volvo, ReleaseDate = new DateTime(2010, 9, 1) },

                // --- Porsche (6) ---
                new CarModel() { BrandId = porsche.Id, Model = "911 Carrera", Series = "996", Brand = porsche, ReleaseDate = new DateTime(1997, 9, 1) },
                new CarModel() { BrandId = porsche.Id, Model = "Cayman", Series = "987", Brand = porsche, ReleaseDate = new DateTime(2005, 9, 1) },
                new CarModel() { BrandId = porsche.Id, Model = "Cayenne", Series = "955", Brand = porsche, ReleaseDate = new DateTime(2002, 12, 1) },
                new CarModel() { BrandId = porsche.Id, Model = "Macan", Series = "Gen 1", Brand = porsche, ReleaseDate = new DateTime(2013, 11, 1) },
                new CarModel() { BrandId = porsche.Id, Model = "Panamera", Series = "970", Brand = porsche, ReleaseDate = new DateTime(2009, 4, 1) },
                new CarModel() { BrandId = porsche.Id, Model = "Taycan", Series = "Gen 1", Brand = porsche, ReleaseDate = new DateTime(2019, 9, 1) },

                // --- Lexus (7) ---
                new CarModel() { BrandId = lexus.Id, Model = "RX", Series = "AL10", Brand = lexus, ReleaseDate = new DateTime(2008, 11, 1) },
                new CarModel() { BrandId = lexus.Id, Model = "NX", Series = "AZ10", Brand = lexus, ReleaseDate = new DateTime(2014, 4, 1) },
                new CarModel() { BrandId = lexus.Id, Model = "IS", Series = "XE20", Brand = lexus, ReleaseDate = new DateTime(2005, 9, 1) },
                new CarModel() { BrandId = lexus.Id, Model = "ES", Series = "XV40", Brand = lexus, ReleaseDate = new DateTime(2006, 3, 1) },
                new CarModel() { BrandId = lexus.Id, Model = "LS", Series = "XF40", Brand = lexus, ReleaseDate = new DateTime(2006, 9, 1) },
                new CarModel() { BrandId = lexus.Id, Model = "LC500", Series = "Z100", Brand = lexus, ReleaseDate = new DateTime(2017, 3, 1) },
                new CarModel() { BrandId = lexus.Id, Model = "LFA", Series = "L10", Brand = lexus, ReleaseDate = new DateTime(2010, 12, 1) },

                // --- Tesla (5) ---
                new CarModel() { BrandId = tesla.Id, Model = "Model S", Series = "Gen 1", Brand = tesla, ReleaseDate = new DateTime(2012, 6, 22) },
                new CarModel() { BrandId = tesla.Id, Model = "Model 3", Series = "Gen 1", Brand = tesla, ReleaseDate = new DateTime(2017, 7, 7) },
                new CarModel() { BrandId = tesla.Id, Model = "Model X", Series = "Gen 1", Brand = tesla, ReleaseDate = new DateTime(2015, 9, 29) },
                new CarModel() { BrandId = tesla.Id, Model = "Model Y", Series = "Gen 1", Brand = tesla, ReleaseDate = new DateTime(2020, 3, 13) },
                new CarModel() { BrandId = tesla.Id, Model = "Roadster", Series = "Gen 1", Brand = tesla, ReleaseDate = new DateTime(2008, 3, 17) },

                // --- Ferrari (5) ---
                new CarModel() { BrandId = ferrari.Id, Model = "488 GTB", Series = "Gen 1", Brand = ferrari, ReleaseDate = new DateTime(2015, 3, 1) },
                new CarModel() { BrandId = ferrari.Id, Model = "F8 Tributo", Series = "Gen 1", Brand = ferrari, ReleaseDate = new DateTime(2019, 3, 1) },
                new CarModel() { BrandId = ferrari.Id, Model = "LaFerrari", Series = "F150", Brand = ferrari, ReleaseDate = new DateTime(2013, 3, 1) },
                new CarModel() { BrandId = ferrari.Id, Model = "SF90 Stradale", Series = "Gen 1", Brand = ferrari, ReleaseDate = new DateTime(2019, 5, 1) },
                new CarModel() { BrandId = ferrari.Id, Model = "Roma", Series = "Gen 1", Brand = ferrari, ReleaseDate = new DateTime(2019, 11, 1) },

                // --- Lamborghini (5) ---
                new CarModel() { BrandId = lamborghini.Id, Model = "Aventador", Series = "LP 700-4", Brand = lamborghini, ReleaseDate = new DateTime(2011, 2, 1) },
                new CarModel() { BrandId = lamborghini.Id, Model = "Huracan", Series = "LP 610-4", Brand = lamborghini, ReleaseDate = new DateTime(2014, 3, 1) },
                new CarModel() { BrandId = lamborghini.Id, Model = "Urus", Series = "Gen 1", Brand = lamborghini, ReleaseDate = new DateTime(2018, 6, 1) },
                new CarModel() { BrandId = lamborghini.Id, Model = "Gallardo", Series = "Coupe", Brand = lamborghini, ReleaseDate = new DateTime(2003, 3, 1) },
                new CarModel() { BrandId = lamborghini.Id, Model = "Murcielago", Series = "VT", Brand = lamborghini, ReleaseDate = new DateTime(2001, 9, 1) },

                // --- Aston Martin (5) ---
                new CarModel() { BrandId = astonMartin.Id, Model = "DB11", Series = "Gen 1", Brand = astonMartin, ReleaseDate = new DateTime(2016, 3, 1) },
                new CarModel() { BrandId = astonMartin.Id, Model = "Vantage", Series = "Gen 2", Brand = astonMartin, ReleaseDate = new DateTime(2018, 6, 1) },
                new CarModel() { BrandId = astonMartin.Id, Model = "DBS", Series = "Superleggera", Brand = astonMartin, ReleaseDate = new DateTime(2018, 6, 1) },
                new CarModel() { BrandId = astonMartin.Id, Model = "DBX", Series = "Gen 1", Brand = astonMartin, ReleaseDate = new DateTime(2020, 7, 1) },
                new CarModel() { BrandId = astonMartin.Id, Model = "Valhalla", Series = "Gen 1", Brand = astonMartin, ReleaseDate = new DateTime(2021, 7, 1) },

                // --- Land Rover (5) ---
                new CarModel() { BrandId = landRover.Id, Model = "Defender", Series = "L663", Brand = landRover, ReleaseDate = new DateTime(2019, 9, 1) },
                new CarModel() { BrandId = landRover.Id, Model = "Range Rover", Series = "L460", Brand = landRover, ReleaseDate = new DateTime(2021, 10, 1) },
                new CarModel() { BrandId = landRover.Id, Model = "Discovery", Series = "L462", Brand = landRover, ReleaseDate = new DateTime(2016, 9, 1) },
                new CarModel() { BrandId = landRover.Id, Model = "Evoque", Series = "L551", Brand = landRover, ReleaseDate = new DateTime(2018, 11, 1) },
                new CarModel() { BrandId = landRover.Id, Model = "Velar", Series = "L560", Brand = landRover, ReleaseDate = new DateTime(2017, 3, 1) },

                // --- BYD (5) ---
                new CarModel() { BrandId = byd.Id, Model = "Han", Series = "Gen 1", Brand = byd, ReleaseDate = new DateTime(2020, 7, 1) },
                new CarModel() { BrandId = byd.Id, Model = "Tang", Series = "Gen 2", Brand = byd, ReleaseDate = new DateTime(2018, 6, 1) },
                new CarModel() { BrandId = byd.Id, Model = "Atto 3", Series = "Gen 1", Brand = byd, ReleaseDate = new DateTime(2022, 2, 1) },
                new CarModel() { BrandId = byd.Id, Model = "Seal", Series = "Gen 1", Brand = byd, ReleaseDate = new DateTime(2022, 7, 1) },
                new CarModel() { BrandId = byd.Id, Model = "Dolphin", Series = "Gen 1", Brand = byd, ReleaseDate = new DateTime(2021, 8, 1) }
            };

            await context.CarModels.AddRangeAsync(carModels);  
            await context.SaveChangesAsync();
        }

        public async Task SeedCarsAndAdvertsAsync(AppDbContext context)
        {
            if (await context.Cars.AnyAsync()) return;

            var models = await context.CarModels.Include(x => x.Brand).ToListAsync();
            var seller = await context.Users.FirstOrDefaultAsync();
            if (seller == null) return;

            var unsplashImages = new[]
            {
                "https://images.unsplash.com/photo-1503376780353-7e6692767b70?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1525609004556-c46c7d6cf0a3?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1542282088-fe8426682b8f?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1552519507-da3b142c6e3d?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1583121274602-3e2820c69888?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1492144534655-ae79c964c9d7?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1494976388531-d1058494cdd8?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1502877338535-766e1452684a?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1563720223185-11003d516935?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1617814076367-b759c7d7e738?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1618843479313-40f8afb4b4d8?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1605559424843-9e4c228bf1c2?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1511919884226-fd3cad34687c?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1549399542-7e3f8b79c341?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1606016159991-dfe4f2746ad5?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1580273916550-e323be2ae537?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1614162692292-7ac56d7f7f1e?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1553440569-bcc63803a83d?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1506015391300-4802dc74de2e?auto=format&fit=crop&w=800&q=80",
                "https://images.unsplash.com/photo-1549927681-0b673b8243ab?auto=format&fit=crop&w=800&q=80"
            };

            var colors = (ColorType[])Enum.GetValues(typeof(ColorType));
            var cars = new List<Car>();
            var adverts = new List<Advert>();

            for (int i = 1; i <= 200; i++)
            {
                var model = models[i % models.Count];
                var color = colors[i % colors.Length];
                var year = 2005 + (i % 21);
                var km = (i * 1873) % 280000;
                var status = (i % 5 == 0) ? CarStatus.New : CarStatus.SecondHand;
                var availability = (i % 15 == 0) ? CarAvailability.Sold : CarAvailability.Available;
                var hp = (90 + (i * 27) % 450) + " HP";
                var warranty = (i % 2 == 0);

                decimal basePrice = 500000m;
                var brandName = model.Brand.BrandName.ToLower();
                if (brandName.Contains("ferrari") || brandName.Contains("lamborghini") || brandName.Contains("aston martin"))
                {
                    basePrice = 12000000m;
                }
                else if (brandName.Contains("porsche") || brandName.Contains("tesla"))
                {
                    basePrice = 4500000m;
                }
                else if (brandName.Contains("mercedes") || brandName.Contains("bmw") || brandName.Contains("audi") || brandName.Contains("land rover") || brandName.Contains("lexus"))
                {
                    basePrice = 2800000m;
                }
                else if (brandName.Contains("volvo") || brandName.Contains("jeep") || brandName.Contains("byd"))
                {
                    basePrice = 1800000m;
                }

                decimal ageFactor = (year - 2005) / 20.0m;
                if (ageFactor < 0.2m) ageFactor = 0.2m;
                decimal unitPrice = basePrice * ageFactor;

                decimal kmFactor = 1.0m - (km / 400000.0m);
                if (kmFactor < 0.4m) kmFactor = 0.4m;
                unitPrice = unitPrice * kmFactor;

                unitPrice = Math.Round(unitPrice / 1000) * 1000;

                var car = new Car
                {
                    BrandName = model.Brand.BrandName,
                    ModelName = model.Model,
                    Series = model.Series,
                    MotorPower = hp,
                    Year = year,
                    KM = km,
                    Color = color,
                    Status = status,
                    Availability = availability,
                    CarModelId = model.Id
                };

                cars.Add(car);

                var imageUrl = unsplashImages[i % unsplashImages.Length];
                var advert = new Advert
                {
                    AdvertTitle = $"{year} {model.Brand.BrandName} {model.Model} {model.Series}",
                    Description = $"{year} model premium {model.Brand.BrandName} {model.Model} {model.Series}. {km:N0} KM'de, hatasız, tüm bakımları yetkili servisinde yapılmış prestijli bir araçtır. Detaylar ve sürüş testi için iletişime geçebilirsiniz.",
                    Warranty = warranty,
                    UnitPrice = unitPrice,
                    SellerId = seller.Id,
                    Car = car,
                    Thumbnails = new List<Image>
                    {
                        new Image
                        {
                            ImageUrl = imageUrl,
                            ImageType = ImageType.Thumbnail
                        }
                    }
                };

                adverts.Add(advert);
            }

            await context.Cars.AddRangeAsync(cars);
            await context.Adverts.AddRangeAsync(adverts);
            await context.SaveChangesAsync();
        }
    }
}

