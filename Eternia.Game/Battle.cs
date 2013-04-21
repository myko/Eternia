using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using EterniaGame.Triggers;
using EterniaGame.Abilities;
using EterniaGame.Actors;

namespace EterniaGame
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
                var aggroRadius = 10f; // TODO: REMOVE HARD CODED AGGRO RADIUS
                if (actor.DistanceFrom(other) < aggroRadius)
                    actor.ThreatList.Add(other);
            }

            // Drop dead actors from threat lists
            actor.ThreatList.RemoveAll(t => !t.Actor.IsAlive);

            if (IsReadyToSelectNewTarget(actor))
            {
                actor.SelectTarget(Actors.Where(x => x.IsAlive));
            }

            actor.CurrentMana += 1f * deltaTime;
            actor.CurrentEnergy += 10f * deltaTime;

            CoolAbilities(deltaTime, actor);
            RunActorAuras(deltaTime, turn, actor);

            if (actor.IsAlive)
            {
                actor.PickDestination();
                        
                if (actor.Targets.Any())
                {
                    var target = actor.Targets.Peek();
                    if (target.DistanceFrom(actor) > 0.01f)
                        actor.Direction = Vector2.Normalize(target.Position - actor.Position);
                }
                actor.BaseAnimationState = BaseAnimationState.Casting;
                if (actor.CastingAbility != null)
                {
                    RunAbilityCast(deltaTime, turn, actor);
                }
                else
                {
                    var ability = actor.SelectAbility();
                    if (actor.Destination == null && ability != null)
                    {
                        UseAbility(turn, actor, ability);
                    }
                    else
                    {
                        if (actor.Move(deltaTime, Actors.Where(x => x.IsAlive && x != actor)))
                            actor.BaseAnimationState = BaseAnimationState.Walking;
                        else
                            actor.BaseAnimationState = BaseAnimationState.Idle;
                    }
                }
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

            if (actor.CastingProgress != null && actor.CastingProgress.Current > 0f && actor.CastingAbility != null)
                return false;

            return true;
        }

        private void KillActor(Turn turn, Actor actor)
        {
            actor.IsAlive = false;
            actor.Targets.Clear();
            actor.Auras.Clear();
            turn.Events.Add(new Event(EventTypes.ActorDeath)
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
                    var damage = aura.Damage.CalculateDamage(aura.Owner, actor);
                    var healing = aura.Healing.CalculateHealing(aura.Owner, actor);
                    if (damage > 0)
                    {
                        turn.Events.Add(new Event(EventTypes.AuraDamage) { Actor = aura.Owner, Target = actor, Damage = damage });
                        actor.CurrentHealth -= damage;
                    }
                    if (healing > 0)
                    {
                        turn.Events.Add(new Event(EventTypes.AuraHealing) { Actor = aura.Owner, Target = actor, Healing = healing });
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
                    turn.Events.Add(new Event(EventTypes.AuraExpired) { Actor = actor, Target = actor });
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

            if (actor.CastingProgress.IsReady)
            {
                ApplyCastAbility(turn, actor, actor.CastingAbility);
            }
        }

        private bool UseAbility(Turn turn, Actor actor, Ability ability)
        {
            if (ability.DamageType == DamageTypes.SingleTarget)
            {
                var abilityTarget = actor.Targets.FirstOrDefault();
                if (ability.TargettingType == TargettingTypes.Self)
                    abilityTarget = actor;

                if (!actor.PlayerControlled)
                    abilityTarget = actor.GetAbilityTarget(ability.TargettingType);

                if (abilityTarget != null && abilityTarget.IsAlive)
                {
                    if (ability.TargettingType == TargettingTypes.Hostile && actor.Faction == abilityTarget.Faction)
                        return false;

                    if (ability.TargettingType == TargettingTypes.Friendly && actor.Faction != abilityTarget.Faction)
                        return false;

                    if (abilityTarget.DistanceFrom(actor).In(ability.Range + actor.Radius + abilityTarget.Radius))
                    {
                        actor.CastingAbility = ability;
                        actor.CastingProgress = new Cooldown(ability.Duration);
                        actor.CastingProgress.Incur();

                        return true;
                    }
                }
            }
            else if (ability.DamageType == DamageTypes.PointBlankArea)
            {
                //if (Actors.Any(x => ValidPointBlankAreaTarget(actor, x, ability)))
                {
                    actor.CastingAbility = ability;
                    actor.CastingProgress = new Cooldown(ability.Duration);
                    actor.CastingProgress.Incur();

                    return true;
                }
            }
            else if (ability.DamageType == DamageTypes.Cleave)
            {
                var primaryTarget = actor.Targets.FirstOrDefault();

                if (!actor.PlayerControlled)
                    primaryTarget = actor.GetAbilityTarget(ability.TargettingType);

                if (primaryTarget != null && primaryTarget.IsAlive)
                {
                    if (ability.TargettingType == TargettingTypes.Hostile && actor.Faction == primaryTarget.Faction)
                        return false;

                    if (ability.TargettingType == TargettingTypes.Friendly && actor.Faction != primaryTarget.Faction)
                        return false;

                    if (primaryTarget.DistanceFrom(actor).In(ability.Range + actor.Radius + primaryTarget.Radius))
                    {
                        actor.CastingAbility = ability;
                        actor.CastingProgress = new Cooldown(ability.Duration);
                        actor.CastingProgress.Incur();
                        
                        return true;
                    }
                }
            }

            return false;
        }

        private void ApplyCastAbility(Turn turn, Actor actor, Ability ability)
        {
            if (ability.DamageType == DamageTypes.SingleTarget)
            {
                var abilityTarget = actor.GetAbilityTarget(ability.TargettingType);

                if (abilityTarget != null && abilityTarget.IsAlive)
                {
                    if (abilityTarget.DistanceFrom(actor).In(ability.Range + actor.Radius + abilityTarget.Radius))
                        ApplySingleTargetAbility(turn, actor, ability, abilityTarget);
                }
            }
            else if (ability.DamageType == DamageTypes.PointBlankArea)
            {
                ApplyPointBlankAreaAbility(turn, actor, ability);
            }
            else if (ability.DamageType == DamageTypes.Cleave)
            {
                var primaryTarget = actor.GetAbilityTarget(ability.TargettingType);

                if (primaryTarget != null && primaryTarget.IsAlive)
                {
                    if (primaryTarget.DistanceFrom(actor).In(ability.Range + actor.Radius + primaryTarget.Radius))
                        ApplyCleaveAbility(turn, actor, ability, primaryTarget);
                }
            }

            actor.CastingAbility = null;
            actor.CastingProgress = null;
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

            GraphicEffects.Enqueue(new GraphicsEffectDefinition { Position = actor.Position });
            ability.Cooldown.Incur();
        }

        private bool ValidPointBlankAreaTarget(Actor actor, Actor target, Ability ability)
        {
            if (target.Faction == actor.Faction && ability.TargettingType == TargettingTypes.Hostile)
                return false;

            if (target.Faction != actor.Faction && ability.TargettingType == TargettingTypes.Friendly)
                return false;

            if (target.DistanceFrom(actor).In(ability.Range + target.Radius))
                return true;

            return false;
        }

        private void ApplyCleaveAbility(Turn turn, Actor actor, Ability ability, Actor primaryTarget)
        {
            foreach (var secondaryTarget in Actors.Where(x => x.IsAlive))
            {
                if ((secondaryTarget.DistanceFrom(primaryTarget) - primaryTarget.Radius - secondaryTarget.Radius) > 1)
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
                damage -= Math.Min(damage, actor.CurrentStatistics.For<Eternia.Game.Stats.DamageReduction>().ArmorRating);
            }

            actor.CurrentMana -= ability.ManaCost;
            actor.CurrentEnergy -= ability.EnergyCost;
            target.CurrentHealth -= (damage);
            target.CurrentHealth += (healing);

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

            turn.Events.Add(new Event(EventTypes.Ability)
            {
                Actor = actor,
                Target = target,
                Damage = damage,
                Damage2 = damage2,
                Healing = healing,
                Ability = ability,
                CombatOutcome = combatOutcome
            });
        }
    }
}
