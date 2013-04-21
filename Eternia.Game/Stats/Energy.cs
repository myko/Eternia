using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class Energy: FloatValueStat<Energy>
    {
        public override string Name { get { return "Energy"; } }

        public Energy()
        {
            Value = 0;
        }

        public Energy(float value)
        {
            Value = value;
        }

        public override Energy Add(Energy s)
        {
            return new Energy(Value + s.Value);
        }

        public override Energy Subtract(Energy s)
        {
            return new Energy(Value - s.Value);
        }

        public override StatBase Negate()
        {
            return new Energy(-Value);
        }

        public override StatBase Multiply(float f)
        {
            return new Energy(Value * f);
        }
    }
}
