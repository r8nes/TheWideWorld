using System;
using System.Collections.Generic;
using TheWideWorld.Adventures;
using TheWideWorld.Adventures.Interfaces;
using TheWideWorld.Adventures.Models;
using TheWideWorld.Entities.Interfaces;
using TheWideWorld.Entities.Models;
using TheWideWorld.Game.Interfaces;
using TheWideWorld.Utilites.Interfaces;

namespace TheWideWorld.Game
{

    public class GameService : IGameService
    {
        private IAdventureService adventureService;
        private ICharacterService characterService;
        private IMessageHandler messageHandler;
        private Character character;

        public GameService(IAdventureService AdventureService, ICharacterService CharacterService, IMessageHandler MessageHandler)
        {
            adventureService = AdventureService;
            characterService = CharacterService;
            messageHandler = MessageHandler;
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
                CreateDescription(adventure);

                List<Character> charactersInRange = characterService.GetCharactersLevel(adventure.MinimumLevel, adventure.MaxLevel);

                if (charactersInRange.Count == 0)
                {
                messageHandler.Write("Probably, the goblin king killed your character erlier. Try to create a new.");
                    return false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                messageHandler.Write("WHO DARE WISH TO CHANCE THE DEATH???????");
                    Console.ResetColor();
                    int characterCount = 0;
                    foreach (Character character in charactersInRange)
                    {
                    messageHandler.Write($"# {characterCount} {character.Name}: {character.Class} {character.Level}");
                        characterCount++;
                    }
                }
                string name = messageHandler.Read();
                character = characterService.LoadCharacter(charactersInRange[Convert.ToInt32(name)].Name);

            var rooms = adventure.Rooms;
            RoomProcessor(rooms[0]);
            return true;
        }

        private void RoomProcessor(Room room)
        {
            messageHandler.Clear();
            messageHandler.Write(new string('_', 40));
            messageHandler.Write($"{room.RoomNumber} {room.Description}");

            if (room.Exits.Count == 1) {
                messageHandler.Write($"There are exits on the {room.Exits[0].WallLocation} wall");
            }
            else {
                var exitDescription = "";
                foreach  (Exit exit in room.Exits)
                {
                    exitDescription += $"{exit.WallLocation}";
                }
                exitDescription.Remove(exitDescription.Length - 1);

                messageHandler.Write($"There room has exits on the {exitDescription} walls");
            }
        }

        /// <summary>
        /// Метод, создающий баннер-заголовок.
        /// </summary>
        /// <param name="title"></param>
        private void CreateAdventureBanner(string title)
        {

            messageHandler.Clear();
            messageHandler.Read();

            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i <= title.Length + 3; i++)
            {
                messageHandler.Write("*");
                if (i == title.Length + 3)
                {
                    messageHandler.Write("\n");
                }
            }

            messageHandler.Write($"| {title} |");

            for (int i = 0; i <= title.Length + 3; i++)
            {
                messageHandler.Write("*");
                if (i == title.Length + 3)
                {
                    messageHandler.Write("\n");
                }
            }
            Console.ResetColor();
        }
        /// <summary>
        /// Метод создающий описание приключения.
        /// </summary>
        /// <param name="description">Текст описания.</param>
        private void CreateDescription(Adventure adventure)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n{adventure.Description.ToUpper()}");
            Console.ResetColor();

            messageHandler.Write($"\nLevels : {adventure.MinimumLevel} - {adventure.MaxLevel}");
            messageHandler.Write($"\nComletion Rewards - { adventure.CompletionGoldReward} gold & {adventure.CompletionXPReward} XP");
            messageHandler.Write();
        }
    }
}
