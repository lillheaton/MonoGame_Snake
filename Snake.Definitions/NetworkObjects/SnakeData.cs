using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Definitions.NetworkObjects
{
    public class SnakeData
    {
        public DateTime UpdateTimeStamp { get; set; }
        public List<Vector2> SnakeParts { get; set; }

        public SnakeData()
        {
            SnakeParts = new List<Vector2>();
        }
    }
}
