using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EterniaGame.Actors;

namespace EterniaGame
{
    public class Threat
    {
        public int Value { get; set; }
        public Actor Actor { get; set; }
    }

    public class ThreatList: SortingList<Threat>
    {
        public ThreatList()
            : base((t1, t2) => 
            {
                if (t1.Value == t2.Value)
                    return t1.Actor.Name.CompareTo(t2.Actor.Name);
                else
                    return -t1.Value.CompareTo(t2.Value); 
            })
        {

        }

        public void Add(Actor actor)
        {
            if (!base.Exists(t => t.Actor == actor))
                base.Add(new Threat() { Value = 0, Actor = actor });

            System.Diagnostics.Debug.Assert(this.All(t => this.Count(t2 => t2.Actor == t.Actor) == 1));
        }

        public void Increase(Actor actor, int threatValue)
        {
            var threat = base.Find(t => t.Actor == actor);
            if (threat == null)
                base.Add(new Threat() { Value = (int)(threatValue), Actor = actor });
            else
                threat.Value += (int)(threatValue);

            System.Diagnostics.Debug.Assert(this.All(t => this.Count(t2 => t2.Actor == t.Actor) == 1));
        }

        public int ThreatOf(Actor actor)
        {
            var threat = base.Find(t => t.Actor == actor);
            if (threat == null)
                return 0;
            else
                return threat.Value;
        }
    }
}
