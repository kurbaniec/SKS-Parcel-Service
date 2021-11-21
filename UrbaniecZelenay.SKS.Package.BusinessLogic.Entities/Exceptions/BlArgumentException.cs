using System;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class BlArgumentException : BlException
    {
        public BlArgumentException(string message) : base(message)
        {
        }

        public BlArgumentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}