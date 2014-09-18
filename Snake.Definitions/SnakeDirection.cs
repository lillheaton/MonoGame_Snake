using Microsoft.Xna.Framework;

namespace Definitions
{
    public class SnakeDirection
    {
        public static SnakeDirection North = new SnakeDirection(_north);
        public static SnakeDirection South = new SnakeDirection(_south);
        public static SnakeDirection East = new SnakeDirection(_east);
        public static SnakeDirection West = new SnakeDirection(_west);

        public static Vector2 NorthVector = new Vector2(0, -1);
        public static Vector2 SouthVector = new Vector2(0, 1);
        public static Vector2 EastVector = new Vector2(1, 0);
        public static Vector2 WestVector = new Vector2(-1, 0);


        private const int _north = -1;
        private const int _south = 1;
        private const int _east = 2;
        private const int _west = -2;

        private int direction;
        

        // Constructor
        private SnakeDirection(int direction)
        {
            this.direction = direction;
        }
        public SnakeDirection(SnakeDirection snakeDirection)
        {
            this.direction = snakeDirection.direction;
        }



        
        public void SetDirection(SnakeDirection snakeDirection)
        {
            this.direction = snakeDirection.direction;
        }

        public bool isOppositeOf(SnakeDirection direction)
        {
            return (direction.direction + this.direction == 0);
        }

        public Vector2 GetVector()
        {
            switch(this.direction)
            {
                case _north: return SnakeDirection.NorthVector;
                case _south: return SnakeDirection.SouthVector;
                case _east: return SnakeDirection.EastVector;
                case _west: return SnakeDirection.WestVector;
            }
            return new Vector2(0, 0);
        }
    }
}
