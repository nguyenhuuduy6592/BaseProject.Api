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
            dataContext.People.Add(entity);
            await dataContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> DeleteAsync(object id)
        {
            if (!await ExistAsync(id))
            {
                return false;
            }

            dataContext.People.Remove(new Person
            {
                Id = (int)id
            });
            await dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistAsync(object id)
        {
            return await dataContext.People.AnyAsync(x => x.Id == (int)id);
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
            var output = await dataContext.People
                .Select(x => new Person
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DateOfBirth = x.DateOfBirth
                })
                .FirstOrDefaultAsync(x => x.Id == (int)id);
            return output;
        }

        public async Task<bool> UpdateAsync(Person entity)
        {
            if (!await ExistAsync(entity.Id))
            {
                return false;
            }

            dataContext.People.Attach(entity).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();

            return true;
        }
    }
}
