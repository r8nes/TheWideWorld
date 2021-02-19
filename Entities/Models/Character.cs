using System.Collections.Generic;
using TheWideWorld.Items.Models;

namespace TheWideWorld.Entities.Models
{
    public class Character : Entity
    {
        public string Name;
        public int Level;
        public int ArmorClass;
        public int Gold;
        public Abilities Abilities;
        public List<Item> Inventory;
        public int InventoryWeight;
        public string Background;
        public bool isAlive;
        public List<string> AdventuresPlayed;
        public CharacterClass Class;
    }

    public class Abilities
    {
        public int Strength;
        public int Dexterity;
        public int Constitution;
        public int Intelligence;
        public int Wisdom;
        public int Charisma;
    }

    public enum CharacterClass
    {
        Fighter,
        Rough,
        Mage,
        Cleric,
        Ranger
    }
}
