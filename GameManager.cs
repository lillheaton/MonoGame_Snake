using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGameTest_V1
{
    public static class GameManager
    {
        public static bool SnakeAlive = true;

        // Game state, could be some enum for actual state
        private bool _running = false;
        public bool Running { get { return this._running; } }

        // Parent game
        private SnakeGame _game;


        // Snakes
        private List<Snake> _snakes;

        // Foods
        private SnakeFood _foodManager;

        // Power ups
        //private PowerUpManager _powerUpManager;

        // Particles
        //private ParticleManager _particleManager;



        public GameManager(SnakeGame game)
        {
            this._game = game;

            // init stuff
            this.Init();
        }

        public void Init()
        {
            this._snakes = new List<Snake>();

            this._foodManager.Init();


            // Spoof out a snake
            this.AddSnake(new Vector2(10.0f, 0.0f), SnakeDirection.East, ColorUtil.RandomColor());
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

            // Update managers

            // Update effects, particles, other states
        }

        public void CheckCollisions()
        {
            // Check snake collisions

            // Check map collisions

            // Check food collisions
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
            _foodManager.Draw(spriteBatch);

            // Draw effects, particles
        }
    }
}
