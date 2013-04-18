using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class DamageTaken: Stat<DamageTaken>
    {
        public float Value { get; set; }
        public override string Name { get { return "Damage taken"; } }
        public override Color Color { get { return Color.White; } }

        public DamageTaken()
        {
            Value = 1;
        }

        public DamageTaken(float value)
        {
            Value = value;
        }

        public override DamageTaken Add(DamageTaken s)
        {
            return new DamageTaken(Value + s.Value);
        }

        public override DamageTaken Subtract(DamageTaken s)
        {
            return new DamageTaken(Value - s.Value);
        }

        public override StatBase Negate()
        {
            return new DamageTaken(-Value);
        }

        public override StatBase Multiply(float f)
        {
            return new DamageTaken(Value * f);
        }
    }
}
