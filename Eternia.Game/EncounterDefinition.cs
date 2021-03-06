﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Eternia.Game.Actors;
using Eternia.Game.Items;

namespace Eternia.Game
{
    public class EncounterDefinition
    {
        public string Name { get; set; }
        
        [ContentSerializer(Optional=true)]
        public int HeroLimit { get; set; }
        
        [ContentSerializer(Optional = true)]
        public int ItemLevel { get; set; }
        
        [ContentSerializer(Optional = true)]
        public List<string> PrerequisiteEncounters { get; private set; }

        [ContentSerializer(Optional = true)]
        public List<ActorDefinition> Actors { get; private set; }
        
        [ContentSerializer(Optional = true)]
        public List<TriggerDefinition> Triggers { get; private set; }

        [ContentSerializer(Optional = true)]
        public List<ItemDefinition> Loot { get; private set; }

        [ContentSerializer(Optional = true)]
        public Map Map { get; set; }

        public EncounterDefinition()
        {
            Name = "Unnamed Encounter";
            HeroLimit = 4;
            ItemLevel = 10;
            PrerequisiteEncounters = new List<string>();
            Actors = new List<ActorDefinition>();
            Triggers = new List<TriggerDefinition>();
            Loot = new List<ItemDefinition>();
            Map = new Map();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
