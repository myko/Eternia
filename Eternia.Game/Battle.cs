using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Eternia.Game.Triggers;
using Eternia.Game.Abilities;
using Eternia.Game.Actors;
using Eternia.Game.Events;
using Eternia.Game.Stats;

namespace Eternia.Game
{
    public class Battle
    {
        private readonly EncounterDefinition encounterDefinition;
        private static Random random = new Random();

        public List<Actor> Actors { get; private set; }
        public List<Projectile> Projectiles { get; private set; }
        public List<Trigger> Triggers { get; private set; }
        public Queue<GraphicsEffectDefinition> GraphicEffects { get; set; }

        private int timeIndex = 0;

        public Battle()
        {
            Actors = new List<Actor>();
            Projectiles = new List<Projectile>();
            Triggers = new List<Trigger>();
            GraphicEffects = new Queue<GraphicsEffectDefinition>();
        }

        public Battle(EncounterDefinition encounterDefinition)
            : this()
        {
            this.encounterDefinition = encounterDefinition;

            foreach (var triggerDefinition in encounterDefinition.Triggers)
            {
                Triggers.Add(new Trigger(triggerDefinition));
            }
        }

        public void Start()
        {
            RunTriggers();
        }

        public Turn Run()
        {
            return Run(0.1f);
        }

        public Turn Run(float deltaTime)
        {
            var turn = new Turn() { TimeIndex = timeIndex };

            RunTriggers();

            RunActors(deltaTime, turn);

            RunProjectiles(deltaTime, turn);

            timeIndex++;

            return turn;
        }

        private void RunTriggers()
        {
            foreach (var trigger in Triggers)
            {
                if (trigger.Conditions.All(x => x.IsTrue(this)))
                {
                    trigger.Actions.ForEach(x => x.Execute(encounterDefinition, this));
                }
            }

            Triggers.RemoveAll(x => x.Conditions.All(y => y.IsTrue(this)));
        }

        private void RunProjectiles(float deltaTime, Turn turn)
        {
            foreach (var projectile in Projectiles.Where(x => x.IsAlive))
            {
                RunProjectile(deltaTime, turn, projectile);
            }

            Projectiles.RemoveAll(x => !x.IsAlive);
        }

        private void RunProjectile(float deltaTime, Turn turn, Projectile projectile)
        {
            MoveProjectile(deltaTime, projectile);

            if (projectile.Target.DistanceFrom(projectile.Position) < 0.75f)
            {
                var actor = projectile.Owner;
                var ability = projectile.Ability;
                var abilityTarget = projectile.Target;
                
                ApplyAbilityOutcome(turn, actor, ability, abilityTarget);

                projectile.IsAlive = false;
            }
        }

        private void MoveProjectile(float deltaTime, Projectile projectile)
        {
            var d = new Vector3(projectile.Target.Position, projectile.Target.Radius) - projectile.Position;
            if (d.Length() > 1)
                d.Normalize();
            projectile.Position += d * projectile.Speed * deltaTime;
        }

        private void RunActors(float deltaTime, Turn turn)
        {
            foreach (var actor in Actors)
            {
                RunActor(deltaTime, turn, actor);
            }

            // Collision detection
            //foreach (var actor in Actors.Where(x => x.IsAlive))
            //{
            //    foreach (var otherActor in Actors.Where(x => x != actor && x.IsAlive))
            //    {
            //        if (Vector2.Distance(actor.Position, otherActor.Position) < (actor.Radius + otherActor.Radius - 0.01f))
            //        {
            //            var direction = Vector2.Normalize(actor.Position - otherActor.Position);
            //            actor.Position = otherActor.Position + direction * (actor.Radius + otherActor.Radius);
            //        }
            //    }
            //}
        }

