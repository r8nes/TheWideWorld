using TheWideWorld.Game;

namespace TheWideWorld.Adventures.Models
{
    public class Trap
    {
        public TrapType TrapType;
        public DiceType DamageDie = DiceType.D4;
    }

    public enum TrapType { 
            Pit,
            Poison,
            Spike,
            Fire,
            Thunder
    }
}
