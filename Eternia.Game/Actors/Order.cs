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
        public Actor Target  { get; private set; }

        public Order(Ability ability, Actor actor, Actor target)
        {
            if (ability == null)
                throw new ArgumentNullException("target");

            if (target == null)
                throw new ArgumentNullException("target");

            if (ability.TargettingType == TargettingTypes.Hostile && target.Faction == actor.Faction)
                throw new ArgumentException("Ability " + ability.Name + " must target hostile actors.");

            if (ability.TargettingType == TargettingTypes.Friendly && target.Faction != actor.Faction)
                throw new ArgumentException("Ability " + ability.Name + " must target friendly actors.");

            if (ability.TargettingType == TargettingTypes.Self && target != actor)
                throw new ArgumentException("Ability " + ability.Name + " must target self.");

            this.Ability = ability;
            this.Target = target;
        }
    }
}
