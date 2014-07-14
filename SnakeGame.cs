#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace MonoGameTest_V1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SnakeGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Snake snake;
        private SnakeFood snakeFood;
        private NetworkClientManager networkManager;

        public SnakeGame() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.networkManager = new NetworkClientManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferWidth = ScreenManager.Width;
            this.graphics.PreferredBackBufferHeight = ScreenManager.Height;
            this.networkManager.Connect();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenManager.Instance.LoadContentManager(Content);

            snakeFood = new SnakeFood(spriteBatch);
            snake = new Snake(spriteBatch, snakeFood);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            ScreenManager.Instance.Content.Unload();
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GameManager.SnakeAlive)
            {
                snake.Update(gameTime);
                snakeFood.Update(gameTime);
                networkManager.Listen();
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Color clearColor = Color.FromNonPremultiplied(221,221,221, 255);
            GraphicsDevice.Clear(clearColor);
            this.spriteBatch.Begin();

            snake.Draw();
            snakeFood.Draw();

            if (!GameManager.SnakeAlive)
            {
                TextGraphicsHelper.DrawText(spriteBatch, "Game Over", "Fonts/MyFont", new Vector2(300, 200), Color.Black, 3);
            }

            Debugger.DrawWindowInformation(spriteBatch, graphics);

            this.spriteBatch.End();
            base.Draw(gameTime);   
        }
    }
}
