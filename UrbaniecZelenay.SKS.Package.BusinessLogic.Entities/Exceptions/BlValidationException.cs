using System;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions
{
    [ExcludeFromCodeCoverage]
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