using System;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class Webhook
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public long? Id { get; set; }

        public Parcel Parcel { get; set; } = null!;

        /// <summary>
        /// Gets or Sets TrackingId
        /// </summary>
        public string TrackingId => Parcel.TrackingId!;

        /// <summary>
        /// Gets or Sets Url
        /// </summary>
        public string Url { get; set; } = null!;

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>
        public DateTime? CreatedAt { get; set; }
    }
}