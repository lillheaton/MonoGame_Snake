using Lidgren.Network;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Definitions.NetworkPackages
{
    public class SnakePartsPackage : IPackage<List<Vector2>>
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
            package.Write(Snake.BodyParts.Count);

            foreach (var bodypart in Snake.BodyParts)
            {
                package.Write(bodypart.X);
                package.Write(bodypart.Y);
            }
            
            return package;
        }

        List<Vector2> IPackage<List<Vector2>>.Decrypt(NetIncomingMessage package)
        {
            return Decrypt(package);
        }

        public static List<Vector2> Decrypt(NetIncomingMessage package)
        {
            // Need to read the first byte
            var type = (PackageType)package.ReadByte();
            int numberOfBodyParts = package.ReadInt32();
            var snakeParts = new List<Vector2>();

            for (int i = 0; i < numberOfBodyParts; i++)
            {
                float x = package.ReadFloat();
                float y = package.ReadFloat();
                snakeParts.Add(new Vector2(x, y));
            }

            return snakeParts;
        }
    }
}
