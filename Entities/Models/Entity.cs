using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWideWorld.Entities.Models
{
    public abstract class Entity
    {
        public int HitPoints = 0;
        public Attack Attack;
    }

    public class Attack {
        public int BaseDice;
        public int BonusDamage;
    }
}
