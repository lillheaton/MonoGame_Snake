using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;

using Microsoft.Xna.Framework;

namespace Definitions.NetworkPackages
{
    public class FoodPackage : IPackage<List<Vector2>>
    {
        private List<Vector2> foodList;

        public FoodPackage(List<Vector2> foodList)
        {
            this.foodList = foodList;
        }

        public NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var message = peer.CreateMessage();
            message.Write((byte)PackageType.FoodPackage);
            message.Write(foodList.Count);

            foreach (var vector2 in foodList)
            {
                message.Write(vector2.X);
                message.Write(vector2.Y);
            }
            return message;
        }

        public static List<Vector2> Decrypt(NetIncomingMessage package)
        {
            package.ReadByte();
            var numberOfFood = package.ReadInt32();
            var list = new List<Vector2>();

            for (int i = 0; i < numberOfFood; i++)
            {
                var vector = new Vector2();
                vector.X = package.ReadFloat();
                vector.Y = package.ReadFloat();

                list.Add(vector);
            }
            return list;
        }

        List<Vector2> IPackage<List<Vector2>>.Decrypt(NetIncomingMessage package)
        {
            return Decrypt(package);
        }
    }
}
