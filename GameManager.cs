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
            this.Snakes = new List<ClientSnake>();
            this._foodManager = new SnakeFood();
            this._particleManager = new ParticleManager(this);

            _networkClientManager = new NetworkClientManager();
            _networkClientManager.Connect();
            _networkClientManager.IncomingDataPackage += _networkClientManager_IncomingDataPackage;
        }

        private StandardPackageData incoming;
        private void _networkClientManager_IncomingDataPackage(object sender, PackageEventArgs e)
        {
            var incomingData = e.IncomingPackage;
            switch (incomingData.PeekByte())
            {
                //case (byte)PackageType.Snake:
                //    var ipAddress = incomingData.SenderEndpoint.Address;
                //    var snake = Snakes.FirstOrDefault(s => s.IpAddress == ipAddress);
                //    if (snake != null)
                //    {
                //        snake.BodyParts = SnakePartsPackage.Decrypt(incomingData);    
                //    }
                //    else
                //    {
                //        this.AddSnake(SnakePartsPackage.Decrypt(incomingData), Color.Red, ipAddress);
                //    }
                    
                //    break;

                //case (byte)PackageType.BaseParticle:
                //    _particleManager.Particles = BaseParticlePackage.Decrypt(incomingData);
                //    break;

                //case (byte)PackageType.FoodPackage:
                //    _foodManager.FoodList = FoodPackage.Decrypt(incomingData);
                //    break;
                case (byte)PackageType.StandardPackage:
                    incoming = StandardPackage.Decrypt(incomingData);
                    _foodManager.FoodList = incoming.SnakeFood;
                    _particleManager.Particles = incoming.Particles;
                    
                    var ipAddress = incomingData.SenderEndpoint.Address;
                    var snake = Snakes.FirstOrDefault(s => s.IpAddress == ipAddress);
                    if (snake != null)
                    {
                        snake.BodyParts = incoming.Snakes.Select(s => new SnakePart(s)).ToList();
                    }
                    else
                    {
                        this.AddSnake(incoming.Snakes.Select(s => new SnakePart(s)).ToList(), Color.Red, ipAddress);
                    }

                    break;
            }
        }

        public void AddSnake(List<SnakePart> bodyPart, Color color, IPAddress ipAddress)
        {
            var snake = new ClientSnake(bodyPart, color, ipAddress);
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
