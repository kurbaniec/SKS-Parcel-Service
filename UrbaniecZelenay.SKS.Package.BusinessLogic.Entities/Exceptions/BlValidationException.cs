using System;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions
{
    public class BlValidationException : BlException
    {
        public BlValidationException(string message) : base(message)
        {
        }

        public BlValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}