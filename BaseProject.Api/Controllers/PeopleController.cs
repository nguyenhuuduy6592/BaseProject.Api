using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseProject.Api.Data.People;
using BaseProject.Api.Data.People.DTO;

namespace BaseProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : BaseController
    {
        private readonly IPersonRepository personRepository;

        public PeopleController(IMapper mapper, IPersonRepository personRepository) : base(mapper)
        {
            this.personRepository = personRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonQueryResponse>> Get()
        {
            var data = await personRepository.GetAllAsync();
            var persons = mapper.Map<IEnumerable<PersonQueryResponse>>(data);
            return persons;
        }
    }
}
