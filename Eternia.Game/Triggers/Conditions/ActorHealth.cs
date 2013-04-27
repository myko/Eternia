using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eternia.Game.Triggers.Conditions
{
    public class ActorHealth : TriggerCondition
    {
        public string ActorId { get; set; }
        public float Health { get; set; }

        public override bool IsTrue(Battle battle)
        {
            return (battle.Actors.Any(x => x.Id == ActorId && x.CurrentHealth <= Health));
        }
    }
}
