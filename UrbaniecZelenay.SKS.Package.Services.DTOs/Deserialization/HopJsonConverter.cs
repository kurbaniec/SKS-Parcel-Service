using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace UrbaniecZelenay.SKS.Package.Services.DTOs.Deserialization
{
    // Customize JsonConverter to implement polymorphic data binding
    // See: https://www.tutorialdocs.com/article/webapi-data-binding.html
    [ExcludeFromCodeCoverage]
    public class HopJsonConverter : JsonCreationConverter<Hop>
    {
        protected override Hop Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["numberPlate"] != null)
            {
                return new Truck();
            }

            if (jObject["level"] != null)
            {
                return new Warehouse();
            }

            if (jObject["logisticsPartner"] != null)
            {
                return new Transferwarehouse();
            }

            return new Hop();
        }
    }
}