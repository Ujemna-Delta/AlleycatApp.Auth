using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Dto;
using AutoMapper;

namespace AlleycatApp.Auth.Infrastructure.Profiles
{
    public class RaceProfile : Profile
    {
        public RaceProfile()
        {
            CreateMap<Race, RaceDto>();
            CreateMap<RaceDto, Race>();
        }
    }
}
