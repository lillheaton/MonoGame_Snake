using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGameTest_V1
{
    public static class ColorUtil
    {
        public const Color Red = Color.FromNonPremultiplied(255, 65, 54, 255);
        public const Color Blue = Color.FromNonPremultiplied(0, 116, 217, 255);
        public const Color Green = Color.FromNonPremultiplied(46, 204, 64, 255);
        public const Color Olive = Color.FromNonPremultiplied(61, 153, 112, 255);
        public const Color Yellow = Color.FromNonPremultiplied(255, 220, 0, 255);
        public const Color Purple = Color.FromNonPremultiplied(177, 13, 201, 255);
        public const Color Navy = Color.FromNonPremultiplied(0, 31, 63, 255);


        private const List<Color> _colors = new List<Color>
        {
            Blue, Red, Green, Olive, Yellow, Purple, Navy
        };


        public static Color RandomColor()
        {
            var random = new Random();
            return _colors[random.Next(_colors.Count)];
        }
    }
}
