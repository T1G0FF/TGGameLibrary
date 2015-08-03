using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TGameLibrary.Enums;

namespace TGameLibrary
{
    /// <summary>
    /// An animated sprite with the ability to respond to input.
    /// </summary>
    public class MoveableAnimatedSprite : AnimatedSprite
    {
        #region Properties
        /// <summary>
        /// Vector corresponding to the direction of travel relative to Current position.
        /// </summary>
        protected Vector2 _direction = Vector2.Zero;

        /// <summary>
        /// Vector corresponding to travel distance relative to Current position.
        /// </summary>
        protected Vector2 _speed = Vector2.Zero;

        /// <summary>
        /// The maximum movement speed of this <see cref="MoveableAnimatedSprite"/>
        /// </summary>
        public float MovementSpeed { get; protected set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new <see cref="MoveableAnimatedSprite"/> object.
        /// </summary>
        /// <param name="game">Reference to the current <see cref="Game"/> object.</param>
        /// <param name="rows">Number of rows in the Texture file, each row represents a specific direction.</param>
        /// <param name="columns">Number of columns in the Texture file, each column represents a frames in the Animation.</param>
        /// <param name="animationLength">Time in seconds a complete set of frames should take to play.</param>
        /// <param name="footprint">Size of collision footprint.</param>
        /// <param name="facing">Enumerator corresponding to the direction this <see cref="MoveableAnimatedSprite"/> is facing.</param>
        /// <param name="movementSpeed">The maximum movement speed of this <see cref="MoveableAnimatedSprite"/></param>
        /// <param name="scale">Used to scale the object. <c>1.0F</c> is 1:1 scaling of Texture.</param>
        public MoveableAnimatedSprite(Game game, int rows, int columns, float animationLength, Vector2? startPosition = null, Rectangle? footprint = null, float? movementSpeed = 100, Face? facing = Face.Down, float? scale = 1.0F)
            : base(game, rows, columns, animationLength, startPosition, footprint, facing, scale)
        {
            MovementSpeed = (float)movementSpeed;
        }
        #endregion

        #region MonoGame Default Methods
        /// <summary>
        /// Updates position and current animation frame.
        /// <remarks>Resets animation to Idle frame when not moving</remarks>
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            OffsetPosition((_direction * _speed) * (float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);

            if (_speed == Vector2.Zero)    // Resets to Idle frame if not moving
            {
                _currentFrame = 0;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Responds to a Player's input and checks for collisions.
        /// </summary>
        /// <param name="inputState">Current state of all controllers.</param>
        /// <param name="index">PlayerIndex of Player to respond to input from.</param>
        /// <param name="bounds">Rectangle containing the bounds of the box to check for collisions with.</param>
        /// <param name="blocks">An arr of <see cref="Obstacle"/>s to check for collisions with.</param>
        public void HandleInput(InputState inputState, PlayerIndex index, Rectangle bounds, Obstacle[] blocks)
        {
            UpdateMovement(inputState, index);
            CheckBounds(bounds);
            CheckCollisions(blocks);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Sets Position to (x, y) Coordinates.
        /// </summary>
        /// <param name="x">X Coordinates</param>
        /// <param name="y">Y Coordinates</param>
        public void SetPosition(float x, float y)
        {
            SetPosition(new Vector2(x, y));
        }

        /// <summary>
        /// Sets Position to newPosition Coordinates.
        /// </summary>
        /// <param name="newPosition">Contains new (X, Y) Coordinates.</param>
        public void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
        }

        /// <summary>
        /// Changes position by (x, y) relative to current position.
        /// </summary>
        /// <param name="x">Difference in X Coordinates relative to current position.</param>
        /// <param name="y">Difference in Y Coordinates relative to current position.</param>
        public void OffsetPosition(float x, float y)
        {
            OffsetPosition(new Vector2(x, y));
        }

        /// <summary>
        /// Changes position by deltaPosition relative to current position.
        /// </summary>
        /// <param name="deltaPosition">Difference in (X, Y) Coordinates relative to current position.</param>
        public void OffsetPosition(Vector2 deltaPosition)
        {
            Position += deltaPosition;
        }
        
        /// <summary>
        /// Set speed and direction of player based on their input.
        /// TODO: Implement variable speed.
        /// </summary>
        /// <param name="inputState">Current state of all controllers.</param>
        /// <param name="index">PlayerIndex of Player to respond to input from.</param>
        public void UpdateMovement(InputState inputState, PlayerIndex index)
        {
            if (CurrentState != State.Dead)
            {
                _speed = Vector2.Zero;
                _direction = Vector2.Zero;

                if (inputState.IsUpHeld(index))
                {
                    _speed.Y = MovementSpeed;
                    _direction.Y = (float)Move.Up;
                    Facing = Face.Up;
                }
                else if (inputState.IsDownHeld(index))
                {
                    _speed.Y = MovementSpeed;
                    _direction.Y = (float)Move.Down;
                    Facing = Face.Down;
                }

                if (inputState.IsLeftHeld(index))
                {
                    _speed.X = MovementSpeed;
                    _direction.X = (float)Move.Left;
                    Facing = Face.Left;
                }
                else if (inputState.IsRightHeld(index))
                {
                    _speed.X = MovementSpeed;
                    _direction.X = (float)Move.Right;
                    Facing = Face.Right;
                }
            }
        }

        /// <summary>
        /// Checks for collisions with bounds and sets position accordingly.
        /// </summary>
        /// <param name="bounds">Rectangle containing the bounds of the box to check for collisions with.</param>
        public byte CheckBounds(Rectangle bounds)
        {
            byte Collided = 0x0;

            if (Footprint.Top < bounds.Top)
            {
                SetPosition(Position.X, bounds.Top - FootprintOffset.Y);
                Collided |= (byte)Collide.Top;
            }
            else if (Footprint.Bottom > bounds.Bottom)
            {
                SetPosition(Position.X, bounds.Bottom - (FootprintOffset.Y + FootprintGeometry.Height));
                Collided |= (byte)Collide.Bottom;
            }

            if (Footprint.Left < bounds.Left)
            {
                SetPosition(bounds.Left - FootprintOffset.X, Position.Y);
                Collided |= (byte)Collide.Left;
            }
            else if (Footprint.Right > bounds.Right)
            {
                SetPosition(bounds.Right - (FootprintOffset.X + FootprintGeometry.Width), Position.Y);
                Collided |= (byte)Collide.Right;
            }

            return Collided;
        }

        /// <summary>
        /// Checks for collisions with each object and sets position accordingly.
        /// </summary>
        /// <param name="obstacles">An arr of objects to check for collisions with.</param>
        public int CheckCollisions<T>(T[] obstacles) where T : AnimatedSprite
        {
            List<T> collidedBlocks = new List<T>();
            foreach (T obstacle in obstacles)
            {
                if (obstacle != null && obstacle.Footprint.Intersects(this.Footprint))
                {
                    collidedBlocks.Add(obstacle);
                }
            }

            foreach (T collision in collidedBlocks)
            {
                Rectangle Overlap = Rectangle.Intersect(this.Footprint, collision.Footprint);

                if (Overlap.Width > Overlap.Height)
                {	// Top or Bottom
                    if (Footprint.Top == Overlap.Top)
                    {	// Collide Bottom
                        OffsetPosition(0, Overlap.Height);
                    }
                    else
                    {	// Collide Top
                        OffsetPosition(0, -Overlap.Height);
                    }
                }
                else
                {	// Left or Right
                    if (Footprint.Left == Overlap.Left)
                    {	// Collide Right
                        OffsetPosition(Overlap.Width, 0);
                    }
                    else
                    {	// Collide Left
                        OffsetPosition(-Overlap.Width, 0);
                    }
                }
            }
            return collidedBlocks.Count;
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
