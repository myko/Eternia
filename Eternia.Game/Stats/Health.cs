﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class Health: FloatValueStat<Health>
    {
        public override string Name { get { return "Health"; } }

        public Health()
        {
            Value = 0;
        }

        public Health(float value)
        {
            Value = value;
        }

        public override Health Add(Health s)
        {
            return new Health(Value + s.Value);
        }

        public override Health Subtract(Health s)
        {
            return new Health(Value - s.Value);
        }

        public override StatBase Negate()
        {
            return new Health(-Value);
        }

        public override StatBase Multiply(float f)
        {
            return new Health(Value * f);
        }
    }
}
