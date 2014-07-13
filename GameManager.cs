using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using MonoGameTest_V1.Util;
using MonoGameTest_V1.Particles;

namespace MonoGameTest_V1
{
    public class GameManager
    {
        public static bool SnakeAlive = true;

        // Game state, could be some enum for actual state
        private bool _running = false;
        public bool Running { get { return this._running; } }

        // Parent game
        private SnakeGame _game;


        // Snakes
        private List<Snake> _snakes;
        public List<Snake> Snakes { get { return _snakes; } }


        private TimeSpan _lastSnakeMoveTime;
        private TimeSpan _snakeMoveDelayMS = TimeSpan.FromMilliseconds(100.0);


        // Foods
        private SnakeFood _foodManager;

        // Power ups
        //private PowerUpManager _powerUpManager;

        // Particles
        private ParticleManager _particleManager;



        public GameManager(SnakeGame game)
        {
            this._game = game;

            // init stuff
            this.Init();
        }

        public void Init()
        {
            this._snakes = new List<Snake>();
            this._foodManager = new SnakeFood();
            this._particleManager = new ParticleManager(this);

            // Spoof out a snake
            this.AddSnake(new Vector2(10.0f, 0.0f), SnakeDirection.East, ColorUtil.RandomColor());
            this._foodManager.SpawnFood(_snakes);
        }

        public void AddSnake(Vector2 startPosition, SnakeDirection direction, Color color)
        {
            var snake = new Snake(startPosition, direction, color);
            this._snakes.Add(snake);
        }



        public void Update(GameTime gameTime)
        {
            // Update snakes
            foreach (Snake snake in _snakes)
            {
                snake.Update(gameTime);
            }

            // Update collisions
            this.CheckCollisions();

            // Update managers
            this._foodManager.Update(gameTime);

            // Update effects, particles, other states
            this._particleManager.Update(gameTime);
        }

        protected void CheckCollisions()
        {
            // used to determine how many new fruits to add
            int pickedFoodCount = 0;

            foreach (Snake snake in this._snakes)
            {
                if (!snake.HasMoved) continue;

                var snakePosition = snake.BodyParts[0].Position;

                // Check self collision
                if (snake.BodyParts.GetRange(1, snake.BodyParts.Count - 1).Any(part => part.Position == snakePosition))
                {
                    // self collision
                    snake.SetDead();
                }

                // Check against others
                foreach (Snake otherSnake in this._snakes)
                {
                    if (otherSnake == snake) continue;

                    if (otherSnake.BodyParts.Any(part => part.Position == snakePosition))
                    {
                        // hit other snake
                        snake.SetDead();
                    }
                }

                // Check map bounds
                if (snakePosition.X > (ScreenManager.Width / SnakePart.Size) || snakePosition.X < 0 ||
                    snakePosition.Y > (ScreenManager.Height / SnakePart.Size) || snakePosition.Y < 0)
                {
                    // outside map
                    snake.SetDead();
                }


                // Check food
                if (this._foodManager.TryPickFoodAtPosition(snakePosition))
                {
                    snake.AddPart();
                    ++pickedFoodCount;

                    ParticleUtil.ParticleExplosion(this._particleManager, snakePosition * SnakePart.Size, ColorUtil.RandomColor());
                }
            }

            while (pickedFoodCount-- > 0)
            {
                this._foodManager.SpawnFood(this._snakes);
            }
        }



        public void Draw(GameTime gameTime)
        {
            var spriteBatch = this._game.SpriteBatch;

            // Draw backgroud, environment

            // Draw snakes
            foreach (Snake snake in _snakes)
            {
                snake.Draw(spriteBatch);
            }

            // Draw food
            this._foodManager.Draw(spriteBatch);

            // Draw effects, particles
            this._particleManager.Draw(spriteBatch);
        }
    }
}
