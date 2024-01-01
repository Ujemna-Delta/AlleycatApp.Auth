using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Models;
using AutoMapper;

namespace AlleycatApp.Auth.Infrastructure.Profiles
{
    public class PointProfile : Profile
    {
        public PointProfile()
        {
            CreateMap<Point, PointDto>();
            CreateMap<PointDto, Point>().ForMember(r => r.Id, opt => opt.Ignore());
            CreateMap<Point, Point>().ForMember(r => r.Id, opt => opt.Ignore());
        }
    }
}
