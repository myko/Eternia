using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Abilities;
using Eternia.Game.Stats;

namespace EterniaGame
{
    public enum CombatOutcome
    {
        None,
        Miss,
        Dodge,
        Crit,
        Hit
    }

    public class CombatTable
    {
        private Random random;
        private int missRating;
        private int dodgeRating;
        private float critChance;
        private int hitRating;

        public CombatTable(Random random, Statistics actorStatistics, Statistics targetStatistics)
        {
            this.random = random;

            missRating = targetStatistics.For<Miss>().Rating;
            dodgeRating = targetStatistics.For<Dodge>().Rating;
            critChance = actorStatistics.For<CriticalStrike>().Chance;
            hitRating = actorStatistics.For<Hit>().Rating;
        }

        public CombatTable(Random random, Statistics actorStatistics, Statistics targetStatistics, Ability ability)
        {
            this.random = random;

            missRating = ability.CanMiss ? targetStatistics.For<Miss>().Rating : 0;
            dodgeRating = ability.CanBeDodged ? targetStatistics.For<Dodge>().Rating : 0;
            critChance = ability.CanCrit ? actorStatistics.For<CriticalStrike>().Chance : 0;
            hitRating = actorStatistics.For<Hit>().Rating;
        }

        public CombatOutcome Roll()
        {
            int roll = random.Next(Math.Max(1, missRating + dodgeRating + hitRating));

            if (roll < missRating)
                return CombatOutcome.Miss;
            roll -= missRating;

            if (roll < dodgeRating)
                return CombatOutcome.Dodge;
            roll -= dodgeRating;

            if (random.NextDouble() < critChance)
                return CombatOutcome.Crit;

            return CombatOutcome.Hit;
        }
    }
}
