using System;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Validators;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Tests
{
    [ExcludeFromCodeCoverage]
    public class StaffLogicTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReportParcelDelivery_TrackingIdNull_ExceptionThrown()
        {
            string trackingId = null;
            IStaffLogic staffLogic = new StaffLogic();
            Assert.Throws<ArgumentNullException>(() => staffLogic.ReportParcelDelivery(trackingId)); 
        }

        [Test]
        public void ReportParcelHop_TrackingIdNull_ExceptionThrown()
        {
            string trackingId = null;
            string code = "AUTA05";
            IStaffLogic staffLogic = new StaffLogic();
            Assert.Throws<ArgumentNullException>(() => staffLogic.ReportParcelHop(trackingId, code)); 
        }
        
        [Test]
        public void ReportParcelHop_CodeNull_ExceptionThrown()
        {
            string trackingId = "PYJRB4HZ6";
            string code = null;
            IStaffLogic staffLogic = new StaffLogic();
            Assert.Throws<ArgumentNullException>(() => staffLogic.ReportParcelHop(trackingId, code)); 
        }
        [Test]
        public void TrackParcel_ValidTrackingId_ParcelReturned()
        {
            string trackingId = "PYJRB4HZ6";            
            IRecipientLogic recipientLogic = new RecipientLogic();
            Parcel blParcel = recipientLogic.TrackParcel(trackingId);
            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var validationResult = parcelValidator.Validate(blParcel);
            Assert.IsTrue(validationResult.IsValid);
        }
        [Test]
        public void TrackParcel_TrackingIdNull_ExceptionThrown()
        {
            string trackingId = null;            
            IRecipientLogic recipientLogic = new RecipientLogic();
            Assert.Throws<ArgumentNullException>(() => recipientLogic.TrackParcel(trackingId));
        }
    }
}