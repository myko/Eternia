using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EterniaGame
{
    public class Modifiers
    {
        public float HealthModifier { get; set; }
        public float AttackPowerModifier { get; set; }
        public float SpellPowerModifier { get; set; }

        public Modifiers()
        {
            HealthModifier = 1.0f;
            AttackPowerModifier = 1.0f;
            SpellPowerModifier = 1.0f;
        }
    }
}
