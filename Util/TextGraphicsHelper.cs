﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    public static class TextGraphicsHelper
    {
        private static SpriteFont Font { get; set; }

        public static void DrawText(SpriteBatch spriteBatch, string text, string fontLocation, int x, int y, Color color)
        {
            Font = Font ?? ScreenManager.Instance.Content.Load<SpriteFont>(fontLocation);
            spriteBatch.DrawString(Font, text, new Vector2(x, y), color);
        }

        public static void DrawText(SpriteBatch spriteBatch, string text, string fontLocation, Vector2 position, Color color)
        {
            Font = Font ?? ScreenManager.Instance.Content.Load<SpriteFont>(fontLocation);
            spriteBatch.DrawString(Font, text, position, color);
        }

        public static void DrawText(SpriteBatch spriteBatch, string text, string fontLocation, Vector2 position, Color color, float scale)
        {
            Font = Font ?? ScreenManager.Instance.Content.Load<SpriteFont>(fontLocation);
            spriteBatch.DrawString(Font, text, position, color, 0, new Vector2(0,0), scale, SpriteEffects.None, 0);
        }
    }
}
