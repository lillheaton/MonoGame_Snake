using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Definitions.NetworkPackages
{
    public class SnakePartsPackage : IPackage<List<SnakePart>>
    {
        public PackageType Type;
        private Snake Snake { get; set; }

        public SnakePartsPackage(Snake snake)
        {
            Type = PackageType.Snake;
            this.Snake = snake;
        }

        public NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var package = peer.CreateMessage();
            package.Write((byte)Type);
            package.Write(Snake.BodyParts.Count);

            Console.WriteLine(Snake.BodyParts.First().Position);

            foreach (var bodypart in Snake.BodyParts)
            {
                package.Write(bodypart.Position.X);
                package.Write(bodypart.Position.Y);
            }
            
            return package;
        }

        List<SnakePart> IPackage<List<SnakePart>>.Decrypt(NetIncomingMessage package)
        {
            return Decrypt(package);
        }

        public static List<SnakePart> Decrypt(NetIncomingMessage package)
        {
            // Need to read the first byte
            var type = (PackageType)package.ReadByte();
            int numberOfBodyParts = package.ReadInt32();
            var snakeParts = new List<SnakePart>();

            for (int i = 0; i < numberOfBodyParts; i++)
            {
                float x = package.ReadFloat();
                float y = package.ReadFloat();
                snakeParts.Add(new SnakePart(new Vector2(x, y)));
            }

            Console.WriteLine(snakeParts.First().Position);

            return snakeParts;
        }
    }
}
