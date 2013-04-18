//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Content;
//using EterniaGame;

//namespace Eternia.Game.Stats
//{
//    public class OldStatistics
//    {
//        // Values
//        [ContentSerializer(Optional = true)]
//        public float Health { get; set; }
//        [ContentSerializer(Optional = true)]
//        public float Mana { get; set; }
//        [ContentSerializer(Optional = true)]
//        public float Energy { get; set; }
//        [ContentSerializer(Optional = true)]
//        public float AttackPower { get; set; }
//        [ContentSerializer(Optional = true)]
//        public float SpellPower { get; set; }

//        // Ratings
//        [ContentSerializer(Optional = true)]
//        public DamageReduction DamageReduction { get; set; }
//        [ContentSerializer(Optional = true)]
//        public int MissRating { get; set; }
//        [ContentSerializer(Optional = true)]
//        public int DodgeRating { get; set; }
//        [ContentSerializer(Optional = true)]
//        public int HitRating { get; set; }
//        [ContentSerializer(Optional = true)]
//        public int CritRating { get; set; }
//        [ContentSerializer(Optional = true)]
//        public int PrecisionRating { get; set; }

//        // Modifiers
//        [ContentSerializer(Optional = true)]
//        public float DamageDone { get; set; }
//        [ContentSerializer(Optional = true)]
//        public float DamageTaken { get; set; }
//        [ContentSerializer(Optional = true)]
//        public float HealingDone { get; set; }
//        [ContentSerializer(Optional = true)]
//        public float HealingTaken { get; set; }

//        // Other
//        [ContentSerializer(Optional = true)]
//        public int ExtraRewards { get; set; }

//        public float CritChance
//        {
//            get
//            {
//                return 0.075f + CritRating / (2f * CritRating + 1000f);
//            }
//        }

//        public float Precision
//        {
//            get
//            {
//                return 0.50f + PrecisionRating / (2f * PrecisionRating + 1200f);
//            }
//        }

//        public OldStatistics()
//        {
//            DamageDone = 1;
//            DamageTaken = 1;
//            HealingDone = 1;
//            HealingTaken = 1;
//            DamageReduction = new DamageReduction();
//        }

//        public static OldStatistics operator +(OldStatistics s1, OldStatistics s2)
//        {
//            return new OldStatistics()
//            {
//                Health = s1.Health + s2.Health,
//                Mana = s1.Mana + s2.Mana,
//                Energy = s1.Energy + s2.Energy,
//                AttackPower = s1.AttackPower + s2.AttackPower,
//                SpellPower = s1.SpellPower + s2.SpellPower,

//                //DamageReduction = s1.DamageReduction + s2.DamageReduction,
//                MissRating = s1.MissRating + s2.MissRating,
//                DodgeRating = s1.DodgeRating + s2.DodgeRating,
//                CritRating = s1.CritRating + s2.CritRating,
//                HitRating = s1.HitRating + s2.HitRating,
//                PrecisionRating = s1.PrecisionRating + s2.PrecisionRating,

//                DamageDone = s1.DamageDone * s2.DamageDone,
//                DamageTaken = s1.DamageTaken * s2.DamageTaken,
//                HealingDone = s1.HealingDone * s2.HealingDone,
//                HealingTaken = s1.HealingTaken * s2.HealingTaken,

//                ExtraRewards = s1.ExtraRewards + s2.ExtraRewards
//            };
//        }

//        public static OldStatistics operator -(OldStatistics s1, OldStatistics s2)
//        {
//            return new OldStatistics()
//            {
//                Health = s1.Health - s2.Health,
//                Mana = s1.Mana - s2.Mana,
//                Energy = s1.Energy - s2.Energy,
//                AttackPower = s1.AttackPower - s2.AttackPower,
//                SpellPower = s1.SpellPower - s2.SpellPower,

//                //DamageReduction = s1.DamageReduction - s2.DamageReduction,
//                MissRating = s1.MissRating - s2.MissRating,
//                DodgeRating = s1.DodgeRating - s2.DodgeRating,
//                CritRating = s1.CritRating - s2.CritRating,
//                HitRating = s1.HitRating - s2.HitRating,
//                PrecisionRating = s1.PrecisionRating - s2.PrecisionRating,

//                // TODO: Not valid to subtract modifiers this way
//                DamageDone = s1.DamageDone - s2.DamageDone,
//                DamageTaken = s1.DamageTaken - s2.DamageTaken,
//                HealingDone = s1.HealingDone - s2.HealingDone,
//                HealingTaken = s1.HealingTaken - s2.HealingTaken,

//                ExtraRewards = s1.ExtraRewards - s2.ExtraRewards
//            };
//        }

//        public static OldStatistics operator *(OldStatistics s1, float f)
//        {
//            return new OldStatistics()
//            {
//                Health = s1.Health * f,
//                Mana = s1.Mana * f,
//                Energy = s1.Energy * f,
//                AttackPower = s1.AttackPower * f,
//                SpellPower = s1.SpellPower * f,

//                //DamageReduction = s1.DamageReduction * f,
//                MissRating = (int)(s1.MissRating * f),
//                DodgeRating = (int)(s1.DodgeRating * f),
//                CritRating = (int)(s1.CritRating * f),
//                HitRating = (int)(s1.HitRating * f),
//                PrecisionRating = (int)(s1.PrecisionRating * f),

//                DamageDone = s1.DamageDone, 
//                DamageTaken = s1.DamageTaken, 
//                HealingDone = s1.HealingDone, 
//                HealingTaken = s1.HealingTaken,

//                ExtraRewards = s1.ExtraRewards
//            };
//        }

//        public static OldStatistics operator *(float f, OldStatistics s1)
//        {
//            return s1 * f;
//        }
//    }
//}
