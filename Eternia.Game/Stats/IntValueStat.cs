﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public abstract class IntValueStat<StatT>: Stat<StatT> where StatT: StatBase, new()
    {
        public int Value { get; set; }
        public override Color Color { get { return Value < 0 ? Color.Salmon : Color.LightGreen; } }

        public IntValueStat()
        {
            Value = 0;
        }

        public IntValueStat(int value)
        {
            Value = value;
        }

        public override string ToValueString()
        {
            return Value.ToString();
        }

        public override string ToItemTooltipString()
        {
            if (Value >= 0)
                return "+" + Value.ToString() + " " + Name;
            else
                return Value.ToString() + " " + Name;
        }

        public override string ToItemUpgradeString()
        {
            if (Value >= 0)
                return Name + ": " + "+" + Value.ToString();
            else
                return Name + ": " + Value.ToString();
        }
    }
}
