using Lidgren.Network;

namespace Snake.Server.Packages
{
    public class HandshakePackage : BasePackage
    {
        public HandshakePackage()
        {
            this.Type = PackageType.Handshake;
        }

        public override NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var package = peer.CreateMessage();
            package.Write((byte)this.Type);
            return package;
        }

        public override void Decrypt(NetIncomingMessage package)
        {
            this.Type = (PackageType)package.ReadByte();
        }
    }
}
