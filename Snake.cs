using System;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTest_V1
{
    public class Snake
    {
        /// <summary>
        /// The size of the rectangle for the graphics
        /// </summary>
        public const int SnakeBodySize = 20;

        // change to cool enum state stuffz 
        protected bool _alive;

        private List<SnakePart> BodyParts { get; set; }

        private SnakeDirection NextMoveDirection { get; set; }
        private SnakeDirection CurrentMoveDirection { get; set; }

        private TimeSpan lastUpdateTime;
        private readonly TimeSpan updatesPerMilliseconds;

        private KeyboardState oldState;

        protected Color _color;

        public Snake(Vector2 position, SnakeDirection direction, Color color)
        {
            int partCount = 4;
            this.Init(position, partCount, direction);

            this._color = color;
            updatesPerMilliseconds = TimeSpan.FromMilliseconds(100);
        }

        public void Init(Vector2 startPosition, int partCount, SnakeDirection startDirection)
        {
            this._alive = true;

            BodyParts = new List<SnakePart>();
            for (int i = 0; i < 10; i++)
            {
                BodyParts.Add(new SnakePart(startPosition - new Vector2(i, 0)));
            }

            NextMoveDirection = CurrentMoveDirection = SnakeDirection.East;
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
            if (newState.IsKeyDown(Keys.Left) || gamePad.ThumbSticks.Left.X < -epsilon)
            {
                TrySetNextMove(SnakeDirection.West);
            }

            if (newState.IsKeyDown(Keys.Right) || gamePad.ThumbSticks.Left.X > epsilon)
            {
                TrySetNextMove(SnakeDirection.East);
            }

            if (newState.IsKeyDown(Keys.Down) || gamePad.ThumbSticks.Left.Y < -epsilon)
            {
                TrySetNextMove(SnakeDirection.South);
            }

            if (newState.IsKeyDown(Keys.Up) || gamePad.ThumbSticks.Left.Y > epsilon)
            {
                TrySetNextMove(SnakeDirection.North);
            }

            oldState = newState;
        }
        public void TrySetNextMove(SnakeDirection direction)
        {
            bool isOpposite = direction.isOppositeOf(CurrentMoveDirection);
            if (!isOpposite)
            {
                NextMoveDirection = direction;
            }
        }

        private void UpdatePosition()
        {
            // Calculate next position
            var newPosition = BodyParts[0].Position + NextMoveDirection.GetVector();

            this.HandleCollision(newPosition);

            for (int i = BodyParts.Count - 1; i > 1; i--)
            {
                BodyParts[i].Position = BodyParts[i - 1].Position;
            }
            BodyParts.RemoveAt(BodyParts.Count - 1);

            CurrentMoveDirection = NextMoveDirection;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            int margin = 1;
            var rect = new Rectangle();
            rect.Width = SnakeBodySize - (margin * 2);
            rect.Height = SnakeBodySize - (margin * 2);

            foreach (var snakePart in BodyParts)
            {
                rect.X = (int)snakePart.Position.X * SnakeBodySize + margin;
                rect.Y = (int)snakePart.Position.Y * SnakeBodySize + margin;

                RectangleGraphicsHelper.DrawRectangle(spriteBatch, rect, _color);
            }
        }

        

        private void HandleCollision(Vector2 newPosition)
        {
            // Snake should not colide with itself
            if (BodyParts.Any(s => s.Position == newPosition))
            {
                GameManager.SnakeAlive = false;
                return;
            }

            // Snake should not go outside of screen
            if (newPosition.X > (ScreenManager.Width / SnakeBodySize) || newPosition.X < 0
                || newPosition.Y > (ScreenManager.Height / SnakeBodySize) || newPosition.Y < 0)
            {
                GameManager.SnakeAlive = false;
                return;
            }

            // Check if next position contains food
            if (SnakeFood.FoodList.Contains(newPosition))
            {
                SnakeFood.FoodList.Remove(newPosition);
                BodyParts.Insert(0, new SnakePart(newPosition));
                BodyParts.Insert(0, new SnakePart(newPosition + NextMoveDirection.GetVector()));
            }
            else
            {
                BodyParts.Insert(0, new SnakePart(newPosition));
            }
        }
    }
}
