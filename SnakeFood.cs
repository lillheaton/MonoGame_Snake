using System.Linq;

using Definitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Client
{
    public class SnakeFood
    {
        public List<Vector2> FoodList { get; set; }
        public const int FoodSize = BaseSnake.SnakeBodySize;

        public SnakeFood()
        {
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

        public bool TryPickFoodAtPosition(Vector2 position)
        {
            if (FoodList.Any(s => s.Equals(position)))
            {
                return true;
            }
            return false;
        }
    }
}
