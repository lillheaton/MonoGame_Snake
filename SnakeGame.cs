#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Client.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

#endregion

namespace Client
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SnakeGame : Game
    {
        /**
         * Hehe, sneaky todo-list in SnakieSnakieGame
         * 
         * -    Something, something fonts...
         * -    Centralize collision checks in manager
         *          Makes transition to server handling easier
         * -    Server + multiple players
         * -    Cool particle effects ( we talkin' bad-ass cool stuffz )
         * -    Figure out some smooth graphical style
         * -    Other graphical effects
         *          Post processing
         *              Glow
         *          Hit effects
         *  -   Add power-ups and abilitys
         * 
         */


        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch { get { return this.spriteBatch; } }
        private GameManager _gameManager;
        private NetworkClientManager NetworkClientManager { get; set; }

        public SnakeGame() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            NetworkClientManager = new NetworkClientManager();
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
            this._gameManager = new GameManager(this);
            NetworkClientManager.Connect();

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

            InputHelper.SendKeyBoardInput(NetworkClientManager);

            //this._gameManager.Update(gameTime);
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

            this._gameManager.Draw(gameTime);


            if (!GameManager.SnakeAlive)
            {
                //TextGraphicsHelper.DrawText(spriteBatch, "Game Over", "Fonts/MyFont", new Vector2(300, 200), Color.Black, 3);
            }

            Debugger.DrawWindowInformation(spriteBatch, graphics);

            this.spriteBatch.End();
            base.Draw(gameTime);   
        }
    }
}
