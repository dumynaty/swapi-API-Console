using ConsoleUI.Exceptions;
using System.Net.Http.Json;
using System.Text.Json;

namespace ConsoleUI.Services
{
    public class APIService
    {
        private readonly HttpClient _client;
        public APIService(HttpClient client)
        {
            _client = client;
        }


        public async Task<T> SafeApiCall<T>(string endpoint)
        {
            try
            {
                var response = await _client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var model = await response.Content.ReadFromJsonAsync<T>();
                    return model ?? throw new JsonException();
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new APIException($"Error {response.StatusCode}. {errorMessage}");
                }
            }
            catch (HttpRequestException)
            {
                throw new APIException($"Request error.");
            }
            catch (TaskCanceledException)
            {
                throw new APIException($"Request timeout or canceled.");
            }
            catch (JsonException)
            {
                throw new APIException($"Error deserializing response. The JSON format may not match the expected model.");
            }
            catch (APIException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new APIException($"An unexpected error occurred while calling the API. {ex.Message}");
            }
        }
    }
}
