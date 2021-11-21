using System;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DalSaveException: DalException
    {
        public DalSaveException(string message) : base(message)
        {
        }

        public DalSaveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}