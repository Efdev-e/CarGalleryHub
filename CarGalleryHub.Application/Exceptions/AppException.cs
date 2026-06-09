using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.Exceptions
{
    public class AppException : Exception
    {
        public int StatusCode { get; }
        public AppException(string message, int statuscode) : base(message)
        {
            StatusCode = statuscode;
        }
    }
}
