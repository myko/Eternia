using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace EterniaGame
{
    public class Statistics
    {
        // Values
        [ContentSerializer(Optional = true)]
        public float Health { get; set; }
        [ContentSerializer(Optional = true)]
        public float Mana { get; set; }
        [ContentSerializer(Optional = true)]
        public float AttackPower { get; set; }
        [ContentSerializer(Optional = true)]
        public float SpellPower { get; set; }

        // Ratings
        [ContentSerializer(Optional = true)]
        public int ArmorRating { get; set; }
        [ContentSerializer(Optional = true)]
        public int MissRating { get; set; }
        [ContentSerializer(Optional = true)]
        public int DodgeRating { get; set; }
        [ContentSerializer(Optional = true)]
        public int HitRating { get; set; }
        [ContentSerializer(Optional = true)]
        public int CritRating { get; set; }
        [ContentSerializer(Optional = true)]
        public int PrecisionRating { get; set; }

        // Modifiers
        [ContentSerializer(Optional = true)]
        public float DamageDone { get; set; }
        [ContentSerializer(Optional = true)]
        public float DamageTaken { get; set; }
        [ContentSerializer(Optional = true)]
        public float HealingDone { get; set; }
        [ContentSerializer(Optional = true)]
        public float HealingTaken { get; set; }

        // Other
        [ContentSerializer(Optional = true)]
        public int ExtraRewards { get; set; }

        // Calculated
        public float ArmorReduction
        {
            get
            {
                return 0.75f * ArmorRating / (ArmorRating + 1000f);
            }
        }

        public float CritChance
        {
            get
            {
                return 0.075f + CritRating / (2f * CritRating + 1000f);
            }
        }

        public float Precision
        {
            get
            {
                return 0.50f + PrecisionRating / (2f * PrecisionRating + 1200f);
            }
        }

        public Statistics()
        {
            DamageDone = 1;
            DamageTaken = 1;
            HealingDone = 1;
            HealingTaken = 1;
        }

        public static Statistics operator +(Statistics s1, Statistics s2)
        {
            return new Statistics()
            {
                Health = s1.Health + s2.Health,
                Mana = s1.Mana + s2.Mana,
                AttackPower = s1.AttackPower + s2.AttackPower,
                SpellPower = s1.SpellPower + s2.SpellPower,

                ArmorRating = s1.ArmorRating + s2.ArmorRating,
                MissRating = s1.MissRating + s2.MissRating,
                DodgeRating = s1.DodgeRating + s2.DodgeRating,
                CritRating = s1.CritRating + s2.CritRating,
                HitRating = s1.HitRating + s2.HitRating,
                PrecisionRating = s1.PrecisionRating + s2.PrecisionRating,

                DamageDone = s1.DamageDone * s2.DamageDone,
                DamageTaken = s1.DamageTaken * s2.DamageTaken,
                HealingDone = s1.HealingDone * s2.HealingDone,
                HealingTaken = s1.HealingTaken * s2.HealingTaken,

                ExtraRewards = s1.ExtraRewards + s2.ExtraRewards
            };
        }

        public static Statistics operator -(Statistics s1, Statistics s2)
        {
            return new Statistics()
            {
                Health = s1.Health - s2.Health,
                Mana = s1.Mana - s2.Mana,
                AttackPower = s1.AttackPower - s2.AttackPower,
                SpellPower = s1.SpellPower - s2.SpellPower,

                ArmorRating = s1.ArmorRating - s2.ArmorRating,
                MissRating = s1.MissRating - s2.MissRating,
                DodgeRating = s1.DodgeRating - s2.DodgeRating,
                CritRating = s1.CritRating - s2.CritRating,
                HitRating = s1.HitRating - s2.HitRating,
                PrecisionRating = s1.PrecisionRating - s2.PrecisionRating,

                // TODO: Not valid to subtract modifiers this way
                DamageDone = s1.DamageDone - s2.DamageDone,
                DamageTaken = s1.DamageTaken - s2.DamageTaken,
                HealingDone = s1.HealingDone - s2.HealingDone,
                HealingTaken = s1.HealingTaken - s2.HealingTaken,

                ExtraRewards = s1.ExtraRewards - s2.ExtraRewards
            };
        }

        public static Statistics operator *(Statistics s1, float f)
        {
            return new Statistics()
            {
                Health = s1.Health * f,
                Mana = s1.Mana * f,
                AttackPower = s1.AttackPower * f,
                SpellPower = s1.SpellPower * f,

                ArmorRating = (int)(s1.ArmorRating * f),
                MissRating = (int)(s1.MissRating * f),
                DodgeRating = (int)(s1.DodgeRating * f),
                CritRating = (int)(s1.CritRating * f),
                HitRating = (int)(s1.HitRating * f),
                PrecisionRating = (int)(s1.PrecisionRating * f),

                DamageDone = s1.DamageDone, 
                DamageTaken = s1.DamageTaken, 
                HealingDone = s1.HealingDone, 
                HealingTaken = s1.HealingTaken,

                ExtraRewards = s1.ExtraRewards
            };
        }

        public static Statistics operator *(float f, Statistics s1)
        {
            return s1 * f;
        }
    }
}
