using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.Services.Controllers;
using UrbaniecZelenay.SKS.Package.Services.DTOs;

namespace UrbaniecZelenay.SKS.Package.Services.Tests
{
    [ExcludeFromCodeCoverage]
    public class StaffApiTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReportParcelDelivery_ValidTrackingId_SuccessStatusReturned()
        {
            var trackingId = "PYJRB4HZ6";
            var controller = new StaffApiController();

            var result = controller.ReportParcelDelivery(trackingId);
            
            var statusCode = result as StatusCodeResult;
            Assert.NotNull(statusCode);
            Assert.AreEqual(200, statusCode.StatusCode);
        }
        
        [Test]
        public void ReportParcelDelivery_NullTrackingId_ErrorReturned()
        {
            string trackingId = null;
            var controller = new StaffApiController();

            var result = controller.ReportParcelDelivery(trackingId);
            
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var error = objectResult.Value as Error;
            Assert.NotNull(error);
            Assert.NotNull(error.ErrorMessage);
        }

        [Test]
        public void ReportParcelHop_ValidTrackingIdAndCode_SuccessStatusReturned()
        {
            var trackingId = "PYJRB4HZ6";
            var hopCode = "AAAA1234";

            var controller = new StaffApiController();

            var result = controller.ReportParcelHop(trackingId, hopCode);

            var statusCode = result as StatusCodeResult;
            Assert.NotNull(statusCode);
            Assert.AreEqual(200, statusCode.StatusCode);
        }
        
        [Test]
        public void ReportParcelHop_NullCode_ErrorReturned()
        {
            var trackingId = "PYJRB4HZ6";
            string hopCode = null;

            var controller = new StaffApiController();

            var result = controller.ReportParcelHop(trackingId, hopCode);
            
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var error = objectResult.Value as Error;
            Assert.NotNull(error);
            Assert.NotNull(error.ErrorMessage);
        }
    }
}