        private void RunActor(float deltaTime, Turn turn, Actor actor)
        {
            // Aggro radius threat list calculations
            foreach (var other in Actors.Where(ba => ba.IsAlive))
            {
                var aggroRadius = 10f; // TODO: Remove hard coded aggro radius
                if (actor.DistanceFrom(other) < aggroRadius)
                    actor.ThreatList.Add(other);
            }

            // Drop dead actors from threat lists
            actor.ThreatList.RemoveAll(t => !t.Actor.IsAlive);

            // Drop dead targets from order lists
            actor.Orders.RemoveAll(o => o.HasExpired());

            while (actor.Targets.Any() && !actor.Targets.Peek().IsAlive)
                actor.Targets.Dequeue();
            
            // TODO: Remove hard coded mana regen
            actor.CurrentMana += 1f * deltaTime;
            // TODO: Remove hard coded energy regen
            actor.CurrentEnergy += 10f * deltaTime;

            CoolAbilities(deltaTime, actor);
            RunActorAuras(deltaTime, turn, actor);

            if (actor.IsAlive)
            {
                actor.SelectTarget(Actors.Where(x => x != actor));
                actor.PickDestination();

                if (actor.CurrentOrder != null)
                {
                    RunAbilityCast(deltaTime, turn, actor);
                }
                else if (actor.Destination != null)
                {
                    actor.Move(deltaTime, Actors.Where(x => x.IsAlive && x != actor));
                }
                else
                {
                    if (!actor.Orders.Any())
                        actor.FillOrderQueue();

                    if (actor.Orders.Any())
                    {
                        var order = actor.Orders.First();

                        if (UseOrder(turn, actor, order))
                        {
                            actor.Orders.Remove(order);
                        }
                    }
                }

                //actor.PickDestination();
                        
                //if (actor.Targets.Any())
                //{
                //    var target = actor.Targets.Peek();
                //    if (target.DistanceFrom(actor) > 0.01f)
                //        actor.Direction = Vector2.Normalize(target.Position - actor.Position);
                //}
                //actor.BaseAnimationState = BaseAnimationState.Casting;
                //if (actor.CastingAbility != null)
                //{
                //    RunAbilityCast(deltaTime, turn, actor);
                //}
                //else
                //{
                //    var ability = actor.SelectAbility();
                //    if (actor.Destination == null && ability != null)
                //    {
                //        UseAbility(turn, actor, ability);
                //    }
                //    else
                //    {
                //        if (actor.Move(deltaTime, Actors.Where(x => x.IsAlive && x != actor)))
                //            actor.BaseAnimationState = BaseAnimationState.Walking;
                //        else
                //            actor.BaseAnimationState = BaseAnimationState.Idle;
                //    }
                //}
            }
            else
            {
                actor.BaseAnimationState = BaseAnimationState.Dead;
            }

            if (actor.IsAlive && actor.CurrentHealth <= 0f)
            {
                KillActor(turn, actor);
            }
        }

        private bool IsReadyToSelectNewTarget(Actor actor)
        {
            if (!actor.IsAlive)
                return false;

            if (actor.CastingProgress != null && actor.CastingProgress.Current > 0f && actor.CurrentOrder != null)
                return false;

            return true;
        }

        private void KillActor(Turn turn, Actor actor)
        {
            actor.IsAlive = false;
            actor.Targets.Clear();
            actor.Auras.Clear();
            turn.Events.Add(new OldEvent(EventTypes.ActorDeath)
            {
                Actor = actor
            });

            foreach (var actorWithDeadTarget in Actors.Where(x => x.PlayerControlled))
            {
                while (actorWithDeadTarget.Targets.Any() && !actorWithDeadTarget.Targets.Peek().IsAlive)
                    actorWithDeadTarget.Targets.Dequeue();
            }
        }

        private void RunActorAuras(float deltaTime, Turn turn, Actor actor)
        {
            foreach (var aura in actor.Auras)
            {
                aura.Cooldown.Cool(deltaTime);
                aura.Duration -= deltaTime;

                if (aura.Cooldown.IsReady)
                {
                    var combatTable = new CombatTable(new Random(), aura.Owner.CurrentStatistics, actor.CurrentStatistics);
                    var roll = combatTable.Roll();
                    var damage = aura.Damage.CalculateDamage(aura.Owner, actor);
                    var healing = aura.Healing.CalculateHealing(aura.Owner, actor);
                    if (roll.IsCrit)
                    {
                        damage *= 2;
                        healing *= 2;
                    }

                    if (damage > 0)
                    {
                        Event.Raise(new ActorTookDamage { Actor = actor, Damage = damage, IsCrit = roll.IsCrit });
                        turn.Events.Add(new OldEvent(EventTypes.AuraDamage) { Actor = aura.Owner, Target = actor, Damage = damage });
                        actor.CurrentHealth -= damage;
                    }
                    if (healing > 0)
                    {
                        Event.Raise(new ActorWasHealed { Actor = actor, Healing = healing, IsCrit = roll.IsCrit });
                        turn.Events.Add(new OldEvent(EventTypes.AuraHealing) { Actor = aura.Owner, Target = actor, Healing = healing });
                        actor.CurrentHealth += healing;
                    }
                    aura.Cooldown.Incur();
                }
            }

            actor.Auras.ForEach(aura =>
            {
                if (aura.Duration <= 0f)
                {
                    actor.Auras.Remove(aura);
                    turn.Events.Add(new OldEvent(EventTypes.AuraExpired) { Actor = actor, Target = actor });
                }
            });

            actor.Auras.RemoveAll(ba => ba.Duration <= 0f);
        }

