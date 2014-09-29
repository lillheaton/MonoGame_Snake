using Definitions;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Server
{
    public class Player : BaseSnake
    {
        public NetConnection Connection { get; set; }

        public Player(Vector2 position, SnakeDirection direction, NetConnection connection) : base(position, direction)
        {
            this.Connection = connection;
        }
    }
}
