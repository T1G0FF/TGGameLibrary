using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGameLibrary
{
    public class Player : MoveableAnimatedSprite
    {
        // Asset information
        const string ASSETNAME = "Image\\Dude";
        const float ANIMATION_TIME = 0.5F;
        const int NO_OF_DIRECTIONS = 3;
        const int NO_OF_FRAMES = 8;

        // In-game Information
        const int DEFAULT_SPEED = 200;
        const float SCALE = 2.0F;

        private Color _color = Color.Black;
        public PlayerIndex Index;

        #region Constructor
        public Player(Game game, PlayerIndex playerIndex, Color color, Vector2? startPosition = null, Rectangle? footprint = null, float? movementSpeed = DEFAULT_SPEED, Face? facing = Face.Down, float? scale = SCALE)
            : base(game, NO_OF_DIRECTIONS, NO_OF_FRAMES, ANIMATION_TIME, startPosition, footprint, movementSpeed, facing, scale)
        {
            Index = playerIndex;
            _color = color;

            if (!footprint.HasValue )
            { Footprint = new Rectangle(0, 0, Size.Width, (int)(Size.Height / 3)); }
        }
        #endregion
        public void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager, ASSETNAME);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float adjustedDepth = Depth;
            for (int i = 0; i < (int)Index; i++)
            { adjustedDepth.NextBefore(); }
            base.Draw(spriteBatch, _color, adjustedDepth);
        }
    }
}