        private void CoolAbilities(float deltaTime, Actor actor)
        {
            foreach (var ability in actor.Abilities)
            {
                ability.Cooldown.Cool(deltaTime);
            }
        }

        private void RunAbilityCast(float deltaTime, Turn turn, Actor actor)
        {
            actor.CastingProgress.Cool(deltaTime);

            if (actor.ChannelProgress != null)
            {
                actor.ChannelProgress.Cool(deltaTime);

                if (actor.ChannelProgress.IsReady)
                {
                    ApplyChannelAbility(turn, actor, actor.CurrentOrder);
                }
            }
            else
            {
                if (actor.CastingProgress.IsReady)
                {
                    ApplyCastAbility(turn, actor, actor.CurrentOrder);
                }
            }
        }

        private bool UseOrder(Turn turn, Actor actor, Order order)
        {
            var ability = order.Ability;

            if (!ability.Cooldown.IsReady)
                return false;
            
            if (ability.DamageType == DamageTypes.SingleTarget)
            {
                var abilityTarget = order.TargetActor;
                //if (ability.TargettingType == TargettingTypes.Self)
                //    abilityTarget = actor;

                //if (!actor.PlayerControlled)
                //    abilityTarget = actor.GetAbilityTarget(ability.TargettingType);

                if (abilityTarget != null && abilityTarget.IsAlive)
                {
                    if (ability.TargettingType == TargettingTypes.Hostile && actor.Faction == abilityTarget.Faction)
                        return false;

                    if (ability.TargettingType == TargettingTypes.Friendly && actor.Faction != abilityTarget.Faction)
                        return false;

                    if (order.GetTargetLocation().DistanceFrom(actor).In(ability.Range + actor.Radius + abilityTarget.Radius))
                    {
                        actor.CurrentOrder = order;
                        actor.CastingProgress = new Cooldown(ability.Duration);
                        actor.CastingProgress.Incur();
                        if (ability.IsChanneled)
                        {
                            actor.ChannelProgress = new Cooldown(1.0f);
                            actor.ChannelProgress.Incur();
                        }

                        return true;
                    }
                }
            }
            else if (ability.DamageType == DamageTypes.PointBlankArea)
            {
                //if (Actors.Any(x => ValidPointBlankAreaTarget(actor, x, ability)))
                {
                    actor.CurrentOrder = order;
                    actor.CastingProgress = new Cooldown(ability.Duration);
                    actor.CastingProgress.Incur();
                    if (ability.IsChanneled)
                    {
                        actor.ChannelProgress = new Cooldown(1.0f);
                        actor.ChannelProgress.Incur();
                    }

                    return true;
                }
            }
            else if (ability.DamageType == DamageTypes.Cleave)
            {
                var primaryTarget = order.TargetActor;

                //if (!actor.PlayerControlled)
                //    primaryTarget = actor.GetAbilityTarget(ability.TargettingType);

                if (primaryTarget != null && primaryTarget.IsAlive)
                {
                    if (ability.TargettingType == TargettingTypes.Hostile && actor.Faction == primaryTarget.Faction)
                        return false;

                    if (ability.TargettingType == TargettingTypes.Friendly && actor.Faction != primaryTarget.Faction)
                        return false;

                    if (order.GetTargetLocation().DistanceFrom(actor).In(ability.Range + actor.Radius + order.GetTargetRadius()))
                    {
                        actor.CurrentOrder = order;
                        actor.CastingProgress = new Cooldown(ability.Duration);
                        actor.CastingProgress.Incur();
                        if (ability.IsChanneled)
                        {
                            actor.ChannelProgress = new Cooldown(1.0f);
                            actor.ChannelProgress.Incur();
                        }

                        return true;
                    }
                }
            }
            else if (ability.DamageType == DamageTypes.Location)
            {
                if (order.GetTargetLocation().DistanceFrom(actor).In(ability.Range + actor.Radius))
                {
                    actor.CurrentOrder = order;
                    actor.CastingProgress = new Cooldown(ability.Duration);
                    actor.CastingProgress.Incur();
                    if (ability.IsChanneled)
                    {
                        actor.ChannelProgress = new Cooldown(1.0f);
                        actor.ChannelProgress.Incur();
                    }

                    return true;
                }
            }

            return false;
        }

