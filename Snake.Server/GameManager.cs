﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using Definitions;
using Definitions.Particles;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Server
{
    public class GameManager
    {
        public static bool SnakeAlive = true;

        // Game state, could be some enum for actual state
        private bool _running = false;
        public bool Running { get { return this._running; } }

        // Snakes
        private List<Snake> _snakes;
        public List<Definitions.Snake> Snakes { get { return _snakes; } }

        // Foods
        private SnakeFood _foodManager;

        // Power ups
        //private PowerUpManager _powerUpManager;

        // Particles
        private ParticleManager _particleManager;

        private NetworkServerManager _server;

        public GameManager()
        {
            // init stuff
            this.Init();
        }

        public void Init()
        {
            this._server = new NetworkServerManager();
            this._snakes = new List<Definitions.Snake>();
            this._foodManager = new SnakeFood();
            this._particleManager = new ParticleManager();

            // Spoof out a snake
            //this.AddSnake(new Vector2(10.0f, 0.0f), SnakeDirection.East, );
            this._foodManager.SpawnFood(_snakes);

            _server.Connect();
            _server.NewConnection += _server_NewConnection;
            _server.IncomingDataPackage += _server_IncomingDataPackage;
        }

        private void _server_NewConnection(object sender, EventArgs e)
        {
            // New player wants to join the game
            var connection = sender as NetConnection;
            this.AddSnake(new Vector2(10.0f, 0.0f), SnakeDirection.East, connection);
        }

        private void _server_IncomingDataPackage(object sender, EventArgs e)
        {
            var incomingPackage = sender as NetIncomingMessage;
            if (incomingPackage != null)
            {
                var snake = Snakes.FirstOrDefault(s => s.Connection == incomingPackage.SenderConnection);
                var packageType = (PackageType)incomingPackage.ReadByte();

                switch (packageType)
                {
                    case PackageType.KeyboardInput:
                        snake.UpdateInput((Direction)incomingPackage.ReadByte());
                        break;
                }    
            }
        }

        public void AddSnake(Vector2 startPosition, SnakeDirection direction, NetConnection connection)
        {
            var snake = new Definitions.Snake(startPosition, direction, connection);
            this._snakes.Add(snake);
        }



        public void Update(GameTime gameTime)
        {
            // Update snakes
            foreach (var snake in _snakes)
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

            foreach (Definitions.Snake snake in this._snakes)
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
                foreach (Definitions.Snake otherSnake in this._snakes)
                {
                    if (otherSnake == snake) continue;

                    if (otherSnake.BodyParts.Any(part => part.Position == snakePosition))
                    {
                        // hit other snake
                        snake.SetDead();
                    }
                }

                // Check map bounds
                if (snakePosition.X > (800 / SnakePart.Size) || snakePosition.X < 0 ||
                    snakePosition.Y > (480 / SnakePart.Size) || snakePosition.Y < 0)
                {
                    // outside map
                    snake.SetDead();
                }


                // Check food
                if (this._foodManager.TryPickFoodAtPosition(snakePosition))
                {
                    snake.AddPart();
                    ++pickedFoodCount;

                    //ParticleUtil.ParticleExplosion(this._particleManager, snakePosition * SnakePart.Size, ColorUtil.RandomColor());
                }
            }

            while (pickedFoodCount-- > 0)
            {
                this._foodManager.SpawnFood(this._snakes);
            }
        }
    }
}