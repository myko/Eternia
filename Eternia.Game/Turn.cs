using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game.Abilities;
using Eternia.Game.Actors;

namespace Eternia.Game
{
    public enum EventTypes
    {
        Ability,
        ActorDeath,
        AuraApplied,
        AuraExpired,
        AuraDamage,
        AuraHealing
    }

    public class OldEvent
    {
        public EventTypes Type { get; set; }
        public Actor Actor { get; set; }
        public Actor Target { get; set; }
        public float Damage { get; set; }
        public float Damage2 { get; set; }
        public float Healing { get; set; }
        public Ability Ability { get; set; }
        public CombatOutcome CombatOutcome { get; set; }
        public DateTime TimeStamp { get; set; }

        public OldEvent(EventTypes type)
        {
            Type = type;
            TimeStamp = DateTime.Now;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case EventTypes.Ability:

                    if (CombatOutcome.IsMiss)
                        return string.Format("{0}'s {2} missed {1}", Actor.Name, Target.Name, Ability.Name);

                    if (CombatOutcome.IsDodge)
                        return string.Format("{0}'s {2} was dodged by {1}", Actor.Name, Target.Name, Ability.Name);

                    if (CombatOutcome.IsCrit)
                    {
                        if (Damage > 0f && Healing > 0f)
                            return string.Format("{0}'s {2} did {3:0} damage to {1} and healed {1} for {4:0} (critical)", Actor.Name, Target.Name, Ability.Name, Damage, Healing);
                        else if (Damage > 0f && Healing <= 0f)
                            return string.Format("{0}'s {2} did {3:0} damage to {1} (critical)", Actor.Name, Target.Name, Ability.Name, Damage);
                        else if (Damage <= 0f && Healing > 0f)
                            return string.Format("{0}'s {2} healed {1} for {3:0} (critical)", Actor.Name, Target.Name, Ability.Name, Healing);
                        //else
                        //    return string.Format("{0}'s {2} had no effect on {1} (critical)", Actor.Name, Target.Name, Ability.Name);
                    }

                    if (CombatOutcome.IsHit)
                    {
                        if (Damage > 0f && Healing > 0f)
                            return string.Format("{0}'s {2} did {3:0} damage to {1} and healed {1} for {4:0}", Actor.Name, Target.Name, Ability.Name, Damage, Healing);
                        else if (Damage > 0f && Healing <= 0f)
                            return string.Format("{0}'s {2} did {3:0} damage to {1}", Actor.Name, Target.Name, Ability.Name, Damage);
                        else if (Damage <= 0f && Healing > 0f)
                            return string.Format("{0}'s {2} healed {1} for {3:0}", Actor.Name, Target.Name, Ability.Name, Healing);
                        //else
                        //    return string.Format("{0}'s {2} had no effect on {1}", Actor.Name, Target.Name, Ability.Name);
                    }

                    return string.Format("{0}'s {2} had an unknown outcome on {1}", Actor.Name, Target.Name, Ability.Name);
                case EventTypes.ActorDeath:
                    return string.Format("{0} dies.", Actor.Name);
                case EventTypes.AuraApplied:
                    return string.Format("{0} gained an aura.", Target.Name);
                case EventTypes.AuraDamage:
                    return string.Format("{0} took {1:0} damage from an aura.", Target.Name, Damage);
                case EventTypes.AuraHealing:
                    return string.Format("{0} was healed for {1:0} by an aura.", Target.Name, Healing);
                case EventTypes.AuraExpired:
                    return string.Format("An aura expired from {0}.", Target.Name);
            }

            return base.ToString();
        }
    }

    public class Turn
    {
        public int TimeIndex { get; set; }
        public List<OldEvent> Events { get; set; }

        public Turn()
        {
            Events = new List<OldEvent>();
        }
    }
}
