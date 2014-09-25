using Definitions.Particles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Definitions.NetworkPackages
{
    public class StandardPackageData
    {
        public List<Vector2> Snakes { get; set; }
        public List<Vector2> Particles { get; set; }
        public List<Vector2> SnakeFood { get; set; }

        public StandardPackageData()
        {
            Snakes = new List<Vector2>();
            Particles = new List<Vector2>();
            SnakeFood = new List<Vector2>();
        }
    }
}