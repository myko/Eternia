using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;

namespace EterniaGame
{
    [System.Diagnostics.DebuggerDisplay("Ability '{Name}' (Damage: {Damage}, Healing: {Healing})")]
    public class Ability
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TextureName { get; set; }

        [ContentSerializer(Optional = true)] public Damage Damage { get; set; }
        [ContentSerializer(Optional = true)] public Damage Healing { get; set; }
        [ContentSerializer(Optional = true)] public int ManaCost { get; set; }
        [ContentSerializer(Optional = true)] public Cooldown Cooldown { get; set; }
        [ContentSerializer(Optional = true)] public float CastTime { get; set; }
        [ContentSerializer(Optional = true)] public Range Range { get; set; }
        [ContentSerializer(Optional = true)] public DamageTypes DamageType { get; set; }
        [ContentSerializer(Optional = true)] public TargettingTypes TargettingType { get; set; }
        [ContentSerializer(Optional = true)] public float ThreatModifier { get; set; }
        [ContentSerializer(Optional = true)] public bool CanMiss { get; set; }
        [ContentSerializer(Optional = true)] public virtual bool CanBeDodged { get; set; }
        [ContentSerializer(Optional = true)] public virtual bool CanCrit { get; set; }
        [ContentSerializer(Optional = true)] public ProjectileDefinition SpawnsProjectile { get; set; }
        [ContentSerializer(Optional = true)] public List<Aura> AurasApplied { get; set; }

        [ContentSerializerIgnore]
        public bool Enabled { get; set; }

        public Ability()
        {
            Damage = new Damage();
            Healing = new Damage();
            DamageType = DamageTypes.SingleTarget;
            TargettingType = TargettingTypes.Hostile;
            Cooldown = new Cooldown(0f);
            Range = new Range(20f);
            ThreatModifier = 1f;
            AurasApplied = new List<Aura>();

            CanMiss = true;
            CanBeDodged = true;
            CanCrit = true;

            Enabled = true;
        }
    }
}
