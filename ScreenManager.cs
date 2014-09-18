using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Client
{
    public class ScreenManager
    {
        public const int Width = 800;
        public const int Height = 480;
        private static ScreenManager _instance;
        public ContentManager Content { get; set; }
        
        // Gets the instance
        public static ScreenManager Instance
        {
            get
            {
                return _instance ?? (_instance = new ScreenManager());
            }
        }

        // Constructor
        public ScreenManager()
        {
        }

        public void LoadContentManager(ContentManager contentManager)
        {
            this.Content = contentManager;
        }
    }
}