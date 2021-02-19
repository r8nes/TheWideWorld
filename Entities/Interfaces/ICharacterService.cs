using System.Collections.Generic;
using TheWideWorld.Entities.Models;

namespace TheWideWorld.Entities.Interfaces
{
    public interface ICharacterService
    {
        public Character LoadCharacter(string name);
        public bool SaveCharacter(Character character);
        public List<Character> GetCharactersLevel(int minLevel = 0, int maxLevel = 20);
    }
}
