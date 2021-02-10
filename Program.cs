using System;
using TheWideWorld.Game;
using TheWideWorld.Adventures;
using TheWideWorld.Entities;

namespace TheWideWorld
{
    class Program
    {
        private static AdventureService adventuresService = new AdventureService();
        private static CharacterService characterService = new CharacterService();
        private static GameService gameService = new GameService(adventuresService, characterService);


        static void Main(string[] args)
        {
            MakeMainTitle();
            MakeMainMenu();
        }

        private static void MakeMainTitle()
        {
            Console.WriteLine("______________________________________________________________");
            Console.WriteLine("");
            Console.WriteLine("   _____ _          _ _ _ _   _        _ _ _         _   _ ");
            Console.WriteLine("  |_   _| |_ ___   | | | |_|_| |___   | | | |___ ___| |_| |");
            Console.WriteLine("    | | |   | -_|  | | | | | . | -_|  | | | | . |  _| | . |");
            Console.WriteLine("    |_| |_|_|___|  |_____|_|___|___|  |_____|___|_| |_|___|");
            Console.WriteLine("");
            Console.WriteLine("_______________________An epic RPG game_______________________");

        }
        /// <summary>
        /// Метод с логикой выбора в главном меню.
        /// </summary>
        private static void MakeMainMenu()
        {
            MenuOptions();

            bool isInputValid = false;

            while (!isInputValid)
            {

                string playerChose = Console.ReadLine().ToUpper();
                Console.WriteLine($"Your chose is {playerChose}");

                switch (playerChose)
                {
                    case "S":
                        gameService.StartTheGame();
                        isInputValid = true;
                        break;

                    case "L":
                        LoadTheGame();
                        isInputValid = true;
                        break;

                    case "C":
                        CreateCharacter();
                        isInputValid = true;
                        break;

                    default:
                        Console.WriteLine("Pick the right letter!");
                        MenuOptions();
                        isInputValid = false;
                        break;
                }
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Метод с отрисовкой главного меню.
        /// </summary>
        private static void MenuOptions()
        {
            Console.WriteLine($"\n\n(S)tart the game");
            Console.WriteLine($"(L)oad the game");
            Console.WriteLine($"(C)reate a new character");
        }

        private static void LoadTheGame()
        {
            Console.WriteLine("Load");
        }

        private static void CreateCharacter()
        {
            Console.WriteLine("Great");
        }
    }
}
