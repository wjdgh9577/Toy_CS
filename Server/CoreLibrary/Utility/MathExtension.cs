using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utility
{
    public static class MathExtension
    {
        public static float Epsilon = 1e-4f;

        public static bool IsBetween(this float a, float b, float c, bool closed = true)
        {
            if (closed)
                return a - Epsilon >= Math.Min(b, c) && a + Epsilon <= Math.Max(b, c);
            else
                return a - Epsilon > Math.Min(b, c) && a + Epsilon < Math.Max(b, c);
        }

        public static int ToInt32Safe(this float a)
        {
            var round = Convert.ToInt32(a);
            var interval = Math.Abs(round - a);
            return interval < Epsilon ? round : (int)a;
        }
    }
}