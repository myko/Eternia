using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Actors;

namespace Eternia.Game.Events
{
    public class ActorWasHealed
    {
        public Actor Actor { get; set; }
        public float Healing { get; set; }
        public bool IsCrit { get; set; }
    }
}
