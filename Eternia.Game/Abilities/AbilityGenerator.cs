using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Actors;
using System.Reflection;

namespace EterniaGame.Abilities
{
    public enum AbilityPowerTypes
    {
        AttackPower,
        SpellPower,
        Hybrid
    }

    public class AbilityGenerator
    {
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
            var abilityTypes = typeof(Ability).Assembly.GetTypes().Where(x => x != typeof(Ability) && x.IsSubclassOf(typeof(Ability))).ToArray();
            var abilityType = randomizer.From(abilityTypes);

            Ability ability = (Ability)Activator.CreateInstance(abilityType);

            ability.TextureName = "Ability_meleedamage";
            ability.AnimationName = "ability2";

            ability.Generate(randomizer, resourceType);

            return ability;
        }
    }
}
