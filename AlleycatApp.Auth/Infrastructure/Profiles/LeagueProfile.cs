using AlleycatApp.Auth.Models.Dto;
using AlleycatApp.Auth.Models;
using AutoMapper;

namespace AlleycatApp.Auth.Infrastructure.Profiles
{
    public class LeagueProfile : Profile
    {
        public LeagueProfile()
        {
            CreateMap<League, LeagueDto>();
            CreateMap<LeagueDto, League>().ForMember(r => r.Id, opt => opt.Ignore());
            CreateMap<League, League>().ForMember(r => r.Id, opt => opt.Ignore());

            CreateMap<LeagueScore, LeagueScoreDto>();
            CreateMap<LeagueScoreDto, LeagueScore>().ForMember(r => r.Id, opt => opt.Ignore());
            CreateMap<LeagueScore, LeagueScore>().ForMember(r => r.Id, opt => opt.Ignore());
        }
    }
}
