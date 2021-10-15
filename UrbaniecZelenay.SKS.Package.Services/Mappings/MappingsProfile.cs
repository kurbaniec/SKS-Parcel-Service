using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using UrbaniecZelenay.SKS.Package.Services.DTOs;
using BlParcel = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Parcel;
using BlRecipient = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Recipient;
using BlWarehouse = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Warehouse;
using BlWarehouseNextHops = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.WarehouseNextHops;
using BlHop = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Hop;
using BlGeoCoordinate = UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.GeoCoordinate;

namespace UrbaniecZelenay.SKS.Package.Services.Mappings
{
    /// <summary>
    /// Configure AutoMapper.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MappingsProfile : Profile
    {
        /// <summary>
        /// Create mappings for AutoMapper.
        /// </summary>
        public MappingsProfile()
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
                .ReverseMap();
            CreateMap<BlParcel, TrackingInformation>();
            CreateMap<BlParcel, NewParcelInfo>();
            CreateMap<Recipient, BlRecipient>().ReverseMap();
            CreateMap<Warehouse, BlWarehouse>().ReverseMap();
            CreateMap<WarehouseNextHops, BlWarehouseNextHops>().ReverseMap();
            CreateMap<Hop, BlHop>().ReverseMap();
            CreateMap<GeoCoordinate, BlGeoCoordinate>().ReverseMap();
        }
    }
}