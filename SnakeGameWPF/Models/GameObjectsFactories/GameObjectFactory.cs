namespace SnakeGameWPF.Models
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
