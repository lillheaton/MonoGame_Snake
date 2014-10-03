using System.Collections.Generic;
using System.Linq;
using Definitions.NetworkObjects;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;

namespace Definitions.NetworkPackages
{
    public class SnakePartsPackage : IPackage<List<TimeFrame>>
    {
        private BaseSnake Snake { get; set; }

        public SnakePartsPackage(BaseSnake snake)
        {
            this.Snake = snake;
        }

        public NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var package = peer.CreateMessage();
            package.Write((byte)PackageType.Snake);

            package.Write(Snake.TimeFrames.Count());
            foreach (var timeframe in Snake.TimeFrames)
            {
                package.Write(timeframe.Id);

                package.Write(Snake.BodyParts.Count);
                foreach (var bodypart in Snake.BodyParts)
                {
                    package.Write(bodypart.X);
                    package.Write(bodypart.Y);
                }    
            }
            
            return package;
        }

        List<TimeFrame> IPackage<List<TimeFrame>>.Decrypt(NetIncomingMessage package)
        {
            return Decrypt(package);
        }

        public static List<TimeFrame> Decrypt(NetIncomingMessage package)
        {
            var model = new List<TimeFrame>();

            package.ReadByte();

            int numberOfFrames = package.ReadInt32();
            for (int i = 0; i < numberOfFrames; i++)
            {
                var timeFrame = new TimeFrame();
                timeFrame.Id = package.ReadInt32();

                int numberOfBodyParts = package.ReadInt32();
                for (int j = 0; j < numberOfBodyParts; j++)
                {
                    timeFrame.BodyParts.Add(new Vector2(package.ReadFloat(), package.ReadFloat()));
                }

                model.Add(timeFrame);
            }

            return model;
        }
    }
}
