using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Actors;

namespace EterniaGame.Abilities
{
    public class FlashHeal: Ability
    {
        public FlashHeal()
        {
            Name = "Flash Heal";
            Range = new Range(20);
            DamageType = DamageTypes.SingleTarget;
            Duration = 1.5f;
            TargettingType = TargettingTypes.Friendly;
            CanMiss = false;
            CanBeDodged = false;
        }

        internal override void Generate(Randomizer randomizer, ActorResourceTypes resourceType)
        {
            Healing = GenerateDamage(AbilityPowerTypes.SpellPower, randomizer);
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
