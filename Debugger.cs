
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest_V1
{
    public static class Debugger
    {
        public static void DrawWindowInformation(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            var width = string.Format("PreferredBackBufferWidth: {0}", graphics.PreferredBackBufferWidth);
            var height = string.Format("PreferredBackBufferHeight: {0}", graphics.PreferredBackBufferHeight);

            TextGraphicsHelper.DrawText(spriteBatch, width, "Fonts/MyFont", new Vector2(0, 0), Color.Black);
            TextGraphicsHelper.DrawText(spriteBatch, height, "Fonts/MyFont", new Vector2(0, 20), Color.Black);
        }
    }
}
