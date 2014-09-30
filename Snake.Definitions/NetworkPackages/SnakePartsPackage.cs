using Definitions.NetworkObjects;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;

namespace Definitions.NetworkPackages
{
    public class SnakePartsPackage : IPackage<SnakeData>
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
            package.WriteTime(Snake.UpdateTimeStamp.Ticks, false);
            package.Write(Snake.BodyParts.Count);

            foreach (var bodypart in Snake.BodyParts)
            {
                package.Write(bodypart.X);
                package.Write(bodypart.Y);
            }
            
            return package;
        }

        SnakeData IPackage<SnakeData>.Decrypt(NetIncomingMessage package)
        {
            return Decrypt(package);
        }

        public static SnakeData Decrypt(NetIncomingMessage package)
        {
            package.ReadByte();
            var model = new SnakeData();
            model.UpdateTimeStamp = new DateTime((long)package.ReadTime(false));

            int numberOfBodyParts = package.ReadInt32();            
            for (int i = 0; i < numberOfBodyParts; i++)
            {
                float x = package.ReadFloat();
                float y = package.ReadFloat();
                model.SnakeParts.Add(new Vector2(x, y));
            }

            return model;
        }
    }
}
