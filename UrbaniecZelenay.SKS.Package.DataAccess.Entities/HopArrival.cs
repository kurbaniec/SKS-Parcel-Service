using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class HopArrival
    {
        [Key] public int Id { get; set; }

        private Hop hop { get; set; }
        
        /// <summary>
        /// The date/time the parcel arrived at the hop.
        /// </summary>
        /// <value>The date/time the parcel arrived at the hop.</value>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Unique CODE of the hop.
        /// </summary>
        /// <value>Unique CODE of the hop.</value>
        [RegularExpression(@"^[A-Z]{4}\d{1,4}$")]
        [NotMapped]
        public string Code => hop.Code;

        /// <summary>
        /// Description of the hop.
        /// </summary>
        /// <value>Description of the hop.</value>
        [NotMapped]
        public string Description => hop.Description;
    }
}