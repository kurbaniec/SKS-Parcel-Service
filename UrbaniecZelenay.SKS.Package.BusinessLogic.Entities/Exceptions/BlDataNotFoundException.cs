using System;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class BlDataNotFoundException : BlException
    {
        public BlDataNotFoundException(string message) : base(message)
        {
        }

        public BlDataNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}