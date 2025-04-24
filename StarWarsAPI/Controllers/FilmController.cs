using Microsoft.AspNetCore.Mvc;
using StarWarsAPI.Models;

namespace StarWarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : BaseStatusCodeController
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://swapi.info/api/";
        public FilmController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri($"{ BaseUrl }");
        }


        [HttpGet("filmnumber/{id}")]
        public async Task<IActionResult> GetFilm(int id)
        {
            var response = await _httpClient.GetAsync($"films/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Return HttpResponseMessage Ok Film object
                var film = await response.Content.ReadFromJsonAsync<FilmModel>();
                return Ok(film);
            }
            else
            {
                // Return HttpResponseMessage Error
                var result = HandleHttpResponse(response, id, "Film");
                return result;
            }
        }
    }
}
