using System;
using System.Media;
using TheWideWorld.Game;
using TheWideWorld.Adventures;
using TheWideWorld.Entities;
using TheWideWorld.Utilites;

namespace TheWideWorld
{
     public class Program
    {
        private static readonly AdventureService adventuresService = new AdventureService();
        private static readonly CharacterService characterService = new CharacterService();
        private static readonly ConsoleMessageHandler messageHandler = new ConsoleMessageHandler();
        private static GameService gameService = new GameService(adventuresService, characterService, messageHandler);


        static void Main(string[] args)
        {
            MakeMainTitle();
            using (SoundPlayer player = new SoundPlayer($"{AppDomain.CurrentDomain.BaseDirectory}/Sounds/shortIntro.wav")) { 
            
            player.Play();
            MakeMainMenu(player);
            }
        }

        public static void MakeMainTitle()
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
        private static void MakeMainMenu(SoundPlayer player)
        {
            MenuOptions();

            bool isInputValid = false;
            try
            {
                while (!isInputValid)
                {
                    switch (Console.ReadLine().ToUpper())
                    {
                        case "S":
                            player.Stop();
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
            }
            catch (Exception excptn)
            {
                Console.WriteLine("Something went wrong. I think it's orcs!\n Please, restart the game.");
                Console.WriteLine($"The message : {excptn.Message}");
            }
        }
        /// <summary>
        /// Метод с отрисовкой главного меню.
        /// </summary>
        public static void MenuOptions()
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
