using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Actors;

namespace EterniaGame.Abilities
{
    public class AbilityGenerator
    {
        private enum SkillPowerTypes
        {
            AttackPower,
            SpellPower,
            Hybrid
        }

        private enum SkillRoles
        {
            Damage,
            Heal,
            Threat,
        }

        private readonly Randomizer randomizer;

        public AbilityGenerator(Randomizer randomizer)
        {
            this.randomizer = randomizer;
        }

        public Ability Generate()
        {
            return Generate(randomizer.Next<ActorResourceTypes>());
        }

        public Ability Generate(ActorResourceTypes resourceType)
        {
            var powerType = randomizer.Next<SkillPowerTypes>();
            var skillRole = randomizer.Next<SkillRoles>();

            var ability = new Ability
            {
                Description = skillRole.ToString() + " things!",
                TextureName = "Ability_meleedamage",
                AnimationName = "ability2",
                DamageType = randomizer.Next<DamageTypes>(),
                Duration = randomizer.Between(0.1f, 2.0f),
                Cooldown = new Cooldown(randomizer.Between(0, 10))
            };

            ability.Name = ability.DamageType + " " + skillRole.ToString();

            switch (skillRole)
            {
                case SkillRoles.Damage:
                    ability.Damage = GenerateDamage(powerType, ability.DamageType);
                    ability.TargettingType = TargettingTypes.Hostile;
                    break;
                case SkillRoles.Heal:
                    ability.Healing = GenerateDamage(powerType, ability.DamageType);
                    ability.TargettingType = TargettingTypes.Friendly;
                    ability.CanMiss = false;
                    ability.CanBeDodged = false;
                    break;
                case SkillRoles.Threat:
                    ability.Damage = GenerateDamage(powerType, ability.DamageType);
                    ability.ThreatModifier = 2.5f;
                    ability.TargettingType = TargettingTypes.Hostile;
                    break;
            }

            switch (resourceType)
            {
                case ActorResourceTypes.Mana:
                    ability.ManaCost = randomizer.Between(0, 50);
                    break;
                case ActorResourceTypes.Energy:
                    ability.EnergyCost = randomizer.Between(-50, 100);
                    break;
            }

            return ability;
        }

        private Damage GenerateDamage(SkillPowerTypes powerType, DamageTypes damageType)
        {
            var multipleTargetsFactor = 1.0f;

            switch (damageType)
            {
                case DamageTypes.Cleave:
                    multipleTargetsFactor = 0.5f;
                    break;
                case DamageTypes.PointBlankArea:
                    multipleTargetsFactor = 0.3f;
                    break;
            }

            var damage = new Damage();
            damage.Value = randomizer.Between(1f, 5f) * multipleTargetsFactor;

            switch (powerType)
            {
                case SkillPowerTypes.AttackPower:
                    damage.AttackPowerScale = randomizer.Between(0.5f, 4f) * multipleTargetsFactor;
                    break;
                case SkillPowerTypes.SpellPower:
                    damage.SpellPowerScale = randomizer.Between(0.5f, 4f) * multipleTargetsFactor;
                    break;
                case SkillPowerTypes.Hybrid:
                    damage.AttackPowerScale = randomizer.Between(0.25f, 2f) * multipleTargetsFactor;
                    damage.SpellPowerScale = randomizer.Between(0.25f, 2f) * multipleTargetsFactor;
                    break;
            }

            return damage;
        }
    }
}
