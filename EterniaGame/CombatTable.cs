using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            missRating = targetStatistics.MissRating;
            dodgeRating = targetStatistics.DodgeRating;
            critChance = actorStatistics.CritChance;
            hitRating = actorStatistics.HitRating;
        }

        public CombatTable(Random random, Statistics actorStatistics, Statistics targetStatistics, Ability ability)
        {
            this.random = random;

            missRating = ability.CanMiss ? targetStatistics.MissRating : 0;
            dodgeRating = ability.CanBeDodged ? targetStatistics.DodgeRating : 0;
            critChance = ability.CanCrit ? actorStatistics.CritChance : 0;
            hitRating = actorStatistics.HitRating;
        }

        public CombatOutcome Roll()
        {
            int roll = random.Next(missRating + dodgeRating + hitRating);

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
