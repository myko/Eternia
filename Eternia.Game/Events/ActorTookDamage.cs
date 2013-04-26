using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Actors;

namespace Eternia.Game.Events
{
    public class ActorTookDamage
    {
        public Actor Actor { get; set; }
        public float Damage { get; set; }
        public bool IsCrit { get; set; }
    }
}
