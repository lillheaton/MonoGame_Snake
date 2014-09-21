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
        public List<Vector2> FoodList { get; private set; }
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

        public void Update(GameTime gameTime)
        {
            lastUpdateTime += gameTime.ElapsedGameTime;
            if (lastUpdateTime > updatesPerMilliseconds)
            {
                lastUpdateTime -= updatesPerMilliseconds;
                //this.SpawnRandomFood();
            }
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


        public void SpawnFood(List<ClientSnake> snakes)
        {
            var newFood = this.GenerateUniqueLocation(snakes);
            FoodList.Add(newFood);
            Console.WriteLine(newFood);
        }
        private Vector2 GenerateUniqueLocation(List<ClientSnake> snakes)
        {
            const int X = ScreenManager.Width / FoodSize;
            const int Y = ScreenManager.Height / FoodSize;
            var location = new Vector2(random.Next(X), random.Next(Y));


            if (FoodList.Contains(location) || snakes.Any(snake => snake.BodyParts.Any(part => part.Position == location)))
            {
                return this.GenerateUniqueLocation(snakes);
            }
            return location;
        }



        public bool TryPickFoodAtPosition(Vector2 position)
        {
            if(FoodList.Contains(position))
            {
                FoodList.Remove(position);
                return true;
            }
            return false;
        }
    }
}
