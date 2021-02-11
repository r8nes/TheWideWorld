﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TheWideWorld.Entities.Interfaces;
using TheWideWorld.Entities.Models;

namespace TheWideWorld.Entities
{
    class CharacterService : ICharacterService
    {

        /// <summary>
        /// Получаем и загружаем персонажа
        /// </summary>
        /// <returns></returns>
        public Character LoadCharacter (string name) 
        {
            string basePath = $"{AppDomain.CurrentDomain.BaseDirectory}Character";
            Character character = new Character();

            if (File.Exists($"{basePath}\\{name}.json"))
            {
                DirectoryInfo directory = new DirectoryInfo(basePath);
                FileInfo[] characterJSONFile = directory.GetFiles($"{name}.json");

                using StreamReader fl = File.OpenText(characterJSONFile[0].FullName);
                character = JsonConvert.DeserializeObject<Character>(fl.ReadToEnd());
            }
            else 
            {
                throw new Exception("Character not found");
            }
            return character;
        }

        /// <summary>
        /// Получаем список персонажей
        /// </summary>
        /// <param name="minLevel"></param>
        /// <param name="maxLevel"></param>
        /// <returns></returns>
        public List<Character> GetCharacterLevels(int minLevel = 0, int maxLevel = 20)
        {
            string basePath = $"{AppDomain.CurrentDomain.BaseDirectory}Character";
            List<Character> charactersInRange = new List<Character>();

            try
            {

                DirectoryInfo directory = new DirectoryInfo(basePath);
                foreach (FileInfo file in directory.GetFiles($"*.json"))
                {
                    using StreamReader fl = File.OpenText(file.FullName);
                    Character potentialCharacterInRange = JsonConvert.DeserializeObject<Character>(fl.ReadToEnd());
                    if (potentialCharacterInRange.isAlive && (potentialCharacterInRange.Level >= minLevel && potentialCharacterInRange.Level <= maxLevel))
                    {
                        charactersInRange.Add(potentialCharacterInRange);
                    }
                }
            }
            catch (Exception excptn)
            {

                Console.WriteLine($"Goblinz robbed your character! {excptn.Message}");
            }
            return charactersInRange;
           
        }
    }
}