using Lidgren.Network;

namespace Definitions.NetworkPackages
{
    public class InputPackage : IPackage<Direction>
    {
        public PackageType Type;
        public Direction Direction { get; set; }

        public InputPackage(Direction direction)
        {
            this.Direction = direction;
            this.Type = PackageType.KeyboardInput;
        }

        public NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var package = peer.CreateMessage();
            package.Write((byte)Type);
            package.Write((byte)Direction);
            return package;
        }

        Direction IPackage<Direction>.Decrypt(NetIncomingMessage package)
        {
            return Decrypt(package);
        }

        public static Direction Decrypt(NetIncomingMessage package)
        {
            var type = (PackageType)package.ReadByte();
            return (Direction)package.ReadByte();            
        }
    }
}
