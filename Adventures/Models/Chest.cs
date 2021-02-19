using System.Collections.Generic;
using TheWideWorld.Game;
using TheWideWorld.Items.Models;

namespace TheWideWorld.Adventures.Models
{
    public class Chest
    {
        public Lock Lock;
        public Trap Trap;
        public List<Item> Treasure;
        public int Gold;
    }
}
