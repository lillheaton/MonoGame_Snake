using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Snake.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MonoGameTest_V1
{
    public class Snake
    {
        public IPEndPoint SenderEndpoint { get; set; }

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

        // if greater than 0, this represents the time until the snake respawns
        protected TimeSpan _deadCounter;

        public bool HasMoved { get; private set; }
        public bool Dead { get { return this._deadCounter.Ticks > 0; } }

        public Snake(Vector2 position, SnakeDirection direction, IPEndPoint senderEndpoint)
        {
            int partCount = 4;
            this.Init(position, partCount, direction);

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
                //UpdateInput();

                lastUpdateTime += gameTime.ElapsedGameTime;
                if (lastUpdateTime > updatesPerMilliseconds)
                {
                    lastUpdateTime -= updatesPerMilliseconds;
                    UpdatePosition();
                }
            }
        }

        public void UpdateInput(Direction direction)
        {
            if (direction == Direction.West)
            {
                TrySetNextMove(SnakeDirection.West);
            }

            if (direction == Direction.East)
            {
                TrySetNextMove(SnakeDirection.East);
            }

            if (direction == Direction.South)
            {
                TrySetNextMove(SnakeDirection.South);
            }

            if (direction == Direction.North)
            {
                TrySetNextMove(SnakeDirection.North);
            }
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
