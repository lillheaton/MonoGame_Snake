﻿using Microsoft.Xna.Framework;

namespace MonoGameTest_V1
{
    public class SnakePart
    {
        public Vector2 Position { get; set; }

        public SnakePart(Vector2 position)
        {
            Position = position;
        }
    }
}