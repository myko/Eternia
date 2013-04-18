using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class DamageDone: Stat<DamageDone>
    {
        public float Value { get; private set; }
        public override string Name { get { return "Damage done"; } }
        public override Color Color { get { return Color.White; } }

        public DamageDone()
        {
            Value = 1;
        }

        public DamageDone(float value)
        {
            Value = value;
        }

        public override DamageDone Add(DamageDone s)
        {
            return new DamageDone(Value + s.Value);
        }

        public override DamageDone Subtract(DamageDone s)
        {
            return new DamageDone(Value - s.Value);
        }

        public override StatBase Negate()
        {
            return new DamageDone(-Value);
        }

        public override StatBase Multiply(float f)
        {
            return new DamageDone(Value * f);
        }
    }
}
