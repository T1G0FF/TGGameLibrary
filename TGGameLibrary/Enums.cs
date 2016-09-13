#region File Description
//-----------------------------------------------------------------------------
// Enums.cs
//
// Written by Thomas
// Last Updated: 2016-09-13
//-----------------------------------------------------------------------------
#endregion

namespace TGGameLibrary
{
    namespace Enums
    {
        /// <summary>
        /// Enumerator game's current state.
        /// </summary>
        public enum GameStates {    
            Menu, 
            Loading, 
            Playing, 
            Paused 
        };
        
        /// <summary>
        /// Enumerator corresponding to the direction an object is facing.
        /// </summary>
        public enum Face : byte { 
            Up, 
            Down, 
            Right, 
            Left 
        }

        /// <summary>
        /// Enumerator corresponding to the state an object is in.
        /// </summary>
        public enum State : byte { 
            Alive, 
            Dead, 
            Standing, 
            Crouching, 
            Laying, 
            Walking, 
            Running 
        }

        /// <summary>
        /// Enumerator corresponding to a directions position relative to Origin [0,0].
        /// </summary>
        /// <example>
        /// To move Down one must move in the +Y.
        /// To move Left one must move in the -X.
        /// </example>
        public enum Move : sbyte { 
            Up      = -1, 
            Down    = +1, 
            Right   = +1, 
            Left    = -1 
        }

        /// <summary>
        /// Enumerator corresponding to the direction an object has collided with something, as a bitmask.
        /// </summary>
        public enum Collide : byte { 
            Top     = 0x1, 
            Right   = 0x2, 
            Bottom  = 0x4, 
            Left    = 0x8
        }
    }
}
