using System;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DalConnectionException : DalException
    {
        public DalConnectionException(string message) : base(message)
        {
        }

        public DalConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}