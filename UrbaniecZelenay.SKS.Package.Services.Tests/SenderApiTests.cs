using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.Services.Controllers;
using UrbaniecZelenay.SKS.Package.Services.DTOs;

namespace UrbaniecZelenay.SKS.Package.Services.Tests
{
    [ExcludeFromCodeCoverage]
    public class SenderApiTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SubmitParcel_ValidParcel_NewParcelInfoReturned()
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
            var controller = new SenderApiController();

            var result = controller.SubmitParcel(validParcel);
            
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var parcelInfo = objectResult.Value as NewParcelInfo;
            Assert.NotNull(parcelInfo);
        }
        
        [Test]
        public void SubmitParcel_NullParcel_ErrorReturned()
        {
            Parcel nullParcel = null;
            var controller = new SenderApiController();

            var result = controller.SubmitParcel(nullParcel);
            
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            var error = objectResult.Value as Error;
            Assert.NotNull(error);
            Assert.NotNull(error.ErrorMessage);
        }
    }
}