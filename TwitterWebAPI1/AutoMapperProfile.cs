using AutoMapper;
using TwitterWebAPI1.Dtos;
using TwitterWebAPI1.Model;

namespace TwitterWebAPI1
{
    public class AutoMapperProfile : Profile    
    {
        public AutoMapperProfile()
        {
            CreateMap<UserMaster, UserListDto>();
        }
    }
}
