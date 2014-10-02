
using System.Threading;
using Microsoft.Xna.Framework;
using System;

namespace Server
{
    public class SnakeServer
    {
        public static GameManager GameManager { get; private set; }

        static SnakeServer()
        {
            GameManager = new GameManager();
        }

        static void Main(string[] args)
        {
            var gameTime = new GameTime();
            var lastupdate = DateTime.Now;

            // Main loop
            while (true)
            {
                // Calculate elapsedGameTime
                gameTime.ElapsedGameTime = DateTime.Now - lastupdate;

                // Update game
                GameManager.Update(gameTime);
                
                // Set latest update to now
                lastupdate = DateTime.Now;

                Thread.Sleep(30);
            }
        }
    }
}
