using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using NetTopologySuite.Geometries;
using UrbaniecZelenay.SKS.Package.Services.DTOs;
using BlParcel = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Parcel;
using BlRecipient = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Recipient;
using BlWarehouse = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Warehouse;
using BlWarehouseNextHops = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.WarehouseNextHops;
using BlHop = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Hop;
using BlGeoCoordinate = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.GeoCoordinate;
using BlTruck = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Truck;
using BlTransferwarehouse = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Transferwarehouse;

namespace UrbaniecZelenay.SKS.Package.Services.Mappings
{
    /// <summary>
    /// Configure AutoMapper.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MappingsProfileSvcBl : Profile
    {
        /// <summary>
        /// Create mappings for AutoMapper.
        /// </summary>
        public MappingsProfileSvcBl()
        {
            // Does not change behavior when property does not exist on Source
            // See: https://docs.automapper.org/en/stable/Lists-and-arrays.html#handling-null-collections
            AllowNullCollections = false;
            CreateMap<Parcel, BlParcel>()
                // Map non existing properties on Source
                // See https://stackoverflow.com/a/36866422/12347616 & https://stackoverflow.com/a/38727807/12347616
                // NullSubstitute does not work when property does not exist on Source!
                //.ForMember(blParcel => blParcel.VisitedHops, opt => opt.NullSubstitute(new List<HopArrival>())).ReverseMap();
                .ForMember(blParcel => blParcel.FutureHops, opt => opt.MapFrom(x => new List<HopArrival>()))
                .ForMember(blParcel => blParcel.VisitedHops, opt => opt.MapFrom(x => new List<HopArrival>()))
                // Need to check if the if InTransport State is correct!
                .ForMember(blParcel => blParcel.State, opt => opt.MapFrom(x => BlParcel.StateEnum.InTransportEnum))
                .ReverseMap();
            CreateMap<BlParcel, TrackingInformation>();
            CreateMap<BlParcel, NewParcelInfo>();
            CreateMap<Recipient, BlRecipient>().ReverseMap();
            // Check for inheritance
            // See: https://docs.automapper.org/en/stable/Mapping-inheritance.html
            // IncludeAllDerived needs to be called on base and reversed mapping
            // See: https://stackoverflow.com/a/62085857/12347616
            CreateMap<Hop, BlHop>().IncludeAllDerived().ForMember(blHop => blHop.LocationCoordinates,
                    opt => opt.MapFrom(x
                        => new Point(x.LocationCoordinates.Lon, x.LocationCoordinates.Lat) { SRID = 4326 }))
                .ReverseMap().IncludeAllDerived().ForMember(svcHop => svcHop.LocationCoordinates, opt =>
                    opt.MapFrom(x => new BlGeoCoordinate()
                    {
                        Lon = x.LocationCoordinates.X,
                        Lat = x.LocationCoordinates.Y
                    }));
            CreateMap<Warehouse, BlWarehouse>().ReverseMap();
            CreateMap<WarehouseNextHops, BlWarehouseNextHops>().ReverseMap();
            CreateMap<Truck, BlTruck>().ReverseMap();
            CreateMap<Transferwarehouse, BlTransferwarehouse>().ReverseMap();
            CreateMap<GeoCoordinate, BlGeoCoordinate>().ReverseMap();
        }
    }
}