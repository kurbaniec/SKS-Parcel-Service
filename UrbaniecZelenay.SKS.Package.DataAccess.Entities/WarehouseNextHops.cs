using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class WarehouseNextHops
    {
        [Key] public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets TraveltimeMins
        /// </summary>
        public int TraveltimeMins { get; set; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        public Hop Hop { get; set; }
    }
}