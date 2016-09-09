#region File Description
//-----------------------------------------------------------------------------
// AnimatedSprite.cs
//
// 
// 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TGExtensions;
using TGGameLibrary.Enums;
#endregion

namespace TGGameLibrary
{
    /// <summary>
    /// An animated sprite.
    /// </summary>
    public partial class AnimatedSprite : FlatRectangle
    {
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
        /// Position and size of sprite. Based on Texture size and number of Rows and Columns.
        /// </summary>
        protected Rectangle Geometry = Rectangle.Empty;

        /// <summary>
        /// X and Y coordinates of top left of sprite.
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(Geometry.X, Geometry.Y); }
            protected set { Geometry.X = (int)value.X; Geometry.Y = (int)value.Y; }
        }

        public HealthStruct Health = new HealthStruct();
        public StatusStruct Status = new StatusStruct();
        public Dictionary<string, DamageStruct> DamageType = new Dictionary<string,DamageStruct>();

        /// <summary>
        /// The current state of this <see cref="AnimatedSprite"/>.
        /// </summary>
        public State CurrentState = State.Alive;

        /// <summary>
        /// Offset and size of sprite's collision footprint.
        /// </summary>
        protected Rectangle FootprintGeometry = Rectangle.Empty;

        /// <summary>
        /// Returns offset of sprite's collision footprint relative to top left of sprite.
        /// </summary>
        public Vector2 FootprintOffset => new Vector2(FootprintGeometry.X, FootprintGeometry.Y);

        /// <summary>
        /// X and Y coordinates of top left of sprite's collision footprint.
        /// </summary>
        public Vector2 FootprintPosition
        {
            get { return new Vector2(Position.X + FootprintOffset.X, Position.Y + FootprintOffset.Y); }
            protected set { Geometry.X = (int)(value.X - FootprintOffset.X); Geometry.Y = (int)(value.Y - FootprintOffset.Y); }
        }

        /// <summary>
        /// Returns position and size of sprite's collision footprint.
        /// </summary>
        public Rectangle Footprint => new Rectangle((int)FootprintPosition.X, (int)FootprintPosition.Y, 
                                                    FootprintGeometry.Width, FootprintGeometry.Height);

        /// <summary>
        /// Enumerator corresponding to the direction this <see cref="AnimatedSprite"/> is facing.
        /// </summary>
        public Face Facing;

        /// <summary>
        /// The path\name of the asset file to load, relative to <c>Content.RootDirectory</c>.
        /// </summary>
        public string AssetName { get; private set; }

        /// <summary>
        /// Number of rows and columns in the Texture file.
        /// <remarks>Each row represents a specific direction. Left is represented by a horizontal translation of the Right Animation</remarks>
        /// <remarks>Each column represents a frame in the Animation. Column 1 is the Idle Frame.</remarks>
        /// </summary>
        public Vector2 TextureSize { get; private set; }
        public int Rows => (int)TextureSize.Y;
        public int Columns => (int)TextureSize.X;

        /// <summary>
        /// Used to scale the object. (<c>1.0F</c> is 1:1 scaling of Texture).
        /// </summary>
        public float Scale { get; protected set; }

        /// <summary>
        /// Returns Y Position relative to Viewport height, as a value between 0.0 and 1.0.
        /// </summary>
        public float Depth => Footprint.Y / (float)Game.GraphicsDevice.Viewport.Height;
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
        public AnimatedSprite(Game game, Vector2 textureSize, float animationLength, Vector2? position = null, Rectangle? footprintGeometry = null, Face? facing = Face.Down, float? scale = 1.0F)
            : base(game)
        {
            TextureSize = textureSize;
            _timeStep = animationLength / TextureSize.Y;
            DamageType.Add("Generic", new DamageStruct());
            Status.Invunerable = true;

            if (facing.HasValue)
                Facing = (Face)facing;
            if (scale.HasValue)
                Scale = (float)scale;
            if (position.HasValue)
                Position = (Vector2)position;

            if (footprintGeometry.HasValue)
                FootprintGeometry = (Rectangle)footprintGeometry;
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
        /// Loads Content.
        /// </summary>
        /// <param name="texture"> The <see cref="Texture"/> object of the sprite.</param>
        /// <param name="assetName">The path\name of the asset file to load.</param>
        public void LoadContent(Texture2D texture, string assetName = "Generated_In_Memory")
        {
            Texture = texture;
            AssetName = assetName;
            updateSizeAndFootprint(); // Now that texture is loaded perform Scale transformations.
        }

        public new void UnloadContent()
        {
            Texture?.Dispose();
            base.UnloadContent();
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
                _currentTime = 0.0F;

                if (_currentFrame == Columns)
                {
                    _currentFrame = 0;
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
            switch (CurrentState)
            {
                case State.Dead:
                    break;
                default:
                    int row = 0;
                    int column = _currentFrame % Columns;
                    SpriteEffects translation = SpriteEffects.None;

                    if (Rows > 1)
                    {
                        row = (int)Facing;

                        if (Rows == 3 && Facing == Face.Left)
                        {
                            row = (int)Face.Right;
                            translation = SpriteEffects.FlipHorizontally;
                        }
                    }

                    Rectangle assetRectangle = new Rectangle((Texture.Width / Columns) * column, (Texture.Height / Rows) * row, (Texture.Width / Columns), (Texture.Height / Rows));

                    if (Config.Debug)
                    {
                        spriteBatch.Draw(DummyTexture, FootprintPosition, Footprint, color, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, depth > 0 ? depth.NextBefore() : 0.0F);
                    }

                    spriteBatch.Draw(Texture, Position, assetRectangle, Color.White, 0.0F, Vector2.Zero, Scale, translation, depth > 0 ? depth : 0.0F.NextAfter());
                    break;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Scale transformations on Size and Footprint. Requires Texture to be loaded.
        /// </summary>
        private void updateSizeAndFootprint()
        {
            Geometry = new Rectangle(Geometry.X, Geometry.Y, (int)((Texture.Width / Columns) * Scale), (int)((Texture.Height / Rows) * Scale));
            if (FootprintGeometry == Rectangle.Empty)
            {
                FootprintGeometry = new Rectangle(Geometry.X, Geometry.Y, Geometry.Width, Geometry.Height);
            }
            else
            {
                FootprintGeometry = new Rectangle((int)(FootprintGeometry.X * Scale), (int)(FootprintGeometry.Y * Scale), 
                                                  (int)(Footprint.Width * Scale), (int)(Footprint.Height * Scale));
            }
        }
        #endregion

        #region Public Methods
        public bool TakeDamage(string damageType, float damageAmount)
        {
            bool hasDied = false;
            bool isAlive = (this.CurrentState != State.Dead);
            bool canDamage = (this.Status.Invunerable == false);
            
            if (isAlive && canDamage && DamageType.ContainsKey(damageType))
            {
                DamageStruct current;
                if (DamageType.TryGetValue(damageType, out current))
                {
                    float DamageTaken = damageAmount - current.Armour;
                    this.Health.Current -= (DamageTaken > 0) ? DamageTaken : 0;

                    if (this.Health.Current <= 0)
                    {
                        this.Health.Current = 0;
                        CurrentState = State.Dead;
                        hasDied = true;
                    }
                }
            }
            return hasDied;
        }
        #endregion
    }
}
