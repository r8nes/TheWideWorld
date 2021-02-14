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

        /// <summary>
        /// Метод, создающий баннер-заголовок.
        /// </summary>
        /// <param name="title"></param>
        private void CreateAdventureBanner(string title)
        {
            messageHandler.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i <= title.Length + 3; i++)
            {
                messageHandler.Write("*", false);
                if (i == title.Length + 3)
                {
                    messageHandler.Write("\n", false);
                }
            }

            messageHandler.Write($"| {title} |");

            for (int i = 0; i <= title.Length + 3; i++)
            {
                messageHandler.Write("*", false);
                if (i == title.Length + 3)
                {
                    messageHandler.Write("\n", false);
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
        /// <summary>
        /// Главный метод для отрисовки комнаты
        /// </summary>
        /// <param name="room"></param>
        private void RoomProcessor(Room room)
        {
            RoomDescription(room);
            RoomOptions(room);
        }

        /// <summary>
        /// Создаем описание комнаты для игрока.
        /// </summary>
        /// <param name="room"></param>
        private void RoomDescription(Room room)
        {
            messageHandler.Clear();
            messageHandler.Write(new string('_', 40));
            messageHandler.Write($"{room.RoomNumber} {room.Description}");

            if (room.Exits.Count == 1)
            {
                messageHandler.Write($"There are exits on the {room.Exits[0].WallLocation} wall");
            }
            else
            {
                string exitDescription = "";
                foreach (Exit exit in room.Exits)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    exitDescription += $" {exit.WallLocation},";
                    Console.ResetColor();
                }

                messageHandler.Write($"There room has exits on the{exitDescription.Remove(exitDescription.Length - 1)} walls.");
            }

            if (room.Chest != null)
            {
                messageHandler.Write($"There is a chest in a room!");
            }
        }

        /// <summary>
        /// Метод создающий отрисовку нужных опций для комнаты
        /// </summary>
        /// <param name="room"></param>
        private void RoomOptions(Room room)
        {
            messageHandler.Write("MAKE A CHOSE:");
            messageHandler.Write(new string('_', 40));
            messageHandler.Write("(L)ook for traps");
            messageHandler.Write("Use an exit: ");

            foreach (Exit exit in room.Exits)
            {
                messageHandler.Write($"({exit.WallLocation.ToString().Substring(0, 1)}){exit.WallLocation.ToString().Substring(1)}");
            }
            if (room.Chest != null)
            {
                messageHandler.Write("(O)pen the chest");
                messageHandler.Write("(C)heck chest for traps");
            }

            string playerDecision = messageHandler.Read().ToLower();
            bool exitRoom = false;

            while (!exitRoom) {
                switch (playerDecision) {
                    case "l":
                        CheckForTraps(room);
                        break;
                    case "o":
                        if (room.Chest != null)
                        {
                            OpenChest(room.Chest);
                        }
                        else {
                            messageHandler.Write("You don't see any chests here.");
                        }
                        break;
                    case "w":
                    case "s":
                    case "n":
                    case "e":
                        ExitRoom(room);
                        break;
                }
            }
        }

        private void CheckForTraps(Room room)
        {
            if (room.Trap != null)
            {
                if (room.Trap.TrippedOrDisarmed) {
                    messageHandler.Write("You've already found the trap.");
                    return;
                }
                if (room.Trap.SearchedFor)
                {
                    messageHandler.Write($"You've already search for a trap, {character.Class}");
                    return;
                }

                int trapBonus = 0 + character.Abilities.Intelligence;
                if (character.Class == CharacterClass.Rough) {
                    trapBonus += 2;
                }

                var dice = new Dice();
                var findTrapRoll = dice.RollDice(new List<DiceType> { DiceType.D20 }) + trapBonus;

                if (findTrapRoll < 12)
                {
                    messageHandler.Write("You find NOOOO trap.");
                    room.Trap.SearchedFor = true;
                    return;
                }
                messageHandler.Write($"Oh, you found the trap!! Trying disarm...");
                int disarmTrapRoll = dice.RollDice(new List<DiceType> { DiceType.D20 }) + trapBonus;
                if (disarmTrapRoll < 11)
                {
                    messageHandler.Write("WOOOOP! Trap was activated. You take n damage");

                }
                else 
                {
                    messageHandler.Write("Lucky... Trap was disarmed");
                }
                room.Trap.TrippedOrDisarmed = true;
                return;
            }
            messageHandler.Write("No traps.");
            return;
        }
        private void ExitRoom(Room room)
        {
            throw new NotImplementedException();
        }

        private void OpenChest(Chest chest)
        {
            throw new NotImplementedException();
        }

    }
}
