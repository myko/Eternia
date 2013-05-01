using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
            return Value.ToString() + " " + Name;
        }

        public override string ToItemUpgradeString()
        {
            if (Value >= 0)
                return Name + ": " + "+" + Value.ToString();
            else
                return Name + ": " + Value.ToString();
        }

        public override void SetItemValue(int level, Items.ItemArmorClasses armorClass, float itemSlotModifier)
        {
            Value = (int)(Items.ItemGenerator.GetItemLevelMultiplier(level) * 6.25f * itemSlotModifier);
        }
    }
}
