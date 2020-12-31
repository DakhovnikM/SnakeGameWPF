using SnakeGameWPF.Models.GameObjects;

namespace SnakeGameWPF.Models.GameObjectsFactories
{
    internal abstract class GameObjectFactory
    {
        public GameSettings Settings { get; }

        public GameObjectFactory(GameSettings gameSettings)
        {
            Settings = gameSettings;
        }

        public abstract GameObject GetObject();
    }
}
