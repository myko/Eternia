using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EterniaGame
{
    public class Player
    {
        public List<Actor> Heroes { get; set; }
        public List<Item> Inventory { get; set; }

        public Player()
        {
            Heroes = new List<Actor>();
            Inventory = new List<Item>();
        }
    }
}
