﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Actors;

namespace EterniaGame.Abilities
{
    public class Fireball: Ability
    {
        public Fireball()
        {
            Name = "Fireball";
            Range = new Range(20);
            DamageType = DamageTypes.SingleTarget;
            Duration = 3.0f;
            TargettingType = TargettingTypes.Hostile;
            SpawnsProjectile = new ProjectileDefinition() { ModelName = "rocket1", TextureName = "rocket1_diffuse", Speed = 16 };
        }

        internal override void Generate(Randomizer randomizer, ActorResourceTypes resourceType)
        {
            Damage = GenerateDamage(AbilityPowerTypes.SpellPower, randomizer) * 2f;
            Damage.School = DamageSchools.Fire;

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
