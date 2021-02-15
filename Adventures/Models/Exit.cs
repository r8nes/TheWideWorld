namespace TheWideWorld.Adventures.Models
{
    public class Exit
    {
        public bool Locked = false;
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
