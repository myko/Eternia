using System;
using System.Linq;
using Eternia.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using System.Collections.Generic;
using Eternia.Game.Abilities;
using Eternia.Game.Actors;
using Eternia.Game.Stats;

namespace EterniaXna
{
    public class AbilityTooltip: Control
    {
        private class Line
        {
            public Color Color { get; set; }
            public string Text { get; set; }
        }

        private Binding<Actor> actorBinding;
        private Ability ability;
        private List<Line> lines;

        public AbilityTooltip(Binding<Actor> actor, Ability ability)
        {
            this.actorBinding = actor;
            this.ability = ability;
            lines = new List<Line>();

        }

        public override void Update(GameTime gameTime)
        {
            Width = 160;

            var actor = actorBinding.GetValue();

            if (actor != null)
            {
                lines.Clear();

                int abilityDamageUpper = (int)((actor.CurrentStatistics.For<AttackPower>().Value * ability.Damage.AttackPowerScale + actor.CurrentStatistics.For<SpellPower>().Value * ability.Damage.SpellPowerScale + ability.Damage.Value) * actor.CurrentStatistics.For<DamageDone>().Value);
                int abilityDamageLower = (int)((abilityDamageUpper * actor.CurrentStatistics.For<Precision>().Chance) * actor.CurrentStatistics.For<DamageDone>().Value);
                float averageDamage = ((abilityDamageLower + abilityDamageUpper) / 2.0f) * (actor.CurrentStatistics.For<CriticalStrike>().Chance + 1.0f);

                int abilityHealingUpper = (int)((actor.CurrentStatistics.For<AttackPower>().Value * ability.Healing.AttackPowerScale + actor.CurrentStatistics.For<SpellPower>().Value * ability.Healing.SpellPowerScale + ability.Healing.Value) * actor.CurrentStatistics.For<HealingDone>().Value);
                int abilityHealingLower = (int)((abilityHealingUpper * actor.CurrentStatistics.For<Precision>().Chance) * actor.CurrentStatistics.For<HealingDone>().Value);
                float averageHealing = ((abilityHealingLower + abilityHealingUpper) / 2.0f) * (actor.CurrentStatistics.For<CriticalStrike>().Chance + 1.0f);

                var damageString = ability.Damage.Value.ToString("0") + " " + ability.Damage.School.ToString() + " damage";
                if (ability.Damage.SpellPowerScale > 0f)
                    damageString = ability.Damage.SpellPowerScale.ToString("#0%") + " spell power + " + damageString;
                if (ability.Damage.AttackPowerScale > 0f)
                    damageString = ability.Damage.AttackPowerScale.ToString("#0%") + " attack power + " + damageString;

                var healingString = ability.Healing.Value.ToString("0") + " healing";
                if (ability.Healing.SpellPowerScale > 0f)
                    healingString = ability.Healing.SpellPowerScale.ToString("#0%") + " spell power + " + healingString;
                if (ability.Healing.AttackPowerScale > 0f)
                    healingString = ability.Healing.AttackPowerScale.ToString("#0%") + " attack power + " + healingString;

                lines.Add(new Line { Color = Color.LightGray, Text = ability.Description });
                lines.Add(new Line { Color = Color.LightGray, Text = ability.DamageType.ToString() });

                if (ability.Damage.Value > 0)
                    lines.Add(new Line { Color = Color.LightGray, Text = damageString });
                if (ability.Healing.Value > 0)
                    lines.Add(new Line { Color = Color.LightGray, Text = healingString });

                if (ability.ManaCost > 0)
                    lines.Add(new Line { Color = actor.CurrentMana >= ability.ManaCost ? Color.LightGray : Color.Tomato, Text = ability.ManaCost.ToString() + " mana" });
                if (ability.EnergyCost > 0)
                    lines.Add(new Line { Color = actor.CurrentEnergy >= ability.EnergyCost ? Color.LightGray : Color.Tomato, Text = ability.EnergyCost.ToString() + " energy" });
                if (ability.EnergyCost < 0)
                    lines.Add(new Line { Color = Color.LightGray, Text = "Generates " + (-ability.EnergyCost).ToString() + " energy" });
                if (ability.Damage.Value > 0)
                    lines.Add(new Line { Color = Color.LightGray, Text = abilityDamageLower.ToString() + " - " + abilityDamageUpper.ToString() + " damage" });
                if (ability.Healing.Value > 0)
                    lines.Add(new Line { Color = Color.LightGray, Text = abilityHealingLower.ToString() + " - " + abilityHealingUpper.ToString() + " healing" });
                if (ability.Cooldown.Duration > 0)
                    lines.Add(new Line { Color = Color.LightGray, Text = ability.Cooldown.Duration.ToString("0.00") + " seconds cooldown" });
                if (ability.Duration > 0)
                    lines.Add(new Line { Color = Color.LightGray, Text = ability.Duration.ToString("0.00") + " seconds cast" });

                var timeCost = Math.Max(ability.Duration, ability.Cooldown.Duration);
                if (ability.Damage.Value > 0 && (ability.Duration > 0 || ability.Cooldown.Duration > 0))
                    lines.Add(new Line { Color = Color.LightGray, Text = (averageDamage / timeCost).ToString("0.00") + " DPS" });
                if (ability.Damage.Value > 0 && (ability.Duration > 0))
                    lines.Add(new Line { Color = Color.LightGray, Text = (averageDamage / ability.Duration).ToString("0.00") + " DPCT" });
                if (ability.Healing.Value > 0 && (ability.Duration > 0 || ability.Cooldown.Duration > 0))
                    lines.Add(new Line { Color = Color.LightGray, Text = (averageHealing / timeCost).ToString("0.00") + " HPS" });
                if (ability.Damage.Value > 0 && ability.ThreatModifier > 1.0f && (ability.Duration > 0 || ability.Cooldown.Duration > 0))
                    lines.Add(new Line { Color = Color.LightGray, Text = (ability.ThreatModifier * averageDamage / timeCost).ToString("0.00") + " TPS" });

                if (ability.Damage.Value > 0 && ability.ManaCost > 0)
                    lines.Add(new Line { Color = Color.LightGray, Text = (averageDamage / ability.ManaCost).ToString("0.00") + " DPM" });
                if (ability.Damage.Value > 0 && ability.EnergyCost > 0)
                    lines.Add(new Line { Color = Color.LightGray, Text = (averageDamage / ability.EnergyCost).ToString("0.00") + " DPE" });
                if (ability.Healing.Value > 0 && ability.ManaCost > 0)
                    lines.Add(new Line { Color = Color.LightGray, Text = (averageHealing / ability.ManaCost).ToString("0.00") + " HPM" });
                if (ability.Healing.Value > 0 && ability.EnergyCost > 0)
                    lines.Add(new Line { Color = Color.LightGray, Text = (averageHealing / ability.EnergyCost).ToString("0.00") + " HPE" });
            }

            Height = 80;

            base.Update(gameTime);
        }

