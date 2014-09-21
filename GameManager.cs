using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Network;
using Client.Particles;
using Client.Util;
using Definitions;
using Definitions.NetworkPackages;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Snake = Client.Objects.Snake;
using SnakePart = Client.Objects.SnakePart;

namespace Client
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
        public List<Snake> Snakes { get; private set; }


        private TimeSpan _lastSnakeMoveTime;
        private TimeSpan _snakeMoveDelayMS = TimeSpan.FromMilliseconds(100.0);


        // Foods
        private SnakeFood _foodManager;

        // Power ups
        //private PowerUpManager _powerUpManager;

        // Particles
        private ParticleManager _particleManager;

        // Network
        private NetworkClientManager _networkClientManager;



        public GameManager(SnakeGame game)
        {
            this._game = game;

            // init stuff
            this.Init();
        }

        public void Init()
        {
            this.Snakes = new List<Snake>();
            this._foodManager = new SnakeFood();
            this._particleManager = new ParticleManager(this);
            _networkClientManager = new NetworkClientManager();

            _networkClientManager.IncomingDataPackage += _networkClientManager_IncomingDataPackage;
            // Spoof out a snake
            //this.AddSnake(new Vector2(10.0f, 0.0f), SnakeDirection.East, ColorUtil.RandomColor());
            this._foodManager.SpawnFood(Snakes);
        }

        void _networkClientManager_IncomingDataPackage(object sender, EventArgs e)
        {
            var incomingData = sender as NetIncomingMessage;
            switch (incomingData.PeekByte())
            {
                case (byte)PackageType.Snake:
                    var snakePackage = new SnakePackage(incomingData);
            }
        }

        public void AddSnake(Vector2 startPosition, SnakeDirection direction, Color color)
        {
            var snake = new Snake(startPosition, direction, color);
            this.Snakes.Add(snake);
        }



        public void Update(GameTime gameTime)
        {
            // Update snakes
            foreach (Snake snake in Snakes)
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

            foreach (Snake snake in this.Snakes)
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
                foreach (Snake otherSnake in this.Snakes)
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
                this._foodManager.SpawnFood(this.Snakes);
            }
        }



        public void Draw(GameTime gameTime)
        {
            var spriteBatch = this._game.SpriteBatch;

            // Draw backgroud, environment

            // Draw snakes
            foreach (Snake snake in Snakes)
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
