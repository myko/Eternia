using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myko.Xna.Ui
{
    public struct Binding<T>
    {
        private Func<T> func;

        public Binding(Func<T> func)
        {
            this.func = func;
        }

        public Binding(T constant)
        {
            this.func = () => constant;
        }

        public T GetValue()
        {
            if (func == null)
                return default(T);

            return func();
        }

        public static implicit operator Binding<T>(T v)
        {
            return new Binding<T>(v);
        }

        public static implicit operator Binding<T>(Func<T> f)
        {
            return new Binding<T>(f);
        }

        public static implicit operator T(Binding<T> binding)
        {
            return binding.GetValue();
        }
    }
}
