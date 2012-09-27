using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Actors;

namespace EterniaGame
{
    public class Player
    {
        public int Gold { get; set; }
        public List<Actor> Heroes { get; set; }
        public List<Item> Inventory { get; set; }
        public List<TargettingStrategies> UnlockedTargetingStrategies { get; set; }
        public List<string> CompletedEncounters { get; set; }

        public Player()
        {
            Heroes = new List<Actor>();
            Inventory = new List<Item>();
            UnlockedTargetingStrategies = new List<TargettingStrategies>();
            CompletedEncounters = new List<string>();
        }

        public static Player CreateWithDefaults()
        {
            var player = new Player();
            player.UnlockedTargetingStrategies.Add(TargettingStrategies.Manual);
            player.Gold = 5000000;

            return player;
        }
    }
}
