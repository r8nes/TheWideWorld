using System.Collections.Generic;
using TheWideWorld.Adventures.Models;

namespace TheWideWorld.Adventures
{
    public class Adventure
    {
        public string GUID;
        public string Title;
        public string Description;
        public string CompletionXPReward;
        public string CompletionGoldReward;
        public int MaxLevel;
        public int MinimumLevel;
        public List<Room> Rooms;
    }
}
