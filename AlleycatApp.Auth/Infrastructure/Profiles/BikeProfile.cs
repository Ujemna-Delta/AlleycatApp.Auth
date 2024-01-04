using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Models;
using AutoMapper;

namespace AlleycatApp.Auth.Infrastructure.Profiles
{
    public class BikeProfile : Profile
    {
        public BikeProfile()
        {
            CreateMap<Bike, BikeDto>();
            CreateMap<BikeDto, Bike>()
                .ForMember(r => r.Id, opt => opt.Ignore())
                .ForMember(r => r.AttendeeId, opt => opt.Ignore());
            CreateMap<Bike, Bike>().ForMember(r => r.Id, opt => opt.Ignore());
        }
    }
}