        private void ApplyCastAbility(Turn turn, Actor actor, Order order)
        {
            if (order.Ability.DamageType == DamageTypes.SingleTarget)
            {
                var abilityTarget = order.TargetActor;

                if (abilityTarget != null && abilityTarget.IsAlive)
                {
                    if (abilityTarget.DistanceFrom(actor).In(order.Ability.Range + actor.Radius + abilityTarget.Radius))
                        ApplySingleTargetAbility(turn, actor, order.Ability, abilityTarget);
                }
            }
            else if (order.Ability.DamageType == DamageTypes.PointBlankArea)
            {
                ApplyPointBlankAreaAbility(turn, actor, order.Ability);
            }
            else if (order.Ability.DamageType == DamageTypes.Cleave)
            {
                var primaryTarget = order.TargetActor;

                if (primaryTarget != null && primaryTarget.IsAlive)
                {
                    if (primaryTarget.DistanceFrom(actor).In(order.Ability.Range + actor.Radius + primaryTarget.Radius))
                        ApplyCleaveAbility(turn, actor, order.Ability, primaryTarget);
                }
            }
            else if (order.Ability.DamageType == DamageTypes.Location)
            {
                ApplyLocationAbility(turn, actor, order.Ability, order.TargetLocation.Value);
            }

            actor.CurrentOrder = null;
            actor.CastingProgress = null;
            actor.ChannelProgress = null;
        }

        private void ApplyChannelAbility(Turn turn, Actor actor, Order order)
        {
            if (order.Ability.DamageType == DamageTypes.SingleTarget)
            {
                var abilityTarget = order.TargetActor;

                if (abilityTarget != null && abilityTarget.IsAlive)
                {
                    if (abilityTarget.DistanceFrom(actor).In(order.Ability.Range + actor.Radius + abilityTarget.Radius))
                        ApplySingleTargetAbility(turn, actor, order.Ability, abilityTarget);
                }
            }
            else if (order.Ability.DamageType == DamageTypes.PointBlankArea)
            {
                ApplyPointBlankAreaAbility(turn, actor, order.Ability);
            }
            else if (order.Ability.DamageType == DamageTypes.Cleave)
            {
                var primaryTarget = order.TargetActor;

                if (primaryTarget != null && primaryTarget.IsAlive)
                {
                    if (primaryTarget.DistanceFrom(actor).In(order.Ability.Range + actor.Radius + primaryTarget.Radius))
                        ApplyCleaveAbility(turn, actor, order.Ability, primaryTarget);
                }
            }
            else if (order.Ability.DamageType == DamageTypes.Location)
            {
                ApplyLocationAbility(turn, actor, order.Ability, order.TargetLocation.Value);
            }

            if (actor.CastingProgress.IsReady)
            {
                actor.CurrentOrder = null;
                actor.CastingProgress = null;
                actor.ChannelProgress = null;
            }
            else
            {
                actor.ChannelProgress.Incur();
            }
        }

        private void ApplySingleTargetAbility(Turn turn, Actor actor, Ability ability, Actor abilityTarget)
        {
            if (ability.SpawnsProjectile != null)
            {
                SpawnProjectile(actor, abilityTarget, ability);
            }
            else
            {
                ApplyAbilityOutcome(turn, actor, ability, abilityTarget);
            }

            ability.Cooldown.Incur();
        }

        private void ApplyPointBlankAreaAbility(Turn turn, Actor actor, Ability ability)
        {
            foreach (var target in Actors.Where(x => x.IsAlive))
            {
                if (ValidPointBlankAreaTarget(actor, target, ability))
                {
                    ApplyAbilityOutcome(turn, actor, ability, target);
                }
            }

            GraphicEffects.Enqueue(new GraphicsEffectDefinition { Position = actor.Position, Scale = ability.Area });
            ability.Cooldown.Incur();
        }

        private bool ValidPointBlankAreaTarget(Actor actor, Actor target, Ability ability)
        {
            if (target.Faction == actor.Faction && ability.TargettingType == TargettingTypes.Hostile)
                return false;

            if (target.Faction != actor.Faction && ability.TargettingType == TargettingTypes.Friendly)
                return false;

            if (target.DistanceFrom(actor) <= ability.Area + target.Radius)
                return true;

            return false;
        }

        private void ApplyLocationAbility(Turn turn, Actor actor, Ability ability, Vector2 location)
        {
            foreach (var target in Actors.Where(x => x.IsAlive))
            {
                if (ValidLocationAreaTarget(actor, target, ability, location))
                {
                    ApplyAbilityOutcome(turn, actor, ability, target);
                }
            }

            GraphicEffects.Enqueue(new GraphicsEffectDefinition { Position = location, Scale = ability.Area });
            ability.Cooldown.Incur();
        }

