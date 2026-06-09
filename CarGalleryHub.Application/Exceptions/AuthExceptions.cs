using CarGalleryHub.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.Exceptions
{
    public class MissingCredentials : AppException
    {
        public MissingCredentials(string missing) : base($"Missing {missing}",401) { }
    }

    public class NotFound : AppException 
    {
        public NotFound(string missing) : base($"{missing} Not Found", 404) { }
    }

    public class InvalidCredentials : AppException 
    {
        public InvalidCredentials(string message) : base(message, 401) { }
    }
}
