﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class HealingDone: Stat<HealingDone>
    {
        public float Value { get; private set; }
        public override string Name { get { return "Healing done"; } }
        public override Color Color { get { return Color.White; } }

        public HealingDone()
        {
            Value = 1;
        }

        public HealingDone(float value)
        {
            Value = value;
        }

        public override HealingDone Add(HealingDone s)
        {
            return new HealingDone(Value + s.Value);
        }

        public override HealingDone Subtract(HealingDone s)
        {
            return new HealingDone(Value - s.Value);
        }

        public override StatBase Negate()
        {
            return new HealingDone(-Value);
        }

        public override StatBase Multiply(float f)
        {
            return new HealingDone(Value * f);
        }
    }
}
