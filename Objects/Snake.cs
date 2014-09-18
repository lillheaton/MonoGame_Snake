using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client.Objects
{
    public class Snake
    {
        /// <summary>
        /// The size of the rectangle for the graphics
        /// </summary>
        public const int SnakeBodySize = 20;

        public const float RespawnDelayMS = 1000.0f;

        // change to cool enum state stuffz 
        protected bool _alive;

        private List<SnakePart> _bodyParts;
        public List<SnakePart> BodyParts { get { return _bodyParts; } }

        private SnakeDirection NextMoveDirection { get; set; }
        private SnakeDirection CurrentMoveDirection { get; set; }

        private TimeSpan lastUpdateTime;
        private readonly TimeSpan updatesPerMilliseconds;

        private KeyboardState oldState;

        protected Color _color;

        // if greater than 0, this represents the time until the snake respawns
        protected TimeSpan _deadCounter;

        public bool HasMoved { get; private set; }
        public bool Dead { get { return this._deadCounter.Ticks > 0; } }

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

            _bodyParts = new List<SnakePart>();
            for (int i = 0; i < partCount; i++)
            {
                _bodyParts.Add(new SnakePart(startPosition - new Vector2(i, 0)));
            }

            NextMoveDirection = CurrentMoveDirection = SnakeDirection.East;
        }

        

        public void Update(GameTime gameTime)
        {
            this.HasMoved = false;

            // check if snake is in dead state
            if (this.Dead)
            {
                this._deadCounter -= gameTime.ElapsedGameTime;

                if (this._deadCounter.Ticks <= 0)
                {
                    this.Init(new Vector2(5, 0), 4, SnakeDirection.South);
                }
            }
            // else update snake
            else
            {
                UpdateInput();

                lastUpdateTime += gameTime.ElapsedGameTime;
                if (lastUpdateTime > updatesPerMilliseconds)
                {
                    lastUpdateTime -= updatesPerMilliseconds;
                    UpdatePosition();
                }
            }
        }

        public void UpdateInput()
        {
            KeyboardState newState = Keyboard.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            float epsilon = 0.1f;
            if (newState.IsKeyDown(Keys.Left) || gamePad.ThumbSticks.Left.X < -epsilon)
            {
                Console.WriteLine("lkdsfnsd");
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
            for (int i = _bodyParts.Count - 1; i > 0; i--)
            {
                _bodyParts[i].Position = _bodyParts[i - 1].Position;
            }
            _bodyParts[0].Position += NextMoveDirection.GetVector();

            CurrentMoveDirection = NextMoveDirection;
            this.HasMoved = true;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            int margin = 1;
            var color = this.Dead ? Color.Lerp(_color, Color.Red, (float)Math.Cos(this._deadCounter.Ticks * 0.01)) : _color;
            var rect = new Rectangle();
            rect.Width = SnakeBodySize - (margin * 2);
            rect.Height = SnakeBodySize - (margin * 2);

            foreach (var snakePart in _bodyParts)
            {
                rect.X = (int)snakePart.Position.X * SnakeBodySize + margin;
                rect.Y = (int)snakePart.Position.Y * SnakeBodySize + margin;   

                RectangleGraphicsHelper.DrawRectangle(spriteBatch, rect, color);
            }
        }

        



        public void SetDead()
        {
            this._deadCounter = TimeSpan.FromMilliseconds(Snake.RespawnDelayMS);
        }

        public void AddPart()
        {
            _bodyParts.Add(new SnakePart(_bodyParts.Last<SnakePart>().Position));
        }
    }
}
