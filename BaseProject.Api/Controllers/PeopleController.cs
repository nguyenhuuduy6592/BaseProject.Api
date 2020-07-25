using AutoMapper;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using BaseProject.Api.Data.People;
using BaseProject.Api.Data.People.DTO;
using BaseProject.Api.Infrastructure.Languages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

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
        [ProducesResponseType(typeof(IEnumerable<PersonQueryResponse>), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<PersonQueryResponse>> Get()
        {
            var data = await personRepository.GetAllAsync();
            var persons = mapper.Map<IEnumerable<PersonQueryResponse>>(data);
            return persons;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PersonQueryResponse), (int)HttpStatusCode.OK)]
        public async Task<PersonQueryResponse> GetById([FromRoute] int id)
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
                throw new ApiException(ModelState.AllErrors());
            }

            var person = mapper.Map<Person>(model);
            var createdId = await personRepository.CreateAsync(person);
            return new ApiResponse(Messages.CreatedSucessfully, createdId, (int)HttpStatusCode.Created);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<ApiResponse> Put([FromRoute] int id, [FromBody] PersonDTO model)
        {
            if (!ModelState.IsValid)
            {
                throw new ApiException(ModelState.AllErrors());
            }

            var person = mapper.Map<Person>(model);
            person.Id = id;
            if (await personRepository.UpdateAsync(person))
            {
                return new ApiResponse(Messages.UpdatedSuccessfully, true);
            }
            else
            {
                throw new ApiException(string.Format(Messages.ResourceNotFound, id), 404);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<ApiResponse> Delete([FromRoute] int id)
        {
            if (await personRepository.DeleteAsync(id))
            {
                return new ApiResponse(Messages.DeletedSuccessfully, true);
            }
            else
            {
                throw new ApiException(string.Format(Messages.ResourceNotFound, id), 404);
            }
        }
    }
}
