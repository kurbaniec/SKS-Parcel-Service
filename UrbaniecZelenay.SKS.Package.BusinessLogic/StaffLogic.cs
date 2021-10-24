using System;
using AutoMapper;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class StaffLogic : IStaffLogic
    {
        private readonly IParcelRepository parcelRepository;
        private readonly IWarehouseRepository warehouseRepository;
        private readonly IMapper mapper;

        public StaffLogic(IParcelRepository parcelRepository, IWarehouseRepository warehouseRepository, IMapper mapper)
        {
            this.parcelRepository = parcelRepository;
            this.warehouseRepository = warehouseRepository;
            this.mapper = mapper;
        }

        // TODO create NotFoundException
        public void ReportParcelDelivery(string? trackingId)
        {
            if (trackingId == null)
            {
                throw new ArgumentNullException(nameof(trackingId));
            }

            var dalParcel = parcelRepository.GetByTrackingId(trackingId);
            if (dalParcel == null) return;
            var blParcel = mapper.Map<Parcel>(dalParcel);
            blParcel.State = Parcel.StateEnum.DeliveredEnum;
            dalParcel = mapper.Map<DalParcel>(blParcel);
            parcelRepository.Update(dalParcel);
        }

        public void ReportParcelHop(string? trackingId, string? code)
        {
            if (trackingId == null)
            {
                throw new ArgumentNullException(nameof(trackingId));
            }

            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            // TODO test this with database
            // property hop must be generated from BlHopArrvial info
            
            // Query hop from DAL
            // Convert to BL 
            // Create HopArrival from it
            // Add HopArrival to parcel
            // Convert to DAL
            
            var dalParcel = parcelRepository.GetByTrackingId(trackingId);
            if (dalParcel == null) return;
            var dalHop = warehouseRepository.GetHopByCode(code);
            if (dalHop == null) return;

            var blParcel = mapper.Map<Parcel>(dalParcel);
            var blHop = mapper.Map<Hop>(dalHop);

            var hopArrival = new HopArrival()
            {
                Code = blHop.Code,
                DateTime = DateTime.Now,
                Description = blHop.Description
            };
            blParcel.VisitedHops.Add(hopArrival);

            dalParcel = mapper.Map<DalParcel>(blParcel);
            parcelRepository.Update(dalParcel);
        }
    }
}