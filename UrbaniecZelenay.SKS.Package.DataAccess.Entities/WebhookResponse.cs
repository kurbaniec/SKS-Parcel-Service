﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class WebhookResponse
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Key]
        public long? Id { get; set; }
        
        public Parcel Parcel { get; set; } = null!;

        /// <summary>
        /// Gets or Sets TrackingId
        /// </summary>
        [NotMapped]
        public string TrackingId { get; set; } = null!;

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