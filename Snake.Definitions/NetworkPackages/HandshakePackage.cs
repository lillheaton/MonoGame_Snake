using Lidgren.Network;

namespace Definitions.NetworkPackages
{
    public class HandshakePackage : IPackage<PackageType>
    {
        private PackageType Type;

        public HandshakePackage()
        {
            this.Type = PackageType.Handshake;
        }

        public NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var package = peer.CreateMessage();
            package.Write((byte)this.Type);
            return package;
        }

        public PackageType Decrypt(NetIncomingMessage package)
        {
            this.Type = (PackageType)package.ReadByte();
            return Type;
        }
    }
}
