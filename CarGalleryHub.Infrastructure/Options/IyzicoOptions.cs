using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Infrastructure.Options
{
    public class IyzicoOptions
    {
        public const string SectionName = "Iyzico";
        public string ApiKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://sandbox-api.iyzipay.com";
    }
}
