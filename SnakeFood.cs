using Client.Objects;

using Definitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Client
{
    public class SnakeFood
    {
        public List<Vector2> FoodList { get; set; }
        public const int FoodSize = Snake.SnakeBodySize;

        private TimeSpan lastUpdateTime;
        private readonly TimeSpan updatesPerMilliseconds;
        private readonly Random random;

        public SnakeFood()
        {
            this.updatesPerMilliseconds = TimeSpan.FromMilliseconds(3000);
            this.random = new Random();

            this.Init();
        }

        public void Init()
        {
            FoodList = new List<Vector2>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var food in FoodList)
            {
                var rect = new Rectangle();
                rect.Width = FoodSize;
                rect.Height = FoodSize;
                rect.X = (int)food.X * FoodSize;
                rect.Y = (int)food.Y * FoodSize;

                Color foodColor = Color.FromNonPremultiplied(75, 0, 130, 255);
                RectangleGraphicsHelper.DrawRectangle(spriteBatch, rect, foodColor);
            }
        }
    }
}
