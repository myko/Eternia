using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Actors;

namespace EterniaGame.Abilities
{
    public class ShieldBlock: Ability
    {
        public ShieldBlock()
        {
            Name = "Shield Block";
            Description = "Reduces damage taken by 20% for 10 seconds.";
            Range = new Range(0);
            DamageType = DamageTypes.SingleTarget;
            Duration = 0.5f;
            Cooldown = new Cooldown(20);
            TargettingType = TargettingTypes.Self;
            AurasApplied.Add(new Aura { Name = "Shield Block", Duration = 10, Statistics = { DamageTaken = 0.8f } });
            CanMiss = false;
            CanBeDodged = false;
        }

        internal override void Generate(Randomizer randomizer, ActorResourceTypes resourceType)
        {
            switch (resourceType)
            {
                case ActorResourceTypes.Mana:
                    ManaCost = 20;
                    Damage = Damage * 1.1f;
                    break;
                case ActorResourceTypes.Energy:
                    EnergyCost = 20;
                    break;
            }
        }
    }
}
