using System.Windows;

namespace SnakeGameWPF
{
    abstract class GameObject
    {
        public Point ObjectCoordinate;

        public UIElement ObjectImage { get; set; }

        public GameObjectType ObjectType { get; set; }

        //private readonly Random rndNumber = new Random();
        //public int Rnd { get => rndNumber.Next(0, 58) * 10; }
    }
}
