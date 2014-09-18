
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    public static class RectangleGraphicsHelper
    {
        private static Texture2D Texture { get; set; }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            Texture = Texture ?? new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Texture.SetData(new[] { new Color(Color.White, 255) });
            spriteBatch.Draw(Texture, rectangle, color);
        }


        public static void DrawFilledRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int alpha)
        {
            Texture = Texture ?? new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Texture.SetData(new[] { new Color(Color.White, alpha) });
            spriteBatch.Draw(Texture, rectangle, color);
        }
    }
}
