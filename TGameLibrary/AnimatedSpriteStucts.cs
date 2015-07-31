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
        public struct HealthStruct
        {
            public HealthStruct(float max = 100, float current = 100)
            {
                Max = max;
                Current = current;
            }
            
            public float Max;
            public float Current;

            public float Percent { get { return Current / Max; } }
        }

        public struct ArmourStruct
        {
            public ArmourStruct(float generic = 0)
            {
                Generic = generic;
                Stabbing = 0;
                Piercing = 0;
                Crushing = 0;
            }

            public ArmourStruct(float stabbing = 0, float piercing = 0, float crushing = 0)
            {
                Generic = 0;
                Stabbing = stabbing;
                Piercing = piercing;
                Crushing = crushing;
            }

            public float Generic;
            public float Stabbing;
            public float Piercing;
            public float Crushing;
        }

        public struct DamageStruct
        {
            public DamageStruct(int generic = 0)
            {
                Generic = generic;
                Stabbing = 0;
                Piercing = 0;
                Crushing = 0;
            }

            public DamageStruct(float stabbing = 0, float piercing = 0, float crushing = 0)
            {
                Generic = 0;
                Stabbing = stabbing;
                Piercing = piercing;
                Crushing = crushing;
            }

            public float Generic;
            public float Stabbing;
            public float Piercing;
            public float Crushing;
        }

        public struct StatusStruct
        {
            public StatusStruct(bool invulnerable = true)
            {
                Invunerable = invulnerable;
            }

            public bool Invunerable;
        }
    }
}
