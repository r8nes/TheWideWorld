using System;
using System.Collections.Generic;
using TheWideWorld.Adventures;
using TheWideWorld.Adventures.Interfaces;
using TheWideWorld.Entities.Interfaces;
using TheWideWorld.Entities.Models;
using TheWideWorld.Game.Interfaces;



namespace TheWideWorld.Game
{

    public class GameService : IGameService
    {
        private IAdventureService adventureService;
        private ICharacterService characterService;
        private Character character;

        public GameService(IAdventureService AdventureService, ICharacterService CharacterService)
        {
            adventureService = AdventureService;
            characterService = CharacterService;
        }
        /// <summary>
        /// Метод запускающий приключение и предоставляющий выбор персонажа.
        /// </summary>
        /// <param name="adventure">Выбираем приключение.</param>
        /// <returns>Запускает или не запускат дальнейшую игру</returns>
        public bool StartTheGame(Adventure adventure = null)
        {

                if (adventure == null)
                {
                    adventure = adventureService.GetInitialAdventure();
                }

                CreateAdventureBanner(adventure.Title);
                CreateDescription(adventure.Description);

                List<Character> charactersInRange = characterService.GetCharacterLevels(adventure.MinimumLevel, adventure.MaxLevel);

                if (charactersInRange.Count == 0)
                {
                    Console.WriteLine("Probably, the goblin king killed your character erlier. Try to create a new.");
                    return false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("WHO DARE WISH TO CHANCE THE DEATH???????");
                    Console.ResetColor();
                    int characterCount = 0;
                    foreach (var character in charactersInRange)
                    {
                        Console.WriteLine($"# {characterCount} {character.Name}: {character.Class} {character.Level}");
                        characterCount++;
                    }
                }
                string name = Console.ReadLine();
                character = characterService.LoadCharacter(charactersInRange[Convert.ToInt32(name)].Name);

            


            
            return true;
        }

      

        /// <summary>
        /// Метод, создающий баннер-заголовок.
        /// </summary>
        /// <param name="title"></param>
        private void CreateAdventureBanner(string title)
        {

            Console.Clear();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i <= title.Length + 3; i++)
            {
                Console.Write("*");
                if (i == title.Length + 3)
                {
                    Console.Write("\n");
                }
            }

            Console.WriteLine($"| {title} |");

            for (int i = 0; i <= title.Length + 3; i++)
            {
                Console.Write("*");
                if (i == title.Length + 3)
                {
                    Console.Write("\n");
                }
            }
            Console.ResetColor();
        }
        private static void CreateDescription(string description)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n{description.ToUpper()}");
            Console.ResetColor();
        }
    }
}
