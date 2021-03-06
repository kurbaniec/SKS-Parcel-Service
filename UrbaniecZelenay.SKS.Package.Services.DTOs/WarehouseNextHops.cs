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
    public partial class WarehouseNextHops
    {
        /// <summary>
        /// Gets or Sets TraveltimeMins
        /// </summary>
        [Required]
        [DataMember(Name = "traveltimeMins")]
        public int? TraveltimeMins { get; set; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        [Required]
        [DataMember(Name = "hop")]
        public Hop Hop { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class WarehouseNextHops {\n");
            sb.Append("  TraveltimeMins: ").Append(TraveltimeMins).Append("\n");
            sb.Append("  Hop: ").Append(Hop).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
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
                if (TraveltimeMins != null)
                    hashCode = hashCode * 59 + TraveltimeMins.GetHashCode();
                if (Hop != null)
                    hashCode = hashCode * 59 + Hop.GetHashCode();
                return hashCode;
            }
        }
    }
}