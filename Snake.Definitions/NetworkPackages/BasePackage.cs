using Lidgren.Network;

namespace Snake.Definitions.NetworkPackages
{
    public abstract class BasePackage
    {
        protected PackageType Type;

        public abstract NetOutgoingMessage Encrypt(NetPeer peer);

        public abstract void Decrypt(NetIncomingMessage package);
    }
}
