using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class GeoCoordinate
    {
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// Latitude of the coordinate.
        /// </summary>
        /// <value>Latitude of the coordinate.</value>
        public double Lat { get; set; }

        /// <summary>
        /// Longitude of the coordinate.
        /// </summary>
        /// <value>Longitude of the coordinate.</value>
        public double Lon { get; set; }
    }
}