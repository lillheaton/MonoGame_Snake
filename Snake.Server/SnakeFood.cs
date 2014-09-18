using System;
using System.Collections.Generic;
using System.Linq;
using Definitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Server
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

        public void SpawnFood(List<Definitions.Snake> snakes)
        {
            var newFood = this.GenerateUniqueLocation(snakes);
            FoodList.Add(newFood);
            Console.WriteLine(newFood);
        }
        private Vector2 GenerateUniqueLocation(List<Definitions.Snake> snakes)
        {
            // TODO: Set this in definitions project
            const int X = 800 / FoodSize;
            const int Y = 480 / FoodSize;
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
