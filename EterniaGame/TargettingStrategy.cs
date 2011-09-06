using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EterniaGame
{
    public enum TargettingStrategies
    {
        Threat,
        Manual,
        ClosestEnemy,
        ClosestFriend,
        LowestHealthEnemy,
        LowestHealthFriend,
        LowestThreat
    }

    public class TargettingStrategy
    {
        public Actor SelectTarget()
        {
            return null;
        }
    }
}
