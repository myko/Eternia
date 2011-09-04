using System;
using Microsoft.Xna.Framework.Content;

namespace EterniaGame
{
    public class Damage
    {
        private static Random random = new Random();

        [ContentSerializer(Optional = true)]
        public float Value { get; set; }
        [ContentSerializer(Optional = true)]
        public float AttackPowerScale { get; set; }
        [ContentSerializer(Optional = true)]
        public float SpellPowerScale { get; set; }

        public float CalculateDamage(Actor actor, Actor target)
        {
            var value = (actor.CurrentStatistics.AttackPower * AttackPowerScale + actor.CurrentStatistics.SpellPower * SpellPowerScale + Value);
            value = random.Between(value * actor.CurrentStatistics.Precision, value);
            value = value * actor.CurrentStatistics.DamageDone;
            value = value * target.CurrentStatistics.DamageTaken;
            value = value * (1f - target.CurrentStatistics.ArmorReduction);

            return value;
        }

        public float CalculateHealing(Actor actor, Actor target)
        {
            var value = (actor.CurrentStatistics.AttackPower * AttackPowerScale + actor.CurrentStatistics.SpellPower * SpellPowerScale + Value);
            value = random.Between(value * actor.CurrentStatistics.Precision, value);
            value = value * actor.CurrentStatistics.HealingDone;
            value = value * target.CurrentStatistics.HealingTaken;

            return value;
        }
    }
}
