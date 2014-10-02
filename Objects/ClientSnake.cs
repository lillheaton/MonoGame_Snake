using System;
using System.Linq;
using Definitions;
using Definitions.NetworkObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Net;

namespace Client.Objects
{
    public class ClientSnake : BaseSnake
    {
        public IPAddress IpAddress { get; set; }
        private readonly Color _color;

        public ClientSnake(SnakeData snakeData, Color color, IPAddress ipAddress) : base(snakeData.SnakeParts.First(), SnakeDirection.East)
        {
            _color = color;
            IpAddress = ipAddress;
            BodyParts = snakeData.SnakeParts;
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

                RectangleGraphicsHelper.DrawRectangle(spriteBatch, rect, _color);
            }
        }
    }
}
