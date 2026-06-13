using CarGalleryHub.Application.DTOs.Address;
using CarGalleryHub.Application.DTOs.User.Address;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddressController(IUnitOfWork work)
        {
            _unitOfWork = work;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ViewAddress(int id) 
        {
            var user = await _unitOfWork.Users.GetByIdAsync(GetUserId());
            if (user is null) return Invalid();
            var Address = user.Addresses.FirstOrDefault(x => x.Id == id);
            if (Address is null) return Invalid();

            var addressDto = new AddressDto()
            {
                FullAddress = Address.FullAddress,
                City = Address.City,
                District = Address.District,
                FullName = Address.FullName,
                Phone = Address.Phone,
                PostalCode = Address.PostalCode,
                Email = Address.Email,
                Id = Address.Id
            };
            

            return Ok(addressDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListAddresses()
        {
            var user = await _unitOfWork.Users.GetByIdAsync(GetUserId());
            if (user is null) return Invalid();
            if (user.Addresses is null) return Ok(Array.Empty<AddressDto>, "Başarız Oldu");

            var Addresses = user.Addresses.Select(x => new AddressDto()
            {
                City = x.City,
                District = x.District,
                FullAddress = x.FullAddress,
                FullName = x.FullName,
                Phone = x.Phone,
                PostalCode = x.PostalCode,
                Email = x.Email,
                Id = x.Id
            }).ToArray();

           

            return Ok(Addresses);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAddress(int id, AddressDto addressDto) 
        {
            var user = await _unitOfWork.Users.GetByIdAsync(GetUserId());
            if (user is null) return Invalid();

            if (addressDto is null) return Invalid();

            if (string.IsNullOrWhiteSpace(addressDto.City) ||
                string.IsNullOrWhiteSpace(addressDto.District) ||
                string.IsNullOrWhiteSpace(addressDto.FullAddress) ||
                string.IsNullOrWhiteSpace(addressDto.FullName) ||
                string.IsNullOrWhiteSpace(addressDto.Phone))
            {
                return Invalid();
            }
            if (user.Addresses is null) return Invalid();
            var address = user.Addresses.FirstOrDefault(x => x.Id == id);
            if (address is null) return Invalid();

            address.City = addressDto.City;
            address.District = addressDto.District;
            address.FullAddress = addressDto.FullAddress;
            address.FullName = addressDto.FullName;
            address.Phone = addressDto.Phone;
            address.PostalCode = addressDto.PostalCode;
            address.Email = addressDto.Email;

            _unitOfWork.Addresses.Update(address);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAddress(CreateAddressDto addressDto) 
        {
            var user = await _unitOfWork.Users.GetByIdAsync(GetUserId());
            if (user is null) return Invalid();

            
            var AddressExist = await _unitOfWork.Addresses.FirstOrDefaultAsync(x =>
                x.FullAddress == addressDto.FullAddress &&
                x.City == addressDto.City &&
                x.District == addressDto.District &&
                x.PostalCode == addressDto.PostalCode);
            if (AddressExist is null) 
            {
                var adr = new Address()
                {
                    City = addressDto.City,
                    District = addressDto.District,
                    FullAddress = addressDto.FullAddress,
                    FullName = addressDto.FullName,
                    Phone = addressDto.Phone,
                    PostalCode = addressDto.PostalCode,
                    Email = addressDto.Email,
                    Users = new List<User>() { user }
                };

                await _unitOfWork.Addresses.AddAsync(adr);
            }
            else 
            {
                AddressExist.Users ??= new List<User>();
                if (!AddressExist.Users.Any(u => u.Id == user.Id)) 
                {
                    AddressExist.Users.Add(user);
                    _unitOfWork.Addresses.Update(AddressExist);
                }
            }
            
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}
