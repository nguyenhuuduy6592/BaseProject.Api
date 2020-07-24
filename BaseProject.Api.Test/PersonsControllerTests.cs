using AutoMapper;
using BaseProject.Api.Controllers;
using BaseProject.Api.Data.People;
using BaseProject.Api.Data.People.DTO;
using BaseProject.Api.Infrastructure.ConfigRegister;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BaseProject.Api.Test
{
    public class PersonsControllerTests
    {
        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly PeopleController _controller;

        public PersonsControllerTests()
        {
            var mapperProfile = new AutoMapperProfileConfig();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            var mapper = new Mapper(configuration);

            _mockPersonRepository = new Mock<IPersonRepository>();

            _controller = new PeopleController(mapper, _mockPersonRepository.Object);
        }

        [Fact]
        public async Task Get_Return_OK()
        {
            // Arrange
            _mockPersonRepository.Setup(x => x.GetAllAsync())
               .ReturnsAsync(GetFakePersonLists());

            // Act
            var result = await _controller.Get();

            // Assert
            var persons = Assert.IsType<List<PersonQueryResponse>>(result);
            Assert.Equal(2, persons.Count);
        }

        private IEnumerable<Person> GetFakePersonLists()
        {
            return new List<Person>
            {
                new Person()
                {
                    Id = 1,
                    FirstName = "Vynn Markus",
                    LastName = "Durano",
                    DateOfBirth = Convert.ToDateTime("01/15/2016")
                },
                new Person()
                {
                    Id = 2,
                    FirstName = "Vianne Maverich",
                    LastName = "Durano",
                    DateOfBirth = Convert.ToDateTime("02/15/2016")
                }
            };
        }

    }
}
