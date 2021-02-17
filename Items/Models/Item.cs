using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWideWorld.Items.Interfaces;

namespace TheWideWorld.Items.Models
{
    public class Item : IItem
    {
        public ItemType Name;
        public string Description;
        public int ObjectiveNumber;
        public int Weight;
        public int GoldValue;
    }

    public enum ItemType
    { 
        Rope,
        Torch,
        HolySymbol,
        Water,
        Food,
        TinderBox,
        Stone,
        Rune,
        Silver,
        Gold,
        Wood 
    }
}
