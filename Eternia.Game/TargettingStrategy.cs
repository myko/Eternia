using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eternia.Game
{
    public enum TargettingStrategies
    {
        Manual,
        Threat,
        ClosestEnemy,
        ClosestFriend,
        LowestHealthEnemy,
        LowestHealthFriend,
        LowestThreat
    }

    public class TargetingStrategy
    {
        public TargettingStrategies Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }

        public override string ToString()
        {
            return Name;
        }

        private static TargetingStrategy[] all;
        public static TargetingStrategy[] All()
        {
            if (all == null)
                all = new TargetingStrategy[] {
                    new TargetingStrategy { Value = TargettingStrategies.Manual, Name = "Manual", Description = "Manually select target.", Cost = 0 },
                    new TargetingStrategy { Value = TargettingStrategies.Threat, Name = "Threat", Description = "Attack the enemy that has damaged you the most.", Cost = 100 },
                    new TargetingStrategy { Value = TargettingStrategies.ClosestEnemy, Name = "Closest (Enemy)", Description = "Attack the closest enemy.", Cost = 100 },
                    new TargetingStrategy { Value = TargettingStrategies.ClosestFriend, Name = "Closest (Friend)", Description = "Aid the closest friend.", Cost = 100 },
                    new TargetingStrategy { Value = TargettingStrategies.LowestHealthEnemy, Name = "Lowest Health (Enemy)", Description = "Attack the enemy with lowest health.", Cost = 200 },
                    new TargetingStrategy { Value = TargettingStrategies.LowestHealthFriend, Name = "Lowest Health (Friend)", Description = "Aid the friend with lowest health (fractional).", Cost = 200 },
                    new TargetingStrategy { Value = TargettingStrategies.LowestThreat, Name = "Lowest Threat", Description = "Attack the enemy that hates you the least.", Cost = 500 },
                };

            return all;
        }
    }
}
