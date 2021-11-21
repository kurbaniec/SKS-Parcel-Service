using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
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

        public BlGeoCoordinate EncodeAddress(string street, string postalCode, string city, string country)
        {
            BlGeoCoordinate geoCoordinate = null;
            // HTTP synchronous request: https://stackoverflow.com/questions/14435520/why-use-httpclient-for-synchronous-connection
            string url =
                $"https://nominatim.openstreetmap.org/search.php?street={HttpUtility.UrlEncode(street)}&" +
                $"city={HttpUtility.UrlEncode(postalCode)} {HttpUtility.UrlEncode(city)}&" +
                $"county={HttpUtility.UrlEncode(country)}&format=jsonv2";

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

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;

                // by calling .Result you are synchronously reading the result
                string responseString = responseContent.ReadAsStringAsync().Result;
                JArray parsedResult = JArray.Parse(responseString);
                double? lat = parsedResult[0]?["lat"]?.Value<double>();
                double? lon = parsedResult[0]?["lon"]?.Value<double>();
                string? displayName = parsedResult[0]?["display_name"]?.Value<string>();
                // TODO Check if display name matches address??
                // issue how? because request language does not always match result language e.g. Wien - Vienna
                if (lat != null && lon != null && displayName != null)
                {
                    geoCoordinate = new BlGeoCoordinate
                    {
                        Lat = (double)lat,
                        Lon = (double)lon
                    };
                }
            }


            return geoCoordinate;
        }
    }
}