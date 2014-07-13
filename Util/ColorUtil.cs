using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoGameTest_V1.Util
{
    public static class ColorUtil
    {
        public static Color Red = Color.FromNonPremultiplied(255, 65, 54, 255);
        public static Color Blue = Color.FromNonPremultiplied(0, 116, 217, 255);
        public static Color Green = Color.FromNonPremultiplied(46, 204, 64, 255);
        public static Color Olive = Color.FromNonPremultiplied(61, 153, 112, 255);
        public static Color Yellow = Color.FromNonPremultiplied(255, 220, 0, 255);
        public static Color Purple = Color.FromNonPremultiplied(177, 13, 201, 255);
        public static Color Navy = Color.FromNonPremultiplied(0, 31, 63, 255);


        private static List<Color> _colors = new List<Color>
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
