using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameTest_V1
{
    public class InputHelper
    {
        private static KeyboardState oldState;

        public static void SendKeyBoardInput(NetworkClientManager networkClientManager)
        {
            KeyboardState newState = Keyboard.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            float epsilon = 0.1f;
            if (oldState.IsKeyUp(Keys.Left) && newState.IsKeyDown(Keys.Left) || gamePad.ThumbSticks.Left.X < -epsilon)
            {
                //Move(SnakeDirection.West);
            }

            if (oldState.IsKeyUp(Keys.Right) && newState.IsKeyDown(Keys.Right) || gamePad.ThumbSticks.Left.X > epsilon)
            {
                //Move(SnakeDirection.East);
            }

            if (oldState.IsKeyUp(Keys.Down) && newState.IsKeyDown(Keys.Down) || gamePad.ThumbSticks.Left.Y < -epsilon)
            {
                //Move(SnakeDirection.South);
            }

            if (oldState.IsKeyUp(Keys.Up) && newState.IsKeyDown(Keys.Up) || gamePad.ThumbSticks.Left.Y > epsilon)
            {
                //Move(SnakeDirection.North);
            }

            oldState = newState;
        }
    }
}
