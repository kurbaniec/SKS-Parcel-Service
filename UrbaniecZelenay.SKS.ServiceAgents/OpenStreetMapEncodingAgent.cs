using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Linq;
using BlGeoCoordinate = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.GeoCoordinate;
using UrbaniecZelenay.SKS.ServiceAgents.Interfaces;

namespace UrbaniecZelenay.SKS.ServiceAgents
{
    public class OpenStreetMapEncodingAgent : IGeoEncodingAgent
    {
        private readonly HttpClient _client;
        private readonly ILogger<OpenStreetMapEncodingAgent> logger;


        public OpenStreetMapEncodingAgent(ILogger<OpenStreetMapEncodingAgent> logger)
        {
            this.logger = logger;
            _client = new HttpClient();
        }

        public OpenStreetMapEncodingAgent(HttpClient client, ILogger<OpenStreetMapEncodingAgent> logger)
        {
            _client = client;
            this.logger = logger;
        }

        public Point? EncodeAddress(string street, string postalCode, string city, string country)
        {
            Point? geoCoordinate = null;
            // HTTP synchronous request: https://stackoverflow.com/questions/14435520/why-use-httpclient-for-synchronous-connection
            var streetParsed = ExtractStreetNumber(street);
            string url =
                $"https://nominatim.openstreetmap.org/search?street={streetParsed.housenumber}" +
                $"{HttpUtility.UrlEncode(' ' + streetParsed.streetname)}&" +
                $"postalcode={HttpUtility.UrlEncode(postalCode)}&city={HttpUtility.UrlEncode(city)}&" +
                $"country={HttpUtility.UrlEncode(country)}&format=jsonv2";

            _client.DefaultRequestHeaders.Referrer = new Uri("https://sks-team-x.azurewebsites.net/");
            HttpResponseMessage response;
            try
            {
                var task = Task.Run(() => _client.GetAsync(url));
                task.Wait();
                response = task.Result;
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    if (e is HttpRequestException)
                    {
                        logger.LogError(e, $"Error requesting {url}.");
                        return null;
                    }

                    logger.LogError(e, "Error unhandled Exception!");
                }

                return null;
            }

            if (!response.IsSuccessStatusCode) return geoCoordinate;
            var responseContent = response.Content;

            // by calling .Result you are synchronously reading the result
            string responseString = responseContent.ReadAsStringAsync().Result;

            JArray parsedResult = JArray.Parse(responseString);
            if (parsedResult.Count <= 0) return geoCoordinate;
            var lat = parsedResult[0]["lat"]?.Value<double>();
            var lon = parsedResult[0]["lon"]?.Value<double>();
            var displayName = parsedResult[0]["display_name"]?.Value<string>();
            // TODO Check if display name matches address??
            // issue how? because request language does not always match result language e.g. Wien - Vienna
            if (lat != null && lon != null && displayName != null)
            {
                geoCoordinate = new Point((double)lon, (double)lat)
                {
                    SRID = 4326
                };
            }


            return geoCoordinate;
        }

        private readonly struct Street
        {
            public string streetname { get; init; }
            public string housenumber { get; init; }
        }

        // See: https://nominatim.org/release-docs/develop/api/Search/
        private Street ExtractStreetNumber(string street)
        {
            var check = street.LastIndexOf(" ", StringComparison.Ordinal);
            if (check != -1)
            {
                var digits = street[(check + 1)..];
                if (digits is { Length: > 0 } && digits.All(char.IsDigit))
                {
                    var streetname = street.Substring(0, check);
                    return new Street()
                    {
                        streetname = streetname,
                        housenumber = digits
                    };
                }
            }

            return new Street()
            {
                streetname = street,
                housenumber = ""
            };
        }
    }
}