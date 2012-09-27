using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;
using EterniaGame.Actors;
using System.ComponentModel;

namespace EterniaGame.Abilities
{
    [System.Diagnostics.DebuggerDisplay("Ability '{Name}' (Damage: {Damage}, Healing: {Healing})")]
    public class Ability
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AnimationName { get; set; }
        public string TextureName { get; set; }

        [ContentSerializer(Optional = true)] public Damage Damage { get; set; }
        [ContentSerializer(Optional = true)] public Damage Healing { get; set; }
        [ContentSerializer(Optional = true)] public int ManaCost { get; set; }
        [ContentSerializer(Optional = true)] public int EnergyCost { get; set; }
        [ContentSerializer(Optional = true)] public Cooldown Cooldown { get; set; }
        [ContentSerializer(Optional = true)] public float Duration { get; set; }
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
            Name = "!!! Unnamed Ability !!!";
            Description = "";

            Damage = new Damage();
            Healing = new Damage();
            DamageType = DamageTypes.SingleTarget;
            TargettingType = TargettingTypes.Hostile;
            Cooldown = new Cooldown(0f);
            Range = new Range(20f);
            ThreatModifier = 1f;
            AurasApplied = new List<Aura>();
            Duration = 1;

            CanMiss = true;
            CanBeDodged = true;
            CanCrit = true;

            Enabled = true;
        }

        public override string ToString()
        {
            return Name;
        }

        internal virtual void Generate(Randomizer randomizer, ActorResourceTypes resourceType)
        {
        }

        internal Damage GenerateDamage(AbilityPowerTypes powerType, Randomizer randomizer)
        {
            var multipleTargetsFactor = 1.0f;

            switch (DamageType)
            {
                case DamageTypes.Cleave:
                    multipleTargetsFactor = 0.5f;
                    break;
                case DamageTypes.PointBlankArea:
                    multipleTargetsFactor = 0.3f;
                    break;
            }

            var damage = new Damage();
            damage.Value = randomizer.Between(1f, 5f) * multipleTargetsFactor;

            switch (powerType)
            {
                case AbilityPowerTypes.AttackPower:
                    damage.AttackPowerScale = randomizer.Between(0.5f, 2f) * multipleTargetsFactor;
                    break;
                case AbilityPowerTypes.SpellPower:
                    damage.SpellPowerScale = randomizer.Between(0.5f, 2f) * multipleTargetsFactor;
                    break;
                case AbilityPowerTypes.Hybrid:
                    damage.AttackPowerScale = randomizer.Between(0.25f, 1f) * multipleTargetsFactor;
                    damage.SpellPowerScale = randomizer.Between(0.25f, 1f) * multipleTargetsFactor;
                    break;
            }

            return damage;
        }
    }
}
