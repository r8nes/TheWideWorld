using Newtonsoft.Json;
using System;
using System.IO;
using TheWideWorld.Adventures.Interfaces;

namespace TheWideWorld.Adventures
{
    public class AdventureService : IAdventureService
    {

        /// <summary>
        /// Метод получающий текущее приключение.
        /// </summary>
        /// <returns></returns>
        public Adventure GetInitialAdventure() 
        {
            string basePath = $"{AppDomain.CurrentDomain.BaseDirectory}Adventures";
            Adventure initialAdventure = new Adventure();

            if (File.Exists($"{basePath}\\initial.json"))
            {
                DirectoryInfo directory = new DirectoryInfo(basePath);
                FileInfo[] initialJsonFile = directory.GetFiles("initial.json");

                using (StreamReader fl = File.OpenText(initialJsonFile[0].FullName))
                {
                    initialAdventure = JsonConvert.DeserializeObject<Adventure>(fl.ReadToEnd());
                }

            }
            else
            {
                throw new Exception("Initial adventure not found");
            }
            return initialAdventure;
        }

    }
}
