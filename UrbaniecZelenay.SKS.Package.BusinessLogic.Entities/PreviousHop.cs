using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class PreviousHop
    {
        /// <summary>
        /// Gets or Sets TraveltimeMins
        /// </summary>
        [Required]
        public int TraveltimeMins { get; set; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        [Required]
        public Hop Hop { get; set; }
    }
}