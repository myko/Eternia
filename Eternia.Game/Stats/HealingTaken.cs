using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Xna.Framework.Graphics;   //Tommy 2013-04-26  Upgrade to XNA 4.0. Color was moved from namespace Microsoft.Xna.Framework.Graphics to Microsoft.Xna.Framework
using Microsoft.Xna.Framework;  //Tommy 2013-04-26  Upgrade to XNA 4.0. Color was moved from namespace Microsoft.Xna.Framework.Graphics to Microsoft.Xna.Framework


namespace Eternia.Game.Stats
{
    public class HealingTaken: FloatValueStat<HealingTaken>
    {
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
