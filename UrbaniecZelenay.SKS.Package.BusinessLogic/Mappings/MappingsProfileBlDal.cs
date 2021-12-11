using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using DalParcel = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Parcel;
using DalRecipient = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Recipient;
using DalWarehouse = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Warehouse;
using DalWarehouseNextHops = UrbaniecZelenay.SKS.Package.DataAccess.Entities.WarehouseNextHops;
using DalHop = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Hop;
using DalGeoCoordinate = UrbaniecZelenay.SKS.Package.DataAccess.Entities.GeoCoordinate;
using DalHopArrival = UrbaniecZelenay.SKS.Package.DataAccess.Entities.HopArrival;
using DalTruck = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Truck;
using DalTransferwarehouse = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Transferwarehouse;
using DalPreviousHop = UrbaniecZelenay.SKS.Package.DataAccess.Entities.PreviousHop;
using DalWebhook = UrbaniecZelenay.SKS.WebhookManager.Entities.Webhook;
using DalWebhookMessage = UrbaniecZelenay.SKS.WebhookManager.Entities.WebhookMessage;
using DalWebhookHop = UrbaniecZelenay.SKS.WebhookManager.Entities.WebhookHop;
using GeoCoordinate = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.GeoCoordinate;
using Hop = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Hop;
using HopArrival = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.HopArrival;
using Parcel = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Parcel;
using PreviousHop = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.PreviousHop;
using Recipient = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Recipient;
using Transferwarehouse = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Transferwarehouse;
using Truck = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Truck;
using Warehouse = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Warehouse;
using WarehouseNextHops = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.WarehouseNextHops;
using Webhook = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Webhook;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Mappings
{
    /// <summary>
    /// Configure AutoMapper.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MappingsProfileBlDal : Profile
    {
        /// <summary>
        /// Create mappings for AutoMapper.
        /// </summary>
        public MappingsProfileBlDal()
        {
            AllowNullCollections = false;
            CreateMap<Parcel, DalParcel>().ReverseMap();
            CreateMap<Recipient, DalRecipient>().ReverseMap();
            CreateMap<Hop, DalHop>().IncludeAllDerived().ReverseMap().IncludeAllDerived();
            CreateMap<Warehouse, DalWarehouse>().ReverseMap();
            CreateMap<WarehouseNextHops, DalWarehouseNextHops>().ReverseMap();
            CreateMap<Truck, DalTruck>().ReverseMap();
            CreateMap<Transferwarehouse, DalTransferwarehouse>().ReverseMap();
            CreateMap<GeoCoordinate, DalGeoCoordinate>().ReverseMap();
            CreateMap<PreviousHop, DalPreviousHop>().ReverseMap();
            CreateMap<HopArrival, DalHopArrival>().ReverseMap();
            CreateMap<Webhook, DalWebhook>().ReverseMap();
            // Webhooks
            CreateMap<DalHopArrival, DalWebhookHop>();
            CreateMap<DalParcel, DalWebhookMessage>();
        }
    }
}