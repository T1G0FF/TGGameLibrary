using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGameLibrary
{
    public class Obstacle : Sprite
    {
        /// <summary>
        /// Stores it's own footprint because of stupid inheritance issues.
        /// </summary>
        public new Rectangle Footprint;

        public Obstacle(Game game, Rectangle footprint, int height, Face? facing = Face.Down, float? scale = 1.0F)
            : base(game, 1, null, footprint, facing, scale)
        {
            Footprint = footprint;
            Size = new Rectangle(footprint.X, footprint.Y - height, footprint.Width, footprint.Height + height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.Draw(spriteBatch, Color.Black, Depth);
        }

        public override void Draw(SpriteBatch spriteBatch, Color color)
        {
            this.Draw(spriteBatch, color, Depth);
        }

        public override void Draw(SpriteBatch spriteBatch, Color color, float depth)
        {
            spriteBatch.Draw(DummyTexture, null, Size, null, Vector2.Zero, 0.0F, null, color, SpriteEffects.None, depth > 0 ? depth : 0.0F.NextAfter());
        }
    }
}
