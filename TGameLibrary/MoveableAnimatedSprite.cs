using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TGameLibrary
{
    /// <summary>
    /// An animated sprite with the ability to respond to input.
    /// </summary>
    public class MoveableAnimatedSprite : AnimatedSprite
    {
        #region Enumerators
        /// <summary>
        /// Enumerator corresponding to a directions position relative to Origin [0,0].
        /// </summary>
        /// <example>
        /// To move Down one must move in the +Y.
        /// To move Left one must move in the -X.
        /// </example>
        public enum Move { Up = -1, Down = +1, Right = +1, Left = -1 }

        /// <summary>
        /// Enumerator corresponding to the state this object is in.
        /// </summary>
        public enum State { Walking }
        #endregion

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

        /// <summary>
        /// The current state of this <see cref="MoveableAnimatedSprite"/>.
        /// TODO: Implement more states.
        /// </summary>
        public State CurrentState = State.Walking;
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
        #endregion

        #region Private Methods
        /// <summary>
        /// Set speed and direction of player based on their input.
        /// TODO: Implement variable speed.
        /// </summary>
        /// <param name="inputState">Current state of all controllers.</param>
        /// <param name="index">PlayerIndex of Player to respond to input from.</param>
        private void UpdateMovement(InputState inputState, PlayerIndex index)
        {
            if (CurrentState == State.Walking)
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
        private void CheckBounds(Rectangle bounds)
        {
            if (Footprint.Top < bounds.Top)
            {
                SetPosition(Position.X, bounds.Top - FootprintOffset.Y);
            }
            else if (Footprint.Bottom > bounds.Bottom)
            {
                SetPosition(Position.X, bounds.Bottom - (FootprintOffset.Y + FootprintGeometry.Height));
            }

            if (Footprint.Left < bounds.Left)
            {
                SetPosition(bounds.Left - FootprintOffset.X, Position.Y);
            }
            else if (Footprint.Right > bounds.Right)
            {
                SetPosition(bounds.Right - (FootprintOffset.X + FootprintGeometry.Width), Position.Y);
            }
        }

        /// <summary>
        /// Checks for collisions with each <see cref="Obstacle"/> and sets position accordingly.
        /// </summary>
        /// <param name="blocks">An arr of <see cref="Obstacle"/>s to check for collisions with.</param>
        private void CheckCollisions(Obstacle[] blocks)
        {
            List<Obstacle> collidedBlocks = new List<Obstacle>();
            foreach (Obstacle block in blocks)
            {
                if (block != null && block.Footprint.Intersects(this.Footprint))
                {
                    collidedBlocks.Add(block);
                }
            }

            foreach (Obstacle b in collidedBlocks)
            {
                Rectangle Overlap = Rectangle.Intersect(this.Footprint, b.Footprint);

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
        }
        #endregion
    }
}
