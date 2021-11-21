using System;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class BlRepositoryException : BlException
    {
        public BlRepositoryException(string message) : base(message)
        {
        }

        public BlRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}