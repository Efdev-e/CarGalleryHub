using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Exceptions
{
    public class DirectUpdateException : BaseException
    {
        public DirectUpdateException(string message) : base
            ($"Cannot Update Directly")
        {}
    }
}
