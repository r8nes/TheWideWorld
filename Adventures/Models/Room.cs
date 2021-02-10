using System.Collections.Generic;
using TheWideWorld.Entities.Models;

namespace TheWideWorld.Adventures.Models
{
    public class Room
    {
        public int RoomNumber;
        public string Description;
        public Trap Trap;
        public List<Monster> Monsters;
        public Chest Chest;
        public Objective FinalObjective;
        public List<Exit> Exits;
    }
}
