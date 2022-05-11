using AutoMapper;
using PMS.DTOs;
using PMS.Model;

namespace PMS.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ProjectModel, ProjectDto>();
            CreateMap<ProjectDto, ProjectModel>();
        }
    }
}
