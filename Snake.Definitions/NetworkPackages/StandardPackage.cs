using Definitions.Particles;

using Lidgren.Network;

using Microsoft.Xna.Framework;

namespace Definitions.NetworkPackages
{
    public class StandardPackage : IPackage<StandardPackageData>
    {
        public StandardPackageData Data { get; set; }

        public StandardPackage(StandardPackageData data)
        {
            this.Data = data;
        }

        public NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var message = peer.CreateMessage();
            message.Write((byte)PackageType.StandardPackage);

            message.Write(Data.Snakes.Count);
            foreach (var snake in Data.Snakes)
            {
                message.Write(snake.X);
                message.Write(snake.Y);
            }

            message.Write(Data.Particles.Count);
            foreach (var particle in Data.Particles)
            {
                message.Write(particle.X);
                message.Write(particle.Y);
            }

            message.Write(Data.SnakeFood.Count);
            foreach (var food in Data.SnakeFood)
            {
                message.Write(food.X);
                message.Write(food.Y);
            }

            return message;
        }

        public static StandardPackageData Decrypt(NetIncomingMessage package)
        {
            var data = new StandardPackageData();
            package.ReadByte();

            var numberOfSnakes = package.ReadInt32();
            for (int i = 0; i < numberOfSnakes; i++)
            {
                data.Snakes.Add(new Vector2(package.ReadFloat(), package.ReadFloat()));
            }

            var numberOfParticles = package.ReadInt32();
            for (int i = 0; i < numberOfParticles; i++)
            {
                data.Particles.Add(new Vector2(package.ReadFloat(), package.ReadFloat()));
            }

            var numberOfFood = package.ReadInt32();
            for (int i = 0; i < numberOfFood; i++)
            {
                data.SnakeFood.Add(new Vector2(package.ReadFloat(), package.ReadFloat()));
            }

            return data;
        }

        StandardPackageData IPackage<StandardPackageData>.Decrypt(NetIncomingMessage package)
        {
            return Decrypt(package);
        }
    }
}
