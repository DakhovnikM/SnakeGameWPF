using System.Windows;

namespace SnakeGameWPf
{
    abstract class GameObjectFactory
    {
        public GameSettings GameSettings { get; }

        public GameObjectFactory(GameSettings gameSettings)
        {
            GameSettings = gameSettings;
        }

        public abstract GameObject GetObject();
    }
}
