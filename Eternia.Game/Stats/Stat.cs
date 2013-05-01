using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Xna.Framework.Graphics;   //Tommy 2013-04-26  Upgrade to XNA 4.0. Color was moved from namespace Microsoft.Xna.Framework.Graphics to Microsoft.Xna.Framework
using Microsoft.Xna.Framework;
using Eternia.Game.Items;  //Tommy 2013-04-26  Upgrade to XNA 4.0. Color was moved from namespace Microsoft.Xna.Framework.Graphics to Microsoft.Xna.Framework

namespace Eternia.Game.Stats
{
    public abstract class StatBase
    {
        public abstract string Name { get; }
        public abstract Color Color { get; }
        //public abstract bool IsUpgrade { get; }

        public StatBase Add(StatBase s)
        {
            return InternalAdd(s);
        }

        public StatBase Subtract(StatBase s)
        {
            return InternalSubtract(s);
        }

        public abstract StatBase Negate();
        public abstract StatBase Multiply(float f);

        public abstract string ToValueString();
        public abstract string ToItemTooltipString();
        public abstract string ToItemUpgradeString();
        public abstract void SetItemValue(int level, ItemArmorClasses armorClass, float itemSlotModifier);

        protected abstract StatBase InternalAdd(StatBase s);
        protected abstract StatBase InternalSubtract(StatBase s);
    }

    public abstract class Stat<T> : StatBase where T: StatBase, new()
    {
        public abstract T Add(T s);
        public abstract T Subtract(T s);

        protected override StatBase InternalAdd(StatBase s)
        {
            return Add((T)s);
        }

        protected override StatBase InternalSubtract(StatBase s)
        {
            return Subtract((T)s);
        }
    }
}
