using System;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class BlException : Exception
    {
        public BlException(string message) : base(message)
        {
        }

        public BlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}