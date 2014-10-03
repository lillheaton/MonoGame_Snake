using System;
using System.Linq;
using Definitions;
using Definitions.NetworkPackages;
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

        public void HandleInputChange(InputPackageData inputPackageData)
        {
            var timeFrame = TimeFrames.FirstOrDefault(s => s.Id == inputPackageData.UpdateId);
            if (timeFrame != null && timeFrame.SnakeDirection.GetEnumDirection() != inputPackageData.Direction)
            {
                this.BodyParts = timeFrame.BodyParts;
                Console.WriteLine("Changed!!!");
            }
            this.UpdateInput(inputPackageData.Direction);
        }
    }
}
