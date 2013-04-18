using System;
using Microsoft.Xna.Framework.Content;
using EterniaGame.Actors;
using Eternia.Game.Stats;

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
            var value = (actor.CurrentStatistics.For<AttackPower>().Value * AttackPowerScale + actor.CurrentStatistics.For<SpellPower>().Value * SpellPowerScale + Value);
            value = random.Between(value * actor.CurrentStatistics.For<Precision>().Chance, value);
            value = value * actor.CurrentStatistics.For<DamageDone>().Value;
            value = value * target.CurrentStatistics.For<DamageTaken>().Value;
            value = value * (1f - target.CurrentStatistics.For<DamageReduction>().GetReductionForSchool(School));

            return value;
        }

        public float CalculateHealing(Actor actor, Actor target)
        {
            var value = (actor.CurrentStatistics.For<AttackPower>().Value * AttackPowerScale + actor.CurrentStatistics.For<SpellPower>().Value * SpellPowerScale + Value);
            value = random.Between(value * actor.CurrentStatistics.For<Precision>().Chance, value);
            value = value * actor.CurrentStatistics.For<HealingDone>().Value;
            value = value * target.CurrentStatistics.For<HealingTaken>().Value;

            return value;
        }

        public override string ToString()
        {
            string text = Value.ToString("0.00");
            
            if (SpellPowerScale != 0)
                text = SpellPowerScale.ToString("0.00") + "*SP + " + text;

            if (AttackPowerScale != 0)
                text = AttackPowerScale.ToString("0.00") + "*AP + " + text;

            return text;
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
