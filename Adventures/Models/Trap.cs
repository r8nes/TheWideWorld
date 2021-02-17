using TheWideWorld.Game;

namespace TheWideWorld.Adventures.Models
{
    public class Trap
    {
        public TrapType TrapType;
        public DiceType DamageDie = DiceType.D4;
        public bool SearchedFor = false;
        public bool TrippedOrDisarmed = false;
    }

    public enum TrapType { 
            Poison,
            Spike,
            Fire,
            Thunder
    }
}
