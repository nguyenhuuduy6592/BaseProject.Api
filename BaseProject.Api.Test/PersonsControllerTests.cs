using AutoMapper;
using AutoWrapper.Wrappers;
using BaseProject.Api.Controllers;
using BaseProject.Api.Data.People;
using BaseProject.Api.Data.People.DTO;
using BaseProject.Api.Infrastructure.ConfigRegister;
using BaseProject.Api.Test.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public async Task GET_Get_RETURN_Ok()
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

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GET_GetById_RETURN_Ok(int id)
        {
            // Arrange
            var person = GetPersonById(id);
            _mockPersonRepository.Setup(x => x.GetByIdAsync(id))
               .ReturnsAsync(person);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var output = Assert.IsType<PersonQueryResponse>(result);
            Assert.Equal(person.Id, output.Id);
            Assert.Equal(person.FirstName, output.FirstName);
            Assert.Equal(person.LastName, output.LastName);
            Assert.Equal(person.DateOfBirth, output.DateOfBirth);
        }

        [Theory]
        [InlineData(3)]
        public async Task GET_GetById_RETURN_Not_Found(int id)
        {
            // Arrange
            var person = GetPersonById(id);
            _mockPersonRepository.Setup(x => x.GetByIdAsync(id))
               .ReturnsAsync(person);

            // Act
            var apiException = await Assert.ThrowsAsync<ApiException>(() => _controller.GetById(id));

            // Assert
            Assert.Equal(404, apiException.StatusCode);
        }

        [Theory]
        [InlineData("FirstName")]
        [InlineData("LastName")]
        [InlineData("DateOfBirth")]
        public async Task POST_Post_RETURN_Bad_Request(string errorField)
        {
            // Arrange
            _controller.ModelState.AddModelError(errorField, "Required");

            // Act
            var apiException = await Assert.ThrowsAsync<ApiException>(() => _controller.Post(new PersonDTO()));

            // Assert
            Assert.Equal(apiException.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task POST_Post_RETURN_Ok()
        {
            // Arrange
            _mockPersonRepository.Setup(x => x.CreateAsync(It.IsAny<Person>()))
                 .ReturnsAsync(It.IsAny<long>());

            // Act
            var apiResponse = await _controller.Post(new PersonDTO());

            // Assert
            Assert.IsType<ApiResponse>(apiResponse);
            Assert.Equal(apiResponse.StatusCode, (int)HttpStatusCode.Created);
        }

        [Theory]
        [InlineData("FirstName")]
        [InlineData("LastName")]
        [InlineData("DateOfBirth")]
        public async Task PUT_Put_RETURN_Bad_Request(string errorField)
        {
            // Arrange
            _controller.ModelState.AddModelError(errorField, "Required");

            // Act
            var apiException = await Assert.ThrowsAsync<ApiException>(() => _controller.Put(0, new PersonDTO()));

            // Assert
            Assert.Equal(apiException.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PUT_Put_RETURN_Not_Found()
        {
            // Act
            var apiException = await Assert.ThrowsAsync<ApiException>(() => _controller.Put(0, new PersonDTO()));

            // Assert
            Assert.Equal(apiException.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task PUT_Put_RETURN_OK(int id)
        {
            // Arrange
            _mockPersonRepository.Setup(x => x.UpdateAsync(It.IsAny<Person>()))
                 .ReturnsAsync(true);

            // Act
            var apiResponse = await _controller.Put(id, new PersonDTO());

            // Assert
            Assert.IsType<ApiResponse>(apiResponse);
            Assert.Equal(apiResponse.StatusCode, (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task DELETE_Delete_RETURN_Not_Found()
        {
            // Act
            var apiException = await Assert.ThrowsAsync<ApiException>(() => _controller.Delete(0));

            // Assert
            Assert.Equal(apiException.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DELETE_Delete_RETURN_OK(int id)
        {
            // Arrange
            _mockPersonRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()))
                 .ReturnsAsync(true);

            // Act
            var apiResponse = await _controller.Delete(id);

            // Assert
            Assert.IsType<ApiResponse>(apiResponse);
            Assert.Equal(apiResponse.StatusCode, (int)HttpStatusCode.OK);
        }

        private Person GetPersonById(int id)
        {
            return GetFakePersonLists().FirstOrDefault(x => x.Id == id);
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
