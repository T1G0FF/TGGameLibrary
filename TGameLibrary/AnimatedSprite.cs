using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGExtensions;

namespace TGameLibrary
{
    /// <summary>
    /// An animated sprite.
    /// </summary>
    public class AnimatedSprite : FlatRectangle
    {
        #region Debugging
        /// <summary>
        /// Debugging property used to render sprite collision footprint.
        /// </summary>
        protected bool showFootprint = true;
        #endregion

        #region Enumerators
        /// <summary>
        /// Enumerator corresponding to the direction this object is facing.
        /// </summary>
        public enum Face { Up, Down, Right, Left }
        #endregion

        #region Properties
        /// <summary>
        /// Current animation frame to be displayed.
        /// </summary>
        protected int _currentFrame;

        /// <summary>
        /// Time since last Animation frame was displayed.
        /// </summary>
        protected float _currentTime;

        /// <summary>
        /// Time each animation frame should be displayed.
        /// </summary>
        protected float _timeStep;

        /// <summary>
        /// The Sprite sheet texture.
        /// </summary>
        protected Texture2D Texture;

        /// <summary>
        /// Size of an individual frame of animation and thus size of sprite. Based on Texture size and number of Rows and Columns.
        /// </summary>
        protected Rectangle Size = Rectangle.Empty;

        /// <summary>
        /// Size of collision footprint.
        /// </summary>
        protected Rectangle Footprint = Rectangle.Empty;

        /// <summary>
        /// X and Y Coordinates of top left of <see cref="AnimatedSprite"/>.
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(Size.X, Size.Y); }
            set { Size.X = (int)value.X; Size.Y = (int)value.Y; }
        }

        /// <summary>
        /// Enumerator corresponding to the direction this <see cref="AnimatedSprite"/> is facing.
        /// </summary>
        public Face Facing;

        /// <summary>
        /// The path\name of the asset file to load, relative to <c>Content.RootDirectory</c>.
        /// </summary>
        public string AssetName { get; private set; }

        /// <summary>
        /// Number of rows in the Texture file, each row represents a specific direction.
        /// <remarks>Left is represented by a horizontal translation of the Right Animation</remarks>
        /// </summary>
        public int Rows { get; private set; }

        /// <summary>
        /// Number of columns in the Texture file, each column represents a frame in the Animation.
        /// <remarks>Column 1 is the Idle Frame.</remarks>
        /// </summary>
        public int Columns { get; private set; }

        /// <summary>
        /// Used to scale the object. (<c>1.0F</c> is 1:1 scaling of Texture).
        /// </summary>
        public float Scale { get; protected set; }

        /// <summary>
        /// Returns Y Position relative to Viewport height, as a value between 0.0 and 1.0.
        /// </summary>
        public float Depth { get { return ((float)Footprint.Y / (float)Game.GraphicsDevice.Viewport.Height); } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new <see cref="AnimatedSprite"/> object.
        /// </summary>
        /// <param name="game">Reference to the current <see cref="Game"/> object.</param>
        /// <param name="rows">Number of rows in the Texture file, each row represents a specific direction.</param>
        /// <param name="columns">Number of columns in the Texture file, each column represents a frames in the Animation.</param>
        /// <param name="animationLength">Time in seconds a complete set of frames should take to play.</param>
        /// <param name="footprint">Size of collision footprint.</param>
        /// <param name="facing">Enumerator corresponding to the direction this <see cref="AnimatedSprite"/> is facing.</param>
        /// <param name="scale">Used to scale the object. <c>1.0F</c> is 1:1 scaling of Texture.</param>
        public AnimatedSprite(Game game, int rows, int columns, float animationLength, Vector2? position = null, Rectangle? footprint = null, Face? facing = Face.Down, float? scale = 1.0F)
            : base(game)
        {
            Rows = rows;
            Columns = columns;
            _timeStep = animationLength / columns;
            Facing = (Face)facing;
            Scale = (float)scale;

            if (position.HasValue)
                Position = (Vector2)position;

            if (footprint.HasValue)
                Footprint = (Rectangle)footprint;
        }
        #endregion

        #region MonoGame Default Methods
        /// <summary>
        /// Loads Content.
        /// </summary>
        /// <param name="contentManager"> The <see cref="ContentManger"/> object used to load content.</param>
        /// <param name="assetName">The path\name of the asset file to load.</param>
        public void LoadContent(ContentManager contentManager, string assetName)
        {
            Texture = contentManager.Load<Texture2D>(assetName);
            AssetName = assetName;
            updateSizeAndFootprint(); // Now that texture is loaded perform Scale transformations.
        }

        /// <summary>
        /// Updates current animation frame.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (_currentTime > _timeStep)
            {
                _currentFrame++;
                if (_currentFrame == Columns)
                {
                    _currentFrame = 0;
                    _currentTime = 0.0F;
                }
            }
            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Draws this <see cref="AnimatedSprite"/> instance using the default colour [<c>Color.Red</c>] and current Depth.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch Object used to draw this <see cref="AnimatedSprite"/></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.Draw(spriteBatch, Color.Red, Depth);
        }

        /// <summary>
        /// Draws this <see cref="AnimatedSprite"/> instance using the specified colour and current Depth.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch Object used to draw this <see cref="AnimatedSprite"/></param>
        /// <param name="color">The Color used when drawing this <see cref="AnimatedSprite"/>'s footprint</param>
        public virtual void Draw(SpriteBatch spriteBatch, Color color)
        {
            this.Draw(spriteBatch, color, Depth);
        }

        /// <summary>
        /// Draws this <see cref="AnimatedSprite"/> instance using the specified colour and depth.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch Object used to draw this <see cref="AnimatedSprite"/></param>
        /// <param name="color">The Color used when drawing this <see cref="AnimatedSprite"/>'s footprint</param>
        /// <param name="depth">This <see cref="AnimatedSprite"/>'s layer depth.</param>
        public virtual void Draw(SpriteBatch spriteBatch, Color color, float depth)
        {
            int row = (int)Facing;
            int column = _currentFrame % Columns;
            SpriteEffects translation = SpriteEffects.None;

            if (Rows == 3 && Facing == Face.Left)
            {
                row = (int)Face.Right;
                translation = SpriteEffects.FlipHorizontally;
            }

            Rectangle assetRectangle = new Rectangle((Texture.Width / Columns) * column, (Texture.Height / Rows) * row, (Texture.Width / Columns), (Texture.Height / Rows));

            if (showFootprint)
            {
                Vector2 footprintPosition = new Vector2(Footprint.X, Footprint.Y);
                spriteBatch.Draw(DummyTexture, footprintPosition, Footprint, color, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, depth > 0 ? depth.NextBefore() : 0.0F);
            }

            spriteBatch.Draw(Texture, Position, assetRectangle, Color.White, 0.0F, Vector2.Zero, Scale, translation, depth > 0 ? depth : 0.0F.NextAfter());
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Scale transformations on Size and Footprint. Requires Texture to be loaded.
        /// </summary>
        private void updateSizeAndFootprint()
        {
            Size = new Rectangle(0, 0, (int)((Texture.Width / Columns) * Scale), (int)((Texture.Height / Rows) * Scale));
            if (Footprint == Rectangle.Empty)
            { Footprint = new Rectangle(0, 0, Size.Width, Size.Height); }
            else
            { Footprint = new Rectangle(0, 0, (int)(Footprint.Width * Scale), (int)(Footprint.Height * Scale)); }
        }
        #endregion
    }
}
