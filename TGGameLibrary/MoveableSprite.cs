#region File Description
//-----------------------------------------------------------------------------
// MoveableSprite.cs
//
// Written by Thomas
// Last Updated: 2016-09-13
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using TGGameLibrary.Enums;
#endregion

namespace TGGameLibrary
{
    public class MoveableSprite : MoveableAnimatedSprite
    {
        #region Initialisation
        /// <summary>
        /// Creates a new <see cref="MoveableSprite"/> object, by creating a new <see cref="MoveableAnimatedSprite"/> with a 1 frame animation and an animation length of 0.
        /// </summary>
        /// <param name="game">Reference to the current <see cref="Game"/> object.</param>
        /// <param name="rows">Number of rows in the Texture file, each row represents a specific direction.</param>
        /// <param name="footprint">Size of collision footprint.</param>
        /// <param name="movementSpeed">The maximum movement speed of this <see cref="MoveableSprite"/></param>
        /// <param name="facing">Enumerator corresponding to the direction this <see cref="MoveableSprite"/> is facing.</param>
        /// <param name="scale">Used to scale the object. <c>1.0F</c> is 1:1 scaling of Texture.</param>
        public MoveableSprite(Game game, int rows, Vector2? startPosition = null, Rectangle? footprint = null, float? movementSpeed = 100, Face? facing = Face.Down, float? scale = 1.0F)
            : base(game, new Vector2(1, rows), 0.0F, startPosition, footprint, movementSpeed, facing, scale)
        { }
        #endregion

        #region MonoGame Default Methods
        /// <summary>
        /// Update movement, but override the animation frame counter.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            OffsetPosition((_direction * _speed) * (float)gameTime.ElapsedGameTime.TotalSeconds);            
        }
        #endregion
    }
}
