using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class Warehouse : Hop
    {
        /// <summary>
        /// Gets or Sets Level
        /// </summary>
        [Required]
        public int? Level { get; set; }

        /// <summary>
        /// Next hops after this warehouse (warehouses or trucks).
        /// </summary>
        /// <value>Next hops after this warehouse (warehouses or trucks).</value>
        [Required]
        public List<WarehouseNextHops> NextHops { get; set; }
    }
}