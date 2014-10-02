using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Client.Network;
using Client.Objects;
using Client.Particles;
using Client.Util;
using Definitions;
using Definitions.EventArguments;
using Definitions.NetworkObjects;
using Definitions.NetworkPackages;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    public class GameManager
    {
        public static bool SnakeAlive = true;

        private KeyboardState oldState;

        // Game state, could be some enum for actual state
        private bool _running = false;
        public bool Running { get { return this._running; } }

        // Parent game
        private SnakeGame _game;


        // Snakes
        public List<ClientSnake> Snakes { get; private set; }

        // Foods
        private SnakeFood _foodManager;

        private List<Vector2> _explosionPosition; 

        // Power ups
        //private PowerUpManager _powerUpManager;

        // Particles
        private ParticleManager _particleManager;

        // Network
        private NetworkClientManager _networkClientManager;

        private TimeSpan _lastSnakeMoveTime;
        private TimeSpan _snakeMoveDelayMS = TimeSpan.FromMilliseconds(100.0);

        public GameManager(SnakeGame game)
        {
            this._game = game;

            // init stuff
            this.Init();
        }

        public void Init()
        {
            this.Snakes = new List<ClientSnake>();
            this._foodManager = new SnakeFood();
            this._particleManager = new ParticleManager();
            this._explosionPosition = new List<Vector2>();

            _networkClientManager = new NetworkClientManager();
            _networkClientManager.Connect();
            _networkClientManager.IncomingDataPackage += _networkClientManager_IncomingDataPackage;
        }

        private void _networkClientManager_IncomingDataPackage(object sender, PackageEventArgs e)
        {
            var incomingData = e.IncomingPackage;
            switch (incomingData.PeekByte())
            {
                case (byte)PackageType.Snake:
                    var ipAddress = incomingData.SenderEndpoint.Address;
                    var snake = Snakes.FirstOrDefault(s => s.IpAddress == ipAddress);
                    if (snake != null)
                    {

                        var snakeData = SnakePartsPackage.Decrypt(incomingData);
                        snake.BodyParts = snakeData.SnakeParts;
                    }
                    else
                    {
                        this.AddSnake(SnakePartsPackage.Decrypt(incomingData), Color.Red, ipAddress);
                    }

                    break;

                case (byte)PackageType.FoodPackage:
                    _foodManager.FoodList = FoodPackage.Decrypt(incomingData);
                    break;

                case (byte)PackageType.Particle:
                    _explosionPosition = ExplosionsPackage.Decrypt(incomingData);
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            this.SendKeyBoardInput();

            foreach (var clientSnake in Snakes)
            {
                clientSnake.Update(gameTime);
            }

            if (_explosionPosition.Any())
            {
                foreach (var position in _explosionPosition)
                {
                    ParticleUtil.ParticleExplosion(_particleManager, position, ColorUtil.RandomColor());
                }

                _explosionPosition.Clear();
            }

            //foreach (var snake in Snakes)
            //{
            //    var snakePosition = snake.BodyParts.First();

            //    if (_foodManager.TryPickFoodAtPosition(snakePosition))
            //    {
            //        ParticleUtil.ParticleExplosion(_particleManager, snakePosition, ColorUtil.RandomColor());
            //    }
            //}

            // Update effects, particles, other states
            this._particleManager.Update(gameTime);
        }

        public void AddSnake(SnakeData snakeData, Color color, IPAddress ipAddress)
        {
            var snake = new ClientSnake(snakeData, color, ipAddress);
            this.Snakes.Add(snake);
        }

        public void SendKeyBoardInput()
        {
            KeyboardState newState = Keyboard.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            float epsilon = 0.1f;
            if ((oldState.IsKeyUp(Keys.Left) && newState.IsKeyDown(Keys.Left)) || gamePad.ThumbSticks.Left.X < -epsilon)
            {
                _networkClientManager.Send(new InputPackage(Direction.West));
            }

            if ((oldState.IsKeyUp(Keys.Right) && newState.IsKeyDown(Keys.Right)) || gamePad.ThumbSticks.Left.X > epsilon)
            {
                _networkClientManager.Send(new InputPackage(Direction.East));
            }

            if ((oldState.IsKeyUp(Keys.Down) && newState.IsKeyDown(Keys.Down)) || gamePad.ThumbSticks.Left.Y < -epsilon)
            {
                _networkClientManager.Send(new InputPackage(Direction.South));
            }

            if ((oldState.IsKeyUp(Keys.Up) && newState.IsKeyDown(Keys.Up)) || gamePad.ThumbSticks.Left.Y > epsilon)
            {
                _networkClientManager.Send(new InputPackage(Direction.North));
            }

            oldState = newState;
        }

        public void Draw(GameTime gameTime)
        {
            var spriteBatch = this._game.SpriteBatch;

            // Draw backgroud, environment

            // Draw snakes
            foreach (ClientSnake snake in Snakes)
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
