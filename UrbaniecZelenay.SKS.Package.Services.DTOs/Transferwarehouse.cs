/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace UrbaniecZelenay.SKS.Package.Services.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [ExcludeFromCodeCoverage]
    public partial class Transferwarehouse : Hop
    {
        /// <summary>
        /// GeoJSON of the are covered by the logistics partner.
        /// </summary>
        /// <value>GeoJSON of the are covered by the logistics partner.</value>
        [Required]
        [DataMember(Name = "regionGeoJson")]
        public string RegionGeoJson { get; set; }

        /// <summary>
        /// Name of the logistics partner.
        /// </summary>
        /// <value>Name of the logistics partner.</value>
        [Required]
        [DataMember(Name = "logisticsPartner")]
        public string LogisticsPartner { get; set; }

        /// <summary>
        /// BaseURL of the logistics partner&#x27;s REST service.
        /// </summary>
        /// <value>BaseURL of the logistics partner&#x27;s REST service.</value>
        [Required]
        [DataMember(Name = "logisticsPartnerUrl")]
        public string LogisticsPartnerUrl { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Transferwarehouse {\n");
            sb.Append("  RegionGeoJson: ").Append(RegionGeoJson).Append("\n");
            sb.Append("  LogisticsPartner: ").Append(LogisticsPartner).Append("\n");
            sb.Append("  LogisticsPartnerUrl: ").Append(LogisticsPartnerUrl).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public new string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                if (RegionGeoJson != null)
                    hashCode = hashCode * 59 + RegionGeoJson.GetHashCode();
                if (LogisticsPartner != null)
                    hashCode = hashCode * 59 + LogisticsPartner.GetHashCode();
                if (LogisticsPartnerUrl != null)
                    hashCode = hashCode * 59 + LogisticsPartnerUrl.GetHashCode();
                return hashCode;
            }
        }
    }
}