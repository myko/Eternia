using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace EterniaGame
{
    public class ActorDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Factions Faction { get; set; }

        [ContentSerializer(Optional=true)]
        public float Diameter { get; set; }
        
        public string TextureName { get; set; }
        
        [ContentSerializer(Optional=true)]
        public float ThreatModifier { get; set; }

        public Cooldown Swing { get; set; }
        public Statistics BaseStatistics { get; set; }
        
        [ContentSerializer(Optional=true)]
        public List<Ability> Abilities { get; set; }

        public ActorDefinition()
        {
            Abilities = new List<Ability>();
            Diameter = 1f;
            ThreatModifier = 1f;
        }
    }
}
