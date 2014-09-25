using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Threading;

using Definitions;
using Definitions.EventArguments;
using Definitions.NetworkPackages;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Server
{
    public class GameManager
    {
        public static bool SnakeAlive = true;

        // Snakes
        private List<Snake> _snakes;

        // Foods
        private SnakeFood _foodManager;

        // Power ups
        //private PowerUpManager _powerUpManager;

        // Center of particle explosions
        private List<Vector2> _explosions; 

        // Network manager
        private NetworkServerManager _server;

        private Thread _sendPackageThread;

        public GameManager()
        {
            this.Init();
        }

        public void Init()
        {
            this._server = new NetworkServerManager();
            this._snakes = new List<Snake>();
            this._foodManager = new SnakeFood();
            this._explosions = new List<Vector2>();

            _server.Connect();
            _server.NewConnection += _server_NewConnection;
            _server.IncomingDataPackage += _server_IncomingDataPackage;

            this._sendPackageThread = new Thread(SendPackages);
            this._sendPackageThread.Start();
        }

        private void _server_NewConnection(object sender, ConnectionEventArgs e)
        {
            // New player wants to join the game
            var connection = e.NetConnection;
            this.AddSnake(new Vector2(10.0f, 0.0f), SnakeDirection.East, connection);
            this._foodManager.SpawnFood(_snakes);
        }

        private void _server_IncomingDataPackage(object sender, PackageEventArgs e)
        {
            var incomingPackage = e.IncomingPackage;
            if (incomingPackage != null)
            {
                var snake = _snakes.FirstOrDefault(s => s.Connection == incomingPackage.SenderConnection);
                var packageType = (PackageType)incomingPackage.ReadByte();

                switch (packageType)
                {
                    case PackageType.KeyboardInput:
                        snake.UpdateInput((Direction)incomingPackage.ReadByte());
                        break;
                }
            }
        }

        private void SendPackages()
        {
            while (true)
            {
                foreach (var snake in _snakes)
                {
                    foreach (var otherSnakes in _snakes)
                    {
                        var snakePackage = new SnakePartsPackage(snake);
                        _server.Send(otherSnakes, snakePackage);

                        if (this._explosions.Any())
                        {
                            var particlesPackage = new ExplosionsPackage(this._explosions);
                            _server.Send(snake, particlesPackage);
                            this._explosions.Clear();
                        }

                        if (_foodManager.FoodList.Any())
                        {
                            var foodPackage = new FoodPackage(_foodManager.FoodList);
                            _server.Send(otherSnakes, foodPackage);
                        }
                    }
                }

                Thread.Sleep(30);
            }
        }

        public void AddSnake(Vector2 startPosition, SnakeDirection direction, NetConnection connection)
        {
            var snake = new Snake(startPosition, direction, connection);
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

                    this._explosions.Add(snakePosition * SnakePart.Size);
                }
            }

            while (pickedFoodCount-- > 0)
            {
                this._foodManager.SpawnFood(this._snakes);
            }
        }
    }
}
