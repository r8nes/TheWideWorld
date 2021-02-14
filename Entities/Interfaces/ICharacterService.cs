using System.Collections.Generic;
using TheWideWorld.Entities.Models;

namespace TheWideWorld.Entities.Interfaces
{
    public interface ICharacterService
    {
        Character LoadCharacter(string name);

        List<Character> GetCharactersLevel(int minLevel = 0, int maxLevel = 20);
    }
}
