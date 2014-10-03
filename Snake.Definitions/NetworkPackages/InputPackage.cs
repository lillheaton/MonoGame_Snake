using Lidgren.Network;

namespace Definitions.NetworkPackages
{
    public class InputPackageData
    {
        public Direction Direction { get; set; }
        public int UpdateId { get; set; }
    }

    public class InputPackage : IPackage<InputPackageData>
    {
        public PackageType Type;
        public Direction Direction { get; set; }
        public int UpdateId { get; set; }

        public InputPackage(Direction direction, int updateId)
        {
            this.Direction = direction;
            this.Type = PackageType.KeyboardInput;
            this.UpdateId = updateId;
        }

        public NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var package = peer.CreateMessage();
            package.Write((byte)Type);
            package.Write((byte)Direction);
            package.Write(UpdateId);
            return package;
        }

        InputPackageData IPackage<InputPackageData>.Decrypt(NetIncomingMessage package)
        {
            return Decrypt(package);
        }

        public static InputPackageData Decrypt(NetIncomingMessage package)
        {
            var type = (PackageType)package.ReadByte();
            var model = new InputPackageData();
            model.Direction = (Direction)package.ReadByte();
            model.UpdateId = package.ReadInt32();
            return model;
        }
    }
}
