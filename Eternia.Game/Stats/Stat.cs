using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public abstract class StatBase
    {
        public abstract string Name { get; }
        public abstract Color Color { get; }

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
