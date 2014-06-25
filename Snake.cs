using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTest_V1
{
    public class Snake
    {
        private SpriteBatch SpriteBatch { get; set; }
        private List<SnakePart> BodyParts { get; set; }

        protected SnakeDirection NextMoveDirection { get; set; }
        protected SnakeDirection CurrentMoveDirection { get; set; }


        protected TimeSpan lastUpdateTime;
        protected TimeSpan updatesPerMilliseconds = TimeSpan.FromMilliseconds(100);

        private KeyboardState oldState;


        public Snake(SpriteBatch spriteBatch)
        {
            this.SpriteBatch = spriteBatch;

            BodyParts = new List<SnakePart>();
            for (int i = 0; i < 10; i++)
            {
                BodyParts.Add(new SnakePart(new Vector2(i, 0)));
            }
            BodyParts.Reverse();

            NextMoveDirection = CurrentMoveDirection = SnakeDirection.East;
        }

        public void Move(SnakeDirection direction)
        {
            bool isOpposite = direction.isOppositeOf(CurrentMoveDirection);
            if (!isOpposite)
            {
                NextMoveDirection = direction;
            }
        }

        public void Update(GameTime gameTime)
        {
            UpdateInput();

            lastUpdateTime += gameTime.ElapsedGameTime;
            if (lastUpdateTime > updatesPerMilliseconds)
            {
                lastUpdateTime -= updatesPerMilliseconds;
                UpdatePosition();
            }
        }

        public void UpdateInput()
        {
            KeyboardState newState = Keyboard.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            float epsilon = 0.1f;
            if (oldState.IsKeyUp(Keys.Left) && newState.IsKeyDown(Keys.Left) || gamePad.ThumbSticks.Left.X < -epsilon)
            {
                Move(SnakeDirection.West);
            }

            if (oldState.IsKeyUp(Keys.Right) && newState.IsKeyDown(Keys.Right) || gamePad.ThumbSticks.Left.X > epsilon)
            {
                Move(SnakeDirection.East);
            }

            if (oldState.IsKeyUp(Keys.Down) && newState.IsKeyDown(Keys.Down) || gamePad.ThumbSticks.Left.Y < -epsilon)
            {
                Move(SnakeDirection.South);
            }

            if (oldState.IsKeyUp(Keys.Up) && newState.IsKeyDown(Keys.Up) || gamePad.ThumbSticks.Left.Y > epsilon)
            {
                Move(SnakeDirection.North);
            }

            oldState = newState;
        }

        public void UpdatePosition()
        {
            BodyParts[0].Position = BodyParts[0].Position + NextMoveDirection.GetVector();

            for (int i = BodyParts.Count - 1; i > 0; i--)
            {
                BodyParts[i].Position = BodyParts[i - 1].Position;
            }

            CurrentMoveDirection = NextMoveDirection;
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

                Color snakeColor = Color.FromNonPremultiplied(255,65,54, 255);
                GraphicsHelper.DrawRectangle(SpriteBatch, rect, snakeColor);
            }
        }
    }
}
