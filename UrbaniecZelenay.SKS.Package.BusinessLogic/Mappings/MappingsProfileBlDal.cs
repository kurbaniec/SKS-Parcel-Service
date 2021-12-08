using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
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
        }
    }
}