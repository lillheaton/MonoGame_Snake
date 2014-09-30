using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Definitions
{
    public class BaseSnake
    {
        public const float RespawnDelayMS = 1000.0f;
        public const int SnakeBodySize = 20;
        public List<Vector2> BodyParts { get; set; }

        public DateTime UpdateTimeStamp { get; set; }
        public bool HasMoved { get; private set; }
        public bool Dead { get { return this.DeadCounter.Ticks > 0; } }

        // if greater than 0, this represents the time until the snake respawns
        protected TimeSpan DeadCounter;
        protected bool Alive;

        private SnakeDirection _nextMoveDirection;
        private SnakeDirection _currentMoveDirection;
        private TimeSpan _lastUpdateTime;
        private readonly TimeSpan _updatesPerMilliseconds;

        public BaseSnake()
        {
        }

        public BaseSnake(Vector2 position, SnakeDirection direction)
        {
            this.Alive = true;
            Init(position, direction);
            _updatesPerMilliseconds = TimeSpan.FromMilliseconds(100);
            UpdateTimeStamp = DateTime.Now;
        }

        protected void Init(Vector2 position, SnakeDirection direction)
        {
            BodyParts = new List<Vector2>();
            for (int i = 0; i < 4; i++)
            {
                BodyParts.Add(position - new Vector2(i, 0));
            }

            _nextMoveDirection = _currentMoveDirection = direction;
        }

        public void Update(GameTime gameTime)
        {
            HasMoved = false;

            // check if snake is in dead state
            if (Dead)
            {
                this.DeadCounter -= gameTime.ElapsedGameTime;

                if (DeadCounter.Ticks <= 0)
                {
                    Init(new Vector2(5, 0), SnakeDirection.South);
                }
            }
            // else update snake
            else
            {
                _lastUpdateTime += gameTime.ElapsedGameTime;
                if (_lastUpdateTime > _updatesPerMilliseconds)
                {
                    _lastUpdateTime -= _updatesPerMilliseconds;
                    UpdatePosition();
                    UpdateTimeStamp = DateTime.Now;
                }
            }
        }

        protected void UpdatePosition()
        {
            // Calculate next position
            for (int i = BodyParts.Count - 1; i > 0; i--)
            {
                BodyParts[i] = BodyParts[i - 1];
            }
            BodyParts[0] += _nextMoveDirection.GetVector();

            _currentMoveDirection = _nextMoveDirection;
            HasMoved = true;
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
            bool isOpposite = direction.isOppositeOf(_currentMoveDirection);
            if (!isOpposite)
            {
                _nextMoveDirection = direction;
            }
        }

        public void SetDead()
        {
            this.DeadCounter = TimeSpan.FromMilliseconds(RespawnDelayMS);
        }

        public void AddPart()
        {
            BodyParts.Add(BodyParts.Last());
        }
    }
}
