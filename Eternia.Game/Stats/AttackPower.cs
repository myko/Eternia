using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class AttackPower: FloatValueStat<AttackPower>
    {
        public override string Name { get { return "Attack power"; } }

        public AttackPower()
        {
            Value = 0;
        }

        public AttackPower(float value)
        {
            Value = value;
        }

        public override AttackPower Add(AttackPower s)
        {
            return new AttackPower(Value + s.Value);
        }

        public override AttackPower Subtract(AttackPower s)
        {
            return new AttackPower(Value - s.Value);
        }

        public override StatBase Negate()
        {
            return new AttackPower(-Value);
        }

        public override StatBase Multiply(float f)
        {
            return new AttackPower(Value * f);
        }
    }
}
