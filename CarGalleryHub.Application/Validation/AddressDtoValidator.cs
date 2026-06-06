using CarGalleryHub.Application.DTOs.Address;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.Validation
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            
        }
    }
}
