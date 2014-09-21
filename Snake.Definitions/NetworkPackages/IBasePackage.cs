
using Lidgren.Network;

namespace Definitions.NetworkPackages
{
    public interface IBasePackage
    {
        NetOutgoingMessage Encrypt(NetPeer peer);
    }
}
