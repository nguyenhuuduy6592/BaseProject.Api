using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.Api.Data.People
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DataContext dataContext;

        public PersonRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<long> CreateAsync(Person entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> ExistAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            var output = await dataContext.People
                .Select(x => new Person
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DateOfBirth = x.DateOfBirth
                })
                .ToListAsync();
            return output;
        }

        public async Task<Person> GetByIdAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Person entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
