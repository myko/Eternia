using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game.Abilities;

namespace Eternia.Game.Actors
{
    public class Order
    {
        public Ability Ability { get; private set; }

        public Order(Ability ability)
        {
            this.Ability = ability;
        }
    }
}
