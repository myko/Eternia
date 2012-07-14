using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Actors;

namespace EterniaGame.Abilities
{
    public class HeroicStrike: Ability
    {
        public HeroicStrike()
        {
            Name = "Heroic Strike";
            Range = new Range(2);
            DamageType = DamageTypes.SingleTarget;
            Duration = 0.5f;
            TargettingType = TargettingTypes.Hostile;
            ThreatModifier = 3.5f;
        }

        internal override void Generate(Randomizer randomizer, ActorResourceTypes resourceType)
        {
            Damage = GenerateDamage(AbilityPowerTypes.AttackPower, randomizer) * 1.2f;
            Damage.School = DamageSchools.Physical;

            switch (resourceType)
            {
                case ActorResourceTypes.Mana:
                    ManaCost = 10;
                    Damage = Damage * 1.1f;
                    break;
                case ActorResourceTypes.Energy:
                    EnergyCost = 10;
                    break;
            }
        }
    }
}
