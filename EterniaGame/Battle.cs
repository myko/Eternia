using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using EterniaGame.Triggers;

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
            var d = new Vector3(projectile.Target.Position, 1.5f) - projectile.Position;
            if (d.Length() > 1)
                d.Normalize();
            projectile.Position += d * 8 * deltaTime;
        }

        private void RunActors(float deltaTime, Turn turn)
        {
            foreach (var actor in Actors)
            {
                RunActor(deltaTime, turn, actor);
            }
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

            if (actor.IsAlive)
            {
                actor.SelectTarget(Actors.Where(x => x.IsAlive));
            }

            RunAbilities(deltaTime, turn, actor);

            if (actor.IsAlive)
                if (actor.Move(deltaTime))
                    actor.BaseAnimationState = BaseAnimationState.Walking;
                else
                    actor.BaseAnimationState = BaseAnimationState.Idle;
            else
                actor.BaseAnimationState = BaseAnimationState.Dead;

            MeleeSwing(deltaTime, turn, actor);

            RunActorAuras(deltaTime, turn, actor);

            if (actor.CastingProgress != null && actor.CastingProgress.Current > 0f)
                actor.BaseAnimationState = BaseAnimationState.Casting;

            if (actor.IsAlive && actor.CurrentHealth <= 0f)
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

        private void RunAbilities(float deltaTime, Turn turn, Actor actor)
        {
            if (!actor.IsAlive)
                return;

            if (!actor.PlayerControlled)
                actor.Abilities.ForEach(x => x.Enabled = true);

            foreach (var ability in actor.Abilities)
            {
                ability.Cooldown.Cool(deltaTime);
            }

            actor.GlobalCooldown.Cool(deltaTime);

            if (actor.CastingAbility != null)
            {
                RunAbilityCast(deltaTime, turn, actor);
            }
            else if (actor.GlobalCooldown.IsReady)
            {
                var ability = actor.SelectAbility();
                if (ability != null && UseAbility(turn, actor, ability))
                {
                    actor.GlobalCooldown.Incur();
                }
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
                        if (ability.CastTime > 0)
                        {
                            actor.CastingAbility = ability;
                            actor.CastingProgress = new Cooldown(ability.CastTime);
                            actor.CastingProgress.Incur();
                        }
                        else
                        {
                            actor.CastingAbility = null;

                            ApplySingleTargetAbility(turn, actor, ability, abilityTarget);
                        }

                        return true;
                    }
                }
            }
            else if (ability.DamageType == DamageTypes.PointBlankArea)
            {
                if (Actors.Any(x => ValidPointBlankAreaTarget(actor, x, ability)))
                {
                    if (ability.CastTime > 0)
                    {
                        actor.CastingAbility = ability;
                        actor.CastingProgress = new Cooldown(ability.CastTime);
                        actor.CastingProgress.Incur();
                    }
                    else
                    {
                        actor.CastingAbility = null;

                        ApplyPointBlankAreaAbility(turn, actor, ability);
                    }

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
                        if (ability.CastTime > 0)
                        {
                            actor.CastingAbility = ability;
                            actor.CastingProgress = new Cooldown(ability.CastTime);
                            actor.CastingProgress.Incur();
                        }
                        else
                        {
                            actor.CastingAbility = null;

                            ApplyCleaveAbility(turn, actor, ability, primaryTarget);
                        }

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
                var abilityTarget = actor.Targets.FirstOrDefault();

                if (!actor.PlayerControlled)
                    abilityTarget = actor.GetAbilityTarget(ability.TargettingType);

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
                var primaryTarget = actor.Targets.FirstOrDefault();

                if (!actor.PlayerControlled)
                    primaryTarget = actor.GetAbilityTarget(ability.TargettingType);

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
                Projectiles.Add(new Projectile { Ability = ability, Owner = actor, Target = abilityTarget, Position = new Vector3(actor.Position, 0), ModelName = ability.SpawnsProjectile.ModelName, TextureName = ability.SpawnsProjectile.TextureName });
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
                    Projectiles.Add(new Projectile { Ability = ability, Owner = actor, Target = secondaryTarget, Position = new Vector3(actor.Position, 0), ModelName = ability.SpawnsProjectile.ModelName, TextureName = ability.SpawnsProjectile.TextureName });
                }
                else
                {
                    ApplyAbilityOutcome(turn, actor, ability, secondaryTarget);
                }

                ability.Cooldown.Incur();
            }
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

            switch (combatOutcome)
            {
                case CombatOutcome.Hit:
                    damage = abilityDamage;
                    healing = abilityHealing;
                    break;
                case CombatOutcome.Crit:
                    damage = abilityDamage * 2f;
                    healing = abilityHealing * 2f;
                    break;
            }

            actor.CurrentMana -= ability.ManaCost;
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

        private void MeleeSwing(float deltaTime, Turn turn, Actor actor)
        {
            if (!actor.IsAlive)
                return;

            actor.Swing.Cool(deltaTime);

            if (!actor.Swing.IsReady || actor.CastingAbility != null)
                return;

            if (!actor.Targets.Any() || !actor.Targets.Peek().IsAlive)
                return;

            var target = actor.Targets.Peek();
            var meleeRange = new Range(actor.Radius + target.Radius + 1);
            if (!target.DistanceFrom(actor).In(meleeRange))
                return;

            if (target.Faction == actor.Faction)
                return;

            ApplyMeleeSwingOutcome(turn, actor);
        }

        private void ApplyMeleeSwingOutcome(Turn turn, Actor actor)
        {
            var target = actor.Targets.Peek();
            var combatTable = new CombatTable(random, actor.CurrentStatistics, target.CurrentStatistics);
            var combatOutcome = combatTable.Roll();
            var damage = 0f;
            var damage2 = 0f;

            switch (combatOutcome)
            {
                case CombatOutcome.Crit:
                    damage = random.Between(actor.CurrentStatistics.Precision * actor.CurrentStatistics.AttackPower, actor.CurrentStatistics.AttackPower) * 2 * (1f - target.CurrentStatistics.ArmorReduction);
                    break;
                case CombatOutcome.Hit:
                    damage = random.Between(actor.CurrentStatistics.Precision * actor.CurrentStatistics.AttackPower, actor.CurrentStatistics.AttackPower) * (1f - target.CurrentStatistics.ArmorReduction);
                    break;
            }

            damage = damage * actor.CurrentStatistics.DamageDone * target.CurrentStatistics.DamageTaken;

            actor.Swing.Incur();
            target.CurrentHealth -= damage;
            target.ThreatList.Increase(actor, (int)damage);

            turn.Events.Add(new Event(EventTypes.Swing)
            {
                Actor = actor,
                Target = target,
                Damage = damage,
                Damage2 = damage2,
                CombatOutcome = combatOutcome
            });
        }
    }
}
