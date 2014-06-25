using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGameTest_V1
{
    public class Snake
    {
        private SpriteBatch SpriteBatch { get; set; }
        private List<SnakePart> BodyParts { get; set; }
        private Vector2 MoveVector { get; set; }

        public Snake(SpriteBatch spriteBatch)
        {
            this.SpriteBatch = spriteBatch;

            BodyParts = new List<SnakePart>();
            for (int i = 0; i < 10; i++)
            {
                BodyParts.Add(new SnakePart(new Vector2(i, 0)));
            }
            BodyParts.Reverse();

            MoveVector = new Vector2(1, 0);
        }

        public void Move(Direction direction)
        {
            var firstPart = BodyParts.First();            

            switch (direction)
            {
                case Direction.North:
                    if (firstPart.Direction != Direction.South)
                    {
                        firstPart.Direction = direction;
                        MoveVector = new Vector2(0, -1);
                    }
                    break;

                case Direction.South:
                    if (firstPart.Direction != Direction.North)
                    {
                        firstPart.Direction = direction;
                        MoveVector = new Vector2(0, 1);
                    }
                    break;

                case Direction.East:
                    if (firstPart.Direction != Direction.West)
                    {
                        firstPart.Direction = direction;
                        MoveVector = new Vector2(1, 0);
                    }
                    break;

                case Direction.West:
                    if (firstPart.Direction != Direction.East)
                    {
                        firstPart.Direction = direction;
                        MoveVector = new Vector2(-1, 0);
                    }
                    break;
            }
        }

        public void Update()
        {
            var oldList = BodyParts.Select(
                s =>
                    {
                        var part = new SnakePart(s.Position);
                        part.Direction = s.Direction;
                        return part;
                    }).ToList();

            BodyParts[0].Position = BodyParts[0].Position + MoveVector;

            for (int i = 1; i < BodyParts.Count; i++)
            {
                BodyParts[i].Position = oldList[i - 1].Position;
            }
        }

        public void Draw()
        {
            foreach (var snakePart in BodyParts)
            {
                var rect = new Rectangle();
                rect.Width = 20;
                rect.Height = 20;
                rect.X = (int)snakePart.Position.X * 20;
                rect.Y = (int)snakePart.Position.Y * 20;

                GraphicsHelper.DrawRectangle(SpriteBatch, rect, Color.Red);
            }
        }
    }
}
