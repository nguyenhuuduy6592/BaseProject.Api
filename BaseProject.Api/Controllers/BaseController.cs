using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        public readonly IMapper mapper;

        public BaseController(IMapper mapper)
        {
            this.mapper = mapper;
        }
    }
}
