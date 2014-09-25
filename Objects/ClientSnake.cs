using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Net;

namespace Client.Objects
{
    public class ClientSnake
    {
        public IPAddress IpAddress { get; set; }
        public List<Vector2> BodyParts { get; set; }
        public const int SnakeBodySize = 20;

        private readonly Color color;

        public ClientSnake(List<Vector2> bodyParts, Color color, IPAddress ipAddress)
        {
            this.color = color;
            this.IpAddress = ipAddress;
            BodyParts = bodyParts;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int margin = 1;
            var rect = new Rectangle();
            rect.Width = SnakeBodySize - (margin * 2);
            rect.Height = SnakeBodySize - (margin * 2);

            foreach (var snakePart in BodyParts)
            {
                rect.X = (int)snakePart.X * SnakeBodySize + margin;
                rect.Y = (int)snakePart.Y * SnakeBodySize + margin;   

                RectangleGraphicsHelper.DrawRectangle(spriteBatch, rect, color);
            }
        }
    }
}
