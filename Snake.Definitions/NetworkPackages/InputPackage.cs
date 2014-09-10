using Lidgren.Network;

namespace Snake.Definitions.NetworkPackages
{
    public class InputPackage : BasePackage
    {
        public Direction Direction { get; set; }

        public InputPackage(Direction direction)
        {
            this.Direction = direction;
            this.Type = PackageType.KeyboardInput;
        }

        public override NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var package = peer.CreateMessage();
            package.Write((byte)Type);
            package.Write((byte)Direction);
            return package;
        }

        public override void Decrypt(NetIncomingMessage package)
        {
            this.Type = (PackageType)package.ReadByte();
            this.Direction = (Direction)package.ReadByte();
        }
    }
}
