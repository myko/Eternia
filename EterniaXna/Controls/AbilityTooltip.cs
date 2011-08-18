using System;
using System.Linq;
using EterniaGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myko.Xna.Ui;
using System.Collections.Generic;

namespace EterniaXna
{
    public class AbilityTooltip: Control
    {
        private class Line
        {
            public Color Color { get; set; }
            public string Text { get; set; }
        }

        private Actor actor;
        private Ability ability;
        private List<Line> lines;

        public AbilityTooltip(Actor actor, Ability ability)
        {
            this.actor = actor;
            this.ability = ability;

            Width = 160;

            int abilityDamageUpper = (int)(actor.CurrentStatistics.AttackPower * ability.Damage.AttackPowerScale + actor.CurrentStatistics.SpellPower * ability.Damage.SpellPowerScale + ability.Damage.Value);
            int abilityDamageLower = (int)(abilityDamageUpper * actor.CurrentStatistics.Precision);
            
            int abilityHealingUpper = (int)(actor.CurrentStatistics.AttackPower * ability.Healing.AttackPowerScale + actor.CurrentStatistics.SpellPower * ability.Healing.SpellPowerScale + ability.Healing.Value);
            int abilityHealingLower = (int)(abilityHealingUpper * actor.CurrentStatistics.Precision);

            lines = new List<Line>();
            lines.Add(new Line { Color = Color.LightGray, Text = ability.Description });
            if (ability.ManaCost > 0)
                lines.Add(new Line { Color = actor.CurrentMana >= ability.ManaCost ? Color.LightGray : Color.Tomato, Text = ability.ManaCost.ToString() + " mana" });
            if (ability.Damage.Value > 0)
                lines.Add(new Line { Color = Color.LightGray, Text = abilityDamageLower.ToString() + " - " + abilityDamageUpper.ToString() + " damage" });
            if (ability.Healing.Value > 0)
                lines.Add(new Line { Color = Color.LightGray, Text = abilityHealingLower.ToString() + " - " + abilityHealingUpper.ToString() + " healing" });
            if (ability.Cooldown.Duration > 0)
                lines.Add(new Line { Color = Color.LightGray, Text = ability.Cooldown.Duration.ToString() + " seconds cooldown" });
            if (ability.CastTime > 0)
                lines.Add(new Line { Color = Color.LightGray, Text = ability.CastTime.ToString() + " seconds cast" });

            Height = 80;
        }

        public override void Draw(Vector2 position, GameTime gameTime)
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
            SpriteBatch.DrawString(
                Font, 
                ability.Range.Minimum.ToString() + " - " + ability.Range.Maximum.ToString() + " meter range", new Vector2(x, y += Font.LineSpacing), 
                GetRangeColor(), 
                ZIndex + 0.003f);

            foreach (var line in lines.Skip(1))
            {
                SpriteBatch.DrawString(Font, line.Text, new Vector2(x, y += Font.LineSpacing), line.Color, ZIndex + 0.003f);
            }
        }

        private Color GetRangeColor()
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
