using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grim_Run
{
    struct DamageDealt
    {
        public string AttackerName;
        public string AttackerId;
        public string DefenderName;
        public string DefenderId;
        public float Damage;
        public DamageType Type;
        public CombatType CombatType;
        public DateTime EventTime;
    }
}
