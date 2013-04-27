using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game.Actors;

namespace Eternia.Game.Abilities
{
    public class GreaterHeal: Ability
    {
        public GreaterHeal()
        {
            Name = "Greater Heal";
            Range = new Range(20);
            DamageType = DamageTypes.SingleTarget;
            Duration = 3.0f;
            TargettingType = TargettingTypes.Friendly;
            CanMiss = false;
            CanBeDodged = false;
        }

        internal override void Generate(Randomizer randomizer, ActorResourceTypes resourceType)
        {
            Healing = GenerateDamage(AbilityPowerTypes.SpellPower, randomizer) * 2f;
            Healing.School = DamageSchools.Holy;

            switch (resourceType)
            {
                case ActorResourceTypes.Mana:
                    ManaCost = 20;
                    Healing = Healing * 1.1f;
                    break;
                case ActorResourceTypes.Energy:
                    EnergyCost = 20;
                    break;
            }
        }
    }
}
