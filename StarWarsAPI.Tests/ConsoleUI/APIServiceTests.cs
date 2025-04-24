using System.Net;
using ConsoleUI.Services;
using Moq;
using Moq.Protected;
using ConsoleUI.Models;
using System.Net.Http.Json;

namespace StarWarsAPI.Tests.ConsoleUI
{
    public class APIServiceTests
    {
        // Test Successful response - should return the PersonModel object
        [Fact]
        public async Task SafeApiCall_SuccessfulResponse_ReturnsDeserializedObject()
        {
            // Arrange
            var mockPerson = new PersonModel { Name = "Luke Skywalker", Height = "172", Mass = "77" };
            var jsonMockPersonContent = JsonContent.Create(mockPerson);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
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
                BaseAddress = new Uri("https://localhost:44360/")
            };

            var apiService = new APIService(mockClient);

            // Act
            var person = await apiService.SafeApiCall<PersonModel>("api/people/person/1");

            // Assert
            Assert.NotNull(person);
            Assert.IsType<PersonModel>(person);
            Assert.Equal("Luke Skywalker", person.Name);
            Assert.Equal("172", person.Height);
            Assert.Equal("77", person.Mass);

            mockHttpMessageHandler
                .Protected()
                .Verify
                (
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>
                    (
                        req => req.Method == HttpMethod.Get &&
                        req.RequestUri != null &&
                        req.RequestUri.ToString() == "https://localhost:44360/api/people/person/1"
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

        // Test NotFound response - should return default/null
        [Fact]
        public async Task SafeApiCall_NotFoundResponse_ReturnsDefault()
        {
            // Arrange
            string errorBody = "Person with ID 9999 is not found.";
            var errorContent = new StringContent(errorBody);

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>
                (
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = errorContent
                });

            var mockClient = new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44360/")
            };

            var apiService = new APIService(mockClient);

            // Act
            var person = await apiService.SafeApiCall<PersonModel>("api/people/person/9999");

            // Assert
            Assert.Null(person);

            mockMessageHandler
                .Protected()
                .Verify
                (
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>
                    (
                        req => req.Method == HttpMethod.Get &&
                        req.RequestUri != null &&
                        req.RequestUri.ToString() == "https://localhost:44360/api/people/person/9999"
                        // req.RequestUri.ToString() == "https://localhost:44360/api/people/person/1"
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
    }
}