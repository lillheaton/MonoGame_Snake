using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest_V1
{
    public static class TextGraphicsHelper
    {
        private static SpriteFont Font { get; set; }

        public static void DrawText(SpriteBatch spriteBatch, string text, string fontLoation, int x, int y, Color color)
        {
            Font = Font ?? ScreenManager.Instance.Content.Load<SpriteFont>(fontLoation);
            spriteBatch.DrawString(Font, text, new Vector2(x, y), color);
        }

        public static void DrawText(SpriteBatch spriteBatch, string text, string fontLoation, Vector2 position, Color color)
        {
            Font = Font ?? ScreenManager.Instance.Content.Load<SpriteFont>(fontLoation);
            spriteBatch.DrawString(Font, text, position, color);
        }
    }
}
