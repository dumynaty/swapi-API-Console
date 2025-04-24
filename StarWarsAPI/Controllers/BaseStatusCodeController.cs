using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace StarWarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseStatusCodeController : ControllerBase
    {
        // Returns null when StatusCode is 200 OK
        protected IActionResult? HandleHttpResponse(HttpResponseMessage response, int id, string resource)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return null;
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound($"{resource} with ID {id} is not found.");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest("Invalid request.");
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized("Access is denied.");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return StatusCode(403, "You do not have permission to access this resource.");
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
            else
            {
                return StatusCode((int)response.StatusCode, "An unknown error occurred.");
            }
        }
    }
}
