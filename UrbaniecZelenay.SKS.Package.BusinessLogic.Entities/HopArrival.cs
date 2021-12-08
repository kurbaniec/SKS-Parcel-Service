using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class HopArrival
    {
        public Hop Hop { get; set; }

        /// <summary>
        /// Unique CODE of the hop.
        /// </summary>
        /// <value>Unique CODE of the hop.</value>
        [RegularExpression(@"^[A-Z]{4}\d{1,4}$")]
        public string Code => Hop.Code;

        /// <summary>
        /// Description of the hop.
        /// </summary>
        /// <value>Description of the hop.</value>
        public string Description => Hop.Description;
        /// <summary>
        /// The date/time the parcel arrived at the hop.
        /// </summary>
        /// <value>The date/time the parcel arrived at the hop.</value>
        public DateTime? DateTime { get; set; }
    }
}