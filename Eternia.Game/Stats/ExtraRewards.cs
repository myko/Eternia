using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class ExtraRewards: Stat<ExtraRewards>
    {
        public int Value { get; private set; }
        public override string Name { get { return "Extra rewards"; } }
        public override Color Color { get { return Color.White; } }

        public ExtraRewards()
        {
            Value = 0;
        }

        public ExtraRewards(int value)
        {
            Value = value;
        }

        public override ExtraRewards Add(ExtraRewards s)
        {
            return new ExtraRewards(Value + s.Value);
        }

        public override ExtraRewards Subtract(ExtraRewards s)
        {
            return new ExtraRewards(Value - s.Value);
        }

        public override StatBase Negate()
        {
            return new ExtraRewards(-Value);
        }

        public override StatBase Multiply(float f)
        {
            return new ExtraRewards((int)(Value * f));
        }
    }
}
