using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eternia.Game.Abilities;
using Microsoft.Xna.Framework;

namespace Eternia.Game.Actors
{
    public class Order
    {
        public Ability Ability { get; private set; }
        public Actor TargetActor  { get; private set; }
        public Vector2? TargetLocation { get; private set; }

        public Order(Ability ability, Actor actor, Actor targetActor = null, Vector2? targetLocation = null)
        {
            if (ability == null)
                throw new ArgumentNullException("ability");

            if (targetActor == null && targetLocation == null)
                throw new ArgumentNullException("targetActor OR targetLocation");

            if (targetActor != null && targetLocation != null)
                throw new ArgumentException("Cannot specify both target actor and target location");

            if (ability.TargettingType == TargettingTypes.Hostile && targetActor.Faction == actor.Faction)
                throw new ArgumentException("Ability " + ability.Name + " must target hostile actors.");

            if (ability.TargettingType == TargettingTypes.Friendly && targetActor.Faction != actor.Faction)
                throw new ArgumentException("Ability " + ability.Name + " must target friendly actors.");

            if (ability.TargettingType == TargettingTypes.Self && targetActor != actor)
                throw new ArgumentException("Ability " + ability.Name + " must target self.");

            if (ability.TargettingType == TargettingTypes.Location && targetLocation == null)
                throw new ArgumentException("Ability " + ability.Name + " must target a location.");

            this.Ability = ability;
            this.TargetActor = targetActor;
            this.TargetLocation = targetLocation;
        }

        public Vector2 GetTargetLocation()
        {
            if (Ability.TargettingType == TargettingTypes.Location)
                return TargetLocation.Value;
            else
                return TargetActor.Position;
        }

        public float GetTargetRadius()
        {
            if (TargetActor != null)
                return TargetActor.Radius;

            return 0;
        }

        public bool HasExpired()
        {
            if (TargetActor != null && !TargetActor.IsAlive)
                return true;

            return false;
        }
    }
}
