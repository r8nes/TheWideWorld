﻿using System;
using System.Collections.Generic;
using System.Linq;
using TheWideWorld.Adventures;
using TheWideWorld.Adventures.Interfaces;
using TheWideWorld.Adventures.Models;
using TheWideWorld.Entities.Interfaces;
using TheWideWorld.Entities.Models;
using TheWideWorld.Game.Interfaces;
using TheWideWorld.Items.Models;
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
        private bool gameWon = false;
        private string gameWinnigDescription;
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
                    messageHandler.Write($"# {characterCount} {character.Name}: {character.Class} {character.Level} HP: {character.HitPoints}");
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
                        if (gameWon) {
                            GameOver();
                        }
                        WriteRoomOptions(room);
                        playerDecision = messageHandler.Read().ToLower();
                        break;
                    case "o":
                        if (room.Chest != null)
                        {
                            OpenChest(room.Chest);
                            if (gameWon)
                            {
                                GameOver();
                            }
                            WriteRoomOptions(room);
                            playerDecision = messageHandler.Read().ToLower();                     
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
        /// <summary>
        /// Метод отрисовывающий опции для комнаты
        /// </summary>
        /// <param name="room"></param>
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
                    ProcessTrapMessageAndDamage(room.Trap);

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
        /// <summary>
        /// Метод реализующий работу ловушек.
        /// </summary>
        /// <param name="trap"></param>
        private void ProcessTrapMessageAndDamage(Trap trap)
        {
            Dice dice = new Dice();

            messageHandler.Write($"WOOOOP! Trap was activated. This was {trap.TrapType } trap.");

            int trapDamage = dice.RollDice(new List<DiceType> { trap.DamageDie });
            character.HitPoints -= trapDamage;
            int hitPoints = character.HitPoints;
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
        /// <summary>
        /// Метод реализующий перемещение по локации
        /// </summary>
        /// <param name="room"></param>
        /// <param name="wallLocation"></param>
        private void ExitRoom(Room room, CompassDirection wallLocation)
        {
            if (room.Trap != null && room.Trap.TrippedOrDisarmed == false)
            {
                ProcessTrapMessageAndDamage(room.Trap);
                room.Trap.TrippedOrDisarmed = true;
            }

            Exit exit = room.Exits.FirstOrDefault(x => x.WallLocation == wallLocation);

            if (exit == null) {
                throw new Exception("This room doesnt have that exception");
            }

            Room newRoom = gameAdventure.Rooms.FirstOrDefault(x => x.RoomNumber == exit.LeadsToRoom);

            if (newRoom == null) {
                throw new Exception("The next room doesnt exist. The dragon might destroy it.");
            }
            if ((exit.Lock == null || !exit.Lock.Locked) || TryUnlock(exit.Lock)) 
            { 
              RoomProcessor(newRoom);           
            }
            else
            {
                RoomProcessor(room);
            }
        }
        /// <summary>
        /// Метод реализующий взаимодействие с сундуками
        /// </summary>
        /// <param name="chest"></param>
        private void OpenChest(Chest chest)
        {
            if (chest.Lock == null || !chest.Lock.Locked)
            {
                if (chest.Trap != null && !chest.Trap.TrippedOrDisarmed)
                { 
                        ProcessTrapMessageAndDamage(chest.Trap);
                        chest.Trap.TrippedOrDisarmed = true;
                }
                else 
                {
                    messageHandler.Write("Opening chest... ");
                    if (chest.Gold > 0)
                    {
                    character.Gold += chest.Gold;
                        messageHandler.Write($"Lucky. It's not a mimic. You found {chest.Gold} gold. Your total gold is now {character.Gold}");
                        chest.Gold = 0;
                    }

                    if (chest.Treasure != null && chest.Treasure.Count > 0)
                    {
                        messageHandler.Write($"You found {chest.Treasure.Count} items in this chest:\n");

                        foreach (var item in chest.Treasure)
                        {
                            messageHandler.Write($"{ item.Name.ToString()} - {item.Description.ToString()}");

                            
                            if (item.ObjectiveNumber == gameAdventure.FinalObjective)
                            {
                                gameWon = true;
                                character.Gold += Convert.ToInt32(gameAdventure.CompletionGoldReward);
                                character.XP += Convert.ToInt32(gameAdventure.CompletionXPReward);
                                character.AdventuresPlayed.Add(gameAdventure.GUID);
                            }
                        }
                        messageHandler.Write("\n");

                        character.Inventory.AddRange(chest.Treasure);
                        chest.Treasure = new List<Item>();
                        if (gameWon)
                        {
                            messageHandler.Write("***********************************************************");
                            messageHandler.Write("*             YOU DID IT! THE QUEST COMLITIED!            *");
                            messageHandler.Write("***********************************************************");
                            messageHandler.Write("*" + gameWinnigDescription +"*");
                            messageHandler.Write("You obtained " + gameAdventure.CompletionGoldReward + " gold");
                            messageHandler.Write("You gain " + gameAdventure.CompletionXPReward +  "XP");
                            messageHandler.Write(character.Name + " has now " + character.XP + " XP and" + character.Gold + " gold");
                        }
                        return;
                    }

                    if (chest.Gold == 0 && (chest.Treasure == null || chest.Treasure.Count == 0))
                    {
                        messageHandler.Write("You find a piece of clothes. And it stinks...");
                    }   
                }
            }
            else {

                if (TryUnlock(chest.Lock))
                {
                    OpenChest(chest);
                    if (gameWon)
                    {
                        GameOver();
                    }
                }
            }
        }


        /// <summary>
        /// Метод реализующий действие "Вскрыть замок"
        /// </summary>
        /// <param name="theLock">Замок, передающийся ссылкой</param>
        /// <returns></returns>
        private bool TryUnlock(Lock theLock)
        {
            if (!theLock.Locked) return true;
            bool hasOption = true;
            Dice dice = new Dice();
            Lock theLocalLock = theLock;
            while (hasOption)
            {
                if (!theLock.Attempted)
                {
                    messageHandler.Write("It's locked. Would you like to attempt to unlock the lock?\n" +
                        "(K)ey \n (L)ockpik \n (B)ash \n (W)alk away");
                    string playerDecision = messageHandler.Read().ToLower();
                    switch (playerDecision)
                    {
                        case "k":

                            if (character.Inventory.FirstOrDefault(x => x.Name == ItemType.Key && x.ObjectiveNumber == theLocalLock.KeyNumber) != null)
                            {
                                messageHandler.WriteRead("You have the right key! It unlocks the lock");
                                theLock.Locked = false;
                                return true;
                            }
                            else
                            {
                                messageHandler.Write("You don't have a key for the lock. \n");
                            }

                            break;

                        case "l":

                            if (character.Inventory.FirstOrDefault(x => x.Name == ItemType.Lockpicks) == null)
                            {
                                messageHandler.Write("You dom't have lockpicks.\n");
                                break;
                            }
                            else
                            {

                                int lockPicksBonus = 0 + character.Abilities.Dexterity;

                                if (character.Class == CharacterClass.Rough)
                                {
                                    lockPicksBonus += 2;
                                }

                                int pickRoll = (dice.RollDice(new List<DiceType> { DiceType.D20 }) + lockPicksBonus);

                                if (pickRoll > 12)
                                {
                                    messageHandler.WriteRead("Nice... The lock was bushed. Luck smiles you.");
                                    theLock.Attempted = true;
                                    return true;
                                }
                                messageHandler.WriteRead("Damn. You can't bludge the lock.\n");
                                theLock.Attempted = true;
                                break;
                            }
                        case "b":
                            int bashBonus = 0 + character.Abilities.Strength;
                            if (character.Class == CharacterClass.Fighter)
                            {
                                bashBonus += 2;
                            }
                            int bashRoll = (dice.RollDice(new List<DiceType> { DiceType.D20 }) + bashBonus);
                            if (bashRoll > 16)
                            {
                                messageHandler.WriteRead($"You submissed with a lock and snaped it out with your kick. Nice!");

                                theLock.Locked = false;
                                theLock.Attempted = true;
                                return true;
                            }
                            messageHandler.WriteRead("Ouch. You caught a splinter under your nail. Looser.");
                            theLock.Attempted = true;
                            break;
                        default:
                            return false;
                    }
                }
                else {
                 
                    if (character.Inventory.FirstOrDefault(x => x.Name == ItemType.Key && x.ObjectiveNumber == theLocalLock.KeyNumber) != null)
                    {
                        messageHandler.WriteRead($"You've tried picking or bashing but you have the right key, stupid {character.Class}");
                        theLock.Locked = false;
                        return true;
                    }
                    else
                    {
                        messageHandler.WriteRead("You can do nothing in this situation. Find the key. \n");
                        return false;                     
                    }
                }
            }
            return false;
        }

        private void GameOver()
        {
            characterService.SaveCharacter(character);
            character = new Character();
            messageHandler.WriteRead("The quest is over. Pick a new one.");
            messageHandler.Clear();
            Program.MakeMainTitle();
            Program.MenuOptions();
        }
    }
}
