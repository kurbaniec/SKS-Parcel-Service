using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class GeoCoordinate
    {
        /// <summary>
        /// Latitude of the coordinate.
        /// </summary>
        /// <value>Latitude of the coordinate.</value>
        [Required]
        public double Lat { get; set; }

        /// <summary>
        /// Longitude of the coordinate.
        /// </summary>
        /// <value>Longitude of the coordinate.</value>
        [Required]
        public double Lon { get; set; }
    }
}