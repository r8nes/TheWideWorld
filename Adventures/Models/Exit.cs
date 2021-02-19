using TheWideWorld.Game;

namespace TheWideWorld.Adventures.Models
{
    public class Exit
    {
        public Lock Lock;
        public CompassDirection WallLocation;
        public Riddle Riddle;
        public int LeadsToRoom;
    }

    public enum CompassDirection
    {
        North,
        East,
        South,
        West
    }
}
