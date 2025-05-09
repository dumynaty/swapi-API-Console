﻿using Microsoft.AspNetCore.Mvc;
using StarWarsAPI.Models;

namespace StarWarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : BaseStatusCodeController
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://swapi.info/api/";
        public PeopleController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri($"{ BaseUrl }");
        }


        [HttpGet("person/{id}")]
        public async Task<IActionResult> GetPerson(int id)
        {
            var response = await _httpClient.GetAsync($"people/{id}");

            if (response.IsSuccessStatusCode)
            {
                var person = await response.Content.ReadFromJsonAsync<PersonModel>();
                return Ok(person);
            }
            else
            {
                var errorStatusCode = HandleHttpResponse(response, id, "Person");
                return errorStatusCode;
            }
        }

        [HttpGet("fullperson/{id}")]
        public async Task<IActionResult> GetFullPersonInfo(int id)
        {
            var response = await _httpClient.GetAsync($"people/{id}");

            if (response.IsSuccessStatusCode)
            {
                var person = await response.Content.ReadFromJsonAsync<FullPersonModel>();
                return Ok(person);
            }
            else
            {
                var errorStatusCode = HandleHttpResponse(response, id, "FullPerson");
                return errorStatusCode;
            }
        }
    }
}
