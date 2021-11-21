using System;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DalException : Exception
    {
        public DalException(string message) : base(message)
        {
        }

        public DalException(string message, Exception innerException) : base(message, innerException)
        {
        }
        
    }
}