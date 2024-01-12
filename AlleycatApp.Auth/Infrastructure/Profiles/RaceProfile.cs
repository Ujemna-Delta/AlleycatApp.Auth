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
            CreateMap<RaceDto, Race>().ForMember(r => r.Id, opt => opt.Ignore());
            CreateMap<Race, Race>().ForMember(r => r.Id, opt => opt.Ignore());

            CreateMap<RaceCompletion, RaceCompletionDto>();
            CreateMap<RaceCompletionDto, RaceCompletion>().ForMember(r => r.Id, opt => opt.Ignore());
            CreateMap<RaceCompletion, RaceCompletion>().ForMember(r => r.Id, opt => opt.Ignore());

            CreateMap<RaceAttendance, RaceAttendanceDto>();
            CreateMap<RaceAttendanceDto, RaceAttendance>().ForMember(r => r.Id, opt => opt.Ignore());
            CreateMap<RaceAttendance, RaceAttendance>().ForMember(r => r.Id, opt => opt.Ignore());
        }
    }
}