        public override void Draw(Vector2 position, GameTime gameTime)
        {
            if (lines.Any())
            {
                Height = 20 + (lines.Count + 2) * Font.LineSpacing;
                Width = Math.Max(Width, Font.MeasureString(ability.Name).X + 20);
                Width = Math.Max(Width, lines.Select(l => Font.MeasureString(l.Text).X).Max() + 20);

                var bounds = new Rectangle((int)position.X, (int)position.Y - (int)Height, (int)Width, (int)Height);
                var innerBounds = bounds;
                innerBounds.Inflate(-1, -1);

                SpriteBatch.Draw(BlankTexture, bounds, Color.White, ZIndex + 0.001f);
                SpriteBatch.Draw(BlankTexture, innerBounds, new Color(20, 20, 20), ZIndex + 0.002f);

                int x = (int)position.X + 10;
                int y = (int)position.Y - (int)Height + 10;

                SpriteBatch.DrawString(Font, ability.Name, new Vector2(x, y), Color.Yellow, ZIndex + 0.003f);
                SpriteBatch.DrawString(Font, lines[0].Text, new Vector2(x, y += Font.LineSpacing), lines[0].Color, ZIndex + 0.003f);
                if (ability.TargettingType != TargettingTypes.Self)
                    SpriteBatch.DrawString(Font, ability.Range.ToString() + " meter range", new Vector2(x, y += Font.LineSpacing), GetRangeColor(actorBinding.GetValue()), ZIndex + 0.003f);
                else
                    Height -= Font.LineSpacing;

                foreach (var line in lines.Skip(1))
                {
                    SpriteBatch.DrawString(Font, line.Text, new Vector2(x, y += Font.LineSpacing), line.Color, ZIndex + 0.003f);
                }
            }
        }

        private Color GetRangeColor(Actor actor)
        {
            if (!actor.Targets.Any())
                return Color.LightGray;

            var target = actor.Targets.Peek();
            if (target.DistanceFrom(actor).In(ability.Range + actor.Radius + target.Radius))
                return Color.LightGray;

            return Color.Tomato;
        }
    }
}
