using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using StarWarsAPI.Controllers;
using StarWarsAPI.Models;
using System.Net;
using System.Net.Http.Json;

namespace StarWarsAPI.Tests.StarWarsAPI
{
    public class PeopleControllerTests
    {
        // Test Successful swapi response return
        [Fact]
        public async Task GetPerson_NullResult_ReturnsPerson()
        {
            // Arrange
            var mockPerson = new PersonModel()
            {
                Name = "Luke Skywalker",
                Height = "172",
                Mass = "77"
            };
            var jsonMockPersonContent = JsonContent.Create(mockPerson);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>
                (
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = jsonMockPersonContent
                });

            var mockClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://swapi.info/api/")
            };

            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockClient);

            var controller = new PeopleController(mockFactory.Object);

            // Act
            var result = await controller.GetPerson(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPerson = Assert.IsType<PersonModel>(okResult.Value);

            Assert.Equal("Luke Skywalker", returnedPerson.Name);
            Assert.Equal("172", returnedPerson.Height);
            Assert.Equal("77", returnedPerson.Mass);
        }

        // Test Unauthorized swapi response and return the error specific object
        [Fact]
        public async Task GetPerson_UnauthorizedResult_ReturnsError()
        {
            // Arrange
            var mockPerson = new PersonModel()
            {
                Name = "Luke Skywalker",
                Height = "172",
                Mass = "77"
            };
            var jsonMockPersonContent = JsonContent.Create(mockPerson);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>
                (
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = jsonMockPersonContent
                });

            var mockClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://swapi.info/api/")
            };

            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockClient);

            var controller = new PeopleController(mockFactory.Object);

            // Act
            var result = await controller.GetPerson(1);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Contains("Access is denied.", unauthorizedResult.Value?.ToString());
        }
    }
}
