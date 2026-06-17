using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.Interfaces.Payment
{
    public interface IPaymentProviderFactory
    {
        IPaymentProvider Create(string paymentprovider = "fake");
    }
}
