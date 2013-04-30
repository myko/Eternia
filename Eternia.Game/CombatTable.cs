using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game.Abilities;
using Eternia.Game.Stats;

namespace Eternia.Game
{
    //public enum CombatOutcome
    //{
    //    None,
    //    Miss,
    //    Dodge,
    //    Crit,
    //    Hit
    //}

    public class CombatOutcome
    {
        public bool IsMiss { get; set; }
        public bool IsDodge { get; set; }
        public bool IsHit { get; set; }
        public bool IsCrit { get; set; }
        public bool IsBlock { get; set; }
    }

    public class CombatTable
    {
        private Random random;
        
        private float dodgeChance;
        private float hitChance;
        private float critChance;
        private float blockChance;

        public CombatTable(Random random, Statistics actorStatistics, Statistics targetStatistics)
        {
            this.random = random;

            dodgeChance = targetStatistics.For<Dodge>().Chance;
            hitChance = actorStatistics.For<Hit>().Chance;
            critChance = actorStatistics.For<CriticalStrike>().Chance;
            blockChance = targetStatistics.For<Block>().Chance;
        }

        public CombatTable(Random random, Statistics actorStatistics, Statistics targetStatistics, Ability ability)
        {
            this.random = random;

            dodgeChance = ability.CanBeDodged ? targetStatistics.For<Dodge>().Chance : 0;
            hitChance = ability.CanMiss ? actorStatistics.For<Hit>().Chance : 1;
            critChance = ability.CanCrit ? actorStatistics.For<CriticalStrike>().Chance : 0;
            blockChance = ability.CanBeBlocked ? targetStatistics.For<Block>().Chance : 0;
        }

        public CombatOutcome Roll()
        {
            var result = new CombatOutcome();

            result.IsDodge = random.NextDouble() < dodgeChance;
            result.IsHit = !result.IsDodge && random.NextDouble() < hitChance;
            result.IsMiss = !result.IsHit && !result.IsDodge;
            result.IsCrit = result.IsHit && random.NextDouble() < critChance;
            result.IsBlock = result.IsHit && random.NextDouble() < blockChance;

            return result;
        }
    }
}
