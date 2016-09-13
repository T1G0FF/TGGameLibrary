#region File Description
//-----------------------------------------------------------------------------
// FlatRectangle.cs
//
// Written by Thomas
// Last Updated: 2016-09-13
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TGGameLibrary
{
    /// <summary>
    /// 1px square texture that can be scaled to desired size and tinted the desired color at draw time.
    /// </summary>
    public class FlatRectangle : DrawableGameComponent
    {
        #region Properties
        public Texture2D DummyTexture;
        #endregion

        #region Intialisation
        public FlatRectangle(Game game)
            : base(game)
        {
            this.LoadContent();
        }

        ~FlatRectangle()
        {
            UnloadContent();
        }
        #endregion

        #region MonoGame Default Methods
        public new void LoadContent()
        {
            // Create a 1px square rectangle texture that will be scaled to the
            // desired size and tinted the desired color at draw time
            DummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            DummyTexture.SetData(new[] { Color.White });
            base.LoadContent();
        }

        public new void UnloadContent()
        {
            // If you are creating your texture (instead of loading it with
            // Content.Load) then you must Dispose of it
            DummyTexture.Dispose();
            base.UnloadContent();
        }
        #endregion
    }
}
