using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using NetTopologySuite.Geometries;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class Truck : Hop
    {
        /// <summary>
        /// GeoJSON of the are covered by the truck.
        /// </summary>
        /// <value>GeoJSON of the are covered by the truck.</value>
        [Required]
        public Geometry Region { get; set; }

        /// <summary>
        /// The truck&#x27;s number plate.
        /// </summary>
        /// <value>The truck&#x27;s number plate.</value>
        [Required]
        public string NumberPlate { get; set; }
    }
}