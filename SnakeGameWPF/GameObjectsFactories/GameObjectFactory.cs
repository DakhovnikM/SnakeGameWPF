using System.Windows;

namespace SnakeWPF_Ver2
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
