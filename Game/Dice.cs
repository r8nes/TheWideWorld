using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWideWorld.Game
{
    public class Dice
    {
        public int RollDice(List<DiceType> DiceToRoll) 
        {
            Random rnd = new Random();
            int total = 0;
            foreach (DiceType die in DiceToRoll)
            {
                total += rnd.Next(1,  (int)die);
            }
            return total;    
        }
    }

    public enum DiceType { 
        D4 = 4,
        D6 = 6,
        D8 = 8,
        D10 = 10,
        D12 = 12,
        D20= 20,
        D100 = 100
    }
}
