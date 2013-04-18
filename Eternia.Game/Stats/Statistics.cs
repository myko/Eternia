using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eternia.Game.Stats
{
    public class Statistics: List<StatBase>
    {
        public Statistics()
        {
        }

        public T For<T>() where T: new()
        {
            var stat = this.OfType<T>().SingleOrDefault();
            if (stat == null)
                return new T();
            else
                return stat;
        }

        public static Statistics operator +(Statistics s1, Statistics s2)
        {
            var statistics = new Statistics();

            foreach (var stat in s1)
            {
                statistics.Add(stat);
            }

            foreach (var stat in s2)
            {
                var existing = statistics.SingleOrDefault(x => x.GetType() == stat.GetType());
                if (existing != null)
                {
                    statistics.Remove(existing);
                    statistics.Add(existing.Add(stat));
                }
                else
                {
                    statistics.Add(stat);
                }
            }

            return statistics;
        }

        public static Statistics operator -(Statistics s1, Statistics s2)
        {
            var statistics = new Statistics();

            foreach (var stat in s1)
            {
                statistics.Add(stat);
            }

            foreach (var stat in s2)
            {
                var existing = statistics.SingleOrDefault(x => x.GetType() == stat.GetType());
                if (existing != null)
                {
                    statistics.Remove(existing);
                    statistics.Add(existing.Subtract(stat));
                }
                else
                {
                    statistics.Add(stat.Negate());
                }
            }

            return statistics;
        }

        public static Statistics operator *(Statistics s1, float f)
        {
            var statistics = new Statistics();

            foreach (var stat in s1)
            {
                statistics.Add(stat.Multiply(f));
            }

            return statistics;
        }

        public static Statistics operator *(float f, Statistics s1)
        {
            return s1 * f;
        }
    }
}
