using AutoMapper;
using PMS.Dtos;
using PMS.Model;

namespace PMS.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserAccountModel, UserAccountDto>();
            CreateMap<UserAccountDto, UserAccountModel>();
        }
        
    }
}
