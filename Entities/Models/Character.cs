using System;
using System.Collections.Generic;
using TheWideWorld.Items.Models;

namespace TheWideWorld.Entities.Models
{
    public class Character : Entity
    {
        public string Name;
        public int XP;
        public Abilities Abilities;
        public int InventoryWeight;
        public string Background;
        public List<Guid> AdventuresPlayed;
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
