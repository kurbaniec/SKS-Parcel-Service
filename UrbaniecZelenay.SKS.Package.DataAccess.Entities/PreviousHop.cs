using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class PreviousHop
    {
        [Key] public int Id { get; set; }

        //[ForeignKey("ChildHopCode")] 
        //[ForeignKey("OriginalHopCode")]
        //[InverseProperty("PreviousHop")]
        public string OriginalHopCode { get; set; }
        public Hop OriginalHop { set; get; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        //public string HopCode;
        public string? HopCode { get; set; }
        public Hop Hop { get; set; }

        /// <summary>
        /// Gets or Sets TraveltimeMins
        /// </summary>
        public int TraveltimeMins { get; set; }

        
    }
}