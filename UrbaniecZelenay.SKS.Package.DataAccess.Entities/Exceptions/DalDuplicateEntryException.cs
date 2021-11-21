using System;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DalDuplicateEntryException : DalException
    {
        public DalDuplicateEntryException(string message) : base(message)
        {
        }

        public DalDuplicateEntryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}