using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Exceptions
{
    public abstract class BaseException : Exception
    {
        protected BaseException(string Message) : base(Message) 
        {
        }
    }

}
