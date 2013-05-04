using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Eternia.Game.Abilities;
using Newtonsoft.Json;
using Eternia.Game.Stats;
using Eternia.Game.Items;

namespace Eternia.Game.Actors
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
        public List<Ability> Abilities { get; set; }
        [ContentSerializer(Optional=true)]
        public List<Aura> Auras { get; set; }
        public List<Item> Equipment { get; set; }
        public Factions Faction { get; set; }
        [ContentSerializer(Optional=true)]
        public Vector2 Position { get; set; }
        public float Diameter { get; set; }
        public float Radius { get { return Diameter * 0.5f; } }
        public bool PlayerControlled { get; set; }
        public string TextureName { get; set; }
        public Statistics BaseStatistics { get; set; }
        [ContentSerializer(Optional=true)]
        public TargettingStrategies TargettingStrategy { get; set; }
        public int Cost { get; set; }
        [ContentSerializer(Optional = true)]
        public float MovementSpeed { get; set; }
        public ActorResourceTypes ResourceType { get; set; }

        [XmlIgnore, ContentSerializerIgnore, JsonIgnore]
        public Queue<Actor> Targets { get; set; }
        [XmlIgnore, ContentSerializerIgnore, JsonIgnore]
        public ThreatList ThreatList { get; set; }

        [XmlIgnore, ContentSerializerIgnore, JsonIgnore]
        public List<Order> Orders { get; set; }
        [XmlIgnore, ContentSerializerIgnore, JsonIgnore]
        public Order CurrentOrder { get; set; }
        [XmlIgnore, ContentSerializerIgnore, JsonIgnore]
        public Cooldown CastingProgress { get; set; }
        
        [XmlIgnore, ContentSerializerIgnore, JsonIgnore]
        public Vector2 Direction { get; set; }
        [XmlIgnore, ContentSerializerIgnore, JsonIgnore]
        public Vector2? Destination { get; set; }
        [XmlIgnore, ContentSerializerIgnore, JsonIgnore]
        public Vector2? OrderedDestination { get; set; }
        
        [XmlIgnore, ContentSerializerIgnore, JsonIgnore]
        public BaseAnimationState BaseAnimationState { get; set; }

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
            get { return CurrentStatistics.For<Health>().Value; }
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
            set { currentMana = Math.Max(0, Math.Min(value, MaximumMana)); }
        }

        [ContentSerializerIgnore]
        public float MaximumMana
        {
            get { return CurrentStatistics.For<Mana>().Value; }
        }

        [ContentSerializerIgnore]
        public float ManaFraction
        {
            get { return CurrentMana / MaximumMana; }
        }

        private float currentEnergy;
        [ContentSerializerIgnore]
        public float CurrentEnergy
        {
            get { return Math.Min(currentEnergy, MaximumEnergy); }
            set { currentEnergy = Math.Max(0, Math.Min(value, MaximumEnergy)); }
        }

        [ContentSerializerIgnore]
        public float MaximumEnergy
        {
            get { return CurrentStatistics.For<Energy>().Value; }
        }

        [ContentSerializerIgnore]
        public float EnergyFraction
        {
            get { return CurrentEnergy / MaximumEnergy; }
        }

        [ContentSerializerIgnore]
        public Statistics CurrentStatistics
        {
            get
            {
                return (BaseStatistics + 
                    Auras.Aggregate(new Statistics(), (statistics, aura) => statistics + aura.Statistics) + 
                    Equipment.Aggregate(new Statistics(), (statistics, item) => statistics + item.Statistics));
            }
        }

        public Actor()
        {
            Abilities = new List<Ability>();
            Auras = new List<Aura>();
            Equipment = new List<Item>();
            Orders = new List<Order>();

            IsAlive = true;
            BaseStatistics = new Statistics();

            CurrentHealth = MaximumHealth;
            CurrentMana = MaximumMana;

            Diameter = 1.0f;
            Targets = new Queue<Actor>();
            ThreatList = new ThreatList();

            Direction = new Vector2(0, -1);
            MovementSpeed = 5f;
        }

        public Actor(ActorDefinition actorDefinition)
            : this()
        {
            Id = actorDefinition.Id;
            Abilities.AddRange(actorDefinition.Abilities);
            Equipment.AddRange(actorDefinition.Equipment.Select(x => new Item(x)));
            BaseStatistics = actorDefinition.BaseStatistics;
            Diameter = actorDefinition.Diameter;
            MovementSpeed = actorDefinition.MovementSpeed;
            Faction = actorDefinition.Faction;
            Name = actorDefinition.Name;
            TextureName = actorDefinition.TextureName;
            Cost = actorDefinition.Cost;
            ResourceType = actorDefinition.ResourceType;

            CurrentHealth = MaximumHealth;
            CurrentMana = MaximumMana;
        }

        public void SelectTarget(IEnumerable<Actor> availableTargets)
        {
            var hostileTargets = availableTargets.Where(x => x.Faction != Faction);
            var friendlyTargets = availableTargets.Where(x => x.Faction == Faction);

            switch (TargettingStrategy)
            {
                case TargettingStrategies.Threat:
                    {
                        Targets.Clear();
                        foreach (var target in ThreatList.Select(x => x.Actor).Where(x => x.Faction != Faction))
                            Targets.Enqueue(target);
                    }
                    break;
                case TargettingStrategies.ClosestEnemy:
                    {
                        Targets.Clear();
                        foreach (var target in hostileTargets.OrderBy(x => this.DistanceFrom(x)))
                            Targets.Enqueue(target);
                    }
                    break;
                case TargettingStrategies.ClosestFriend:
                    {
                        Targets.Clear();
                        foreach (var target in friendlyTargets.OrderBy(x => this.DistanceFrom(x)))
                            Targets.Enqueue(target);
                    }
                    break;
                case TargettingStrategies.LowestHealthEnemy:
                    {
                        Targets.Clear();
                        foreach (var target in hostileTargets.OrderBy(x => x.CurrentHealth))
                            Targets.Enqueue(target);
                    }
                    break;
                case TargettingStrategies.LowestHealthFriend:
                    {
                        Targets.Clear();
                        foreach (var target in friendlyTargets.Where(x => x.HealthFraction < 1f).OrderBy(x => x.HealthFraction))
                            Targets.Enqueue(target);
                    }
                    break;
                case TargettingStrategies.LowestThreat:
                    {
                        Targets.Clear();

                        var enemiesNotTargettingMe = hostileTargets.Where(x => x.ThreatList.Any() &&  x.ThreatList.First().Actor != this);
                        if (enemiesNotTargettingMe.Any())
                            foreach (var target in enemiesNotTargettingMe)
                                Targets.Enqueue(target);
                        else
                            foreach (var target in hostileTargets.OrderBy(x => (100 * x.ThreatList.ThreatOf(this)) / (1 + x.ThreatList.Sum(y => y.Value))))
                                Targets.Enqueue(target);
                    }
                    break;
                case TargettingStrategies.Manual:
                    {
                    }
                    break;
            }
        }
        
        public void FillOrderQueue()
        {
            if (Abilities.Any())
            {
                var abilityTarget = Targets.FirstOrDefault();

                Orders.AddRange(Abilities
                    .Where(x => x.Cooldown.IsReady && x.ManaCost <= CurrentMana && x.EnergyCost <= CurrentEnergy)
                    .Where(x => x.TargettingType == TargettingTypes.Self || abilityTarget != null) // || abilityTarget.DistanceFrom(this).In(x.Range + Radius + abilityTarget.Radius))
                    .Where(x =>
                        (x.TargettingType == TargettingTypes.Self) ||
                        (x.TargettingType == TargettingTypes.Hostile && abilityTarget != null && abilityTarget.Faction != Faction) ||
                        (x.TargettingType == TargettingTypes.Friendly && abilityTarget != null && abilityTarget.Faction == Faction))
                    .OrderByDescending(x => x.Cooldown.Duration)
                    .Select(x =>
                    {
                        if (x.TargettingType == TargettingTypes.Self)
                            return new Order(x, this, this);
                        else
                            return new Order(x, this, abilityTarget);
                    }));
            }
        }

        public bool PickDestination()
        {
            if (OrderedDestination.HasValue)
            {
                CurrentOrder = null;
                CastingProgress = null;
                Destination = OrderedDestination;
            }
            else
            {
                // Move actor towards enemy targets.
                if (Orders.Any()) // && Faction != Targets.Peek().Faction)
                {
                    var order = Orders.First();
                    var direction = order.GetTargetLocation() - Position;
                    var distance = direction.Length();

                    //var availableAbilities = Abilities
                    //    .Where(x => 
                    //        x.Cooldown.IsReady && 
                    //        x.ManaCost <= CurrentMana && x.EnergyCost <= CurrentEnergy && 
                    //        x.DamageType != DamageTypes.PointBlankArea &&
                    //        ((x.TargettingType == TargettingTypes.Hostile && Faction != target.Faction) || 
                    //        (x.TargettingType == TargettingTypes.Friendly && Faction == target.Faction)));

                    var minimumRange = order.Ability.Range.Maximum + Radius + order.GetTargetRadius() - 0.2f;

                    if (distance > minimumRange)
                        Destination = Position + Vector2.Normalize(direction) * (distance - minimumRange);
                    else
                        Destination = null;
                }
            }

            if (Destination.HasValue && Vector2.Distance(Position, Destination.Value) < 0.05f)
                Destination = null;

            if (OrderedDestination.HasValue && Vector2.Distance(Position, OrderedDestination.Value) < 0.05f)
                OrderedDestination = null;

            return true;
        }

        public bool Move(float deltaTime, IEnumerable<Actor> otherActors)
        {
            if (Destination.HasValue)
            {
                var direction = Destination.Value - Position;
                var distance = direction.Length();
                if (distance > 0.05f)
                {
                    direction.Normalize();
                    var newPosition = Position + direction * Math.Min(distance, MovementSpeed * deltaTime);

                    var collidingActors = otherActors.Where(x => x.DistanceFrom(newPosition) < (x.Radius + Radius) * 1.05f);
                    if (!collidingActors.Any())
                    {
                        Position = newPosition;
                        Direction = direction;
                        CurrentOrder = null;
                        CastingProgress = null;

                        return true;
                    }
                    else
                    {
                        var collidingActor = collidingActors.OrderBy(x => x.DistanceFrom(newPosition)).First();
                        var intersection = collidingActor.DistanceFrom(newPosition) - (collidingActor.Radius + Radius) * 1.05f;

                        Position = newPosition + (newPosition - collidingActor.Position) * -intersection;
                        Direction = direction;
                        CurrentOrder = null;
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

        public static float DistanceFrom(this Actor actor, Vector2 position)
        {
            return (position - actor.Position).Length();
        }

        public static float DistanceFrom(this Vector2 position, Vector2 destination)
        {
            return (destination - position).Length();
        }

        public static float DistanceFrom(this Vector2 position, Actor actor)
        {
            return (actor.Position - position).Length();
        }

        public static float DistanceFrom(this Actor actor, Vector3 position)
        {
            return (position - new Vector3(actor.Position, position.Z)).Length();
        }
    }
}
