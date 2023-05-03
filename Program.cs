using HighscoreManager.Domain.DTO;
using System.Text;
using System.Text.Json;
using static System.Console;


namespace HighscoreManager;

class Program
{
   static readonly HttpClient httpClient = new()
   {
      BaseAddress = new Uri("http://localhost:5000/api/")
   };
   static void Main(string[] args)
   {
      {


         bool applicationRunning = true;

         Console.CursorVisible = false;

         do
         {
            Console.WriteLine("(1) Sök Spel");
            Console.WriteLine("(2) Skapa Spel");
            Console.WriteLine("(3) Ny score");
            Console.WriteLine("(4) Avsluta");

            ConsoleKeyInfo userInput;

            bool invalidSelection = true;

            do
            {
               userInput = Console.ReadKey(true);

               invalidSelection = !(
                   userInput.Key == ConsoleKey.D1 ||
                   userInput.Key == ConsoleKey.NumPad1 ||
                   userInput.Key == ConsoleKey.D2 ||
                   userInput.Key == ConsoleKey.NumPad2 ||
                   userInput.Key == ConsoleKey.D3 ||
                   userInput.Key == ConsoleKey.NumPad3 ||
                    userInput.Key == ConsoleKey.D4 ||
                   userInput.Key == ConsoleKey.NumPad4
                   );

            } while (invalidSelection);

            Console.Clear();
            Console.CursorVisible = true;

            switch (userInput.Key)
            {
               case ConsoleKey.D1:
               case ConsoleKey.NumPad1:



                  SearchedGames();


                  break;

               case ConsoleKey.D2:
               case ConsoleKey.NumPad2:

                  CreateGame();

                  break;

               case ConsoleKey.D3:
               case ConsoleKey.NumPad3:

                  CreateScore();

                  break;

               case ConsoleKey.D4:
               case ConsoleKey.NumPad4:
                  applicationRunning = false;
                  break;
            }

            Console.Clear();

         } while (applicationRunning);

      }
   }

   private static void CreateScore()
   {
      Write("Spel: ");
      var gametitle = ReadLine();
      var games = GetGames(gametitle);

      foreach(var foundGame in games)
      {
         Console.WriteLine($"ID: {foundGame.Id}, Game: {foundGame.Name}");
      }

      Write("ID: ");
      var gameId = int.Parse(ReadLine());

      var game = GetGameViaId(gameId);

      Write("Player: ");
      var player = ReadLine();
      Write("Date: ");
      var date = DateTime.Parse(ReadLine());
      Write("Points: ");
      var points = int.Parse(ReadLine());

      Clear();

      var newScoreDto = new NewScoreDto
      {
         GameId = gameId,
         HighscoreDate = date,
         PlayerName = player,
         Points = points
      };

      var serializeOptions = new JsonSerializerOptions
      {
         PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      };

      var serializedNewScoreDto = JsonSerializer
         .Serialize(newScoreDto, serializeOptions);

      var body = new StringContent(
         serializedNewScoreDto,
         Encoding.UTF8,
         "application/json");

      var response = httpClient.PostAsync("scores", body).Result;
      try
      {
         response.EnsureSuccessStatusCode();
      }
      catch (Exception ex)
      {
         Console.WriteLine($"{ex.Message}\n {response.Content}");
      }
      

      if (response.IsSuccessStatusCode)
      {
         WriteLine("Score Created!");
      }
      else
      {
         WriteLine($"´Can not create Score {response.StatusCode} {response.Content}");
      }

      ReadKey(true);
   }

   private static GameDto GetGame(string? gametitle)
   {
      var response = httpClient.GetAsync($"games/{gametitle}")
        .Result;

      var json = response.Content
        .ReadAsStringAsync()
        .Result;

      var serializeOptions = new JsonSerializerOptions
      {
         PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      };


      var game = JsonSerializer
         .Deserialize<GameDto>(json, serializeOptions);

      return game;
   }

   private static void CreateGame()
   {
      Write("Namn: ");
      var name = Console.ReadLine();
      Write("Description: ");
      var description = Console.ReadLine();
      Write("Release Date: ");
      var releaseDate = DateTime.Parse(Console.ReadLine());
      Write("Image Url: ");
      var imageUrl = new Uri(Console.ReadLine());
      Write("Url Slug: ");
      var urlSlug = Console.ReadLine();

      Clear();

      var newGameDto = new NewGameDto
      {
         Name = name,
         Description = description,
         ReleaseDate = releaseDate,
         ImageUrl = imageUrl,
         UrlSlug = urlSlug
      };

      var serializeOptions = new JsonSerializerOptions
      {
         PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      };

      var serializedNewGameDto = JsonSerializer
         .Serialize(newGameDto, serializeOptions);

      var body = new StringContent(
         serializedNewGameDto,
         Encoding.UTF8,
         "application/json");

      var response = httpClient.PostAsync("games", body).Result;

      response.EnsureSuccessStatusCode();

      if (response.IsSuccessStatusCode)
      {
         WriteLine("Game Created!");
      }
      else
      {
         WriteLine($"´Can not create game {response.StatusCode}");
      }

      Thread.Sleep(2000);
   }

   private static void SearchedGames()
   {
      Console.Write("Namnet på spel: ");
      var name = Console.ReadLine();
      var games = GetGames(name);


      foreach (var game in games)
      {
         WriteLine($"ID: {game.Id}");
         WriteLine($"Spelets Namn: {game.Name}\n");

      }

      WriteLine("(V)isa");

      ConsoleKeyInfo userInput = ReadKey(true);

      if (userInput.Key == ConsoleKey.V)
      {
         WriteLine("Spel Id: ");
         var gameId = int.Parse(Console.ReadLine());

         var chosedGame = GetGameViaId(gameId);

         Clear();

         WriteLine($"ID: {chosedGame.Id}");
         WriteLine($"Name: {chosedGame.Name}");
         WriteLine($"Description: {chosedGame.Description}");
         WriteLine($"Release Date: {chosedGame.ReleaseDate}");
         WriteLine($"Image Url: {chosedGame.ImageUrl}");
      }
      ReadKey(true);
   }

   private static IEnumerable<GameDto> GetGames(string? name = null)
   {

      var response = name is null
         ? httpClient.GetAsync($"games").Result
         : httpClient.GetAsync($"games?name={name}").Result;

      var json = response.Content
         .ReadAsStringAsync()
         .Result;

      var serializeOptions = new JsonSerializerOptions
      {
         PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      };

      var games = JsonSerializer
         .Deserialize<IEnumerable<GameDto>>
         (json, serializeOptions);

      return games;

   }

   private static GameDto GetGameViaId(int gameId)
   {
      var response = httpClient.GetAsync($"games/{gameId}")
        .Result;

      var json = response.Content
        .ReadAsStringAsync()
        .Result;

      var serializeOptions = new JsonSerializerOptions
      {
         PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      };


      var game = JsonSerializer
         .Deserialize<GameDto>(json, serializeOptions);

      return game;
   }
}