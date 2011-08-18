using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EterniaGame
{
    public class Range
    {
        public float Minimum { get; set; }
        public float Maximum { get; set; }

        public Range()
        {
            Minimum = 0f;
            Maximum = 0f;
        }

        public Range(float maximum)
        {
            Minimum = 0f;
            Maximum = maximum;
        }

        public Range(float minimum, float maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public static bool operator <(float f, Range r)
        {
            return f < r.Maximum;
        }

        public static bool operator >(float f, Range r)
        {
            return f > r.Maximum;
        }

        public static Range operator +(Range r, float f)
        {
            return new Range(r.Minimum, r.Maximum + f);
        }
    }

    public static class RangeExtensions
    {
        public static bool In(this float f, Range r)
        {
            return f >= r.Minimum && f <= r.Maximum;
        }
    }
}
