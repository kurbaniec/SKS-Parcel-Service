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
        
        public Hop Hop { get; set; }

        /// <summary>
        /// Unique CODE of the hop.
        /// </summary>
        /// <value>Unique CODE of the hop.</value>
        [RegularExpression(@"^[A-Z]{4}\d{1,4}$")]
        [NotMapped]
        public string Code => Hop.Code;
        
        /// <summary>
        /// Description of the hop.
        /// </summary>
        /// <value>Description of the hop.</value>
        [NotMapped]
        public string Description => Hop.Description;
        
        /// <summary>
        /// The date/time the parcel arrived at the hop.
        /// </summary>
        /// <value>The date/time the parcel arrived at the hop.</value>
        public DateTime? DateTime { get; set; }
        
        // Configure multiple one-to-many relations
        // See: https://stackoverflow.com/a/54196808/12347616
        
        [ForeignKey("VisitedHopsId")]
        public virtual Parcel VisitedHopsParcel { get; set; }
        
        [ForeignKey("FutureHopsId")]
        public virtual Parcel FutureHopsParcel { get; set; }
    }
}