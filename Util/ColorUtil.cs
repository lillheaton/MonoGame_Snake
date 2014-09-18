using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Client.Util
{
    public static class ColorUtil
    {
        public static Color Navy = Color.FromNonPremultiplied(0, 31, 63, 255);
        public static Color Blue = Color.FromNonPremultiplied(0, 116, 217, 255);
        public static Color Aqua = Color.FromNonPremultiplied(127, 219, 255, 255);
        public static Color Teal = Color.FromNonPremultiplied(57, 204, 204, 255);
        public static Color Olive = Color.FromNonPremultiplied(61, 153, 112, 255);
        public static Color Green = Color.FromNonPremultiplied(46, 204, 64, 255);
        public static Color Lime = Color.FromNonPremultiplied(1, 255, 112, 255);
        public static Color Yellow = Color.FromNonPremultiplied(255, 220, 0, 255);
        public static Color Orange = Color.FromNonPremultiplied(255, 133, 27, 255);
        public static Color Red = Color.FromNonPremultiplied(255, 65, 54, 255);
        public static Color Maroon = Color.FromNonPremultiplied(133, 20, 75, 255);
        public static Color Fuchsia = Color.FromNonPremultiplied(240, 18, 190, 255);
        public static Color Purple = Color.FromNonPremultiplied(177, 13, 201, 255);
        public static Color Black = Color.FromNonPremultiplied(17, 17, 17, 255);
        public static Color Gray = Color.FromNonPremultiplied(170, 170, 170, 255);
        public static Color Silver = Color.FromNonPremultiplied(221, 221, 221, 255);


        private static List<Color> _colors = new List<Color>
        {
            Navy, Blue, Aqua, Teal, Olive, Green, Lime, Yellow, Orange, Red, Maroon, Fuchsia, Purple, Black, Gray, Silver
        };


        public static Color RandomColor()
        {
            var random = new Random();
            return _colors[random.Next(_colors.Count)];
        }
    }
}
