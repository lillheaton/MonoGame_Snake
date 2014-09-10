using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MonoGameTest_V1.Network;

using Snake.Definitions;
using Snake.Definitions.NetworkPackages;

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
            if ((oldState.IsKeyUp(Keys.Left) && newState.IsKeyDown(Keys.Left)) || gamePad.ThumbSticks.Left.X < -epsilon)
            {
                networkClientManager.Send(new InputPackage(Direction.West));
            }

            if ((oldState.IsKeyUp(Keys.Right) && newState.IsKeyDown(Keys.Right)) || gamePad.ThumbSticks.Left.X > epsilon)
            {
                networkClientManager.Send(new InputPackage(Direction.East));
            }

            if ((oldState.IsKeyUp(Keys.Down) && newState.IsKeyDown(Keys.Down)) || gamePad.ThumbSticks.Left.Y < -epsilon)
            {
                networkClientManager.Send(new InputPackage(Direction.South));
            }

            if ((oldState.IsKeyUp(Keys.Up) && newState.IsKeyDown(Keys.Up)) || gamePad.ThumbSticks.Left.Y > epsilon)
            {
                networkClientManager.Send(new InputPackage(Direction.North));
            }

            oldState = newState;
        }
    }
}
