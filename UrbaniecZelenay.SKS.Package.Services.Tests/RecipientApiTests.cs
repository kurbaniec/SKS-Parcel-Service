using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.Services.Controllers;
using UrbaniecZelenay.SKS.Package.Services.DTOs;

namespace UrbaniecZelenay.SKS.Package.Services.Tests
{
    [ExcludeFromCodeCoverage]
    public class RecipientApiTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TrackParcel_ValidTrackingId_TrackingInformationReturned()
        {
            var trackingId = "PYJRB4HZ6";
            var controller = new RecipientApiController();

            var result = controller.TrackParcel(trackingId);
            
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var trackingInfo = objectResult.Value as TrackingInformation;
            Assert.NotNull(trackingInfo);
        }
        
        [Test]
        public void TrackParcel_NullTrackingId_ErrorReturned()
        {
            string trackingId = null;
            var controller = new RecipientApiController();

            var result = controller.TrackParcel(trackingId);
            
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var error = objectResult.Value as Error;
            Assert.NotNull(error);
            Assert.NotNull(error.ErrorMessage);
        }
    }
}