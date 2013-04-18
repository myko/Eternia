using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class HealingTaken: Stat<HealingTaken>
    {
        public float Value { get; private set; }
        public override string Name { get { return "Healing taken"; } }
        public override Color Color { get { return Color.White; } }

        public HealingTaken()
        {
            Value = 1;
        }

        public HealingTaken(float value)
        {
            Value = value;
        }

        public override HealingTaken Add(HealingTaken s)
        {
            return new HealingTaken(Value + s.Value);
        }

        public override HealingTaken Subtract(HealingTaken s)
        {
            return new HealingTaken(Value - s.Value);
        }

        public override StatBase Negate()
        {
            return new HealingTaken(-Value);
        }

        public override StatBase Multiply(float f)
        {
            return new HealingTaken(Value * f);
        }
    }
}
