using System.Runtime.InteropServices;

namespace TGameLibrary
{
    /// <remarks>
    /// Various extension methods
    /// </remarks>
    public static partial class MyExtensions
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct FloatIntUnion
        {
            [FieldOffset(0)]
            public int i;
            [FieldOffset(0)]
            public float f;
        }

        /// <summary>
        /// Returns the next float after x in the direction of y.
        /// </summary>
        /// <param name="x">The float to nudge.</param>
        /// <param name="y">the direction to nudge.</param>
        /// <returns>Returns the next float after x in the direction of y.</returns>
        public static float Nudge(this float x, float y)
        {
            if (float.IsNaN(x) || float.IsNaN(y)) return x + y;
            if (x == y) return y;  // nextafter(0, -0) = -0

            FloatIntUnion u;
            u.i = 0; u.f = x;  // shut up the compiler

            if (x == 0)
            {
                u.i = 1;
                return y > 0 ? u.f : -u.f;
            }

            if ((x > 0) == (y > x))
                u.i++;
            else
                u.i--;
            return u.f;
        }

        /// <summary>
        /// Helper function for Nudge();
        /// </summary>
        /// <param name="x">The float to nudge.</param>
        /// <returns>Returns the next float after x.</returns>
        public static float NextAfter(this float x)
        {
            return x.Nudge(+1);
        }

        /// <summary>
        /// Helper function for Nudge();
        /// </summary>
        /// <param name="x">The float to nudge.</param>
        /// <returns>Returns the next float before x.</returns>
        public static float NextBefore(this float x)
        {
            return x.Nudge(-1);
        }
    }
}
