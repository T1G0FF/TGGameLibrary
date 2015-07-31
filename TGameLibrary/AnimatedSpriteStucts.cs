using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGameLibrary
{
    /// <summary>
    /// An animated sprite.
    /// </summary>
    public partial class AnimatedSprite
    {
        public struct Health
        {
            public static int Max = 100;
            public static int Current = Max;
        }

        public struct Armour
        {
            public static float Generic = 0;
            public static float Stabbing = 0;
            public static float Piercing = 0;
            public static float Crushing = 0;
        }

        public struct Damage
        {
            public static float Generic = 0;
            public static float Stabbing = 0;
            public static float Piercing = 0;
            public static float Crushing = 0;
        }

        public struct Status
        {
            public static bool Invunerable = true;
        }
    }
}
