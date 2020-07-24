using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseProject.Api.Data.People;
using BaseProject.Api.Data.People.DTO;
using AutoWrapper.Wrappers;
using BaseProject.Api.Infrastructure.Languages;
using System.Net;
using System.Linq;
using BaseProject.Api.Infrastructure.Helpers;

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

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PersonQueryResponse), (int)HttpStatusCode.OK)]
        public async Task<PersonQueryResponse> Get([FromRoute] int id)
        {
            var data = await personRepository.GetByIdAsync(id);
            if (data == null)
            {
                throw new ApiException(string.Format(Messages.ResourceNotFound, id), 404);
            }
            var person = mapper.Map<PersonQueryResponse>(data);
            return person;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<ApiResponse> Post([FromBody] PersonDTO model)
        {
            if (!ModelState.IsValid)
            {
                throw new ApiException(ModelState.GetFirstError());
            }

            var person = mapper.Map<Person>(model);
            var createdId = await personRepository.CreateAsync(person);
            return new ApiResponse(Messages.CreatedSucessfully, createdId, (int) HttpStatusCode.Created);
        }
    }
}
