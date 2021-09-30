using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using UrbaniecZelenay.SKS.Package.Services.DTOs;
using BlParcel=UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Parcel;
using BlRecipient=UrbaniecZelenay.SKS.Package.BusinessLogic.Entities.Recipient;

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
            CreateMap<Parcel, BlParcel>().ReverseMap();
            CreateMap<BlParcel, TrackingInformation>();
            CreateMap<BlParcel, NewParcelInfo>();
            CreateMap<Recipient, BlRecipient>().ReverseMap();
        }
    }
}