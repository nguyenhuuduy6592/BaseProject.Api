using AutoMapper;

namespace BaseProject.Api.Controllers
{
    public class BaseController
    {
        public readonly IMapper mapper;

        public BaseController(IMapper mapper)
        {
            this.mapper = mapper;
        }
    }
}
