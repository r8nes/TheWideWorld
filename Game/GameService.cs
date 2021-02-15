using System;
using System.Collections.Generic;
using System.Linq;
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
        private Adventure gameAdventure;
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
            gameAdventure = adventure;
                if (gameAdventure == null)
                {
                    gameAdventure = adventureService.GetInitialAdventure();
                }

                CreateAdventureBanner(gameAdventure.Title);
                CreateDescription(gameAdventure);

                List<Character> charactersInRange = characterService.GetCharactersLevel(gameAdventure.MinimumLevel, gameAdventure.MaxLevel);

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

            var rooms = gameAdventure.Rooms;
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
            WriteRoomOptions(room);

            string playerDecision = messageHandler.Read().ToLower();
            bool exitRoom = false;

            while (!exitRoom)
            {
                switch (playerDecision)
                {
                    case "l":
                        CheckForTraps(room);
                        WriteRoomOptions(room);
                        playerDecision = messageHandler.Read().ToLower();
                        break;
                    case "o":
                        if (room.Chest != null)
                        {
                            OpenChest(room.Chest);
                        }
                        else
                        {
                            messageHandler.Write("You don't see any chests here.");
                        }
                        break;
                    case "n":
                    case "s":
                    case "e":
                    case "w":
                        CompassDirection wallLocation = CompassDirection.North;
                        if (playerDecision == "s") wallLocation = CompassDirection.South;
                        else if (playerDecision == "w") wallLocation = CompassDirection.West;
                        else if (playerDecision == "e") wallLocation = CompassDirection.East;

                        if (room.Exits.FirstOrDefault(x => x.WallLocation == wallLocation) != null) {
                            ExitRoom(room, wallLocation);
                        }
                        else {
                            messageHandler.Write("\n Um... That's a wall...");
                        }
                        ExitRoom(room, wallLocation);
                        break;
                }
            }
        }

        private void WriteRoomOptions(Room room)
        {
            messageHandler.Write("MAKE A CHOSE:");
            messageHandler.Write(new string('_', 40));
            messageHandler.Write("(L)ook for traps");
            if (room.Chest != null)
            {
                messageHandler.Write("(O)pen the chest");
                messageHandler.Write("(C)heck chest for traps");
            }
            messageHandler.Write("Use an exit: ");
            foreach (Exit exit in room.Exits)
            {
                messageHandler.Write($"({exit.WallLocation.ToString().Substring(0, 1)}){exit.WallLocation.ToString().Substring(1)}");
            }     
        }
        /// <summary>
        /// Метод который позволяет нам найти и обезвредить ловушку.
        /// </summary>
        /// <param name="room"></param>
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

                Dice dice = new Dice();
                int findTrapRoll = dice.RollDice(new List<DiceType> { DiceType.D20 }) + trapBonus;

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
                    ProcessTrapMessageAndDamage(room);

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

        private void ProcessTrapMessageAndDamage(Room room)
        {
            Dice dice = new Dice();


            messageHandler.Write($"WOOOOP! Trap was activated. This was {room.Trap.TrapType.ToString()} trap.");

            int trapDamage = dice.RollDice(new List<DiceType> { room.Trap.DamageDie });

            int hitPoints = character.HitPoints - trapDamage;
            if (hitPoints < 1)
            {
                hitPoints = 0;
            }
            messageHandler.Write($"You take {trapDamage} damage. You have {hitPoints} HP");

            if (hitPoints < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                messageHandler.Write("You're dead!");
                Console.ResetColor();
            }
        }

        private void ExitRoom(Room room, CompassDirection wallLocation)
        {
            if (room.Trap != null && room.Trap.TrippedOrDisarmed == false)
            {
                ProcessTrapMessageAndDamage(room);                 
            }

            Exit exit = room.Exits.FirstOrDefault(x => x.WallLocation == wallLocation);

            if (exit == null) {
                throw new Exception("This room doesnt have that exception");
            }

            Room newRoom = gameAdventure.Rooms.FirstOrDefault(x => x.RoomNumber == exit.LeadsToRoom);

            if (newRoom == null) {
                throw new Exception("The next room doesnt exist. The dragon might destroy it.");
            }
            RoomProcessor(newRoom);           
        }

        private void OpenChest(Chest chest)
        {
            throw new NotImplementedException();
        }

    }
}
