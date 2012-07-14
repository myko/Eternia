using System;
using Microsoft.Xna.Framework.Content;
using EterniaGame.Actors;

namespace EterniaGame
{
    public enum DamageSchools
    {
        Physical,
        Fire,
        Frost,
        Arcane,
        Nature,
        Holy,
        Unholy,
    }

    public class Damage
    {
        private static Random random = new Random();

        [ContentSerializer(Optional = true)]
        public float Value { get; set; }
        [ContentSerializer(Optional = true)]
        public float AttackPowerScale { get; set; }
        [ContentSerializer(Optional = true)]
        public float SpellPowerScale { get; set; }
        [ContentSerializer(Optional = true)]
        public DamageSchools School { get; set; }

        public float CalculateDamage(Actor actor, Actor target)
        {
            var value = (actor.CurrentStatistics.AttackPower * AttackPowerScale + actor.CurrentStatistics.SpellPower * SpellPowerScale + Value);
            value = random.Between(value * actor.CurrentStatistics.Precision, value);
            value = value * actor.CurrentStatistics.DamageDone;
            value = value * target.CurrentStatistics.DamageTaken;
            value = value * (1f - target.CurrentStatistics.DamageReduction.GetReductionForSchool(School));

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

        public static Damage operator *(Damage d1, float f)
        {
            return new Damage
            {
                AttackPowerScale = d1.AttackPowerScale * f,
                SpellPowerScale = d1.SpellPowerScale * f,
                Value = d1.Value * f,
                School = d1.School,
            };
        }
    }
}
