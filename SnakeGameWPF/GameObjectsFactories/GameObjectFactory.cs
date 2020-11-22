using System.Windows;

namespace SnakeGameWPF
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
