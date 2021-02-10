using System.Collections.Generic;
using TheWideWorld.Items.Models;

namespace TheWideWorld.Adventures.Models
{
    public class Chest
    {
        public bool Locked = false;
        public Trap Trap;
        public List<Item> Treasure;
        public int Gold;
    }
}
