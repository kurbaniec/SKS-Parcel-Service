using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class Transferwarehouse : Hop
    {
        /// <summary>
        /// GeoJSON of the are covered by the logistics partner.
        /// </summary>
        /// <value>GeoJSON of the are covered by the logistics partner.</value>
        [Required]
        public string RegionGeoJson { get; set; }

        /// <summary>
        /// Name of the logistics partner.
        /// </summary>
        /// <value>Name of the logistics partner.</value>
        [Required]
        public string LogisticsPartner { get; set; }

        /// <summary>
        /// BaseURL of the logistics partner&#x27;s REST service.
        /// </summary>
        /// <value>BaseURL of the logistics partner&#x27;s REST service.</value>
        [Required]
        public string LogisticsPartnerUrl { get; set; }
    }
}