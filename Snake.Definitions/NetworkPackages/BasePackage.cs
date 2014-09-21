using System.Runtime.CompilerServices;
using Lidgren.Network;

namespace Definitions.NetworkPackages
{
    public abstract class BasePackage
    {
        protected PackageType Type;

        public abstract NetOutgoingMessage Encrypt(NetPeer peer);

        public abstract T Decrypt<T>(NetIncomingMessage package);
    }
}
