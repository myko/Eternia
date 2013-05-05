using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game.Actors;

namespace Eternia.Game.Events
{
    public class ActorBlocked
    {
        public Actor Source { get; set; }
        public Actor Target { get; set; }
    }
}
