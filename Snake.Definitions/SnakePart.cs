using Microsoft.Xna.Framework;

namespace Definitions
{
    public class SnakePart
    {
        public const int Size = 20;

        public Vector2 Position { get; set; }

        public SnakePart(Vector2 position)
        {
            Position = position;
        }
    }
}
