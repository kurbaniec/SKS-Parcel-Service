using System;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.Services.Controllers;
using UrbaniecZelenay.SKS.Package.Services.DTOs;

namespace UrbaniecZelenay.SKS.Package.Services.Tests
{
    public class LogisticsPartnerApiTests
    {
        // MethodNameToTest_Scenario_Outcome
        // ===
        // SubmitParcel_ValidParcel_TrackingIdReturned
        // SubmitParcel_ZeroWeight_ValidationError
        // SubmitParcel_NullParcel_ArgumentExpection
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TransitionParcel_ValidParcel_TrackingIdReturned()
        {
            var validParcel = new Parcel
            {
                Weight = 100,
                Sender = new Recipient
                {
                    City = "Wien",
                    Country = "Austria",
                    Name = "Max Mustermann",
                    PostalCode = "1200",
                    Street = "Teststrasse"
                },
                Recipient = new Recipient
                {
                    City = "Wien",
                    Country = "Austria",
                    Name = "Maxine Mustermann",
                    PostalCode = "1210",
                    Street = "Teststrasse 2"
                },
            };
            var controller = new LogisticsPartnerApiController();
            var trackingId = "PYJRB4HZ6";

            var result = controller.TransitionParcel(validParcel, trackingId);
            
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var newParcelInfo = okResult.Value as NewParcelInfo;
            Assert.NotNull(newParcelInfo);
            Assert.AreEqual(trackingId, newParcelInfo.TrackingId);
        }
        
        [Test]
        public void TransitionParcel_ZeroWeight_Error()
        {
            var validParcel = new Parcel
            {
                Weight = 0,
                Sender = new Recipient
                {
                    City = "Wien",
                    Country = "Austria",
                    Name = "Max Mustermann",
                    PostalCode = "1200",
                    Street = "Teststrasse"
                },
                Recipient = new Recipient
                {
                    City = "Wien",
                    Country = "Austria",
                    Name = "Maxine Mustermann",
                    PostalCode = "1210",
                    Street = "Teststrasse 2"
                },
            };
            var controller = new LogisticsPartnerApiController();
            var trackingId = "PYJRB4HZ6";

            var result = controller.TransitionParcel(validParcel, trackingId);
            
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var error = objectResult.Value as Error;
            Assert.NotNull(error);
            Assert.NotNull(error.ErrorMessage);
        }
    }
}