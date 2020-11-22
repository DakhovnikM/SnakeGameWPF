using System.Windows;

namespace SnakeGameWPF
{
    abstract class GameObject
    {
        public Point ObjectCoordinate;

        public UIElement ObjectImage { get; set; }

        public GameObjectType ObjectType { get; set; }

    }
}
