using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Eternia.Game.Stats
{
    public class Mana: Stat<Mana>
    {
        public float Value { get; set; }
        public override string Name { get { return "Mana"; } }
        public override Color Color { get { return Color.White; } }

        public Mana()
        {
            Value = 0;
        }

        public Mana(float value)
        {
            Value = value;
        }

        public override Mana Add(Mana s)
        {
            return new Mana(Value + s.Value);
        }

        public override Mana Subtract(Mana s)
        {
            return new Mana(Value - s.Value);
        }

        public override StatBase Negate()
        {
            return new Mana(-Value);
        }

        public override StatBase Multiply(float f)
        {
            return new Mana(Value * f);
        }
    }
}
