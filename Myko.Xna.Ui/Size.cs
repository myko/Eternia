using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myko.Xna.Ui
{
    public enum SizeTypes
    {
        Fill,
        Fixed
    }

    public struct Size
    {
        public readonly SizeTypes Type;
        public readonly float Value;

        public Size(SizeTypes type, float size)
        {
            this.Type = type;
            this.Value = size;
        }

        public static Size Fill()
        {
            return new Size(SizeTypes.Fill, 1);
        }

        public static Size Fill(float factor)
        {
            return new Size(SizeTypes.Fill, factor);
        }

        public static Size Fixed(float size)
        {
            return new Size(SizeTypes.Fixed, size);
        }

        public static implicit operator Size(int value)
        {
            return Fixed(value);
        }

        public static implicit operator Size(float value)
        {
            return Fixed(value);
        }
    }
}
