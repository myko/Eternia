//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Content;
//using Eternia.Game;
////using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;

//namespace Eternia.Game.Stats
//{
//    public class DamageReduction
//    {
//        private Dictionary<DamageSchools, int> ratings;

//        [ContentSerializer(Optional=true)]
//        public int ArmorRating { get { return GetRatingForSchool(DamageSchools.Physical); } set { SetRatingForSchool(DamageSchools.Physical, value); } }
//        [ContentSerializer(Optional = true)]
//        public int FireResistanceRating { get { return GetRatingForSchool(DamageSchools.Fire); } set { SetRatingForSchool(DamageSchools.Fire, value); } }
//        [ContentSerializer(Optional = true)]
//        public int FrostResistanceRating { get { return GetRatingForSchool(DamageSchools.Frost); } set { SetRatingForSchool(DamageSchools.Frost, value); } }
//        [ContentSerializer(Optional = true)]
//        public int ArcaneResistanceRating { get { return GetRatingForSchool(DamageSchools.Arcane); } set { SetRatingForSchool(DamageSchools.Arcane, value); } }
//        [ContentSerializer(Optional = true)]
//        public int NatureResistanceRating { get { return GetRatingForSchool(DamageSchools.Nature); } set { SetRatingForSchool(DamageSchools.Nature, value); } }
//        [ContentSerializer(Optional = true)]
//        public int HolyResistanceRating { get { return GetRatingForSchool(DamageSchools.Holy); } set { SetRatingForSchool(DamageSchools.Holy, value); } }
//        [ContentSerializer(Optional = true)]
//        public int UnholyResistanceRating { get { return GetRatingForSchool(DamageSchools.Unholy); } set { SetRatingForSchool(DamageSchools.Unholy, value); } }

//        public override string Name { get { return "Damage reduction"; } }
//        public override Color Color { get { return ratings.Sum(x => x.Value) < 0 ? Color.Salmon : Color.LightGreen; } }

//        public DamageReduction()
//        {
//            ratings = new Dictionary<DamageSchools, int>();
//        }

//        public DamageReduction(DamageReduction other)
//            : this()
//        {
//            foreach (var key in other.ratings.Keys)
//            {
//                ratings.Add(key, other.ratings[key]);
//            }
//        }

//        public int GetRatingForSchool(DamageSchools school)
//        {
//            if (ratings.ContainsKey(school))
//                return ratings[school];
//            else
//                return 0;
//        }

//        public void SetRatingForSchool(DamageSchools school, int value)
//        {
//            if (ratings.ContainsKey(school))
//                ratings[school] = value;
//            else
//                ratings.Add(school, value);
//        }

//        public float GetReductionForSchool(DamageSchools school)
//        {
//            var rating = GetRatingForSchool(school);

//            return GetReductionForRating(rating);
//        }

//        private static float GetReductionForRating(int rating)
//        {
//            return 0.75f * rating / (Math.Max(-999, rating) + 1000f);
//        }

//        public string GetNameForSchool(DamageSchools school)
//        {
//            switch (school)
//            {
//                case DamageSchools.Physical:
//                    return "Armor";
//                case DamageSchools.Fire:
//                    return "Fire resistance";
//                case DamageSchools.Frost:
//                    return "Frost resistance";
//                case DamageSchools.Arcane:
//                    return "Arcane resistance";
//                case DamageSchools.Nature:
//                    return "Nature resistance";
//                case DamageSchools.Holy:
//                    return "Holy resistance";
//                case DamageSchools.Unholy:
//                    return "Unholy resistance";
//                default:
//                    return "";
//            }
//        }

//        public override DamageReduction Add(DamageReduction s)
//        {
//            var reduction = new DamageReduction();
//            foreach (DamageSchools school in Enum.GetValues(typeof(DamageSchools)))
//            {
//                reduction.SetRatingForSchool(school, GetRatingForSchool(school) + s.GetRatingForSchool(school));
//            }
//            return reduction;
//        }

//        public override DamageReduction Subtract(DamageReduction s)
//        {
//            var reduction = new DamageReduction();
//            foreach (DamageSchools school in Enum.GetValues(typeof(DamageSchools)))
//            {
//                reduction.SetRatingForSchool(school, GetRatingForSchool(school) - s.GetRatingForSchool(school));
//            }
//            return reduction;
//        }

//        public override StatBase Negate()
//        {
//            var reduction = new DamageReduction();
//            foreach (DamageSchools school in Enum.GetValues(typeof(DamageSchools)))
//            {
//                reduction.SetRatingForSchool(school, -GetRatingForSchool(school));
//            }
//            return reduction;
//        }

//        public override StatBase Multiply(float f)
//        {
//            var reduction = new DamageReduction();
//            foreach (DamageSchools school in Enum.GetValues(typeof(DamageSchools)))
//            {
//                reduction.SetRatingForSchool(school, (int)(GetRatingForSchool(school) * f));
//            }
//            return reduction;
//        }

//        public override string ToValueString()
//        {
//            if (!ratings.Any())
//                return "";

//            return string.Join("\n", ratings
//                .OrderBy(x => GetNameForSchool(x.Key))
//                .Where(x => x.Value != 0)
//                .Select(x => x.Value.ToString() + " " + GetNameForSchool(x.Key) + " (" + GetReductionForRating(x.Value).ToString("P") + ")")
//                .ToArray());
//        }

//        public override string ToItemTooltipString()
//        {
//            if (!ratings.Any())
//                return "";

//            return string.Join("\n", ratings
//                .OrderBy(x => GetNameForSchool(x.Key))
//                .Where(x => x.Value != 0)
//                .Select(x => x.Value.ToString() + " " + GetNameForSchool(x.Key))
//                .ToArray());
//        }

//        public override string ToItemUpgradeString()
//        {
//            if (!ratings.Any())
//                return "";

//            return string.Join("\n", ratings
//                .OrderBy(x => GetNameForSchool(x.Key))
//                .Where(x => x.Value != 0)
//                .Select(x => GetNameForSchool(x.Key) + ": " + (x.Value < 0 ? "" : "+") + x.Value.ToString())
//                .ToArray());
//        }
//    }
//}
