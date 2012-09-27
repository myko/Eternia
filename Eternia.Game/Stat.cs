using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EterniaGame
{
    public struct Stat
    {
        public float Minimum { get; set; }
        public float Current { get; private set; }
        public float Maximum { get; set; }

        public float Fraction
        {
            get
            {
                return Maximum > 0f ? Current / Maximum : 0f;
            }
        }

        public Stat(float value) : this()
        {
            Minimum = 0f;
            Current = value;
            Maximum = value;
        }

        public static Stat operator +(Stat s1, Stat s2)
        {
            return new Stat() { Current = Math.Min(Math.Max(s1.Current + s2.Current, s1.Minimum), s1.Maximum), Minimum = s1.Minimum, Maximum = s1.Maximum };
        }

        public static Stat operator -(Stat s1, Stat s2)
        {
            return new Stat() { Current = Math.Min(Math.Max(s1.Current - s2.Current, s1.Minimum), s1.Maximum), Minimum = s1.Minimum, Maximum = s1.Maximum };
        }

        public static Stat operator +(Stat s1, float value)
        {
            return new Stat() { Current = Math.Min(Math.Max(s1.Current + value, s1.Minimum), s1.Maximum), Minimum = s1.Minimum, Maximum = s1.Maximum };
        }

        public static Stat operator -(Stat s1, float value)
        {
            return new Stat() { Current = Math.Min(Math.Max(s1.Current - value, s1.Minimum), s1.Maximum), Minimum = s1.Minimum, Maximum = s1.Maximum };
        }
    }
}
