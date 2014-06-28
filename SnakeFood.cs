using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoGameTest_V1
{
    public class SnakeFood
    {
        public List<Vector2> FoodList { get; private set; }
        public const int FoodSize = Snake.SnakeBodySize;

        private readonly SpriteBatch spriteBatch;
        private TimeSpan lastUpdateTime;
        private readonly TimeSpan updatesPerMilliseconds;
        private readonly Random random;

        public SnakeFood(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            this.updatesPerMilliseconds = TimeSpan.FromMilliseconds(3000);
            this.random = new Random();
            FoodList = new List<Vector2>();
        }

        public void Update(GameTime gameTime)
        {
            lastUpdateTime += gameTime.ElapsedGameTime;
            if (lastUpdateTime > updatesPerMilliseconds)
            {
                lastUpdateTime -= updatesPerMilliseconds;
                this.RandomFood();
            }
        }

        public void Draw()
        {
            foreach (var food in FoodList)
            {
                var rect = new Rectangle();
                rect.Width = FoodSize;
                rect.Height = FoodSize;
                rect.X = (int)food.X * FoodSize;
                rect.Y = (int)food.Y * FoodSize;

                Color foodColor = Color.FromNonPremultiplied(255, 65, 54, 255);
                RectangleGraphicsHelper.DrawRectangle(spriteBatch, rect, foodColor);
            }
        }

        private void RandomFood()
        {
            var newFood = this.GenerateUniqueLocation();
            FoodList.Add(newFood);
            Console.WriteLine(newFood);
        }

        private Vector2 GenerateUniqueLocation()
        {
            const int X = ScreenManager.Width / FoodSize;
            const int Y = ScreenManager.Height / FoodSize;
            var location = new Vector2(random.Next(X), random.Next(Y));

            if (FoodList.Contains(location))
            {
                return this.GenerateUniqueLocation();
            }
            return location;
        }
    }
}
