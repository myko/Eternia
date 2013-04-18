using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class SpellPower: Stat<SpellPower>
    {
        public float Value { get; set; }
        public override string Name { get { return "Spell power"; } }
        public override Color Color { get { return Color.White; } }

        public SpellPower()
        {
            Value = 0;
        }

        public SpellPower(float value)
        {
            Value = value;
        }

        public override SpellPower Add(SpellPower s)
        {
            return new SpellPower(Value + s.Value);
        }

        public override SpellPower Subtract(SpellPower s)
        {
            return new SpellPower(Value - s.Value);
        }

        public override StatBase Negate()
        {
            return new SpellPower(-Value);
        }

        public override StatBase Multiply(float f)
        {
            return new SpellPower(Value * f);
        }
    }
}
