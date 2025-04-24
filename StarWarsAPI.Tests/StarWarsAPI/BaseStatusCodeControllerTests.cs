using Microsoft.AspNetCore.Mvc;
using StarWarsAPI.Controllers;
using System.Net;

namespace StarWarsAPI.Tests.StarWarsAPI
{
    public class BaseStatusCodeControllerTests
    {
        // Test Successful status code
        [Fact]
        public void HandleHttpResponse_StatusCodeSuccessful_ReturnsOk()
        {
            // Arrange
            var controller = new TestController();
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            int id = 1;
            string resource = "Film";

            // Act
            var result = controller.MockHandleHttpResponse(response, id, resource);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
            var okResult = (OkResult)result;
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        // Test NotFound status code
        [Fact]
        public void HandleHttpResponse_StatusCodeNotFound_ReturnsNotFoundResult()
        {
            // Arange
            var controller = new TestController();
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
            int id = 9999;
            string resource = "Person";

            // Act
            var result = controller.MockHandleHttpResponse(response, id, resource);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.Equal($"{resource} with ID {id} is not found.", notFoundResult.Value);
        }

        // Test multiple status codes
        [Theory]
        [InlineData(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), "Person with ID 5 is not found.")]
        [InlineData(HttpStatusCode.BadRequest, typeof(BadRequestObjectResult), "Invalid request.")]
        [InlineData(HttpStatusCode.Unauthorized, typeof(UnauthorizedObjectResult), "Access is denied.")]
        [InlineData(HttpStatusCode.Forbidden, typeof(ObjectResult), "You do not have permission to access this resource.")]
        [InlineData(HttpStatusCode.InternalServerError, typeof(ObjectResult), "An internal server error occurred. Please try again later.")]
        public void HandleHttpsResponse_MultipleStatusCodes_ReturnsExpected(HttpStatusCode statusCode, Type expectedType, string expectedMessage)
        {
            // Arrange
            var controller = new TestController();
            var response = new HttpResponseMessage { StatusCode = statusCode };
            int id = 5;
            string resource = "Person";

            // Act
            var result = controller.MockHandleHttpResponse(response, id, resource);

            // Assert
            Assert.NotNull(result);
            Assert.IsType(expectedType, result);

            // Cast result to its parent and get the Value property which contains the message
            var resultMessage = (ObjectResult)result;
            Assert.Equal(expectedMessage, resultMessage.Value);
            Assert.Equal((int)statusCode, resultMessage.StatusCode);
        }
    }

    // TestController needed to mock the protected HandleHttpResponse method
    public class TestController : BaseStatusCodeController
    {
        public IActionResult? MockHandleHttpResponse(HttpResponseMessage response, int id, string resource)
        {
            return HandleHttpResponse(response, id, resource);
        }
    }
}