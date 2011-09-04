using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace EterniaGame
{
    public class Cooldown
    {
        public float Duration { get; set; }

        private float current;
        [ContentSerializer(Optional=true)]
        public float Current
        {
            get { return current; }
            set { current = value; }
        }

        public bool IsReady
        {
            get
            {
                return current <= 0f;
            }
        }

        public Cooldown() : this(1f, 0f)
        {
        }

        public Cooldown(float duration) : this(duration, 0f)
        {
        }

        public Cooldown(float duration, float initialValue)
        {
            current = initialValue;
            Duration = duration;
        }

        public void Incur()
        {
            current = Duration;
        }

        public void Cool(float time)
        {
            current = Math.Max(0f, current - time);
        }

        public override string ToString()
        {
            return string.Format("{0:0.00}/{1:0.00}", Current, Duration);
        }
    }
}
