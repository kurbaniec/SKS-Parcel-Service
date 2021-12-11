using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace UrbaniecZelenay.SKS.WebhookManager.Entities
{
    [ExcludeFromCodeCoverage]
    public class WebhookHop
    {
        /// <summary>
        /// Unique CODE of the hop.
        /// </summary>
        /// <value>Unique CODE of the hop.</value>
        [JsonProperty("code")]
        public string Code { get; set; } = null!;

        /// <summary>
        /// Description of the hop.
        /// </summary>
        /// <value>Description of the hop.</value>
        [JsonProperty("description")]
        public string Description { get; set; } = null!;
        
        /// <summary>
        /// The date/time the parcel arrived at the hop.
        /// </summary>
        /// <value>The date/time the parcel arrived at the hop.</value>
        [JsonProperty("dateTime")]
        public DateTime DateTime { get; set; }
    }
}