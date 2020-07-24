using AutoMapper;
using BaseProject.Api.Data.People;
using BaseProject.Api.Data.People.DTO;

namespace BaseProject.Api.Infrastructure.ConfigRegister
{
    public class AutoMapperProfileConfig : Profile
    {
        public AutoMapperProfileConfig()
        {
            CreateMap<Person, PersonQueryResponse>().ReverseMap();
        }
    }
}
