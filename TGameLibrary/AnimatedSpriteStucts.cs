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

            public float Percent { get { return (Current / Max) * 100; } }
        }

        public struct DamageStruct
        {
            public DamageStruct(float armour = 0, float damage = 0)
            {
                Armour = armour;
                Damage = damage;
            }
            
            public float Damage;
            public float Armour;
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
