using System.Collections;
using ConsoleUI.Models;
using ConsoleUI.Services;

namespace ConsoleUI
{
    internal class ConsoleUIProgramStart
    {
        private static readonly HttpClient client = new HttpClient();
        private static APIService api = null!; // null-forgiving operator
        public static async Task Main(string[] args)
        {
            client.BaseAddress = new Uri("https://localhost:44360/");
            api = new APIService(client);
            await ConsoleUI();
        }

        private static async Task ConsoleUI()
        {
            Console.WriteLine("Welcome to the Star Wars API Program! --swapi.info--\n");

            bool loop = true;
            do
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Get a person's basic information by id");
                Console.WriteLine("2. Get a person's full information by id");
                Console.WriteLine("*  Exit program");
                Console.Write("\nOption: ");

                string? input = Console.ReadLine();

                try
                {
                    switch (input)
                    {
                        case "1":
                            await GetPersonBasicInfoById(client);
                            break;
                        case "2":
                            await GetPersonFullInfoById(client);
                            break;
                        case "*":
                            loop = false;
                            break;

                        default:
                            Console.WriteLine("Invalid entry! Please choose an option.");
                            break;
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n{ex.Message}\n");
                }

            }
            while (loop == true);
        }

        private static async Task GetPersonBasicInfoById(HttpClient client)
        {
            Console.Write("Enter a character ID (1-83): ");
            
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nError! Input is not a valid number\n");
                return;
            }

            var person = await api.SafeApiCall<PersonModel>($"api/people/person/{id}");
            PrintProperties(person);
        }

        private static async Task GetPersonFullInfoById(HttpClient client)
        {
            Console.Write("Enter a character ID (1-83): ");
            
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nError! Input is not a valid number\n");
                return;
            }

            var fullPerson = await api.SafeApiCall<FullPersonModel>($"api/people/fullperson/{id}/");
            PrintProperties(fullPerson);
            Console.WriteLine();

            foreach (var filmPath in fullPerson.Films)
            {
                Console.WriteLine("--- --- --- --- --- --- --- --- --- --- --- --- --- ---");
                string filmId = filmPath.Substring(filmPath.Length - 1, 1);
                await GetFilmInfo(client, filmId);
                Console.WriteLine("--- --- --- --- --- --- --- --- --- --- --- --- --- ---");
            }
        }

        private static async Task GetFilmInfo(HttpClient client, string id)
        {
            var film = await api.SafeApiCall<FilmModel>($"api/film/filmnumber/{id}/");
            PrintProperties(film);
        }

        private static void PrintProperties<T>(T model)
        {
            if (model == null)
                return;

            foreach (var prop in model.GetType().GetProperties())
            {
                var value = prop.GetValue(model);

                if (value is IEnumerable enumerable && value is not string)
                {
                    string spaces = new string(' ', prop.Name.Length);
                    bool firstSpacing = true;

                    foreach (var item in enumerable)
                    {
                        if (firstSpacing)
                        {
                            Console.WriteLine($"{prop.Name}: {item}");
                            firstSpacing = false;
                        }
                        else
                        {
                            Console.WriteLine($"{spaces}  {item}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{prop.Name}: {value}");
                }
            }
        }
    }
}
