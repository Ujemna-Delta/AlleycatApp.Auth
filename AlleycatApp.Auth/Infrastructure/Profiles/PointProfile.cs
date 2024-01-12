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

            CreateMap<PointCompletion, PointCompletionDto>();
            CreateMap<PointCompletionDto, PointCompletion>().ForMember(r => r.Id, opt => opt.Ignore());
            CreateMap<RaceCompletion, PointCompletion>().ForMember(r => r.Id, opt => opt.Ignore());

            CreateMap<PointOrderOverride, PointOrderOverrideDto>();
            CreateMap<PointOrderOverrideDto, PointOrderOverride>().ForMember(r => r.Id, opt => opt.Ignore());
            CreateMap<PointOrderOverride, PointOrderOverride>().ForMember(r => r.Id, opt => opt.Ignore());
        }
    }
}
