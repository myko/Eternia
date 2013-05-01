using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Xna.Framework.Graphics;   //Tommy 2013-04-26  Upgrade to XNA 4.0. Color was moved from namespace Microsoft.Xna.Framework.Graphics to Microsoft.Xna.Framework
using Microsoft.Xna.Framework;  //Tommy 2013-04-26  Upgrade to XNA 4.0. Color was moved from namespace Microsoft.Xna.Framework.Graphics to Microsoft.Xna.Framework

namespace Eternia.Game.Stats
{
    public abstract class FloatValueStat<StatT>: Stat<StatT> where StatT: StatBase, new()
    {
        public float Value { get; set; }
        public override Color Color { get { return Value < 0 ? Color.Salmon : Color.LightGreen; } }

        public FloatValueStat()
        {
            Value = 0;
        }

        public FloatValueStat(float value)
        {
            Value = value;
        }

        public override string ToValueString()
        {
            return Value.ToString();
        }

        public override string ToItemTooltipString()
        {
            return Value.ToString("0") + " " + Name;
        }

        public override string ToItemUpgradeString()
        {
            if (Value >= 0)
                return Name + ": " + "+" + Value.ToString("0");
            else
                return Name + ": " + Value.ToString("0");
        }

        public override void SetItemValue(int level, Items.ItemArmorClasses armorClass, float itemSlotModifier)
        {
            Value = Items.ItemGenerator.GetItemLevelMultiplier(level) * 6.25f * itemSlotModifier;
        }
    }
}
