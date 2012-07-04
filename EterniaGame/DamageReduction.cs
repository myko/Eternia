using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace EterniaGame
{
    public class DamageReduction
    {
        private Dictionary<DamageSchools, int> ratings;

        [ContentSerializer(Optional=true)]
        public int ArmorRating { get { return GetRatingForSchool(DamageSchools.Physical); } set { SetRatingForSchool(DamageSchools.Physical, value); } }
        [ContentSerializer(Optional = true)]
        public int FireResistanceRating { get { return GetRatingForSchool(DamageSchools.Fire); } set { SetRatingForSchool(DamageSchools.Fire, value); } }
        [ContentSerializer(Optional = true)]
        public int FrostResistanceRating { get { return GetRatingForSchool(DamageSchools.Frost); } set { SetRatingForSchool(DamageSchools.Frost, value); } }
        [ContentSerializer(Optional = true)]
        public int ArcaneResistanceRating { get { return GetRatingForSchool(DamageSchools.Arcane); } set { SetRatingForSchool(DamageSchools.Arcane, value); } }
        [ContentSerializer(Optional = true)]
        public int NatureResistanceRating { get { return GetRatingForSchool(DamageSchools.Nature); } set { SetRatingForSchool(DamageSchools.Nature, value); } }
        [ContentSerializer(Optional = true)]
        public int HolyResistanceRating { get { return GetRatingForSchool(DamageSchools.Holy); } set { SetRatingForSchool(DamageSchools.Holy, value); } }
        [ContentSerializer(Optional = true)]
        public int UnholyResistanceRating { get { return GetRatingForSchool(DamageSchools.Unholy); } set { SetRatingForSchool(DamageSchools.Unholy, value); } }

        public DamageReduction()
        {
            ratings = new Dictionary<DamageSchools, int>();
        }

        public DamageReduction(DamageReduction other)
            : this()
        {
            foreach (var key in other.ratings.Keys)
            {
                ratings.Add(key, other.ratings[key]);
            }
        }

        public int GetRatingForSchool(DamageSchools school)
        {
            if (ratings.ContainsKey(school))
                return ratings[school];
            else
                return 0;
        }

        public void SetRatingForSchool(DamageSchools school, int value)
        {
            if (ratings.ContainsKey(school))
                ratings[school] = value;
            else
                ratings.Add(school, value);
        }

        public float GetReductionForSchool(DamageSchools school)
        {
            var rating = GetRatingForSchool(school);
            return 0.75f * rating / (Math.Max(-999, rating) + 1000f);
        }

        public static DamageReduction operator +(DamageReduction s1, DamageReduction s2)
        {
            var reduction = new DamageReduction();
            foreach (DamageSchools school in Enum.GetValues(typeof(DamageSchools)))
            {
                reduction.SetRatingForSchool(school, s1.GetRatingForSchool(school) + s2.GetRatingForSchool(school));
            }
            return reduction;
        }

        public static DamageReduction operator -(DamageReduction s1, DamageReduction s2)
        {
            var reduction = new DamageReduction();
            foreach (DamageSchools school in Enum.GetValues(typeof(DamageSchools)))
            {
                reduction.SetRatingForSchool(school, s1.GetRatingForSchool(school) - s2.GetRatingForSchool(school));
            }
            return reduction;
        }

        public static DamageReduction operator *(DamageReduction s1, float f)
        {
            var reduction = new DamageReduction();
            foreach (DamageSchools school in Enum.GetValues(typeof(DamageSchools)))
            {
                reduction.SetRatingForSchool(school, (int)(s1.GetRatingForSchool(school) * f));
            }
            return reduction;
        }
    }
}
