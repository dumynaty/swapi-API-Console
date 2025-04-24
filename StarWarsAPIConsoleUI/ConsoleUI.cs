using StarWarsAPIConsoleUI.Models;
using System.Net.Http.Json;

namespace StarWarsAPIConsoleUI
{
    internal class ConsoleUI
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            string url = "https://swapi.dev/api/people/1";

            var response = await client.GetFromJsonAsync<PersonModel>(url);

            if (response != null)
            {
                Console.WriteLine($"{response.Name} {response.Height} {response.Mass}");
            }
        }
    }
}
