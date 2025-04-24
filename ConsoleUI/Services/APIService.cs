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

        /// <summary>
        /// Reads the HTTP content and returns the value that results from deserializing the content as JSON in an asynchronous operation.
        /// </summary>
        /// <param name="endpoint">The endpoint location of the API to call on.</param>
        /// <typeparam name="T">The target type to deserialize to.</typeparam>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<T?> SafeApiCall<T>(string endpoint)
        {
            try
            {
                var response = await _client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<T>();
                    return result;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"\nError {response.StatusCode}: {content}\n");
                    return default;
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"\nNetwork error! {httpEx.Message}\n");
            }
            catch (NotSupportedException notSupEx)
            {
                Console.WriteLine($"\nUnsupported content type: {notSupEx.Message}\n");
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"\nError parsing response: {jsonEx.Message}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nUnexpected error: {ex.Message}\n{ex.StackTrace}\n");
            }

            return default;
        }
    }
}
