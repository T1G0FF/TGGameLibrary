using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGameLibrary
{
    /// <summary>
    /// 1px square texture that can be scaled to desired size and tinted the desired color at draw time.
    /// </summary>
    public class FlatRectangle : DrawableGameComponent
    {
        public Texture2D DummyTexture;

        public FlatRectangle(Game game)
            : base(game)
        {
            LoadContent();
        }

        ~FlatRectangle()
        {
            UnloadContent();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            // Create a 1px square rectangle texture that will be scaled to the
            // desired size and tinted the desired color at draw time
            DummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            DummyTexture.SetData(new[] { Color.White });
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            // If you are creating your texture (instead of loading it with
            // Content.Load) then you must Dispose of it
            DummyTexture.Dispose();
        }
    }
}
