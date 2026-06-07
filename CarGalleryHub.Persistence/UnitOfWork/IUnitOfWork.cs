using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Address> Addresses { get; }
        IRepository<Advert> Adverts { get; }
        IRepository<Brand> Brands { get; }
        IRepository<Car> Cars { get; }
        IRepository<CarModel> CarModels { get; }
        IRepository<Cart> Carts { get; }
        IRepository<CartItem> CartItems { get; }
        IRepository<Image> Images { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IRepository<Payment> Payments { get; }
        IRepository<User> Users { get; }
        Task<int> SaveChangesAsync();
    }
}
