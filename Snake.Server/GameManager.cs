using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Threading;

using Definitions;
using Definitions.Constants;
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
        private List<Player> _players;

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
            this._players = new List<Player>();
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
            this._foodManager.SpawnFood(_players);
        }

        private void _server_IncomingDataPackage(object sender, PackageEventArgs e)
        {
            var incomingPackage = e.IncomingPackage;
            if (incomingPackage != null)
            {
                var snake = _players.FirstOrDefault(s => s.Connection == incomingPackage.SenderConnection);                
                switch ((PackageType)incomingPackage.PeekByte())
                {
                    case PackageType.KeyboardInput:
                        snake.HandleInputChange(InputPackage.Decrypt(incomingPackage));
                        break;
                }
            }
        }

        private void SendPackages()
        {
            while (true)
            {
                lock (_players)
                {
                    foreach (var snake in _players)
                    {
                        foreach (var otherSnakes in _players)
                        {
                            if (snake.TimeFrames.FirstOrDefault() != null)
                            {
                                var snakePackage = new SnakePartsPackage(snake);
                                _server.Send(otherSnakes, snakePackage);
                            }

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
                }

                Thread.Sleep(60);
            }
        }

        public void AddSnake(Vector2 startPosition, SnakeDirection direction, NetConnection connection)
        {
            var snake = new Player(startPosition, direction, connection);
            this._players.Add(snake);
        }

        public void Update(GameTime gameTime)
        {
            lock (_players)
            {
                // Update snakes
                foreach (var snake in _players)
                {
                    snake.Update(gameTime);
                }        
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

            foreach (Player snake in this._players)
            {
                if (!snake.HasMoved) continue;

                var snakePosition = snake.BodyParts[0];

                // Check self collision
                if (snake.BodyParts.GetRange(1, snake.BodyParts.Count - 1).Any(part => part == snakePosition))
                {
                    // self collision
                    snake.SetDead();
                }

                // Check against others
                foreach (Player otherSnake in this._players)
                {
                    if (otherSnake == snake) continue;

                    if (otherSnake.BodyParts.Any(part => part == snakePosition))
                    {
                        // hit other snake
                        snake.SetDead();
                    }
                }

                // Check map bounds
                if (snakePosition.X > (800 / Settings.SnakePartSize) || snakePosition.X < 0 ||
                    snakePosition.Y > (480 / Settings.SnakePartSize) || snakePosition.Y < 0)
                {
                    // outside map
                    snake.SetDead();
                }


                // Check food
                if (this._foodManager.TryPickFoodAtPosition(snakePosition))
                {
                    snake.AddPart();
                    ++pickedFoodCount;

                    this._explosions.Add(snakePosition * Settings.SnakePartSize);
                }
            }

            while (pickedFoodCount-- > 0)
            {
                this._foodManager.SpawnFood(this._players);
            }
        }
    }
}
