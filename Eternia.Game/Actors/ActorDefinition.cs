using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Eternia.Game.Abilities;
using Eternia.Game.Stats;

namespace Eternia.Game.Actors
{
    public class ActorDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Factions Faction { get; set; }

        [ContentSerializer(Optional=true)]
        public float Diameter { get; set; }
        [ContentSerializer(Optional = true)]
        public float MovementSpeed { get; set; }

        public string TextureName { get; set; }

        public Statistics BaseStatistics { get; set; }
        
        [ContentSerializer(Optional=true)]
        public List<Ability> Abilities { get; private set; }

        public ActorDefinition()
        {
            Abilities = new List<Ability>();
            Diameter = 1f;
            MovementSpeed = 5f;
        }
    }
}
