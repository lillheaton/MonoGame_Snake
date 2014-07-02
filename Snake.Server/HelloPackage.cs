using Lidgren.Network;

namespace Snake.Server
{
    public class HelloPackage : BasePackage
    {
        public string HelloMessage { get; set; }

        public HelloPackage()
        {
            this.Type = PackageType.Hello;
        }

        public override NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var newPackage = peer.CreateMessage();
            newPackage.Write((byte)Type);
            newPackage.Write(HelloMessage);
            return newPackage;
        }

        public override void Decrypt(NetIncomingMessage package)
        {
            this.Type = (PackageType)package.ReadByte();
            this.HelloMessage = package.ReadString();
        }
    }
}
