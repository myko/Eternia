using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace EterniaGame
{
    public enum Factions
    {
        Friend,
        Enemy,
        Neutral
    }

    public enum BaseAnimationState
    {
        Idle,
        Walking,
        Casting,
        Dead
    }

    [System.Diagnostics.DebuggerDisplay("Actor '{Name}'")]
    public class Actor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [ContentSerializerIgnore]
        public bool IsAlive { get; set; }
        [XmlIgnore, Microsoft.Xna.Framework.Content.ContentSerializerIgnore]
        public Queue<Actor> Targets { get; set; }
        public List<Ability> Abilities { get; set; }
        [ContentSerializer(Optional=true)]
        public List<Aura> Auras { get; set; }
        public List<Item> Equipment { get; set; }
        //[ContentSerializer(Optional = true)]
        //public List<Item> Inventory { get; set; }
        public Factions Faction { get; set; }
        [ContentSerializer(Optional=true)]
        public Vector2 Position { get; set; }
        [XmlIgnore, ContentSerializerIgnore]
        public Vector2 Direction { get; set; }
        [XmlIgnore, ContentSerializerIgnore]
        public Vector2? Destination { get; set; }
        public float Diameter { get; set; }
        public float Radius { get { return Diameter * 0.5f; } }
        public bool PlayerControlled { get; set; }
        [XmlIgnore, ContentSerializerIgnore]
        public ThreatList ThreatList { get; set; }
        public string TextureName { get; set; }
        public float ThreatModifier { get; set; }
        public Cooldown Swing { get; set; }
        [XmlIgnore, ContentSerializerIgnore]
        public Ability CastingAbility { get; set; }
        [XmlIgnore, ContentSerializerIgnore]
        public Cooldown CastingProgress { get; set; }
        public Statistics BaseStatistics { get; set; }
        [XmlIgnore, ContentSerializerIgnore]
        public BaseAnimationState BaseAnimationState { get; set; }
        [XmlIgnore, ContentSerializerIgnore]
        public Cooldown GlobalCooldown { get; set; }

        private float currentHealth;
        [ContentSerializerIgnore]
        public float CurrentHealth
        {
            get { return Math.Min(currentHealth, MaximumHealth); }
            set { currentHealth = Math.Min(value, MaximumHealth); }
        }

        [ContentSerializerIgnore]
        public float MaximumHealth
        {
            get { return CurrentStatistics.Health; }
        }

        [ContentSerializerIgnore]
        public float HealthFraction
        {
            get { return CurrentHealth / MaximumHealth; }
        }

        private float currentMana;
        [ContentSerializerIgnore]
        public float CurrentMana
        {
            get { return Math.Min(currentMana, MaximumMana); }
            set { currentMana = Math.Min(value, MaximumMana); }
        }

        [ContentSerializerIgnore]
        public float MaximumMana
        {
            get { return CurrentStatistics.Mana; }
        }

        [ContentSerializerIgnore]
        public float ManaFraction
        {
            get { return CurrentMana / MaximumMana; }
        }

        [ContentSerializerIgnore]
        public Statistics CurrentStatistics
        {
            get
            {
                return BaseStatistics + 
                    Auras.Aggregate(new Statistics(), (statistics, aura) => statistics + aura.Statistics) + 
                    Equipment.Aggregate(new Statistics(), (statistics, item) => statistics + item.Statistics);
            }
        }

        //[XmlIgnore, ContentSerializerIgnore]
        //public AnimationPlayer AnimationPlayer { get; set; }

        public Actor()
        {
            Abilities = new List<Ability>();
            Auras = new List<Aura>();
            Equipment = new List<Item>();

            IsAlive = true;
            BaseStatistics = new Statistics() { Health = 125, Mana = 125 };

            CurrentHealth = MaximumHealth;
            CurrentMana = MaximumMana;

            Swing = new Cooldown(1.5f);
            Diameter = 1.0f;
            ThreatModifier = 1.0f;
            Targets = new Queue<Actor>();
            ThreatList = new ThreatList();

            Direction = new Vector2(0, -1);
            GlobalCooldown = new Cooldown(1f);
        }

        public Actor(ActorDefinition actorDefinition)
            : this()
        {
            Id = actorDefinition.Id;
            Abilities.AddRange(actorDefinition.Abilities);
            BaseStatistics = actorDefinition.BaseStatistics;
            Swing = actorDefinition.Swing;
            Diameter = actorDefinition.Diameter;
            Faction = actorDefinition.Faction;
            Name = actorDefinition.Name;
            TextureName = actorDefinition.TextureName;
            ThreatModifier = actorDefinition.ThreatModifier;

            CurrentHealth = MaximumHealth;
            CurrentMana = MaximumMana;
            ThreatList.Clear();
        }

        public Actor SelectTarget(IEnumerable<Actor> availableTargets, TargettingTypes targettingType)
        {
            if (targettingType == TargettingTypes.Self)
                return this;

            if (targettingType == TargettingTypes.Heal)
                return availableTargets.Where(target => target.Faction == Faction && target.CurrentHealth < target.MaximumHealth).OrderBy(target => target.HealthFraction).FirstOrDefault();

            if (targettingType == TargettingTypes.Hostile)
                return availableTargets.FirstOrDefault(target => target.Faction != Faction);
            else
                return availableTargets.FirstOrDefault(target => target.Faction == Faction);
        }

        public bool Move(float deltaTime)
        {
            float movementSpeed = 5f;

            if (Destination.HasValue)
            {
                var direction = Destination.Value - Position;
                var distance = direction.Length();
                if (distance > 0.05f)
                {
                    direction.Normalize();
                    Position += direction * movementSpeed * deltaTime;
                    Direction = direction;
                    CastingAbility = null;
                    CastingProgress = null;

                    return true;
                }
                else
                {
                    Destination = null;

                    return false;
                }
            }
            else
            {
                // Move actor towards enemy targets.
                if (Targets.Any() && Faction != Targets.Peek().Faction)
                {
                    var target = Targets.Peek();
                    var direction = target.Position - Position;
                    var distance = direction.Length();
                    direction.Normalize();
                    Direction = direction;

                    float minimumRange;
                    var availableAbilities = Abilities.Where(x => x.Cooldown.IsReady && x.Enabled && x.ManaCost <= CurrentMana && x.DamageType != DamageTypes.PointBlankArea);
                    if (availableAbilities.Any())
                        minimumRange = availableAbilities.Max(x => x.Range.Maximum + Radius + target.Radius - 0.2f);
                    else
                        minimumRange = Radius + target.Radius + 0.8f;

                    if (distance > minimumRange)
                    {
                        Position += direction * movementSpeed * deltaTime;
                        CastingAbility = null;
                        CastingProgress = null;

                        return true;
                    }
                    else if (distance < (Radius + target.Radius) * 0.75f)
                    {
                        Position -= direction * movementSpeed * deltaTime;
                        Direction = -direction;
                        CastingAbility = null;
                        CastingProgress = null;

                        return true;
                    }
                }
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        public void Equip(Player player, Item item)
        {
            if (!player.Inventory.Contains(item))
                throw new ArgumentException();
            
            var oldItem = Equipment.Find(i => i.Slot == item.Slot);
            if (oldItem != null)
                Unequip(player, oldItem);

            Equipment.Add(item);
            player.Inventory.Remove(item);
        }

        public void Unequip(Player player, Item item)
        {
            if (!Equipment.Contains(item))
                throw new ArgumentException();

            Equipment.Remove(item);
            player.Inventory.Add(item);
        }

        public Statistics GetItemUpgrade(Item item)
        {
            var oldItem = Equipment.Find(i => i.Slot == item.Slot);
            var oldItemStatistics = new Statistics();
            if (oldItem != null)
            {
                oldItemStatistics = oldItem.Statistics;
            }

            return item.Statistics - oldItemStatistics;
        }
    }

    public static class ActorExtensions
    {
        public static float DistanceFrom(this Actor actor, Actor target)
        {
            return (target.Position - actor.Position).Length();
        }

        public static float DistanceFrom(this Actor actor, Vector3 position)
        {
            return (position - new Vector3(actor.Position, position.Z)).Length();
        }
    }
}
