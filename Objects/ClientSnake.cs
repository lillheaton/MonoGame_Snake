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

        public ClientSnake(List<Vector2> bodyParts, Color color, IPAddress ipAddress) : base(new Vector2(10.0f, 0.0f), SnakeDirection.East)
        {
            _color = color;
            IpAddress = ipAddress;
        }

        public void HandleTimeFrames(List<TimeFrame> timeFrames)
        {
            if (this.TimeFrames.FirstOrDefault() == null)
            {
                return;
            }

            foreach (var timeFrame in timeFrames)
            {
                var clientTimeFrame = this.TimeFrames.FirstOrDefault(s => s.Id == timeFrame.Id);
                if (clientTimeFrame != null)
                {
                    clientTimeFrame.Approved = true;
                }
            }
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
