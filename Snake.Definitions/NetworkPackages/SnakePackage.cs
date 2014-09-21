using System;
using System.Collections.Generic;
using System.Reflection;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Definitions.NetworkPackages
{
    public class SnakePackage : BasePackage
    {
        public Snake Snake { get; set; }

        public SnakePackage(Snake snake)
        {
            this.Snake = snake;
            this.Type = PackageType.Snake;
        }

        public override NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var package = peer.CreateMessage();
            package.Write((byte)Type);
            package.WriteVariableInt32(Snake.BodyParts.Count);

            foreach (var bodypart in Snake.BodyParts)
            {
                package.Write(bodypart.Position.X);
                package.Write(bodypart.Position.Y);
            }
            
            return package;
        }

        public override Snake Decrypt<T>(NetIncomingMessage package)
        {
            throw new NotImplementedException();
        }

        public Snake Decrypt(NetIncomingMessage package)
        {
            this.Type = (PackageType)package.ReadByte();
            int numberOfBodyParts = package.ReadInt32();
            
            Snake.BodyParts = new List<SnakePart>(numberOfBodyParts);
            for (int i = 0; i < numberOfBodyParts; i++)
            {
                float x = package.ReadFloat();
                float y = package.ReadFloat();
                Snake.BodyParts[i] = new SnakePart(new Vector2(x, y));
            }           
        }
    }
}
