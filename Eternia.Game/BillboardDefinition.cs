using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Eternia.Game
{
    public class Interpolator<T>
    {
        public virtual Func<float, T> ToFunc()
        {
            return x => default(T);
        }
    }

    public class LinearFloatInterpolator : Interpolator<float>
    {
        public float From { get; set; }
        public float To { get; set; }

        public LinearFloatInterpolator()
        {
            From = 0f;
            To = 1f;
        }

        public override Func<float, float> ToFunc()
        {
            return x => (1 - x) * From + x * To;
        }
    }

    public class QuadraticFloatInterpolator : Interpolator<float>
    {
        public float From { get; set; }
        public float To { get; set; }

        public QuadraticFloatInterpolator()
        {
            From = 0f;
            To = 1f;
        }

        public override Func<float, float> ToFunc()
        {
            return x => (1 - x * x) * From + x * x * To;
        }
    }

    public class GraphicsEffectDefinition
    {
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
    }

    public class BillboardDefinition: GraphicsEffectDefinition
    {
        public string TextureName { get; set; }
        public float LifeTime { get; set; }
        public float Angle { get; set; }

        [ContentSerializer(Optional = true)] 
        public Interpolator<float> AlphaFunc { get; set; }
        [ContentSerializer(Optional = true)]
        public Interpolator<float> AngleFunc { get; set; }
        [ContentSerializer(Optional = true)]
        public Interpolator<float> ScaleFunc { get; set; }

        public BillboardDefinition()
        {
            Scale = 1f;
            LifeTime = 1f;
        }
    }

    public class ParticleSystemDefinition : GraphicsEffectDefinition
    {
        public string TextureName { get; set; }

        [ContentSerializer(Optional = true)]
        public float LifeSpan { get; set; }

        [ContentSerializer(Optional = true)]
        public Interpolator<float> AlphaFunc { get; set; }
        [ContentSerializer(Optional = true)]
        public Interpolator<float> AngleFunc { get; set; }
        [ContentSerializer(Optional = true)]
        public Interpolator<float> ScaleFunc { get; set; }

        public ParticleSystemDefinition()
        {
            Scale = 1f;
            LifeSpan = float.PositiveInfinity;
        }
    }
}
