using System.Collections.Generic;
using TheWideWorld.Items.Models;

namespace TheWideWorld.Entities.Models
{
    public abstract class Entity
    {
        public int HitPoints = 0;
        public Attack Attack;
        public int Gold;
        public int Level;
        public bool isAlive;
        public int ArmorClass;
        public List<Item> Inventory;
    }

    public class Attack {
        public int BaseDice;
        public int BonusDamage;
    }
}
