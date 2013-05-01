using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Eternia.Game.Abilities;
using Eternia.Game.Stats;
using Eternia.Game.Items;

namespace Eternia.Game.Actors
{
    [System.Security.SecuritySafeCritical]
    public class ActorDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Factions Faction { get; set; }

        [ContentSerializer(Optional=true)]
        public ActorResourceTypes ResourceType { get; set; }

        [ContentSerializer(Optional = true)]
        public int Cost { get; set; }

        [ContentSerializer(Optional=true)]
        public float Diameter { get; set; }
        [ContentSerializer(Optional = true)]
        public float MovementSpeed { get; set; }

        public string TextureName { get; set; }

        [ContentSerializer(Optional = true)]
        public Statistics BaseStatistics { get; set; }
        
        [ContentSerializer(Optional=true)]
        public List<Ability> Abilities { get; private set; }

        [ContentSerializer(Optional = true)]
        public List<ItemDefinition> Equipment { get; set; }

        public ActorDefinition()
        {
            BaseStatistics = new Statistics(); 
            Abilities = new List<Ability>();
            Equipment = new List<ItemDefinition>();

            Diameter = 1f;
            MovementSpeed = 5f;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
