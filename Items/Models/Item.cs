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
        public string Description;
        public int ObjectiveNumber;
        public int Weight;
        public int GoldValue;
    }

    public enum IremType
    { 
        Rope,
        Torch,
        HolySymbol,
        Water,
        Food,
        TinderBox
    }
}
