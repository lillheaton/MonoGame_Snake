using Lidgren.Network;

namespace Definitions.NetworkPackages
{
    public interface IPackage<out T> : IBasePackage
    {
        T Decrypt(NetIncomingMessage package);
    }
}
