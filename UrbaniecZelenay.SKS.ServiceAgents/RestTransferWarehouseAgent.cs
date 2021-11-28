using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using UrbaniecZelenay.SKS.ServiceAgents.Interfaces;

namespace UrbaniecZelenay.SKS.ServiceAgents
{
    public class RestTransferWarehouseAgent : ITransferWarehouseAgent
    {
        private readonly HttpClient _client;
        private readonly ILogger<RestTransferWarehouseAgent> logger;

        public RestTransferWarehouseAgent(ILogger<RestTransferWarehouseAgent> logger)
        {
            this.logger = logger;
            _client = new HttpClient();
        }

        public RestTransferWarehouseAgent(ILogger<RestTransferWarehouseAgent> logger, HttpClient client)
        {
            this.logger = logger;
            _client = client;
        }

        public bool TransferParcel(string baseUrl, string trackingId, float weight, string recipientName,
            string recipientStreet,
            string recipientPostalCode, string recipientCity, string recipientCountry, string senderName,
            string senderStreet,
            string senderPostalCode, string senderCity, string senderCountry)
        {
            string url = $"{baseUrl}/parcel/{HttpUtility.UrlEncode(trackingId)}";
            HttpResponseMessage response;
            var jsonContent = JsonContent.Create(new
            {
                weight = weight,
                recipient = new
                {
                    name = recipientName,
                    street = recipientStreet,
                    postalCode = recipientPostalCode,
                    city = recipientCity,
                    country = recipientCountry
                },
                sender = new
                {
                    name = senderName,
                    street = senderStreet,
                    postalCode = senderPostalCode,
                    city = senderCity,
                    country = senderCountry
                }
            });
            // var httpContent = new StringContent(baseUrl, Encoding.UTF8, "application/json");
            try
            {
                var task = Task.Run(() => _client.PostAsync(url, jsonContent));
                task.Wait();
                response = task.Result;
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    logger.LogError(exception, $"Error requesting {url} with body: {jsonContent}");
                }

                return false;
            }

            return response.IsSuccessStatusCode;
        }
    }
}