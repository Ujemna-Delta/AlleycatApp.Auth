using AlleycatApp.Auth.Models;
using AlleycatApp.Auth.Models.Dto;
using AutoMapper;

namespace AlleycatApp.Auth.Infrastructure.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskModel, TaskDto>();
            CreateMap<TaskDto, TaskModel>().ForMember(r => r.Id, opt => opt.Ignore());
            CreateMap<TaskModel, TaskModel>().ForMember(r => r.Id, opt => opt.Ignore());

            CreateMap<TaskCompletion, TaskCompletionDto>();
            CreateMap<TaskCompletionDto, TaskCompletion>().ForMember(r => r.Id, opt => opt.Ignore());
            CreateMap<TaskCompletion, TaskCompletion>().ForMember(r => r.Id, opt => opt.Ignore());
        }
    }
}
