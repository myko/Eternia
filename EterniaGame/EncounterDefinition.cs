using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace EterniaGame
{
    public class EncounterDefinition
    {
        public string Name { get; set; }
        
        [ContentSerializer(Optional=true)]
        public int HeroLimit { get; set; }
        
        [ContentSerializer(Optional = true)]
        public int ItemLevel { get; set; }
        
        [ContentSerializer(Optional = true)]
        public List<ActorDefinition> Actors { get; private set; }
        
        [ContentSerializer(Optional = true)]
        public List<TriggerDefinition> Triggers { get; private set; }

        public EncounterDefinition()
        {
            Name = "Unnamed Encounter";
            HeroLimit = 4;
            ItemLevel = 10;
            Actors = new List<ActorDefinition>();
            Triggers = new List<TriggerDefinition>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
