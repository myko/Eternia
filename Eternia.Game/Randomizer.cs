using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Eternia.Game
{
    public static class RandomExtensions
    {
        public static T Next<T>(this Random rnd)
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(rnd.Next(values.Length));
        }

        public static T From<T>(this Random random, T[] array)
        {
            return array[random.Next(array.Length)];
        }

        public static T From<T>(this Random random, IEnumerable<T> values)
        {
            return values.Skip(random.Next(values.Count())).First();
        }

        public static int Between(this Random random, int min, int max)
        {
            return min + random.Next(max - min);
        }

        public static float Between(this Random random, float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }

        public static Vector3 NextVector3(this Random random, float min, float max)
        {
            return new Vector3(Between(random, min, max), Between(random, min, max), Between(random, min, max));
        }

        public static Vector2 NextUnitVector2(this Random random)
        {
            var result = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f);
            result.Normalize();
            return result;
        }
    }

    public class Randomizer
    {
        private Random random = new Random();

        public virtual T Next<T>()
        {
            return random.Next<T>();
        }

        public virtual int Next()
        {
            return random.Next();
        }

        public virtual int Next(int maxValue)
        {
            return random.Next(maxValue);
        }

        public virtual T From<T>(params T[] array)
        {
            return random.From<T>(array);

        }
        public virtual int Between(int min, int max)
        {
            return random.Between(min, max);
        }

        public virtual float Between(float min, float max)
        {
            return random.Between(min, max);
        }
    }
}
