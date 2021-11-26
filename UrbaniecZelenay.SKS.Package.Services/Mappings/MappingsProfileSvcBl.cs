using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
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
        private JsonSerializer geoJsonConverter = GeoJsonSerializer.Create();

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
            // GeoJSON Conversion
            // See: https://github.com/NetTopologySuite/NetTopologySuite.IO.GeoJSON
            CreateMap<Truck, BlTruck>().ForMember(blTruck => blTruck.Region,
                    opt => opt.MapFrom(x => DeserializeTruckRegion(x.RegionGeoJson)))
                .ReverseMap().ForMember(svcTruck => svcTruck.RegionGeoJson,
                    opt => opt.MapFrom(x => SerializeTruckRegion(x.Region)));
            CreateMap<Transferwarehouse, BlTransferwarehouse>().ForMember(blTw => blTw.Region,
                    opt => opt.MapFrom(x => DeserializeTruckRegion(x.RegionGeoJson)))
                .ReverseMap().ForMember(svcTw => svcTw.RegionGeoJson,
                    opt => opt.MapFrom(x => SerializeTruckRegion(x.Region)));
            CreateMap<GeoCoordinate, BlGeoCoordinate>().ReverseMap();
        }

        // Deserialize RegionGeoJson to Truck Geometry Multipolygon.
        // Note: RegionGeoJSON is delivered as a "Feature" which contains a "Geometry"
        // Usage: https://github.com/NetTopologySuite/NetTopologySuite.IO.GeoJSON
        private Geometry? DeserializeTruckRegion(string? regionGeoJson)
        {
            if (string.IsNullOrEmpty(regionGeoJson)) return null;
            Geometry? geometry = null;

            using var stringReader = new StringReader(regionGeoJson);
            using var jsonReader = new JsonTextReader(stringReader);
            var feature = geoJsonConverter.Deserialize<Feature>(jsonReader);
            if (feature != null)
                geometry = feature.Geometry;

            return geometry;
        }

        private string? SerializeTruckRegion(Geometry? geometry)
        {
            if (geometry == null) return null;
            string? regionGeoJson = null;

            Feature feature = new()
            {
                Geometry = geometry
            };
            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);
            geoJsonConverter.Serialize(jsonWriter, feature);
            regionGeoJson = stringWriter.ToString();

            return regionGeoJson;
        }
    }
}