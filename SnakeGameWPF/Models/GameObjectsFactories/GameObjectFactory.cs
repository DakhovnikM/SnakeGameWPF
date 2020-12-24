using SnakeGameWPF.Models.GameObjects;

namespace SnakeGameWPF.Models.GameObjectsFactories
{
    internal abstract class GameObjectFactory
    {
        public GameSettings GameSettings { get; }

        public GameObjectFactory(GameSettings gameSettings)
        {
            GameSettings = gameSettings;
        }

        public abstract GameObject GetObject();
    }
}