        private bool ValidLocationAreaTarget(Actor actor, Actor target, Ability ability, Vector2 location)
        {
            if (target.Faction == actor.Faction && ability.TargettingType == TargettingTypes.Hostile)
                return false;

            if (target.Faction != actor.Faction && ability.TargettingType == TargettingTypes.Friendly)
                return false;

            if (target.Faction == actor.Faction && ability.TargettingType == TargettingTypes.Location)
                return false;

            if (target.DistanceFrom(location) <= ability.Area + target.Radius)
                return true;

            return false;
        }

        private void ApplyCleaveAbility(Turn turn, Actor actor, Ability ability, Actor primaryTarget)
        {
            foreach (var secondaryTarget in Actors.Where(x => x.IsAlive))
            {
                if ((secondaryTarget.DistanceFrom(primaryTarget) - primaryTarget.Radius - secondaryTarget.Radius) > ability.Area)
                    continue;

                if (secondaryTarget.Faction == actor.Faction && ability.TargettingType == TargettingTypes.Hostile)
                    continue;

                if (secondaryTarget.Faction != actor.Faction && ability.TargettingType == TargettingTypes.Friendly)
                    continue;

                if (ability.SpawnsProjectile != null)
                {
                    SpawnProjectile(actor, secondaryTarget, ability);
                }
                else
                {
                    ApplyAbilityOutcome(turn, actor, ability, secondaryTarget);
                }

                ability.Cooldown.Incur();
            }
        }

        private void SpawnProjectile(Actor actor, Actor target, Ability ability)
        {
            Projectiles.Add(new Projectile
            {
                Ability = ability,
                Owner = actor,
                Target = target,
                Position = new Vector3(actor.Position, actor.Radius),
                Speed = ability.SpawnsProjectile.Speed,
                ModelName = ability.SpawnsProjectile.ModelName,
                TextureName = ability.SpawnsProjectile.TextureName
            });
        }

        private void ApplyAbilityOutcome(Turn turn, Actor actor, Ability ability, Actor target)
        {
            var combatTable = new CombatTable(random, actor.CurrentStatistics, target.CurrentStatistics, ability);
            var combatOutcome = combatTable.Roll();

            var abilityDamage = ability.Damage.CalculateDamage(actor, target);
            var abilityHealing = ability.Healing.CalculateHealing(actor, target);
            
            var damage = 0f;
            var damage2 = 0f;
            var healing = 0f;
            var blocked = 0f;

            if (combatOutcome.IsMiss)
                Event.Raise(new ActorMissed { Actor = target });

            if (combatOutcome.IsDodge)
                Event.Raise(new ActorDodged { Actor = target });

            if (combatOutcome.IsHit)
            {
                damage = abilityDamage;
                healing = abilityHealing;
            }
                    
            if (combatOutcome.IsCrit)
            {
                damage = damage * 2f;
                healing = healing * 2f;
            }

            if (combatOutcome.IsBlock)
            {
                blocked = Math.Min(damage, actor.CurrentStatistics.For<Armor>().Rating * 0.1f);
                damage -= blocked;
                Event.Raise(new ActorBlocked { Actor = target });
            }

            actor.CurrentMana -= ability.ManaCost;
            actor.CurrentEnergy -= ability.EnergyCost;

            if (damage > 0)
            {
                target.CurrentHealth -= (damage);
                Event.Raise(new ActorTookDamage { Actor = target, Damage = damage, IsCrit = combatOutcome.IsCrit });
            }

            if (healing > 0)
            {
                target.CurrentHealth += (healing);
                Event.Raise(new ActorWasHealed { Actor = target, Healing = healing, IsCrit = combatOutcome.IsCrit });
            }

            // Calculate damage threat
            target.ThreatList.Increase(actor, (int)(damage * ability.ThreatModifier));

            // Calculate healing threat for all enemies who knows the healer exists
            foreach (var enemyActor in Actors.Where(a => a.Faction != actor.Faction && a.ThreatList.Exists(healedActor => healedActor.Actor == target)))
            {
                enemyActor.ThreatList.Increase(actor, (int)(healing * 0.05f));
            }

            // Apply auras
            foreach (var aura in ability.AurasApplied)
            {
                target.Auras.Add(new Aura { Owner = actor, Cooldown = aura.Cooldown, Damage = aura.Damage, Healing = aura.Healing, Duration = aura.Duration, Name = aura.Name, Statistics = aura.Statistics });
            }

            turn.Events.Add(new OldEvent(EventTypes.Ability)
            {
                Actor = actor,
                Target = target,
                Damage = damage,
                Damage2 = damage2,
                Healing = healing,
                Blocked = blocked,
                Ability = ability,
                CombatOutcome = combatOutcome
            });
        }
    }
}
