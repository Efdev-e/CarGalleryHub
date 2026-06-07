using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.Context;
using CarGalleryHub.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IRepository<User>? _users;
        private IRepository<Address>? _addresses;
        private IRepository<Advert>? _adverts;
        private IRepository<Brand>? _brands;
        private IRepository<Car>? _cars;
        private IRepository<CarModel>? _carmodels;
        private IRepository<Cart>? _carts;
        private IRepository<CartItem>? _cartitems;
        private IRepository<Image>? _images;
        private IRepository<Order>? _orders;
        private IRepository<OrderItem>? _orderitems;
        private IRepository<Payment>? _payments;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IRepository<Address> Addresses => _addresses ??= new Repositories<Address>(_context);
        public IRepository<Advert> Adverts => _adverts ??= new Repositories<Advert>(_context);

        public IRepository<Brand> Brands => _brands ??= new Repositories<Brand>(_context);

        public IRepository<Car> Cars => _cars ??= new Repositories<Car>(_context);

        public IRepository<CarModel> CarModels => _carmodels ??= new Repositories<CarModel>(_context);

        public IRepository<Cart> Carts => _carts ??= new Repositories<Cart>(_context);

        public IRepository<CartItem> CartItems => _cartitems ??= new Repositories<CartItem>(_context);

        public IRepository<Image> Images => _images ??= new Repositories<Image>(_context);

        public IRepository<Order> Orders => _orders ??= new Repositories<Order>(_context);

        public IRepository<OrderItem> OrderItems => _orderitems ??= new Repositories<OrderItem>(_context);

        public IRepository<Payment> Payments => _payments ??= new Repositories<Payment>(_context);

        public IRepository<User> Users => _users ??= new Repositories<User>(_context);

        public void Dispose() => _context.Dispose();

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
