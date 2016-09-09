using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGGameLibrary.Enums;
using TGExtensions;

namespace TGGameLibrary
{
    public class Player : MoveableAnimatedSprite
    {
        // Asset information
        const string ASSETNAME = "Image\\Dude";
        const float ANIMATION_TIME = 0.5F;
        const int NO_OF_DIRECTIONS = 3;
        const int NO_OF_FRAMES = 8;
        const int FRAME_WIDTH = 20;
        const int FRAME_HEIGHT = 50;
        private static readonly Color[] PlayerColors = new[] {Color.DodgerBlue, Color.Red, Color.Green, Color.Yellow};
        static readonly Color ColorOne = new Color(0, 255, 0);
        static readonly Color ColorTwo = new Color(0, 204, 0); // 255 * 0.8F
        const float ColorTwoAlpha = 0.25F;


        // In-game Information
        const int DEFAULT_SPEED = 200;
        const float SCALE = 2.0F;

        private Color _color = Color.Black;
        public PlayerIndex Index;

        #region Constructor
        public Player(Game game, PlayerIndex playerIndex, Color? color = null, Vector2? startPosition = null, Rectangle? footprint = null, float? movementSpeed = DEFAULT_SPEED, Face? facing = Face.Down, float? scale = SCALE)
            : base(game, new Vector2(NO_OF_FRAMES, NO_OF_DIRECTIONS), ANIMATION_TIME, startPosition, footprint, movementSpeed, facing, scale)
        {
            Index = playerIndex;

            if (color.HasValue)
                _color = (Color)color;
            else
                _color = PlayerColors[(int) playerIndex];
            
            if (footprint.HasValue == false)
            {
                int footprintHeight = (int)(FRAME_HEIGHT / 3);
                FootprintGeometry = new Rectangle(0, FRAME_HEIGHT - footprintHeight, FRAME_WIDTH, footprintHeight);
            }
        }
        #endregion

        public void LoadContent(ContentManager contentManager)
        {
            Texture2D orig = contentManager.Load<Texture2D>(ASSETNAME);
            Texture2D copy = new Texture2D(orig.GraphicsDevice, orig.Width, orig.Height);

            int textureLength = orig.Width * orig.Height;
            Color[] data = new Color[textureLength];

            orig.GetData(data);
            for (int x = 0; x < textureLength; x++)
            {
                if (data[x] == ColorOne)
                    data[x] = _color;
                else if (data[x] == ColorTwo)
                    data[x] = Color.Lerp(_color, Color.Black, ColorTwoAlpha);
            }
            copy.SetData(data);

            base.LoadContent(copy, ASSETNAME);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float adjustedDepth = Depth;
            for (int i = 0; i < (int)Index; i++)
            {
                adjustedDepth.NextBefore();
            }
            base.Draw(spriteBatch, _color, adjustedDepth);
        }
    }
}
