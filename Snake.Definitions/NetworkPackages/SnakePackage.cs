using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Definitions.NetworkPackages
{
    public class SnakePackage : BasePackage
    {
        private Snake Snake { get; set; }

        public SnakePackage(Snake snake)
        {
            this.Snake = snake;
            this.Type = PackageType.Snake;
        }

        public override NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var package = peer.CreateMessage();
            package.Write((byte)Type);
            package.WriteAllProperties(Snake.BodyParts);
            return package;
        }

        public override void Decrypt(NetIncomingMessage package)
        {
            this.Type = (PackageType)package.ReadByte();
            package.ReadAllProperties(Snake.BodyParts);
        }
    }
}
