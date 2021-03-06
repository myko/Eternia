﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;
using Eternia.Game.Actors;
using Newtonsoft.Json;
using Eternia.Game.Stats;

namespace Eternia.Game
{
    public class Aura
    {
        public string Name { get; set; }
        public float Duration { get; set; }
        [ContentSerializer(Optional = true)]
        public Damage Damage { get; set; }
        [ContentSerializer(Optional = true)]
        public Damage Healing { get; set; }
        [ContentSerializer(Optional = true)]
        public Cooldown Cooldown { get; set; }
        [ContentSerializer(Optional = true)]
        public Statistics Statistics { get; set; }
        [ContentSerializer(Optional = true)]
        public bool BreaksOnDamage { get; set; }
        [XmlIgnore, ContentSerializerIgnore, JsonIgnore]
        public Actor Owner { get; set; }

        public Aura()
        {
            Damage = new Damage();
            Healing = new Damage();
            Duration = 1f;
            Cooldown = new Cooldown(1f);
            Statistics = new Statistics();
        }
    }
}
